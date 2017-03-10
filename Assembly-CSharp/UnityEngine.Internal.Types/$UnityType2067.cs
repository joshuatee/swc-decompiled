using StaRTS.Main.Story.Actions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2067 : $UnityType
	{
		public unsafe $UnityType2067()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 841508) = ldftn($Invoke0);
			*(data + 841536) = ldftn($Invoke1);
			*(data + 841564) = ldftn($Invoke2);
			*(data + 841592) = ldftn($Invoke3);
			*(data + 841620) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new ShowPushNotificationsSettingsScreenStoryAction((UIntPtr)0);
		}
	}
}
