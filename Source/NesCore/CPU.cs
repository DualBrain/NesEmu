using System;

namespace NesCore
{
	public class CPU
	{
		private const int InitialExecutionAddress = 0xFFFC;
		private UInt16 _programCounter;
		private UInt16 _stackPointer;
		private byte _accumulator;
		private byte _registerX;
		private bool _cpuFlagN = false;
		private readonly Memory _memory;
		private bool _cpuFlagSEI = false;
		private bool _cpuFlagCLD = false;
		private bool _isResetting = true;

		public CPU (Memory memory)
		{
			_memory = memory;
		}

		private byte Accumulator {
			get {
				return _accumulator;
			}
			set {
				_accumulator = value;
				_cpuFlagN = (value & (1 << 7)) == 0;
			}
		}

		public void OneCpuCycle ()
		{
			UInt16 address;

			if (_isResetting) {
				_isResetting = false;
				_programCounter = _memory.ReadUInt16 (InitialExecutionAddress);
			}

			var opCodeByte = _memory.ReadByte (_programCounter);
			var opCode = (OpCodes)opCodeByte;

			// opcodes and  http://6502.org/tutorials/6502opcodes.html#SEI
			Console.WriteLine (string.Format ("OpCode: {0}", opCode));

			switch (opCode) {
			case OpCodes.SEI:
				_cpuFlagSEI = true;
				_programCounter++;
				break;

			case OpCodes.CLD:
				_cpuFlagCLD = true;
				_programCounter++;
				break;

			case OpCodes.LDA_Immediate:
				Accumulator = _memory.ReadByte (_programCounter + 1);
				_programCounter += 2;
				break;

			case OpCodes.STA:
				address = _memory.ReadUInt16 (_programCounter + 1);
				_programCounter += 3;
				_memory.WriteByteToAddress (_accumulator, address);
				break;

			case OpCodes.LDX:
				_registerX = _memory.ReadByte (_programCounter + 1);
				_programCounter += 2;
				break;

			case OpCodes.TXS:
				_stackPointer = _registerX;
				_programCounter++;
				break;

			case OpCodes.LDA_Absolute:
				address = _memory.ReadUInt16 (_programCounter + 1);
				_programCounter += 3;
				Accumulator = _memory.ReadByte (address);
				break;

			case OpCodes.BPL:
				if (_cpuFlagN) {
					var bplByte = _memory.ReadByte (_programCounter + 1);
					var bplJumpNumberOfBytes = (bplByte < 128) ? bplByte : 255 - bplByte;	//2's complement conversion
					_programCounter =
						(UInt16)(_programCounter + bplJumpNumberOfBytes);
				} else {
					_programCounter += 2;
				}
				break;

			case OpCodes.JSR:
				_stackPointer--;
				_memory.WriteUInt16ToAddress ((UInt16)(_programCounter + 4), _stackPointer);
				_programCounter = _memory.ReadUInt16 (_programCounter + 1);
				break;

			default:
				throw new NotImplementedException (string.Format ("OpCode not implemented: {0:X2}", opCode));
			}
		}
	}
}

