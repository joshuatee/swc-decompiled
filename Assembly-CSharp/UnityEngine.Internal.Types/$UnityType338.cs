using Source.StaRTS.Main.Models.Commands.Player;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType338 : $UnityType
	{
		public unsafe $UnityType338()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 428228) = ldftn($Invoke0);
			*(data + 428256) = ldftn($Invoke1);
			*(data + 428284) = ldftn($Invoke2);
			*(data + 428312) = ldftn($Invoke3);
			*(data + 428340) = ldftn($Invoke4);
			*(data + 428368) = ldftn($Invoke5);
			*(data + 428396) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new GeneratePlayerRequest((UIntPtr)0);
		}
	}
}
