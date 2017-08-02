using System.IO;
using System.Runtime.InteropServices;

namespace PE
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	// ReSharper disable InconsistentNaming
	public struct IMAGE_COR20_HEADER
	{
		public uint cb;
		public ushort MajorRuntimeVersion;
		public ushort MinorRuntimeVersion;
		public IMAGE_DATA_DIRECTORY MetaData;
		public COMIMAGE_FLAGS Flags;
		public uint EntryPointToken;
		public IMAGE_DATA_DIRECTORY Resources;
		public IMAGE_DATA_DIRECTORY StrongNameSignature;
		public IMAGE_DATA_DIRECTORY CodeManagerTable;
		public IMAGE_DATA_DIRECTORY VTableFixups;
		public IMAGE_DATA_DIRECTORY ExportAddressTableJumps;
		public IMAGE_DATA_DIRECTORY ManagedNativeHeader;

		public static unsafe IMAGE_COR20_HEADER Read(BinaryReader reader)
		{
			IMAGE_COR20_HEADER value;
			byte* ptr = (byte*)&value;
			int size = Marshal.SizeOf(typeof(IMAGE_COR20_HEADER));
			for (int i = 0; i < size; ++i)
			{
				*ptr++ = reader.ReadByte();
			}
			return value;
		}
	}
	// ReSharper restore InconsistentNaming
}
