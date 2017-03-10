using StaRTS.Main.Models.Commands.Player.Fue;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1514 : $UnityType
	{
		public unsafe $UnityType1514()
		{
			*(UnityEngine.Internal.$Metadata.data + 658052) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new PlayerFueCompleteCommand((UIntPtr)0);
		}
	}
}
