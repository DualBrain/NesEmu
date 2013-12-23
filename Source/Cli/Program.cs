using System;
using NesCore;

namespace Cli
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args == null || args.Length < 1 || string.IsNullOrWhiteSpace (args [0])) {
				Console.WriteLine ("First argument should be path to NES rom.");
				return;
			}

			var fileInfo = new System.IO.FileInfo (args [0]);
			if (!fileInfo.Exists) {
				Console.WriteLine ("File does not exist: {0}", fileInfo.FullName);
				return;
			}

			var rom = NesRom.Parse (fileInfo);
			var memory = new Memory (0x10000);
			var cpu = new CPU (memory);

			var nesEmulation = new NES (cpu, memory);

			nesEmulation.LoadRom (rom);
			nesEmulation.Reset ();

			nesEmulation.BeginEmulation ();

			Console.WriteLine ("Press any key to whatever.");
			Console.ReadKey ();

			nesEmulation.EndEmulation ();
		}
	}
}
