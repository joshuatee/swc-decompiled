using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1774 : $UnityType
	{
		public unsafe $UnityType1774()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 680788) = ldftn($Invoke0);
			*(data + 680816) = ldftn($Invoke1);
			*(data + 680844) = ldftn($Invoke2);
			*(data + 680872) = ldftn($Invoke3);
			*(data + 680900) = ldftn($Invoke4);
			*(data + 680928) = ldftn($Invoke5);
			*(data + 680956) = ldftn($Invoke6);
			*(data + 680984) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new MovementNode((UIntPtr)0);
		}
	}
}
