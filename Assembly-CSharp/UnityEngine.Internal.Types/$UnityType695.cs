using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType695 : $UnityType
	{
		public unsafe $UnityType695()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 530680) = ldftn($Invoke0);
			*(data + 530708) = ldftn($Invoke1);
			*(data + 530736) = ldftn($Invoke2);
			*(data + 530764) = ldftn($Invoke3);
			*(data + 530792) = ldftn($Invoke4);
			*(data + 530820) = ldftn($Invoke5);
			*(data + 530848) = ldftn($Invoke6);
			*(data + 530876) = ldftn($Invoke7);
			*(data + 530904) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new CombatTriggerManager((UIntPtr)0);
		}
	}
}
