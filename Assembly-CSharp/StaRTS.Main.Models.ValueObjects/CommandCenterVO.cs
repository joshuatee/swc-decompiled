using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class CommandCenterVO : ITimestamped, IValueObject, ICallToAction
	{
		public List<AudienceCondition> AudienceConditions;

		public static int COLUMN_layout
		{
			get;
			private set;
		}

		public static int COLUMN_image
		{
			get;
			private set;
		}

		public static int COLUMN_titleText
		{
			get;
			private set;
		}

		public static int COLUMN_bodyText
		{
			get;
			private set;
		}

		public static int COLUMN_btn1
		{
			get;
			private set;
		}

		public static int COLUMN_btn1action
		{
			get;
			private set;
		}

		public static int COLUMN_btn1data
		{
			get;
			private set;
		}

		public static int COLUMN_btn2
		{
			get;
			private set;
		}

		public static int COLUMN_btn2action
		{
			get;
			private set;
		}

		public static int COLUMN_btn2data
		{
			get;
			private set;
		}

		public static int COLUMN_priority
		{
			get;
			private set;
		}

		public static int COLUMN_startDate
		{
			get;
			private set;
		}

		public static int COLUMN_endDate
		{
			get;
			private set;
		}

		public static int COLUMN_carouselAutoSwipe
		{
			get;
			private set;
		}

		public static int COLUMN_audienceConditions
		{
			get;
			private set;
		}

		public static int COLUMN_isPromo
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public int Layout
		{
			get;
			set;
		}

		public string Image
		{
			get;
			set;
		}

		public string TitleText
		{
			get;
			set;
		}

		public string BodyText
		{
			get;
			set;
		}

		public string Btn1
		{
			get;
			set;
		}

		public string Btn1Action
		{
			get;
			set;
		}

		public string Btn1Data
		{
			get;
			set;
		}

		public string Btn2
		{
			get;
			set;
		}

		public string Btn2Action
		{
			get;
			set;
		}

		public string Btn2Data
		{
			get;
			set;
		}

		public int Priority
		{
			get;
			set;
		}

		public int StartTime
		{
			get;
			set;
		}

		public int EndTime
		{
			get;
			set;
		}

		public float CarouselAutoSwipe
		{
			get;
			set;
		}

		public bool IsPromo
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Layout = row.TryGetInt(CommandCenterVO.COLUMN_layout);
			this.Image = row.TryGetString(CommandCenterVO.COLUMN_image);
			this.TitleText = row.TryGetString(CommandCenterVO.COLUMN_titleText);
			this.BodyText = row.TryGetString(CommandCenterVO.COLUMN_bodyText);
			this.Btn1 = row.TryGetString(CommandCenterVO.COLUMN_btn1);
			this.Btn1Action = row.TryGetString(CommandCenterVO.COLUMN_btn1action);
			this.Btn1Data = row.TryGetString(CommandCenterVO.COLUMN_btn1data);
			this.Btn2 = row.TryGetString(CommandCenterVO.COLUMN_btn2);
			this.Btn2Action = row.TryGetString(CommandCenterVO.COLUMN_btn2action);
			this.Btn2Data = row.TryGetString(CommandCenterVO.COLUMN_btn2data);
			if (string.IsNullOrEmpty(this.Btn1Data))
			{
				this.Btn1Data = string.Empty;
			}
			if (string.IsNullOrEmpty(this.Btn2Data))
			{
				this.Btn2Data = string.Empty;
			}
			this.Priority = row.TryGetInt(CommandCenterVO.COLUMN_priority, 0);
			string text = row.TryGetString(CommandCenterVO.COLUMN_startDate);
			if (!string.IsNullOrEmpty(text))
			{
				DateTime date = DateTime.ParseExact(text, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
				this.StartTime = DateUtils.GetSecondsFromEpoch(date);
			}
			else
			{
				this.StartTime = 0;
				Service.Get<StaRTSLogger>().Warn("CommandCenter VO Start Date Not Specified");
			}
			string text2 = row.TryGetString(CommandCenterVO.COLUMN_endDate);
			if (!string.IsNullOrEmpty(text2))
			{
				DateTime date2 = DateTime.ParseExact(text2, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
				this.EndTime = DateUtils.GetSecondsFromEpoch(date2);
			}
			else
			{
				this.EndTime = 2147483647;
			}
			this.CarouselAutoSwipe = row.TryGetFloat(CommandCenterVO.COLUMN_carouselAutoSwipe);
			this.AudienceConditions = new List<AudienceCondition>();
			string[] array = row.TryGetStringArray(CommandCenterVO.COLUMN_audienceConditions);
			if (array != null)
			{
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					this.AudienceConditions.Add(new AudienceCondition(array[i]));
					i++;
				}
			}
			this.IsPromo = row.TryGetBool(CommandCenterVO.COLUMN_isPromo);
		}

		public CommandCenterVO()
		{
		}

		protected internal CommandCenterVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).BodyText);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Btn1);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Btn1Action);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Btn1Data);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Btn2);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Btn2Action);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Btn2Data);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).CarouselAutoSwipe);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_audienceConditions);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_bodyText);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_btn1);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_btn1action);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_btn1data);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_btn2);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_btn2action);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_btn2data);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_carouselAutoSwipe);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_endDate);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_image);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_isPromo);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_layout);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_priority);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_startDate);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(CommandCenterVO.COLUMN_titleText);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).EndTime);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Image);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).IsPromo);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Layout);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Priority);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).StartTime);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).TitleText);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).BodyText = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Btn1 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Btn1Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Btn1Data = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Btn2 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Btn2Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Btn2Data = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).CarouselAutoSwipe = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			CommandCenterVO.COLUMN_audienceConditions = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			CommandCenterVO.COLUMN_bodyText = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			CommandCenterVO.COLUMN_btn1 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			CommandCenterVO.COLUMN_btn1action = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			CommandCenterVO.COLUMN_btn1data = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			CommandCenterVO.COLUMN_btn2 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			CommandCenterVO.COLUMN_btn2action = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			CommandCenterVO.COLUMN_btn2data = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			CommandCenterVO.COLUMN_carouselAutoSwipe = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			CommandCenterVO.COLUMN_endDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			CommandCenterVO.COLUMN_image = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			CommandCenterVO.COLUMN_isPromo = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			CommandCenterVO.COLUMN_layout = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			CommandCenterVO.COLUMN_priority = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			CommandCenterVO.COLUMN_startDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			CommandCenterVO.COLUMN_titleText = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).EndTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Image = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).IsPromo = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Layout = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Priority = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).StartTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).TitleText = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((CommandCenterVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
