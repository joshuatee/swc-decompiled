using StaRTS.Main.Models.Commands;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1309 : $UnityType
	{
		public unsafe $UnityType1309()
		{
			*(UnityEngine.Internal.$Metadata.data + 649904) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new SharedPrefRequest((UIntPtr)0);
		}
	}
}
