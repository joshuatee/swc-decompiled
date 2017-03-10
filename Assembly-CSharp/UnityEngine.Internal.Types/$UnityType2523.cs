using StaRTS.Main.Views.World.Deploying;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2523 : $UnityType
	{
		public unsafe $UnityType2523()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 986520) = ldftn($Invoke0);
			*(data + 986548) = ldftn($Invoke1);
			*(data + 986576) = ldftn($Invoke2);
			*(data + 986604) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new SquadTroopDeployer((UIntPtr)0);
		}
	}
}
