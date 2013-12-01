using System;

namespace NesCore
{
	public class Memory
	{
		private const int ROMOFFSET = 0x8000;
		private readonly byte[] _bytes;
		private int _position;

		public Memory (int sizeInBytes)
		{
			_position = 0;
			_bytes = new byte[sizeInBytes];
		}

		public int Position {
			get {
				return _position;
			}
			set {
				_position = value;
			}
		}

		public void LoadRom (NesRom rom)
		{
			rom.CopyPRGBytesTo (_bytes, ROMOFFSET);
		}

		public UInt16 ReadUInt16 (int fromAddress)
		{
			_position += 2;

			var lowByte = (int)_bytes [fromAddress];
			var highByte = (int)_bytes [fromAddress + 1];
			var aNumber = (UInt16)((highByte << 8) | lowByte);

			return aNumber;
		}

		public byte ReadByte (int fromAddress)
		{
			return _bytes [fromAddress];
		}
	}
}

