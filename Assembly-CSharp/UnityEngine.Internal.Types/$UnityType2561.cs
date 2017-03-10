using StaRTS.Utils.Diagnostics;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2561 : $UnityType
	{
		public unsafe $UnityType2561()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 995648) = ldftn($Invoke0);
			*(data + 995676) = ldftn($Invoke1);
			*(data + 995704) = ldftn($Invoke2);
			*(data + 995732) = ldftn($Invoke3);
			*(data + 995760) = ldftn($Invoke4);
			*(data + 995788) = ldftn($Invoke5);
			*(data + 995816) = ldftn($Invoke6);
			*(data + 995844) = ldftn($Invoke7);
			*(data + 995872) = ldftn($Invoke8);
			*(data + 995900) = ldftn($Invoke9);
			*(data + 995928) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new StaRTSLogger((UIntPtr)0);
		}
	}
}
