using System;

namespace StaRTS.Main.Views.UserInput
{
	public enum UserInputLayer
	{
		InternalNone,
		InternalLowest,
		Ground = 1,
		World,
		UX,
		Screen,
		InternalHighest = 4
	}
}
