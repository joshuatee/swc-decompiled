using StaRTS.Main.Controllers.Objectives;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType874 : $UnityType
	{
		public unsafe $UnityType874()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 581948) = ldftn($Invoke0);
			*(data + 581976) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new LootObjectiveProcessor((UIntPtr)0);
		}
	}
}
