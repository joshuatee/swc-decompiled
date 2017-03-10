using StaRTS.Main.Models.Commands.Holonet;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1404 : $UnityType
	{
		public unsafe $UnityType1404()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 652452) = ldftn($Invoke0);
			*(data + 652480) = ldftn($Invoke1);
			*(data + 652508) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new HolonetGetMessagesResponse((UIntPtr)0);
		}
	}
}
