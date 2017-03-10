using StaRTS.Main.Views.Entities.Projectiles;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2190 : $UnityType
	{
		public unsafe $UnityType2190()
		{
			*(UnityEngine.Internal.$Metadata.data + 869592) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new ProjectileViewLinearPath((UIntPtr)0);
		}
	}
}
