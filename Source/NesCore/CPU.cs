using System;

namespace NesCore
{
	public class CPU
	{
		private int _executionPointer = 0xFFFC;
		private readonly Memory _memory;

		public CPU (Memory memory)
		{
			_memory = memory;
		}

		public void OneCpuCycle ()
		{
			var bytes = _memory.ReadTwoBytes ();

		}
	}
}

