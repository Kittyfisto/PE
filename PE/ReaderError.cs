namespace PE
{
	/// <summary>
	/// 
	/// </summary>
	public enum ReaderError
	{
		NoError = 0,

		/// <summary>
		/// The given PE image does not even contain enough data to contain the full DOS header.
		/// </summary>
		TooSmallForDosHeader,

		/// <summary>
		/// The magic value of the dos header is not 0x4d, 0x5a
		/// </summary>
		DosHeaderMagicMismatch,

		/// <summary>
		/// 
		/// </summary>
		NewExeHeaderAddressOutOfRange,

		/// <summary>
		/// The given PE image does not even contain enough data to contain the full file header.
		/// </summary>
		TooSmallForFileHeader = 2,

		/// <summary>
		/// The given PE image does not contain enough data to contain the optional header.
		/// </summary>
		TooSmallForOptionalHeader = 3,

		/// <summary>
		/// The magic value of the optional header does not equal 0x10b, nor 0x20b.
		/// </summary>
		OptionalHeaderMagicMismatch = 4,


		UnhandledException = int.MaxValue
	}
}