using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace PE.Test
{
	[TestFixture]
	public sealed class PeHeaderReaderTest
	{
		public static IEnumerable<int> Sizes => Enumerable.Range(0, 311);

		[Test]
		[Description("Verifies that TryReadFrom doesn't throw when its given too small streams")]
		public void TestTryReadFrom1([ValueSource(nameof(Sizes))] int headerSize)
		{
			var header = new byte[headerSize];
			if (header.Length >= 1)
				header[0] = 0x4d;
			if (header.Length >= 2)
				header[1] = 0x5a;
			if (header.Length >= 61)
				header[60] = 64;
			if (header.Length >= 89)
				header[88] = 0x0b;
			if (header.Length >= 90)
				header[89] = 0x01;

			using (var stream = new MemoryStream(header))
			{
				PeHeader reader = null;
				ReaderError error = ReaderError.NoError;
				new Action(() => error = PeHeader.TryReadFrom(stream, out reader, catchAllExceptions: false)).ShouldNotThrow();
				reader.Should().BeNull();
				error.Should().NotBe(ReaderError.NoError);
			}
		}
	}
}