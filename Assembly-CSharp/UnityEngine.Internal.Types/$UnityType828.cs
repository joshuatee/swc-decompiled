using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType828 : $UnityType
	{
		public unsafe $UnityType828()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 573324) = ldftn($Invoke0);
			*(data + 573352) = ldftn($Invoke1);
			*(data + 573380) = ldftn($Invoke2);
			*(data + 573408) = ldftn($Invoke3);
			*(data + 573436) = ldftn($Invoke4);
			*(data + 573464) = ldftn($Invoke5);
			*(data + 573492) = ldftn($Invoke6);
			*(data + 573520) = ldftn($Invoke7);
			*(data + 573548) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new BattlePlaybackState((UIntPtr)0);
		}
	}
}
