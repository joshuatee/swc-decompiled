using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2027 : $UnityType
	{
		public unsafe $UnityType2027()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 838596) = ldftn($Invoke0);
			*(data + 838624) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new DeselectBuildingStoryAction((UIntPtr)0);
		}
	}
}
