using StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1440 : $UnityType
	{
		public unsafe $UnityType1440()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 653656) = ldftn($Invoke0);
			*(data + 653684) = ldftn($Invoke1);
			*(data + 653712) = ldftn($Invoke2);
			*(data + 653740) = ldftn($Invoke3);
			*(data + 653768) = ldftn($Invoke4);
			*(data + 653796) = ldftn($Invoke5);
			*(data + 653824) = ldftn($Invoke6);
			*(data + 653852) = ldftn($Invoke7);
			*(data + 653880) = ldftn($Invoke8);
			*(data + 653908) = ldftn($Invoke9);
			*(data + 653936) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new GetContentResponse((UIntPtr)0);
		}
	}
}
