using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1946 : $UnityType
	{
		public unsafe $UnityType1946()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 769800) = ldftn($Invoke0);
			*(data + 769828) = ldftn($Invoke1);
			*(data + 769856) = ldftn($Invoke2);
			*(data + 769884) = ldftn($Invoke3);
			*(data + 769912) = ldftn($Invoke4);
			*(data + 769940) = ldftn($Invoke5);
			*(data + 769968) = ldftn($Invoke6);
			*(data + 769996) = ldftn($Invoke7);
			*(data + 770024) = ldftn($Invoke8);
			*(data + 770052) = ldftn($Invoke9);
			*(data + 770080) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new PlanetLootVO((UIntPtr)0);
		}
	}
}
