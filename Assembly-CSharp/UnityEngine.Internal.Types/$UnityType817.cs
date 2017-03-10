using StaRTS.Main.Controllers.Entities.Systems;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType817 : $UnityType
	{
		public unsafe $UnityType817()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 572316) = ldftn($Invoke0);
			*(data + 572344) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new MovementSystem((UIntPtr)0);
		}
	}
}
