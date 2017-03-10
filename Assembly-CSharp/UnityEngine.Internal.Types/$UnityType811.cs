using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType811 : $UnityType
	{
		public unsafe $UnityType811()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 571700) = ldftn($Invoke0);
			*(data + 571728) = ldftn($Invoke1);
			*(data + 571756) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new BattleSystem((UIntPtr)0);
		}
	}
}
