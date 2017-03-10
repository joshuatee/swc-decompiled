using StaRTS.Main.Models.Cee.Serializables;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1106 : $UnityType
	{
		public unsafe $UnityType1106()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 640916) = ldftn($Invoke0);
			*(data + 640944) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new CombatEncounter((UIntPtr)0);
		}
	}
}
