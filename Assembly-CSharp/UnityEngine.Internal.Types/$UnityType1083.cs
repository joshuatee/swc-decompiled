using StaRTS.Main.Models.Battle;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1083 : $UnityType
	{
		public unsafe $UnityType1083()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 631536) = ldftn($Invoke0);
			*(data + 631564) = ldftn($Invoke1);
			*(data + 631592) = ldftn($Invoke2);
			*(data + 631620) = ldftn($Invoke3);
			*(data + 631648) = ldftn($Invoke4);
			*(data + 631676) = ldftn($Invoke5);
			*(data + 631704) = ldftn($Invoke6);
			*(data + 631732) = ldftn($Invoke7);
			*(data + 631760) = ldftn($Invoke8);
			*(data + 631788) = ldftn($Invoke9);
			*(data + 631816) = ldftn($Invoke10);
			*(data + 631844) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new BeamTarget((UIntPtr)0);
		}
	}
}
