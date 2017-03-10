using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType875 : $UnityType
	{
		public unsafe $UnityType875()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 582004) = ldftn($Invoke0);
			*(data + 582032) = ldftn($Invoke1);
			*(data + 582060) = ldftn($Invoke2);
			*(data + 582088) = ldftn($Invoke3);
			*(data + 582116) = ldftn($Invoke4);
			*(data + 582144) = ldftn($Invoke5);
			*(data + 582172) = ldftn($Invoke6);
			*(data + 582200) = ldftn($Invoke7);
			*(data + 582228) = ldftn($Invoke8);
			*(data + 582256) = ldftn($Invoke9);
			*(data + 582284) = ldftn($Invoke10);
			*(data + 582312) = ldftn($Invoke11);
			*(data + 582340) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new ObjectiveController((UIntPtr)0);
		}
	}
}
