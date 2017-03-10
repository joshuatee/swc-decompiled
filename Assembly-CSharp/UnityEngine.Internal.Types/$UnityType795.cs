using StaRTS.Main.Controllers.CombineMesh;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType795 : $UnityType
	{
		public unsafe $UnityType795()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 568844) = ldftn($Invoke0);
			*(data + 568872) = ldftn($Invoke1);
			*(data + 568900) = ldftn($Invoke2);
			*(data + 568928) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new BattleBaseCombineMeshHelper((UIntPtr)0);
		}
	}
}
