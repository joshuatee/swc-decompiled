using StaRTS.Main.Controllers.World.Transitions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType996 : $UnityType
	{
		public unsafe $UnityType996()
		{
			*(UnityEngine.Internal.$Metadata.data + 606224) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new WarbaseToWarboardTransition((UIntPtr)0);
		}
	}
}
