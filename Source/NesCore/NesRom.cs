using System.IO;

namespace NesCore
{
	public class NesRom
	{
		private readonly FileInfo fileInfo;

		public NesRom (FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
		}
	}
}

