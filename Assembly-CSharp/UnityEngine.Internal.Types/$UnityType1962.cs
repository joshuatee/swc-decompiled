using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1962 : $UnityType
	{
		public unsafe $UnityType1962()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 797044) = ldftn($Invoke0);
			*(data + 797072) = ldftn($Invoke1);
			*(data + 797100) = ldftn($Invoke2);
			*(data + 797128) = ldftn($Invoke3);
			*(data + 797156) = ldftn($Invoke4);
			*(data + 797184) = ldftn($Invoke5);
			*(data + 797212) = ldftn($Invoke6);
			*(data + 797240) = ldftn($Invoke7);
			*(data + 797268) = ldftn($Invoke8);
			*(data + 797296) = ldftn($Invoke9);
			*(data + 797324) = ldftn($Invoke10);
			*(data + 797352) = ldftn($Invoke11);
			*(data + 797380) = ldftn($Invoke12);
			*(data + 797408) = ldftn($Invoke13);
			*(data + 797436) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new SquadLevelVO((UIntPtr)0);
		}
	}
}
