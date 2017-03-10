using StaRTS.Main.Models.Player.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1832 : $UnityType
	{
		public unsafe $UnityType1832()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 700780) = ldftn($Invoke0);
			*(data + 700808) = ldftn($Invoke1);
			*(data + 700836) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ObjectiveProgress((UIntPtr)0);
		}
	}
}
