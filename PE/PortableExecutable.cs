using System.IO;

namespace PE
{
	/// <summary>
	///     Entry point to parse PE images.
	/// </summary>
	public static class PortableExecutable
	{
		/// <summary>
		///     Parses the headers of the given PE image.
		///     Throws if the headers are malformed.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static PeHeader ReadHeader(string fileName)
		{
			using (var stream = File.OpenRead(fileName))
			{
				return ReadHeader(stream);
			}
		}

		/// <summary>
		///     Parses the headers of the given PE image.
		///     Throws if the headers are malformed.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="leaveOpen"></param>
		/// <returns></returns>
		public static PeHeader ReadHeader(Stream stream, bool leaveOpen = false)
		{
			return PeHeader.ReadFrom(stream, leaveOpen);
		}

		/// <summary>
		///     Parses the headers of the given PE image.
		///     Returns an error if the headers are malformed.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="header"></param>
		/// <param name="leaveOpen"></param>
		/// <returns></returns>
		public static ReaderError TryReadHeader(Stream stream, out PeHeader header, bool leaveOpen = false)
		{
			return PeHeader.TryReadFrom(stream, out header, leaveOpen);
		}
	}
}