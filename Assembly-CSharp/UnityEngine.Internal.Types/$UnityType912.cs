using StaRTS.Main.Controllers.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType912 : $UnityType
	{
		public unsafe $UnityType912()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 593876) = ldftn($Invoke0);
			*(data + 593904) = ldftn($Invoke1);
			*(data + 593932) = ldftn($Invoke2);
			*(data + 593960) = ldftn($Invoke3);
			*(data + 593988) = ldftn($Invoke4);
			*(data + 594016) = ldftn($Invoke5);
			*(data + 594044) = ldftn($Invoke6);
			*(data + 594072) = ldftn($Invoke7);
			*(data + 594100) = ldftn($Invoke8);
			*(data + 594128) = ldftn($Invoke9);
			*(data + 594156) = ldftn($Invoke10);
			*(data + 594184) = ldftn($Invoke11);
			*(data + 594212) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new SquadMsgManager((UIntPtr)0);
		}
	}
}
