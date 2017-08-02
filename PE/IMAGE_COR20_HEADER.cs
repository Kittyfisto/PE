using System.IO;
using System.Runtime.InteropServices;

namespace PE
{
	/// <summary>
	///     The obsolete COM Directory in PEs is now the .NET Directory (I call it this way).
	///     This sections starts with the COR20 structure, also known as CLI header:
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	// ReSharper disable InconsistentNaming
	public struct IMAGE_COR20_HEADER
	{
		/// <summary>
		///     Size of the structure.
		/// </summary>
		public uint cb;

		/// <summary>
		///     Major Runtime Version of the CLR Runtime.
		/// </summary>
		public ushort MajorRuntimeVersion;

		/// <summary>
		///     Minor Runtime Version of the CLR Runtime.
		/// </summary>
		public ushort MinorRuntimeVersion;

		/// <summary>
		///     A Data Directory giving RVA and Size of the MetaData.
		/// </summary>
		public IMAGE_DATA_DIRECTORY MetaData;

		/// <summary>
		/// </summary>
		public COMIMAGE_FLAGS Flags;

		/// <summary>
		/// </summary>
		public uint EntryPointToken;

		/// <summary>
		///     A Data Directory for the Resources. These resources are referenced in the MetaData.
		/// </summary>
		public IMAGE_DATA_DIRECTORY Resources;

		/// <summary>
		///     A Data Directory for the Strong Name Signature. It's a signature to uniquely identify .NET Assemblies. This section
		///     is only present when the COMIMAGE_FLAGS_STRONGNAMESIGNED is set.
		/// </summary>
		public IMAGE_DATA_DIRECTORY StrongNameSignature;

		/// <summary>
		///     Always 0.
		/// </summary>
		public IMAGE_DATA_DIRECTORY CodeManagerTable;

		/// <summary>
		///     Certain languages, which choose not to follow the common type system runtime model, may have virtual functions
		///     which need to be represented in a v-table. These v-tables are laid out by the compiler, not by the runtime. Finding
		///     the correct v-table slot and calling indirectly through the value held in that slot is also done by the compiler.
		///     The VtableFixups field in the runtime header contains the location and size of an array of Vtable Fixups (§14.5.1).
		///     V-tables shall be emitted into a read-write section of the PE file. Each entry in this array describes a contiguous
		///     array of v-table slots of the specified size. Each slot starts out initialized to the metadata token value for the
		///     method they need to call. At image load time, the runtime Loader will turn each entry into a pointer to machine
		///     code for the CPU and can be called directly.
		/// </summary>
		public IMAGE_DATA_DIRECTORY VTableFixups;

		/// <summary>
		///     Always 0.
		/// </summary>
		public IMAGE_DATA_DIRECTORY ExportAddressTableJumps;

		/// <summary>
		///     Always 0 in normal .NET assemblies, only present in native images.
		/// </summary>
		public IMAGE_DATA_DIRECTORY ManagedNativeHeader;

		internal static unsafe IMAGE_COR20_HEADER Read(BinaryReader reader)
		{
			IMAGE_COR20_HEADER value;
			var ptr = (byte*) &value;
			var size = Marshal.SizeOf(typeof(IMAGE_COR20_HEADER));
			for (var i = 0; i < size; ++i)
				*ptr++ = reader.ReadByte();
			return value;
		}
	}
	// ReSharper restore InconsistentNaming
}