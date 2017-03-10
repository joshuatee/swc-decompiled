using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType929 : $UnityType
	{
		public unsafe $UnityType929()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 596536) = ldftn($Invoke0);
			*(data + 596564) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new LangStartupTask((UIntPtr)0);
		}
	}
}
