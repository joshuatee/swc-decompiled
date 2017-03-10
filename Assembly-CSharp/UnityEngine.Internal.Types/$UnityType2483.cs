using StaRTS.Main.Views.UserInput;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2483 : $UnityType
	{
		public unsafe $UnityType2483()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 974928) = ldftn($Invoke0);
			*(data + 974956) = ldftn($Invoke1);
			*(data + 974984) = ldftn($Invoke2);
			*(data + 975012) = ldftn($Invoke3);
			*(data + 975040) = ldftn($Invoke4);
			*(data + 975068) = ldftn($Invoke5);
			*(data + 975096) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new BackButtonManager((UIntPtr)0);
		}
	}
}
