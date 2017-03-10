using StaRTS.Main.Controllers.ServerMessages;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType898 : $UnityType
	{
		public unsafe $UnityType898()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 589088) = ldftn($Invoke0);
			*(data + 589116) = ldftn($Invoke1);
			*(data + 589144) = ldftn($Invoke2);
			*(data + 589172) = ldftn($Invoke3);
			*(data + 589200) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new AdminMessage((UIntPtr)0);
		}
	}
}
