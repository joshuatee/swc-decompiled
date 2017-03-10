using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1728 : $UnityType
	{
		public unsafe $UnityType1728()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 673032) = ldftn($Invoke0);
			*(data + 673060) = ldftn($Invoke1);
			*(data + 673088) = ldftn($Invoke2);
			*(data + 673116) = ldftn($Invoke3);
			*(data + 673144) = ldftn($Invoke4);
			*(data + 673172) = ldftn($Invoke5);
			*(data + 673200) = ldftn($Invoke6);
			*(data + 673228) = ldftn($Invoke7);
			*(data + 673256) = ldftn($Invoke8);
			*(data + 673284) = ldftn($Invoke9);
			*(data + 673312) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new StateComponent((UIntPtr)0);
		}
	}
}
