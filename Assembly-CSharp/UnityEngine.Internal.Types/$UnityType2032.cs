using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2032 : $UnityType
	{
		public unsafe $UnityType2032()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838904) = ldftn($Invoke0);
			*(data + 838932) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new EnableCancelBuildingPlacementStoryAction((UIntPtr)0);
		}
	}
}
