using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace PE.Test
{
	[TestFixture]
	public sealed class PeHeaderReaderAcceptanceTest
	{
		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			string codeBase = Assembly.GetCallingAssembly().CodeBase;
			UriBuilder uri = new UriBuilder(codeBase);
			var assemblyDirectory = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
			var path = Path.Combine(assemblyDirectory, "..", "..", "PE.Test");
			Directory.SetCurrentDirectory(path);
		}

		[Test]
		public void TestParse()
		{
			var header = PeHeader.ReadFrom("Resources\\SharpRemote.dll");
			header.DosHeader.e_magic.Should().Be(0x5a4d);
			header.DosHeader.e_cblp.Should().Be(0x0090);
			header.DosHeader.e_cp.Should().Be(0x0003);
			header.DosHeader.e_crlc.Should().Be(0x0000);
			header.DosHeader.e_cparhdr.Should().Be(0x0004);
			header.DosHeader.e_minalloc.Should().Be(0x0000);
			header.DosHeader.e_maxalloc.Should().Be(0xFFFF);
			header.DosHeader.e_ss.Should().Be(0x0000);
			header.DosHeader.e_sp.Should().Be(0xB8);
			header.DosHeader.e_csum.Should().Be(0x0000);
			header.DosHeader.e_ip.Should().Be(0x0000);
			header.DosHeader.e_cs.Should().Be(0x0000);
			header.DosHeader.e_lfarlc.Should().Be(0x0040);
			header.DosHeader.e_ovno.Should().Be(0x0000);
			header.DosHeader.e_oemid.Should().Be(0x0000);
			header.DosHeader.e_oeminfo.Should().Be(0x0000);
			header.DosHeader.e_lfanew.Should().Be(0x00000080);

			header.FileHeader.Machine.Should().Be(MACHINE.IMAGE_FILE_MACHINE_I386);
			header.FileHeader.NumberOfSections.Should().Be(0x0003);
			header.FileHeader.TimeDateStamp.Should().Be(0x5978284b);
			header.FileHeader.PointerToSymbolTable.Should().Be(0x00000000);
			header.FileHeader.NumberOfSymbols.Should().Be(0x00000000);
			header.FileHeader.SizeOfOptionalHeader.Should().Be(0x00E0);
			header.FileHeader.Characteristics.Should().Be(CHARACTERISTICS.IMAGE_FILE_EXECUTABLE_IMAGE |
														  CHARACTERISTICS.IMAGE_FILE_DLL |
														  CHARACTERISTICS.IMAGE_FILE_LARGE_ADDRESS_AWARE);

			header.OptionalHeader32.Magic.Should().Be(0x010b);
			header.OptionalHeader32.MajorLinkerVersion.Should().Be(0x30);
			header.OptionalHeader32.MinorLinkerVersion.Should().Be(0x00);
			header.OptionalHeader32.SizeOfCode.Should().Be(0x0003c800);
			header.OptionalHeader32.SizeOfInitializedData.Should().Be(0x00000600);
			header.OptionalHeader32.SizeOfUninitializedData.Should().Be(0x00000000);
			header.OptionalHeader32.AddressOfEntryPoint.Should().Be(0x0003E4Fa);
			header.OptionalHeader32.BaseOfCode.Should().Be(0x00002000);
			header.OptionalHeader32.BaseOfData.Should().Be(0x00040000);
			header.OptionalHeader32.ImageBase.Should().Be(0x10000000);
			header.OptionalHeader32.SectionAlignment.Should().Be(0x00002000);
			header.OptionalHeader32.FileAlignment.Should().Be(0x00000200);
			header.OptionalHeader32.MajorOperatingSystemVersion.Should().Be(0x0004);
			header.OptionalHeader32.MinorOperatingSystemVersion.Should().Be(0x0000);
			header.OptionalHeader32.MajorImageVersion.Should().Be(0x0000);
			header.OptionalHeader32.MinorImageVersion.Should().Be(0x0000);
			header.OptionalHeader32.MajorSubsystemVersion.Should().Be(0x0006);
			header.OptionalHeader32.MinorSubsystemVersion.Should().Be(0x0000);
			header.OptionalHeader32.Win32VersionValue.Should().Be(0x00000000);
			header.OptionalHeader32.SizeOfImage.Should().Be(0x00044000);
			header.OptionalHeader32.CheckSum.Should().Be(0x00000000);
			header.OptionalHeader32.Subsystem.Should().Be(SUBSYSTEM.IMAGE_SUBSYSTEM_WINDOWS_CUI);
			header.OptionalHeader32.DllCharacteristics.Should().Be(0x8560);
			header.OptionalHeader32.SizeOfStackReserve.Should().Be(0x00100000);
			header.OptionalHeader32.SizeOfStackCommit.Should().Be(0x00001000);
			header.OptionalHeader32.SizeOfHeapReserve.Should().Be(0x00100000);
			header.OptionalHeader32.SizeOfHeapCommit.Should().Be(0x00001000);
			header.OptionalHeader32.LoaderFlags.Should().Be(0x00000000);
			header.OptionalHeader32.NumberOfRvaAndSizes.Should().Be(0x00000010);

			header.CliHeader.cb.Should().Be(0x00000048);
			header.CliHeader.MajorRuntimeVersion.Should().Be(0x0002);
			header.CliHeader.MinorRuntimeVersion.Should().Be(0x0005);
			header.CliHeader.MetaData.VirtualAddress.Should().Be(0x0001be48);
			header.CliHeader.MetaData.Size.Should().Be(0x00022528);
			header.CliHeader.Flags.Should().Be(COMIMAGE_FLAGS.COMIMAGE_FLAGS_ILONLY);
			header.CliHeader.EntryPointToken.Should().Be(0x00000000);
			header.CliHeader.Resources.VirtualAddress.Should().Be(0x00000000);
			header.CliHeader.Resources.Size.Should().Be(0x00000000);
			header.CliHeader.StrongNameSignature.VirtualAddress.Should().Be(0x00000000);
			header.CliHeader.StrongNameSignature.Size.Should().Be(0x00000000);
			header.CliHeader.CodeManagerTable.VirtualAddress.Should().Be(0x00000000);
			header.CliHeader.CodeManagerTable.Size.Should().Be(0x00000000);
			header.CliHeader.VTableFixups.VirtualAddress.Should().Be(0x00000000);
			header.CliHeader.VTableFixups.Size.Should().Be(0x00000000);
			header.CliHeader.ExportAddressTableJumps.VirtualAddress.Should().Be(0x00000000);
			header.CliHeader.ExportAddressTableJumps.Size.Should().Be(0x00000000);
			header.CliHeader.ManagedNativeHeader.VirtualAddress.Should().Be(0x00000000);
			header.CliHeader.ManagedNativeHeader.Size.Should().Be(0x00000000);
		}
	}
}