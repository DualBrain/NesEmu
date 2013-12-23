using System;

namespace NesCore
{
	public enum OpCodes : byte
	{
		/// <summary>
		/// SEt Interrupt
		/// </summary>
		SEI = 0x78,
		/// <summary>
		/// CLear Decimal
		/// </summary>
		CLD = 0xD8,
		/// <summary>
		/// LoaD Accumulator Immediate
		/// </summary>
		LDA_I = 0xA9,
		/// <summary>
		/// STore Accumulator
		/// </summary>
		STA = 0x8D,
		/// <summary>
		/// LoaD X register
		/// </summary>
		LDX = 0xA2,
		/// <summary>
		/// Transfer X to Stack pointer
		/// </summary>
		TXS = 0x9A,
		/// <summary>
		/// LoaD Accumulator Absolute
		/// </summary>
		LDA_A = 0xAD,
		/// <summary>
		/// Branch on result PLus
		/// </summary>
		BPL = 0x10,
		/// <summary>
		/// Jump to SubRoutine
		/// </summary>
		JSR = 0x20,
	}
}
