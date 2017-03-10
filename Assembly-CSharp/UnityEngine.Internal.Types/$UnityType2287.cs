using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2287 : $UnityType
	{
		public unsafe $UnityType2287()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 911284) = ldftn($Invoke0);
			*(data + 911312) = ldftn($Invoke1);
			*(data + 911340) = ldftn($Invoke2);
			*(data + 911368) = ldftn($Invoke3);
			*(data + 911396) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new CancelScreen((UIntPtr)0);
		}
	}
}
