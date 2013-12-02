using System;

namespace NesCore
{
	public class Memory
	{
		private const int ROMOFFSET = 0x8000;
		private readonly byte[] _bytes;

		public Memory (int sizeInBytes)
		{
			_bytes = new byte[sizeInBytes];
		}

		public void LoadRom (NesRom rom)
		{
			rom.CopyPRGBytesTo (_bytes, ROMOFFSET);
		}

		public UInt16 ReadUInt16 (int fromAddress)
		{
			var lowByte = (int)_bytes [fromAddress];
			var highByte = (int)_bytes [fromAddress + 1];
			var aNumber = (UInt16)((highByte << 8) | lowByte);

			return aNumber;
		}

		public byte ReadByte (int fromAddress)
		{
			return _bytes [fromAddress];
		}

		public void WriteByteToAddress (byte byteToWrite, int addressToWrite)
		{
			_bytes [addressToWrite] = byteToWrite;
		}

		public void WriteUInt16ToAddress (UInt16 valueToWrite, int addressToWrite)
		{
			var lowByte = (byte)valueToWrite;
			var highByte = (byte)(valueToWrite >> 8);
			_bytes [addressToWrite] = lowByte;
			_bytes [addressToWrite + 1] = highByte;
		}
	}
}

