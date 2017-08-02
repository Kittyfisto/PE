using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PE
{
	/// <summary>
	/// From http://www.pinvoke.net/default.aspx/Structures/IMAGE_SECTION_HEADER.html
	/// via https://gist.github.com/caioproiete/b51f29f74f5f5b2c59c39e47a8afc3a3
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	// ReSharper disable once InconsistentNaming
	public struct IMAGE_SECTION_HEADER
	{
		public char Name0;
		public char Name1;
		public char Name2;
		public char Name3;
		public char Name4;
		public char Name5;
		public char Name6;
		public char Name7;
		public UInt32 VirtualSize;
		public UInt32 VirtualAddress;
		public UInt32 SizeOfRawData;
		public UInt32 PointerToRawData;
		public UInt32 PointerToRelocations;
		public UInt32 PointerToLinenumbers;
		public UInt16 NumberOfRelocations;
		public UInt16 NumberOfLinenumbers;
		public DataSectionFlags Characteristics;

		public static unsafe IMAGE_SECTION_HEADER Read(BinaryReader reader)
		{
			IMAGE_SECTION_HEADER value;
			byte* ptr = (byte*)&value;
			int size = Marshal.SizeOf<IMAGE_SECTION_HEADER>();
			for (int i = 0; i < size; ++i)
			{
				*ptr++ = reader.ReadByte();
			}
			return value;
		}
	}
}