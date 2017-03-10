using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType871 : $UnityType
	{
		public unsafe $UnityType871()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 581780) = ldftn($Invoke0);
			*(data + 581808) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DonateTroopIdObjectiveProcessor((UIntPtr)0);
		}
	}
}
