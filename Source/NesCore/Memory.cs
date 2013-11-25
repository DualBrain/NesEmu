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

		public void LoadRom (byte[] bytes)
		{
			Buffer.BlockCopy (bytes, 0, _bytes, ROMOFFSET, bytes.Length);
		}

		public byte[] ReadTwoBytes ()
		{
			//TODO: change to ReadInt32. 6502 is little ending, don't forget to swap bytes!
			byte[] bytesRead = new byte[2];
			Buffer.BlockCopy (_bytes, _position, bytesRead, 0, 2);
			return bytesRead;
		}
	}
}

