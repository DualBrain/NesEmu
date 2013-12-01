using System;

namespace NesCore
{
	public class CPU
	{
		private const int InitialExecutionAddress = 0xFFFC;
		private int _programCounter;
		private byte _accumulator;
		private readonly Memory _memory;
		private bool _cpuFlagSEI = false;
		private bool _cpuFlagCLD = false;
		private bool _isResetting = true;

		public CPU (Memory memory)
		{
			_memory = memory;
		}

		public void OneCpuCycle ()
		{
			if (_isResetting) {
				_isResetting = false;
				_programCounter = _memory.ReadUInt16 (InitialExecutionAddress);
			}

			Byte opCode = _memory.ReadByte (_programCounter);
			_programCounter++;

			// opcodes and  http://6502.org/tutorials/6502opcodes.html#SEI
			switch (opCode) {
			case 0x78:	//SEI (SEt Interrupt)
				_cpuFlagSEI = true;
				break;
			case 0xD8:	//CLD (CLear Decimal)
				_cpuFlagCLD = true;
				break;
			case 0xA9:	//LDA (LoaD Accumulator)
				_accumulator = _memory.ReadByte (_programCounter);
				_programCounter++;
				break;
			case 0x8D:	//STA (STore Accumulator)
				var addressToStoreAccumulator = _memory.ReadUInt16 (_programCounter);
				_programCounter += 2;
				_memory.WriteByteToAddress (_accumulator, addressToStoreAccumulator);
				break;
			case 0xA2:	//LDX (LoaD X register)
				break;
			default:
				throw new NotImplementedException (string.Format ("OpCode not implemented: {0:X2}", opCode));
			}
		}
	}
}

