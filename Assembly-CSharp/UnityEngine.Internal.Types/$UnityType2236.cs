using StaRTS.Main.Views.UX.Controls;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2236 : $UnityType
	{
		public unsafe $UnityType2236()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 890452) = ldftn($Invoke0);
			*(data + 890480) = ldftn($Invoke1);
			*(data + 890508) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new JewelControl((UIntPtr)0);
		}
	}
}
