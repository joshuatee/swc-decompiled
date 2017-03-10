using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2026 : $UnityType
	{
		public unsafe $UnityType2026()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838540) = ldftn($Invoke0);
			*(data + 838568) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DeployStarshipAttackStoryAction((UIntPtr)0);
		}
	}
}
