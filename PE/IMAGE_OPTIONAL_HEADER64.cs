﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PE
{
	/// <summary>
	/// Represents the optional header format.
	/// See https://msdn.microsoft.com/en-us/library/windows/desktop/ms680339(v=vs.85).aspx for more.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	// ReSharper disable once InconsistentNaming
	public struct IMAGE_OPTIONAL_HEADER64
	{
		/// <summary>
		/// The state of the image file.
		/// </summary>
		public UInt16 Magic;

		/// <summary>
		/// The major version number of the linker.
		/// </summary>
		public Byte MajorLinkerVersion;

		/// <summary>
		/// The minor version number of the linker.
		/// </summary>
		public Byte MinorLinkerVersion;

		/// <summary>
		/// The size of the code section, in bytes, or the sum of all such sections if there are multiple code sections.
		/// </summary>
		public UInt32 SizeOfCode;

		/// <summary>
		/// The size of the initialized data section, in bytes, or the sum of all such sections if there are multiple initialized data sections.
		/// </summary>
		public UInt32 SizeOfInitializedData;

		/// <summary>
		/// The size of the uninitialized data section, in bytes, or the sum of all such sections if there are multiple uninitialized data sections.
		/// </summary>
		public UInt32 SizeOfUninitializedData;

		/// <summary>
		/// A pointer to the entry point function, relative to the image base address. For executable files, this is the starting address. For device drivers, this is the address of the initialization function. The entry point function is optional for DLLs. When no entry point is present, this member is zero.
		/// </summary>
		public UInt32 AddressOfEntryPoint;

		/// <summary>
		/// A pointer to the beginning of the code section, relative to the image base.
		/// </summary>
		public UInt32 BaseOfCode;

		/// <summary>
		/// The preferred address of the first byte of the image when it is loaded in memory. This value is a multiple of 64K bytes. The default value for DLLs is 0x10000000. The default value for applications is 0x00400000, except on Windows CE where it is 0x00010000.
		/// </summary>
		public UInt64 ImageBase;

		/// <summary>
		///The alignment of sections loaded in memory, in bytes. This value must be greater than or equal to the FileAlignment member. The default value is the page size for the system.
		/// </summary>
		public UInt32 SectionAlignment;

		/// <summary>
		/// The alignment of the raw data of sections in the image file, in bytes. The value should be a power of 2 between 512 and 64K (inclusive). The default is 512. If the SectionAlignment member is less than the system page size, this member must be the same as SectionAlignment.
		/// </summary>
		public UInt32 FileAlignment;

		/// <summary>
		/// The major version number of the required operating system.
		/// </summary>
		public UInt16 MajorOperatingSystemVersion;

		/// <summary>
		/// The minor version number of the required operating system.
		/// </summary>
		public UInt16 MinorOperatingSystemVersion;

		/// <summary>
		/// The major version number of the image.
		/// </summary>
		public UInt16 MajorImageVersion;

		/// <summary>
		/// The minor version number of the image.
		/// </summary>
		public UInt16 MinorImageVersion;

		/// <summary>
		/// The major version number of the subsystem.
		/// </summary>
		public UInt16 MajorSubsystemVersion;

		/// <summary>
		/// The minor version number of the subsystem.
		/// </summary>
		public UInt16 MinorSubsystemVersion;

		/// <summary>
		/// This member is reserved and must be 0.
		/// </summary>
		public UInt32 Win32VersionValue;

		/// <summary>
		/// The size of the image, in bytes, including all headers. Must be a multiple of SectionAlignment.
		/// </summary>
		public UInt32 SizeOfImage;

		/// <summary>
		/// The combined size of the following items, rounded to a multiple of the value specified in the FileAlignment member.
		/// </summary>
		public UInt32 SizeOfHeaders;

		/// <summary>
		/// The image file checksum. The following files are validated at load time: all drivers, any DLL loaded at boot time, and any DLL loaded into a critical system process.
		/// </summary>
		public UInt32 CheckSum;

		/// <summary>
		/// The subsystem required to run this image.
		/// </summary>
		public SUBSYSTEM Subsystem;

		/// <summary>
		/// The DLL characteristics of the image. 
		/// </summary>
		public UInt16 DllCharacteristics;

		/// <summary>
		/// The number of bytes to reserve for the stack. Only the memory specified by the SizeOfStackCommit member is committed at load time; the rest is made available one page at a time until this reserve size is reached.
		/// </summary>
		public UInt64 SizeOfStackReserve;

		/// <summary>
		/// The number of bytes to commit for the stack.
		/// </summary>
		public UInt64 SizeOfStackCommit;

		/// <summary>
		/// The number of bytes to reserve for the local heap. Only the memory specified by the SizeOfHeapCommit member is committed at load time; the rest is made available one page at a time until this reserve size is reached.
		/// </summary>
		public UInt64 SizeOfHeapReserve;

		/// <summary>
		/// The number of bytes to commit for the local heap.
		/// </summary>
		public UInt64 SizeOfHeapCommit;

		/// <summary>
		/// This member is obsolete.
		/// </summary>
		public UInt32 LoaderFlags;

		/// <summary>
		/// The number of directory entries in the remainder of the optional header. Each entry describes a location and size.
		/// </summary>
		public UInt32 NumberOfRvaAndSizes;

		/// <summary>
		///     Export table address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY ExportTable;

		/// <summary>
		///     Import table address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY ImportTable;

		/// <summary>
		///     Resource table address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY ResourceTable;

		/// <summary>
		///     Exception table address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY ExceptionTable;

		/// <summary>
		///     Certificate table address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY CertificateTable;

		/// <summary>
		///     Base relocation table address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY BaseRelocationTable;

		/// <summary>
		///     Debugging information starting address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY Debug;

		/// <summary>
		///     Architecture-specific data address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY Architecture;

		/// <summary>
		///     Global pointer register relative virtual address
		/// </summary>
		public IMAGE_DATA_DIRECTORY GlobalPtr;

		/// <summary>
		///     Thread local storage (TLS) table address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY TLSTable;

		/// <summary>
		///     Load configuration table address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY LoadConfigTable;

		/// <summary>
		///     Bound import table address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY BoundImport;

		/// <summary>
		///     Import address table address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY IAT;

		/// <summary>
		///     Delay import descriptor address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY DelayImportDescriptor;

		/// <summary>
		///     The CLR header address and size
		/// </summary>
		public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;

		/// <summary>
		///     Reserved
		/// </summary>
		public IMAGE_DATA_DIRECTORY Reserved;

		internal static unsafe IMAGE_OPTIONAL_HEADER64 Read(BinaryReader reader)
		{
			IMAGE_OPTIONAL_HEADER64 value;
			byte* ptr = (byte*)&value;
			int size = Marshal.SizeOf(typeof(IMAGE_OPTIONAL_HEADER64));
			for (int i = 0; i < size; ++i)
			{
				*ptr++ = reader.ReadByte();
			}
			return value;
		}
	}
}