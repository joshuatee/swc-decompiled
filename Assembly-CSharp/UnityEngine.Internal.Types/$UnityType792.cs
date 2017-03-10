using StaRTS.Main.Controllers.CombatTriggers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType792 : $UnityType
	{
		public unsafe $UnityType792()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 568004) = ldftn($Invoke0);
			*(data + 568032) = ldftn($Invoke1);
			*(data + 568060) = ldftn($Invoke2);
			*(data + 568088) = ldftn($Invoke3);
			*(data + 568116) = ldftn($Invoke4);
			*(data + 568144) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new StoryActionCombatTrigger((UIntPtr)0);
		}
	}
}
