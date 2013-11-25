using System;
using System.IO;
using System.Diagnostics;

namespace NesCore
{
	public class NesRom
	{
		private readonly byte[] _bytes;

		public NesRom (byte[] bytes)
		{
			_bytes = bytes;
		}

		public byte[] Bytes {
			get {
				return _bytes;
			}
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
					fileStream.Read (prgBlocks [i], 0, 0x4000);
				}

				for (var i = 0; i < chrBlockCount; ++i) {
					chrBlocks [i] = new byte[8192];
					fileStream.Read (chrBlocks [i], 0, 8192);
				}

				//There may be a remaining 128 bytes of Title data. Not necessary to read.

				var sizeInBytesOfRomData = (prgBlockCount * 16384) + (chrBlockCount * 8192);
				fileStream.Position = 16;
				var romBytes = new byte[sizeInBytesOfRomData];
				var bytesRead = fileStream.Read (romBytes, 0, sizeInBytesOfRomData);
				if (bytesRead != sizeInBytesOfRomData)
					throw new FileLoadException ("", fileInfo.Name);

				return new NesRom (romBytes);
			}
		}
	}
}