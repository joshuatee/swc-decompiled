using StaRTS.Main.Controllers.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType910 : $UnityType
	{
		public unsafe $UnityType910()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 592364) = ldftn($Invoke0);
			*(data + 592392) = ldftn($Invoke1);
			*(data + 592420) = ldftn($Invoke2);
			*(data + 592448) = ldftn($Invoke3);
			*(data + 592476) = ldftn($Invoke4);
			*(data + 592504) = ldftn($Invoke5);
			*(data + 592532) = ldftn($Invoke6);
			*(data + 592560) = ldftn($Invoke7);
			*(data + 592588) = ldftn($Invoke8);
			*(data + 592616) = ldftn($Invoke9);
			*(data + 592644) = ldftn($Invoke10);
			*(data + 592672) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new ChatServerAdapter((UIntPtr)0);
		}
	}
}
