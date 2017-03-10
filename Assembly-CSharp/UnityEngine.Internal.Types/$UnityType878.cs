using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType878 : $UnityType
	{
		public unsafe $UnityType878()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 582816) = ldftn($Invoke0);
			*(data + 582844) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new ReceiveSquadUnitObjectiveProcessor((UIntPtr)0);
		}
	}
}
