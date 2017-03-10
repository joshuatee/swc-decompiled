using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1448 : $UnityType
	{
		public unsafe $UnityType1448()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 654076) = ldftn($Invoke0);
			*(data + 654104) = ldftn($Invoke1);
			*(data + 654132) = ldftn($Invoke2);
			*(data + 654160) = ldftn($Invoke3);
			*(data + 654188) = ldftn($Invoke4);
			*(data + 654216) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new LoginRequest((UIntPtr)0);
		}
	}
}
