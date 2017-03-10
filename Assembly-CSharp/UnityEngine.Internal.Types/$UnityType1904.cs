using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1904 : $UnityType
	{
		public unsafe $UnityType1904()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 733120) = ldftn($Invoke0);
			*(data + 733148) = ldftn($Invoke1);
			*(data + 733176) = ldftn($Invoke2);
			*(data + 733204) = ldftn($Invoke3);
			*(data + 733232) = ldftn($Invoke4);
			*(data + 733260) = ldftn($Invoke5);
			*(data + 733288) = ldftn($Invoke6);
			*(data + 733316) = ldftn($Invoke7);
			*(data + 733344) = ldftn($Invoke8);
			*(data + 733372) = ldftn($Invoke9);
			*(data + 733400) = ldftn($Invoke10);
			*(data + 733428) = ldftn($Invoke11);
			*(data + 733456) = ldftn($Invoke12);
			*(data + 733484) = ldftn($Invoke13);
			*(data + 733512) = ldftn($Invoke14);
			*(data + 733540) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new ConditionVO((UIntPtr)0);
		}
	}
}
