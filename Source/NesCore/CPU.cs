using System;

namespace NesCore
{
	public class CPU
	{
		private const int InitialExecutionAddress = 0xFFFC;
		private UInt16 _programCounter;
		private UInt16 _stackPointer;
		private byte _registerX;
		private byte _accumulator;
		private bool _cpuFlagN = false;
		private readonly Memory _memory;
		private bool _cpuFlagSEI = false;
		private bool _cpuFlagCLD = false;

		public CPU (Memory memory)
		{
			_memory = memory;
		}

		private void SetCpuFlagN (byte value)
		{
			_cpuFlagN = value >= 128;
		}

		private void SetCpuFlagN (UInt16 value)
		{
			_cpuFlagN = value >= 32768;
		}

		public void Reset ()
		{
			_registerX = 0;
			_cpuFlagN = false;
			_cpuFlagSEI = false;
			_cpuFlagCLD = false;
			_accumulator = 0;
			_stackPointer = 0; //TODO: Determine where the stack pointer is initialized to.
			_programCounter = _memory.ReadUInt16 (InitialExecutionAddress);
		}

		public void OneCpuCycle ()
		{
			int programCounterOfInstruction = _programCounter;
			bool is16BitValue = false;
			bool is8BitValue = false;
			bool isValueLess = false;
			UInt16 valueUshort = UInt16.MinValue;
			byte valueByte = byte.MinValue;

			var opCodeByte = _memory.ReadByte (_programCounter);
			var opCode = (OpCodes)opCodeByte;

			switch (opCode) {
			case OpCodes.SEI:
				isValueLess = true;
				_cpuFlagSEI = true;
				_programCounter++;
				break;

			case OpCodes.CLD:
				isValueLess = true;
				_cpuFlagCLD = true;
				_programCounter++;
				break;

			case OpCodes.LDA_I:
				is8BitValue = true;
				valueByte = _memory.ReadByte (_programCounter + 1);
				_accumulator = valueByte;
				SetCpuFlagN (_accumulator);
				_programCounter += 2;
				break;

			case OpCodes.STA:
				is16BitValue = true;
				valueUshort = _memory.ReadUInt16 (_programCounter + 1);
				_memory.WriteByteToAddress (_accumulator, valueUshort);
				SetCpuFlagN (valueUshort);
				_programCounter += 3;
				break;

			case OpCodes.LDX:
				is8BitValue = true;
				valueByte = _memory.ReadByte (_programCounter + 1);
				_registerX = valueByte;
				SetCpuFlagN (_registerX);
				_programCounter += 2;
				break;

			case OpCodes.TXS:
				isValueLess = true;
				_stackPointer = _registerX;
				_programCounter++;
				break;

			case OpCodes.LDA_A:
				is16BitValue = true;
				valueUshort = _memory.ReadUInt16 (_programCounter + 1);
				_accumulator = _memory.ReadByte (valueUshort);
				SetCpuFlagN (_accumulator);
				_programCounter += 3;
				break;

			case OpCodes.BPL:
				is8BitValue = true;
				valueByte = _memory.ReadByte (_programCounter + 1);
				if (!_cpuFlagN) {
					var bplByte = valueByte;
					var bplJumpNumberOfBytes = (bplByte < 128) ? bplByte : bplByte - 254;	//2's complement conversion
					_programCounter = (UInt16)(_programCounter + bplJumpNumberOfBytes);
				} else {
					_programCounter += 2;
				}
				break;

			case OpCodes.JSR:
				is16BitValue = true;
				_stackPointer--;
				_memory.WriteUInt16ToAddress ((UInt16)(_programCounter + 4), _stackPointer);
				valueUshort = _memory.ReadUInt16 (_programCounter + 1);
				SetCpuFlagN (valueUshort);
				_programCounter = valueUshort;
				break;

			default:
				throw new NotImplementedException (string.Format ("Not Implemented: OpCode 0x{0:X2}({0:D}) at {1:X2}", (byte)opCode, programCounterOfInstruction));
			}

			if (isValueLess)
				LogCpuState (programCounterOfInstruction, opCode, _registerX, _accumulator, _cpuFlagN, _cpuFlagSEI, _cpuFlagCLD);
			else if (is16BitValue)
				LogCpuState (programCounterOfInstruction, opCode, valueUshort, _registerX, _accumulator, _cpuFlagN, _cpuFlagSEI, _cpuFlagCLD);
			else if (is8BitValue)
				LogCpuState (programCounterOfInstruction, opCode, valueByte, _registerX, _accumulator, _cpuFlagN, _cpuFlagSEI, _cpuFlagCLD);
		}

		private static void LogCpuState (int programCounterOfInstruction, OpCodes opCode, byte value, byte registerX, byte accumulator, bool cpuFlagN, bool cpuFlagSEI, bool cpuFlagCLD)
		{
			Console.WriteLine (
				"{0:x2}:{1}\t{2:x2}\tAcc:{4:x2} X:{3:x2} N:{5} SEI:{6} CLD:{7}",
				programCounterOfInstruction,
				opCode,
				value,
				registerX,
				accumulator,
				Convert.ToInt32 (cpuFlagN),
				Convert.ToInt32 (cpuFlagSEI),
				Convert.ToInt32 (cpuFlagCLD));
		}

		private static void LogCpuState (int programCounterOfInstruction, OpCodes opCode, UInt16 value, byte registerX, byte accumulator, bool cpuFlagN, bool cpuFlagSEI, bool cpuFlagCLD)
		{
			Console.WriteLine (
				"{0:x2}:{1}\t{2:x2}\tAcc:{4:x2} X:{3:x2} N:{5} SEI:{6} CLD:{7}",
				programCounterOfInstruction,
				opCode,
				value,
				registerX,
				accumulator,
				Convert.ToInt32 (cpuFlagN),
				Convert.ToInt32 (cpuFlagSEI),
				Convert.ToInt32 (cpuFlagCLD));
		}

		private static void LogCpuState (int programCounterOfInstruction, OpCodes opCode, byte registerX, byte accumulator, bool cpuFlagN, bool cpuFlagSEI, bool cpuFlagCLD)
		{
			Console.WriteLine (
				"{0:x2}:{1}\t\tAcc:{3:x2} X:{2:x2} N:{4} SEI:{5} CLD:{6}",
				programCounterOfInstruction,
				opCode,
				registerX,
				accumulator,
				Convert.ToInt32 (cpuFlagN),
				Convert.ToInt32 (cpuFlagSEI),
				Convert.ToInt32 (cpuFlagCLD));
		}
	}
}
