using StaRTS.Main.Views.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2177 : $UnityType
	{
		public unsafe $UnityType2177()
		{
			*(UnityEngine.Internal.$Metadata.data + 867968) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new FadingEntity((UIntPtr)0);
		}
	}
}
