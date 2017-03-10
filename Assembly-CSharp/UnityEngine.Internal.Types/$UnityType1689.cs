using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1689 : $UnityType
	{
		public unsafe $UnityType1689()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 666816) = ldftn($Invoke0);
			*(data + 666844) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new BoardItemComponent((UIntPtr)0);
		}
	}
}
