using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2358 : $UnityType
	{
		public unsafe $UnityType2358()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 933488) = ldftn($Invoke0);
			*(data + 933516) = ldftn($Invoke1);
			*(data + 933544) = ldftn($Invoke2);
			*(data + 933572) = ldftn($Invoke3);
			*(data + 933600) = ldftn($Invoke4);
			*(data + 933628) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new StarportInfoScreen((UIntPtr)0);
		}
	}
}
