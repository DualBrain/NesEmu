using System;
using System.Threading;

namespace NesCore
{
	public class NES
	{
		private bool _isRunning = false;
		private readonly Thread _cpu6502Thread;
		private NesRom _rom;
		private readonly CPU _cpu;
		private readonly Memory _memory;

		public NES (CPU cpu, Memory memory)
		{
			_memory = memory;
			_cpu = cpu;
			_cpu6502Thread = new Thread (Emulate);
		}

		public void Reset ()
		{
			_memory.SetByteAtAddress (0x2002, 0x80);	//HACK: Until there's a PPU, force this value.
			_cpu.Reset ();
		}

		public void LoadRom (NesRom rom)
		{
			_rom = rom;
			_memory.LoadRom (_rom);
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

				_cpu.OneCpuCycle ();
			}
		}
	}
}

