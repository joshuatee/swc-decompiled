using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2380 : $UnityType
	{
		public unsafe $UnityType2380()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 941160) = ldftn($Invoke0);
			*(data + 941188) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new UpdateClientScreen((UIntPtr)0);
		}
	}
}
