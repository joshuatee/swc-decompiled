using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType836 : $UnityType
	{
		public unsafe $UnityType836()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 574360) = ldftn($Invoke0);
			*(data + 574388) = ldftn($Invoke1);
			*(data + 574416) = ldftn($Invoke2);
			*(data + 574444) = ldftn($Invoke3);
			*(data + 574472) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new IntroCameraState((UIntPtr)0);
		}
	}
}
