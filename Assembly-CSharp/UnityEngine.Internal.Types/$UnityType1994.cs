using StaRTS.Main.RUF.RUFTasks;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1994 : $UnityType
	{
		public unsafe $UnityType1994()
		{
			*(UnityEngine.Internal.$Metadata.data + 836160) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new ResumeTutorialRUFTask((UIntPtr)0);
		}
	}
}
