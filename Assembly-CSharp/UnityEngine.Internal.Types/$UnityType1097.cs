using StaRTS.Main.Models.Battle.Replay;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1097 : $UnityType
	{
		public unsafe $UnityType1097()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 637556) = ldftn($Invoke0);
			*(data + 637584) = ldftn($Invoke1);
			*(data + 637612) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new BattleCanceledAction((UIntPtr)0);
		}
	}
}
