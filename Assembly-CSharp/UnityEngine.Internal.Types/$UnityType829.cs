using StaRTS.Main.Controllers.GameStates;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType829 : $UnityType
	{
		public unsafe $UnityType829()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 573576) = ldftn($Invoke0);
			*(data + 573604) = ldftn($Invoke1);
			*(data + 573632) = ldftn($Invoke2);
			*(data + 573660) = ldftn($Invoke3);
			*(data + 573688) = ldftn($Invoke4);
			*(data + 573716) = ldftn($Invoke5);
			*(data + 573744) = ldftn($Invoke6);
			*(data + 573772) = ldftn($Invoke7);
			*(data + 573800) = ldftn($Invoke8);
			*(data + 573828) = ldftn($Invoke9);
			*(data + 573856) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new BattleStartState((UIntPtr)0);
		}
	}
}
