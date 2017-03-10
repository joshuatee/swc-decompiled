using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1893 : $UnityType
	{
		public unsafe $UnityType1893()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 709796) = ldftn($Invoke0);
			*(data + 709824) = ldftn($Invoke1);
			*(data + 709852) = ldftn($Invoke2);
			*(data + 709880) = ldftn($Invoke3);
			*(data + 709908) = ldftn($Invoke4);
			*(data + 709936) = ldftn($Invoke5);
			*(data + 709964) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new BattleScriptVO((UIntPtr)0);
		}
	}
}
