using StaRTS.Main.Controllers.World.Transitions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType997 : $UnityType
	{
		public unsafe $UnityType997()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 606252) = ldftn($Invoke0);
			*(data + 606280) = ldftn($Invoke1);
			*(data + 606308) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new WarboardToBaseTransition((UIntPtr)0);
		}
	}
}
