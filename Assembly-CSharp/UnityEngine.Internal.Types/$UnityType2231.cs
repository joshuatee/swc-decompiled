using StaRTS.Main.Views.UX.Controls;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2231 : $UnityType
	{
		public unsafe $UnityType2231()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 888380) = ldftn($Invoke0);
			*(data + 888408) = ldftn($Invoke1);
			*(data + 888436) = ldftn($Invoke2);
			*(data + 888464) = ldftn($Invoke3);
			*(data + 888492) = ldftn($Invoke4);
			*(data + 888520) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new CountdownControl((UIntPtr)0);
		}
	}
}
