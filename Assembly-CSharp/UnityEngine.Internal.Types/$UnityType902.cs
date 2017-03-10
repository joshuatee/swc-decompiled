using StaRTS.Main.Controllers.ServerMessages;
using System;
using WinRTBridge;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType902 : $UnityType
	{
		public unsafe $UnityType902()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 590040) = ldftn($Invoke0);
			*(data + 590068) = ldftn($Invoke1);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IMessage)GCHandledObjects.GCHandleToObject(instance)).MessageCookie);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IMessage)GCHandledObjects.GCHandleToObject(instance)).MessageEventId);
		}
	}
}
