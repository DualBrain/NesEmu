using System;
using System.IO;
using System.Diagnostics;

namespace NesCore
{
	public class NesRom
	{
		private NesRom ()
		{
		}

		public static NesRom Parse (FileInfo fileInfo)
		{
			Console.WriteLine ("Parsing {0}", fileInfo.FullName);

			using (var fileStream = fileInfo.OpenRead ()) {

				var isValidNesHeader = true;
				isValidNesHeader = isValidNesHeader && fileStream.ReadByte () == 0x4E;	//N
				isValidNesHeader = isValidNesHeader && fileStream.ReadByte () == 0x45;	//E
				isValidNesHeader = isValidNesHeader && fileStream.ReadByte () == 0x53;	//S
				isValidNesHeader = isValidNesHeader && fileStream.ReadByte () == 0x1A;
				if (!isValidNesHeader) {
					throw new Exception (string.Format ("{0} does not have a valid NES header.", fileInfo.Name));
				}

				int prgBlockCount = (int)fileStream.ReadByte ();
				int chrBlockCount = (int)fileStream.ReadByte ();
				fileStream.Position += 10;

				Console.WriteLine ("PRG Block Count: {0}", prgBlockCount);
				Console.WriteLine ("CHR Block Count: {0}", chrBlockCount);

				var prgBlocks = new byte[prgBlockCount][];
				var chrBlocks = new byte[chrBlockCount][];

				for (var i = 0; i < prgBlockCount; ++i) {
					prgBlocks [i] = new byte[16384];
					fileStream.Read (prgBlocks [i], 0, 16384);
				}

				for (var i = 0; i < chrBlockCount; ++i) {
					chrBlocks [i] = new byte[8192];
					fileStream.Read (chrBlocks [i], 0, 8192);
				}

				return new NesRom ();
			}
		}
	}
}