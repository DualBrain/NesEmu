using System;
using System.Threading;

namespace NesCore
{
	public class Emulator
	{
		private bool _isRunning = false;
		private readonly Thread _cpu6502Thread;

		public Emulator ()
		{
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


			}
		}
	}
}

