using StaRTS.Main.Views.UX.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2469 : $UnityType
	{
		public unsafe $UnityType2469()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 969804) = ldftn($Invoke0);
			*(data + 969832) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new SquadWarBoardBuilding((UIntPtr)0);
		}
	}
}
