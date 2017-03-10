using StaRTS.Main.Views.Entities.Projectiles;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2192 : $UnityType
	{
		public unsafe $UnityType2192()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 869648) = ldftn($Invoke0);
			*(data + 869676) = ldftn($Invoke1);
			*(data + 869704) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ProjectileViewPath((UIntPtr)0);
		}
	}
}
