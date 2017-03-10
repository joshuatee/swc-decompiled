using StaRTS.Main.Models.Commands.Cheats;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1355 : $UnityType
	{
		public unsafe $UnityType1355()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 650856) = ldftn($Invoke0);
			*(data + 650884) = ldftn($Invoke1);
			*(data + 650912) = ldftn($Invoke2);
			*(data + 650940) = ldftn($Invoke3);
			*(data + 650968) = ldftn($Invoke4);
			*(data + 650996) = ldftn($Invoke5);
			*(data + 651024) = ldftn($Invoke6);
			*(data + 651052) = ldftn($Invoke7);
			*(data + 651080) = ldftn($Invoke8);
			*(data + 651108) = ldftn($Invoke9);
			*(data + 651136) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new CheatSetResourcesRequest((UIntPtr)0);
		}
	}
}
