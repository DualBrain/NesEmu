using System;
using System.IO;

namespace NesCore
{
	public class NesRom
	{
		private readonly byte[] _bytes;
		private readonly int _prgStartPosition;
		private readonly int _prgTotalSizeInBytes;
		private readonly int _chrStartPosition;
		private readonly int _chrTotalSizeInBytes;

		public NesRom (byte[] bytes, int prgStartPosition, int prgTotalSizeInBytes, int chrStartPosition, int chrTotalSizeInBytes)
		{
			_bytes = bytes;
			_chrTotalSizeInBytes = chrTotalSizeInBytes;
			_chrStartPosition = chrStartPosition;
			_prgTotalSizeInBytes = prgTotalSizeInBytes;
			_prgStartPosition = prgStartPosition;
		}

		public byte[] AllBytes {
			get {
				return _bytes;
			}
		}

		public void CopyPRGBytesTo (byte[] destinationBytes, int destinationOffset)
		{
			Buffer.BlockCopy (_bytes, _prgStartPosition, destinationBytes, destinationOffset, _prgTotalSizeInBytes);
		}

		public void CopyCHRBytesTo (byte[] destinationBytes, int destinationOffset)
		{
			Buffer.BlockCopy (_bytes, _chrStartPosition, destinationBytes, destinationOffset, _chrTotalSizeInBytes);
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

				var prgStartPosition = (int)fileStream.Position - 16;
				for (var i = 0; i < prgBlockCount; ++i) {
					prgBlocks [i] = new byte[16384];
					fileStream.Read (prgBlocks [i], 0, 0x4000);
				}

				var chrStartPosition = (int)fileStream.Position - 16;
				for (var i = 0; i < chrBlockCount; ++i) {
					chrBlocks [i] = new byte[8192];
					fileStream.Read (chrBlocks [i], 0, 8192);
				}

				//There may be a remaining 128 bytes of Title data. Not necessary to read.
				var prgTotalSizeInBytes = prgBlockCount * 0x4000;
				var chrTotalSizeInBytes = chrBlockCount * 0x2000;
				var sizeInBytesOfRomData = prgTotalSizeInBytes + chrTotalSizeInBytes;
				fileStream.Position = 16;
				var romBytes = new byte[sizeInBytesOfRomData];
				var bytesRead = fileStream.Read (romBytes, 0, sizeInBytesOfRomData);
				if (bytesRead != sizeInBytesOfRomData)
					throw new FileLoadException ("", fileInfo.Name);

				return new NesRom (romBytes, prgStartPosition, prgTotalSizeInBytes, chrStartPosition, chrTotalSizeInBytes);
			}
		}
	}
}