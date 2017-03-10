using StaRTS.Main.Controllers.World.Transitions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType994 : $UnityType
	{
		public unsafe $UnityType994()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 606140) = ldftn($Invoke0);
			*(data + 606168) = ldftn($Invoke1);
			*(data + 606196) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new BaseToWarboardTransition((UIntPtr)0);
		}
	}
}
