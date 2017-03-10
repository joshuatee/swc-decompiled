using Source.StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType339 : $UnityType
	{
		public unsafe $UnityType339()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 428424) = ldftn($Invoke0);
			*(data + 428452) = ldftn($Invoke1);
			*(data + 428480) = ldftn($Invoke2);
			*(data + 428508) = ldftn($Invoke3);
			*(data + 428536) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new GeneratePlayerResponse((UIntPtr)0);
		}
	}
}
