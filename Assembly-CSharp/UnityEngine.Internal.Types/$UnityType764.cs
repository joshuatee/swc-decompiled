using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType764 : $UnityType
	{
		public unsafe $UnityType764()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 554256) = ldftn($Invoke0);
			*(data + 554284) = ldftn($Invoke1);
			*(data + 554312) = ldftn($Invoke2);
			*(data + 554340) = ldftn($Invoke3);
			*(data + 554368) = ldftn($Invoke4);
			*(data + 554396) = ldftn($Invoke5);
			*(data + 554424) = ldftn($Invoke6);
			*(data + 554452) = ldftn($Invoke7);
			*(data + 554480) = ldftn($Invoke8);
			*(data + 554508) = ldftn($Invoke9);
			*(data + 554536) = ldftn($Invoke10);
			*(data + 554564) = ldftn($Invoke11);
			*(data + 554592) = ldftn($Invoke12);
			*(data + 554620) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new SpatialIndexController((UIntPtr)0);
		}
	}
}
