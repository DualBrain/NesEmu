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
			var cpu = new CPU ();

			var nesEmulation = new NES (rom, cpu);
		}
	}
}
