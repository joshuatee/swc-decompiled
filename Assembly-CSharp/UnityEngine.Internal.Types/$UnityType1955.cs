using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1955 : $UnityType
	{
		public unsafe $UnityType1955()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 784472) = ldftn($Invoke0);
			*(data + 784500) = ldftn($Invoke1);
			*(data + 784528) = ldftn($Invoke2);
			*(data + 784556) = ldftn($Invoke3);
			*(data + 784584) = ldftn($Invoke4);
			*(data + 784612) = ldftn($Invoke5);
			*(data + 784640) = ldftn($Invoke6);
			*(data + 784668) = ldftn($Invoke7);
			*(data + 784696) = ldftn($Invoke8);
			*(data + 784724) = ldftn($Invoke9);
			*(data + 784752) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new ShaderTypeVO((UIntPtr)0);
		}
	}
}
