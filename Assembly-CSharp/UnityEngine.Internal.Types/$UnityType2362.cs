using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2362 : $UnityType
	{
		public unsafe $UnityType2362()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 933964) = ldftn($Invoke0);
			*(data + 933992) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new StorageUpgradeScreen((UIntPtr)0);
		}
	}
}
