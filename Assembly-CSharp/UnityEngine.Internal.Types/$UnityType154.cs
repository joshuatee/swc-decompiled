using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType154 : $UnityType
	{
		public unsafe $UnityType154()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 386564) = ldftn($Invoke0);
			*(data + 386592) = ldftn($Invoke1);
			*(data + 386620) = ldftn($Invoke2);
			*(data + 386648) = ldftn($Invoke3);
			*(data + 386676) = ldftn($Invoke4);
			*(data + 386704) = ldftn($Invoke5);
			*(data + 386732) = ldftn($Invoke6);
			*(data + 386760) = ldftn($Invoke7);
			*(data + 386788) = ldftn($Invoke8);
			*(data + 386816) = ldftn($Invoke9);
			*(data + 386844) = ldftn($Invoke10);
			*(data + 386872) = ldftn($Invoke11);
			*(data + 386900) = ldftn($Invoke12);
			*(data + 386928) = ldftn($Invoke13);
			*(data + 386956) = ldftn($Invoke14);
			*(data + 386984) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new UIEventTrigger((UIntPtr)0);
		}
	}
}
