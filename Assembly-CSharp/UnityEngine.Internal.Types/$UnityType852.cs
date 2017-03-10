using StaRTS.Main.Controllers.Missions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType852 : $UnityType
	{
		public unsafe $UnityType852()
		{
			*(UnityEngine.Internal.$Metadata.data + 577888) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new MissionProcessorFactory((UIntPtr)0);
		}
	}
}
