using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1983 : $UnityType
	{
		public unsafe $UnityType1983()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 834732) = ldftn($Invoke0);
			*(data + 834760) = ldftn($Invoke1);
			*(data + 834788) = ldftn($Invoke2);
			*(data + 834816) = ldftn($Invoke3);
			*(data + 834844) = ldftn($Invoke4);
			*(data + 834872) = ldftn($Invoke5);
			*(data + 834900) = ldftn($Invoke6);
			*(data + 834928) = ldftn($Invoke7);
			*(data + 834956) = ldftn($Invoke8);
			*(data + 834984) = ldftn($Invoke9);
			*(data + 835012) = ldftn($Invoke10);
			*(data + 835040) = ldftn($Invoke11);
			*(data + 835068) = ldftn($Invoke12);
			*(data + 835096) = ldftn($Invoke13);
			*(data + 835124) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new WarScheduleVO((UIntPtr)0);
		}
	}
}
