using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1959 : $UnityType
	{
		public unsafe $UnityType1959()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 788728) = ldftn($Invoke0);
			*(data + 788756) = ldftn($Invoke1);
			*(data + 788784) = ldftn($Invoke2);
			*(data + 788812) = ldftn($Invoke3);
			*(data + 788840) = ldftn($Invoke4);
			*(data + 788868) = ldftn($Invoke5);
			*(data + 788896) = ldftn($Invoke6);
			*(data + 788924) = ldftn($Invoke7);
			*(data + 788952) = ldftn($Invoke8);
			*(data + 788980) = ldftn($Invoke9);
			*(data + 789008) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new SkinnedShooterFacade((UIntPtr)0);
		}
	}
}
