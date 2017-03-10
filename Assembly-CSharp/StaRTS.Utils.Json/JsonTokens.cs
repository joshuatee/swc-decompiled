using System;

namespace StaRTS.Utils.Json
{
	public enum JsonTokens
	{
		None,
		ObjectOpen,
		ObjectClose,
		ArrayOpen,
		ArrayClose,
		Colon,
		Comma,
		String,
		Number,
		WordFirst = 100,
		True = 100,
		False,
		Null
	}
}
