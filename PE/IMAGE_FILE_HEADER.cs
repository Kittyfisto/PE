using System.IO;
using System.Runtime.InteropServices;

namespace PE
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	// ReSharper disable once InconsistentNaming
	public struct IMAGE_FILE_HEADER
	{
		public uint Unused;
		public MACHINE Machine;
		public ushort NumberOfSections;
		public uint TimeDateStamp;
		public uint PointerToSymbolTable;
		public uint NumberOfSymbols;
		public ushort SizeOfOptionalHeader;
		public CHARACTERISTICS Characteristics;

		public static unsafe IMAGE_FILE_HEADER Read(BinaryReader reader)
		{
			IMAGE_FILE_HEADER value;
			byte* ptr = (byte*)&value;
			int size = Marshal.SizeOf<IMAGE_FILE_HEADER>();
			for (int i = 0; i < size; ++i)
			{
				*ptr++ = reader.ReadByte();
			}
			return value;
		}
	}
}