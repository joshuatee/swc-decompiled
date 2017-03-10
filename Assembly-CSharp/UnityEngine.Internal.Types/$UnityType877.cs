using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType877 : $UnityType
	{
		public unsafe $UnityType877()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 582396) = ldftn($Invoke0);
			*(data + 582424) = ldftn($Invoke1);
			*(data + 582452) = ldftn($Invoke2);
			*(data + 582480) = ldftn($Invoke3);
			*(data + 582508) = ldftn($Invoke4);
			*(data + 582536) = ldftn($Invoke5);
			*(data + 582564) = ldftn($Invoke6);
			*(data + 582592) = ldftn($Invoke7);
			*(data + 582620) = ldftn($Invoke8);
			*(data + 582648) = ldftn($Invoke9);
			*(data + 582676) = ldftn($Invoke10);
			*(data + 582704) = ldftn($Invoke11);
			*(data + 582732) = ldftn($Invoke12);
			*(data + 582760) = ldftn($Invoke13);
			*(data + 582788) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new ObjectiveManager((UIntPtr)0);
		}
	}
}
