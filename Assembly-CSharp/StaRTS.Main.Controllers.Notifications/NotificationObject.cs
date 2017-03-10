using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Notifications
{
	public class NotificationObject
	{
		public string NotificationUid
		{
			get;
			private set;
		}

		public string InProgressMessage
		{
			get;
			private set;
		}

		public string Message
		{
			get;
			private set;
		}

		public string SoundName
		{
			get;
			private set;
		}

		public DateTime Time
		{
			get;
			set;
		}

		public string Key
		{
			get;
			private set;
		}

		public string ObjectId
		{
			get;
			private set;
		}

		public NotificationObject(string notificationUid, string inProgressMessage, string message, string soundName, DateTime time, string key, string objectId)
		{
			this.NotificationUid = notificationUid;
			this.InProgressMessage = inProgressMessage;
			this.Message = message;
			this.SoundName = soundName;
			this.Time = time;
			this.Key = key;
			this.ObjectId = objectId;
		}

		protected internal NotificationObject(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).InProgressMessage);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).Key);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).Message);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).NotificationUid);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).ObjectId);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).SoundName);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).Time);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).InProgressMessage = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).Key = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).Message = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).NotificationUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).ObjectId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).SoundName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((NotificationObject)GCHandledObjects.GCHandleToObject(instance)).Time = *(*(IntPtr*)args);
			return -1L;
		}
	}
}
