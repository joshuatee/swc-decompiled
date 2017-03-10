using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class NotificationTypeVO : IValueObject
	{
		public static int COLUMN_soundName
		{
			get;
			private set;
		}

		public static int COLUMN_desc
		{
			get;
			private set;
		}

		public static int COLUMN_minCompletionTime
		{
			get;
			private set;
		}

		public static int COLUMN_repeatTime
		{
			get;
			private set;
		}

		public static int COLUMN_validTimeRange
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string Desc
		{
			get;
			set;
		}

		public string SoundName
		{
			get;
			set;
		}

		public int MinCompletionTime
		{
			get;
			set;
		}

		public int RepeatTime
		{
			get;
			set;
		}

		public int EarliestValidTime
		{
			get;
			private set;
		}

		public int LatestValidTime
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.SoundName = row.TryGetString(NotificationTypeVO.COLUMN_soundName);
			if (this.SoundName.Equals(""))
			{
				this.SoundName = null;
			}
			this.Desc = row.TryGetString(NotificationTypeVO.COLUMN_desc);
			this.MinCompletionTime = row.TryGetInt(NotificationTypeVO.COLUMN_minCompletionTime);
			this.RepeatTime = row.TryGetInt(NotificationTypeVO.COLUMN_repeatTime);
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			List<StrIntPair> strIntPairs = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(NotificationTypeVO.COLUMN_validTimeRange));
			int earliestValidTime = 10;
			int latestValidTime = 21;
			if (strIntPairs != null)
			{
				int i = 0;
				int count = strIntPairs.Count;
				while (i < count)
				{
					StrIntPair strIntPair = strIntPairs[i];
					string strKey = strIntPair.StrKey;
					if (strKey == "earliest")
					{
						earliestValidTime = strIntPair.IntVal;
					}
					else if (strKey == "latest")
					{
						latestValidTime = strIntPair.IntVal;
					}
					i++;
				}
			}
			this.EarliestValidTime = earliestValidTime;
			this.LatestValidTime = latestValidTime;
		}

		public NotificationTypeVO()
		{
		}

		protected internal NotificationTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(NotificationTypeVO.COLUMN_desc);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(NotificationTypeVO.COLUMN_minCompletionTime);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(NotificationTypeVO.COLUMN_repeatTime);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(NotificationTypeVO.COLUMN_soundName);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(NotificationTypeVO.COLUMN_validTimeRange);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).Desc);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).EarliestValidTime);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).LatestValidTime);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).MinCompletionTime);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).RepeatTime);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).SoundName);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			NotificationTypeVO.COLUMN_desc = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			NotificationTypeVO.COLUMN_minCompletionTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			NotificationTypeVO.COLUMN_repeatTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			NotificationTypeVO.COLUMN_soundName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			NotificationTypeVO.COLUMN_validTimeRange = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).Desc = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).EarliestValidTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).LatestValidTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).MinCompletionTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).RepeatTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).SoundName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((NotificationTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
