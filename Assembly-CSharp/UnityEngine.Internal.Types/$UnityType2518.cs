using StaRTS.Main.Views.World;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2518 : $UnityType
	{
		public unsafe $UnityType2518()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 985288) = ldftn($Invoke0);
			*(data + 985316) = ldftn($Invoke1);
			*(data + 985344) = ldftn($Invoke2);
			*(data + 985372) = ldftn($Invoke3);
			*(data + 985400) = ldftn($Invoke4);
			*(data + 985428) = ldftn($Invoke5);
			*(data + 985456) = ldftn($Invoke6);
			*(data + 985484) = ldftn($Invoke7);
			*(data + 985512) = ldftn($Invoke8);
			*(data + 985540) = ldftn($Invoke9);
			*(data + 985568) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new WallConnector((UIntPtr)0);
		}
	}
}
