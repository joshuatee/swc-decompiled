using StaRTS.Main.Views.World.Deploying;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2520 : $UnityType
	{
		public unsafe $UnityType2520()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 985932) = ldftn($Invoke0);
			*(data + 985960) = ldftn($Invoke1);
			*(data + 985988) = ldftn($Invoke2);
			*(data + 986016) = ldftn($Invoke3);
			*(data + 986044) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ChampionDeployer((UIntPtr)0);
		}
	}
}
