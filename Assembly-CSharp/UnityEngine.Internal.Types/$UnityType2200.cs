using StaRTS.Main.Views.Projectors;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2200 : $UnityType
	{
		public unsafe $UnityType2200()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 870936) = ldftn($Invoke0);
			*(data + 870964) = ldftn($Invoke1);
			*(data + 870992) = ldftn($Invoke2);
			*(data + 871020) = ldftn($Invoke3);
			*(data + 871048) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ProjectorEquipmentBuildingDecorator((UIntPtr)0);
		}
	}
}
