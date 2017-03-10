using StaRTS.Main.Models.Battle.Replay;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1099 : $UnityType
	{
		public unsafe $UnityType1099()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 639320) = ldftn($Invoke0);
			*(data + 639348) = ldftn($Invoke1);
			*(data + 639376) = ldftn($Invoke2);
			*(data + 639404) = ldftn($Invoke3);
			*(data + 639432) = ldftn($Invoke4);
			*(data + 639460) = ldftn($Invoke5);
			*(data + 639488) = ldftn($Invoke6);
			*(data + 639516) = ldftn($Invoke7);
			*(data + 639544) = ldftn($Invoke8);
			*(data + 639572) = ldftn($Invoke9);
			*(data + 639600) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new ChampionDeployedAction((UIntPtr)0);
		}
	}
}
