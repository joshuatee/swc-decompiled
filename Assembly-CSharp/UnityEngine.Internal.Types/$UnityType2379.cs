using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2379 : $UnityType
	{
		public unsafe $UnityType2379()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 941076) = ldftn($Invoke0);
			*(data + 941104) = ldftn($Invoke1);
			*(data + 941132) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new UnderAttackScreen((UIntPtr)0);
		}
	}
}
