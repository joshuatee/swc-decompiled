using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1769 : $UnityType
	{
		public unsafe $UnityType1769()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 680060) = ldftn($Invoke0);
			*(data + 680088) = ldftn($Invoke1);
			*(data + 680116) = ldftn($Invoke2);
			*(data + 680144) = ldftn($Invoke3);
			*(data + 680172) = ldftn($Invoke4);
			*(data + 680200) = ldftn($Invoke5);
			*(data + 680228) = ldftn($Invoke6);
			*(data + 680256) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new GeneratorViewNode((UIntPtr)0);
		}
	}
}
