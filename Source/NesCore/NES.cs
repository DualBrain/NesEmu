using System;
using System.Threading;

namespace NesCore
{
	public class NES
	{
		private bool _isRunning = false;
		private readonly Thread _cpu6502Thread;
		private readonly NesRom _rom;
		private readonly CPU _cpu;

		public NES (NesRom rom, CPU cpu)
		{
			_cpu = cpu;
			_rom = rom;
			_cpu6502Thread = new Thread (Emulate);
		}

		public void BeginEmulation ()
		{
			_isRunning = true;
			_cpu6502Thread.Start ();
		}

		public void EndEmulation ()
		{
			_isRunning = false;
			_cpu6502Thread.Join (TimeSpan.FromSeconds (1));
		}

		private void Emulate ()
		{
			while (_isRunning) {

				//TODO: delay to simulate slower cpu?
			}
		}
	}
}

