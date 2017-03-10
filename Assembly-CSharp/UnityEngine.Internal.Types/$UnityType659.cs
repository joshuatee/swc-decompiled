using StaRTS.GameBoard.Pathfinding;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType659 : $UnityType
	{
		public unsafe $UnityType659()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 516316) = ldftn($Invoke0);
			*(data + 516344) = ldftn($Invoke1);
			*(data + 516372) = ldftn($Invoke2);
			*(data + 516400) = ldftn($Invoke3);
			*(data + 516428) = ldftn($Invoke4);
			*(data + 516456) = ldftn($Invoke5);
			*(data + 516484) = ldftn($Invoke6);
			*(data + 516512) = ldftn($Invoke7);
			*(data + 516540) = ldftn($Invoke8);
			*(data + 516568) = ldftn($Invoke9);
			*(data + 516596) = ldftn($Invoke10);
			*(data + 516624) = ldftn($Invoke11);
			*(data + 516652) = ldftn($Invoke12);
			*(data + 516680) = ldftn($Invoke13);
			*(data + 516708) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new Path((UIntPtr)0);
		}
	}
}
