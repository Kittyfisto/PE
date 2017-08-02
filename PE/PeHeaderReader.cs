using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PE
{
	/// <summary>
	///     Reads in the header information of the Portable Executable format.
	///     Provides information such as the date the assembly was compiled.
	/// </summary>
	public class PeHeaderReader
	{
		#region Private Fields

		/// <summary>
		///     The DOS header
		/// </summary>
		private readonly IMAGE_DOS_HEADER _dosHeader;

		/// <summary>
		///     The file header
		/// </summary>
		private IMAGE_FILE_HEADER _fileHeader;

		/// <summary>
		///     Optional 32 bit file header
		/// </summary>
		private readonly IMAGE_OPTIONAL_HEADER32? _peHeader32;

		/// <summary>
		///     Optional 64 bit file header
		/// </summary>
		private readonly IMAGE_OPTIONAL_HEADER64? _peHeader64;

		/// <summary>
		///     Image Section headers. Number of sections is in the file header.
		/// </summary>
		private readonly IMAGE_SECTION_HEADER[] _imageSectionHeaders;

		private readonly IMAGE_COR20_HEADER? _cliHeader;

		#endregion Private Fields

		#region Public Methods
		
		public PeHeaderReader(IMAGE_DOS_HEADER dosHeader, IMAGE_FILE_HEADER fileHeader, IMAGE_OPTIONAL_HEADER32? peHeader32, IMAGE_OPTIONAL_HEADER64? peHeader64, IMAGE_SECTION_HEADER[] imageSectionHeaders, IMAGE_COR20_HEADER? cliHeader)
		{
			_dosHeader = dosHeader;
			_fileHeader = fileHeader;
			_peHeader32 = peHeader32;
			_peHeader64 = peHeader64;
			_imageSectionHeaders = imageSectionHeaders;
			_cliHeader = cliHeader;
		}

		/// <summary>
		///     Reads in a block from a file and converts it to the struct
		///     type specified by the template parameter
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static T FromBinaryReader<T>(BinaryReader reader) where T : struct
		{
			// Read in a byte array
			var bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));

			// Pin the managed memory while, copy it out the data, then unpin it
			var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			var theStructure = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
			handle.Free();

			return theStructure;
		}

		#endregion Public Methods

		#region Properties

		/// <summary>
		///     Gets if the file header is 32 bit or not
		/// </summary>
		public bool Is32BitHeader => _peHeader32 != null;

		/// <summary>
		///     Gets the file header
		/// </summary>
		public IMAGE_FILE_HEADER FileHeader => _fileHeader;

		/// <summary>
		///     Gets the optional header
		/// </summary>
		public IMAGE_OPTIONAL_HEADER32 OptionalHeader32
		{
			get
			{
				if (_peHeader32 == null)
					throw new InvalidOperationException();

				return _peHeader32.Value;
			}
		}

		/// <summary>
		///     Gets the optional header
		/// </summary>
		public IMAGE_OPTIONAL_HEADER64 PeHeader64
		{
			get
			{
				if (_peHeader64 == null)
					throw new InvalidOperationException();

				return _peHeader64.Value;
			}
		}

		public bool IsClrAssembly => _cliHeader != null;

		public IMAGE_COR20_HEADER CliHeader
		{
			get
			{
				if (_cliHeader == null)
					throw new InvalidOperationException();

				return _cliHeader.Value;
			}
		}

		public IMAGE_DOS_HEADER DosHeader => _dosHeader;

		public IMAGE_SECTION_HEADER[] ImageSectionHeaders => _imageSectionHeaders;

		/// <summary>
		///     Gets the timestamp from the file header
		/// </summary>
		public DateTime TimeStamp
		{
			get
			{
				// Timestamp is a date offset from 1970
				var returnValue = new DateTime(1970, 1, 1, 0, 0, 0);

				// Add in the number of seconds since 1970/1/1
				returnValue = returnValue.AddSeconds(_fileHeader.TimeDateStamp);
				// Adjust to local timezone
				returnValue += TimeZone.CurrentTimeZone.GetUtcOffset(returnValue);

				return returnValue;
			}
		}

		private static long VirtualAddressToFilePosition(uint virtualAddress, IMAGE_SECTION_HEADER[] sectionHeaders)
		{
			for (int i = 0; i < sectionHeaders.Length; ++i)
			{
				long relativeVirtualAddress = (long)virtualAddress - sectionHeaders[i].VirtualAddress;
				if (relativeVirtualAddress >= 0 && relativeVirtualAddress < sectionHeaders[i].SizeOfRawData)
					return sectionHeaders[i].PointerToRawData + relativeVirtualAddress;
			}
			throw new InvalidDataException(string.Format("Could not resolve virtual address 0x{0:X}", virtualAddress));
		}

		#endregion Properties

		public static PeHeaderReader ReadFrom(string fileName)
		{
			using (var stream = File.OpenRead(fileName))
			{
				return ReadFrom(stream);
			}
		}

		public static PeHeaderReader ReadFrom(Stream stream, bool leaveOpen = false)
		{
			PeHeaderReader peReader;
			var error = TryReadFrom(stream, out peReader, leaveOpen);
			if (error != ReaderError.NoError)
				throw new ArgumentException(string.Format("{0}", error));

			return peReader;
		}

		public static ReaderError TryReadFrom(Stream stream, out PeHeaderReader peReader, bool leaveOpen = false, bool catchAllExceptions = true)
		{
			peReader = null;

			try
			{
				using (var reader = new BinaryReader(stream, Encoding.Default, leaveOpen))
				{
					var left = stream.Length - stream.Position;
					if (left < Marshal.SizeOf<IMAGE_DOS_HEADER>())
						return ReaderError.TooSmallForDosHeader;

					var dosHeader = IMAGE_DOS_HEADER.Read(reader);
					if (dosHeader.e_magic != 0x5a4d)
						return ReaderError.DosHeaderMagicMismatch;
					
					var fileHeaderOffset = dosHeader.e_lfanew;
					if (fileHeaderOffset < stream.Position || fileHeaderOffset >= stream.Length)
						return ReaderError.NewExeHeaderAddressOutOfRange;

					stream.Seek(fileHeaderOffset, SeekOrigin.Begin);

					left = stream.Length - stream.Position;
					if (left < Marshal.SizeOf<IMAGE_FILE_HEADER>())
						return ReaderError.TooSmallForFileHeader;

					var fileHeader = IMAGE_FILE_HEADER.Read(reader);

					left = stream.Length - stream.Position;
					if (left < 2)
						return ReaderError.TooSmallForOptionalHeader;

					ushort magic = reader.ReadUInt16();
					stream.Seek(-2, SeekOrigin.Current);

					IMAGE_OPTIONAL_HEADER32? peHeader32 = null;
					IMAGE_OPTIONAL_HEADER64? peHeader64 = null;
					switch (magic)
					{
						case 0x10b:

							left = stream.Length - stream.Position;
							if (left < Marshal.SizeOf<IMAGE_OPTIONAL_HEADER32>())
								return ReaderError.TooSmallForOptionalHeader;

							peHeader32 = IMAGE_OPTIONAL_HEADER32.Read(reader);
							break;

						case 0x20b:

							left = stream.Length - stream.Position;
							if (left < Marshal.SizeOf<IMAGE_OPTIONAL_HEADER64>())
								return ReaderError.TooSmallForOptionalHeader;

							peHeader64 = IMAGE_OPTIONAL_HEADER64.Read(reader);
							break;

						default:
							throw new InvalidOperationException(string.Format("Expected magic value of either 0x10b or 0x20b but found 0x{0:X}", magic));
					}

					var imageSectionHeaders = new IMAGE_SECTION_HEADER[fileHeader.NumberOfSections];
					for (var headerNo = 0; headerNo < imageSectionHeaders.Length; ++headerNo)
						imageSectionHeaders[headerNo] = FromBinaryReader<IMAGE_SECTION_HEADER>(reader);

					IMAGE_DATA_DIRECTORY clrRuntimeHeader;
					if (peHeader32 != null)
					{
						clrRuntimeHeader = peHeader32.Value.CLRRuntimeHeader;
					}
					else
					{
						clrRuntimeHeader = peHeader64.Value.CLRRuntimeHeader;
					}

					IMAGE_COR20_HEADER? cliHeader = null;
					if (clrRuntimeHeader.Size > 0)
					{
						stream.Position = VirtualAddressToFilePosition(clrRuntimeHeader.VirtualAddress, imageSectionHeaders);
						cliHeader = IMAGE_COR20_HEADER.Read(reader);
					}

					peReader = new PeHeaderReader(dosHeader, fileHeader, peHeader32, peHeader64, imageSectionHeaders, cliHeader);
					return ReaderError.NoError;
				}
			}
			catch (Exception e)
			{
				if (catchAllExceptions)
				{
					Console.WriteLine("Caught unexpected exception: {0}", e);
					peReader = null;
					return ReaderError.UnhandledException;
				}

				throw;
			}
		}
	}
}