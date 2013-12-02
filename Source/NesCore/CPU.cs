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
			UInt16 address;

			if (_isResetting) {
				_isResetting = false;
				_programCounter = _memory.ReadUInt16 (InitialExecutionAddress);
			}

			Byte opCode = _memory.ReadByte (_programCounter);
			_programCounter++;

			// opcodes and  http://6502.org/tutorials/6502opcodes.html#SEI
			switch (opCode) {
			case 0x78:	//SEI SEt Interrupt
				_cpuFlagSEI = true;
				break;
			case 0xD8:	//CLD CLear Decimal
				_cpuFlagCLD = true;
				break;
			case 0xA9:	//LDA LoaD Accumulator Immediate
				_accumulator = _memory.ReadByte (_programCounter);
				_programCounter++;
				break;
			case 0x8D:	//STA STore Accumulator
				address = _memory.ReadUInt16 (_programCounter);
				_programCounter += 2;
				_memory.WriteByteToAddress (_accumulator, address);
				break;
			case 0xA2:	//LDX LoaD X register
				_registerX = _memory.ReadByte (_programCounter);
				_programCounter++;
				break;
			case 0x9A:	//TXS Transfer X to Stack pointer
				_stackPointer = _registerX;
				break;
			case 0xAD:	//LDA LoaD Accumulator Absolute
				address = _memory.ReadUInt16 (_programCounter);
				_programCounter += 2;
				_accumulator = _memory.ReadByte (address);
				break;
			case 0x10:	//BPL Branch on result PLus
				//TODO: Pick up here...
				break;
			case 0x20:	//JSR Jump to SubRoutine
				_stackPointer--;
				_memory.WriteUInt16ToAddress ((UInt16)(_programCounter + 3), _stackPointer);
				_programCounter = _memory.ReadUInt16 (_programCounter);
				break;
			default:
				throw new NotImplementedException (string.Format ("OpCode not implemented: {0:X2}", opCode));
			}
		}
	}
}

