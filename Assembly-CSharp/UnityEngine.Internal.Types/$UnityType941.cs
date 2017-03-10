using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType941 : $UnityType
	{
		public unsafe $UnityType941()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 598076) = ldftn($Invoke0);
			*(data + 598104) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new StaticInitStartupTask((UIntPtr)0);
		}
	}
}
