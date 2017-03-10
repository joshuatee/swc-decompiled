using StaRTS.Main.Controllers.ServerMessages;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType899 : $UnityType
	{
		public unsafe $UnityType899()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 589228) = ldftn($Invoke0);
			*(data + 589256) = ldftn($Invoke1);
			*(data + 589284) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new ContractFinishedMessage((UIntPtr)0);
		}
	}
}
