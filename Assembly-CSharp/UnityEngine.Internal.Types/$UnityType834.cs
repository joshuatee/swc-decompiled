using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType834 : $UnityType
	{
		public unsafe $UnityType834()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 574108) = ldftn($Invoke0);
			*(data + 574136) = ldftn($Invoke1);
			*(data + 574164) = ldftn($Invoke2);
			*(data + 574192) = ldftn($Invoke3);
			*(data + 574220) = ldftn($Invoke4);
			*(data + 574248) = ldftn($Invoke5);
			*(data + 574276) = ldftn($Invoke6);
			*(data + 574304) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new HomeState((UIntPtr)0);
		}
	}
}
