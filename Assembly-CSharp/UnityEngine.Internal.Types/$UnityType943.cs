using StaRTS.Main.Controllers.Startup;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType943 : $UnityType
	{
		public unsafe $UnityType943()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 598160) = ldftn($Invoke0);
			*(data + 598188) = ldftn($Invoke1);
			*(data + 598216) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new UserInputStartupTask((UIntPtr)0);
		}
	}
}
