using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2270 : $UnityType
	{
		public unsafe $UnityType2270()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 904844) = ldftn($Invoke0);
			*(data + 904872) = ldftn($Invoke1);
			*(data + 904900) = ldftn($Invoke2);
			*(data + 904928) = ldftn($Invoke3);
			*(data + 904956) = ldftn($Invoke4);
			*(data + 904984) = ldftn($Invoke5);
			*(data + 905012) = ldftn($Invoke6);
			*(data + 905040) = ldftn($Invoke7);
			*(data + 905068) = ldftn($Invoke8);
			*(data + 905096) = ldftn($Invoke9);
			*(data + 905124) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new UXTextureComponent((UIntPtr)0);
		}
	}
}
