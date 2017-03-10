using StaRTS.Main.Models.Battle.Replay;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1101 : $UnityType
	{
		public unsafe $UnityType1101()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 639768) = ldftn($Invoke0);
			*(data + 639796) = ldftn($Invoke1);
			*(data + 639824) = ldftn($Invoke2);
			*(data + 639852) = ldftn($Invoke3);
			*(data + 639880) = ldftn($Invoke4);
			*(data + 639908) = ldftn($Invoke5);
			*(data + 639936) = ldftn($Invoke6);
			*(data + 639964) = ldftn($Invoke7);
			*(data + 639992) = ldftn($Invoke8);
			*(data + 640020) = ldftn($Invoke9);
			*(data + 640048) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new HeroDeployedAction((UIntPtr)0);
		}
	}
}
