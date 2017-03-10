using StaRTS.Main.Models.Commands.Player.Deployable;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1508 : $UnityType
	{
		public unsafe $UnityType1508()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 657688) = ldftn($Invoke0);
			*(data + 657716) = ldftn($Invoke1);
			*(data + 657744) = ldftn($Invoke2);
			*(data + 657772) = ldftn($Invoke3);
			*(data + 657800) = ldftn($Invoke4);
			*(data + 657828) = ldftn($Invoke5);
			*(data + 657856) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new DeployableSpendRequest((UIntPtr)0);
		}
	}
}
