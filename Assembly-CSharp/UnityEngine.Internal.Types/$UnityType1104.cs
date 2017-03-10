using StaRTS.Main.Models.Battle.Replay;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1104 : $UnityType
	{
		public unsafe $UnityType1104()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 640412) = ldftn($Invoke0);
			*(data + 640440) = ldftn($Invoke1);
			*(data + 640468) = ldftn($Invoke2);
			*(data + 640496) = ldftn($Invoke3);
			*(data + 640524) = ldftn($Invoke4);
			*(data + 640552) = ldftn($Invoke5);
			*(data + 640580) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new SquadTroopPlacedAction((UIntPtr)0);
		}
	}
}
