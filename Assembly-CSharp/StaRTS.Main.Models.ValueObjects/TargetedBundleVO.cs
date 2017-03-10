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
	public class TargetedBundleVO : IValueObject
	{
		public List<AudienceCondition> AudienceConditions;

		public List<string> LinkedIAPs;

		public List<string> RewardUIDs;

		public List<string> Groups;

		public static int COLUMN_uid
		{
			get;
			private set;
		}

		public static int COLUMN_rewards
		{
			get;
			private set;
		}

		public static int COLUMN_title
		{
			get;
			private set;
		}

		public static int COLUMN_description
		{
			get;
			private set;
		}

		public static int COLUMN_confirmationString
		{
			get;
			private set;
		}

		public static int COLUMN_groups
		{
			get;
			private set;
		}

		public static int COLUMN_discount
		{
			get;
			private set;
		}

		public static int COLUMN_audienceConditions
		{
			get;
			private set;
		}

		public static int COLUMN_duration
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

		public static int COLUMN_expirationCooldown
		{
			get;
			private set;
		}

		public static int COLUMN_repurchaseCooldown
		{
			get;
			private set;
		}

		public static int COLUMN_globalCooldown
		{
			get;
			private set;
		}

		public static int COLUMN_groupCooldown
		{
			get;
			private set;
		}

		public static int COLUMN_maxPurchases
		{
			get;
			private set;
		}

		public static int COLUMN_linkedPack
		{
			get;
			private set;
		}

		public static int COLUMN_character1Image
		{
			get;
			private set;
		}

		public static int COLUMN_character2Image
		{
			get;
			private set;
		}

		public static int COLUMN_iconImage
		{
			get;
			private set;
		}

		public static int COLUMN_iconString
		{
			get;
			private set;
		}

		public static int COLUMN_offerImage
		{
			get;
			private set;
		}

		public static int COLUMN_autoPop
		{
			get;
			private set;
		}

		public static int COLUMN_priority
		{
			get;
			private set;
		}

		public static int COLUMN_cost
		{
			get;
			private set;
		}

		public static int COLUMN_ignoreCooldown
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string Reward
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string ConfirmationString
		{
			get;
			set;
		}

		public int Discount
		{
			get;
			set;
		}

		public int Duration
		{
			get;
			set;
		}

		public DateTime StartTime
		{
			get;
			set;
		}

		public DateTime EndTime
		{
			get;
			set;
		}

		public int ExpirationCooldown
		{
			get;
			set;
		}

		public int RepurchaseCooldown
		{
			get;
			set;
		}

		public int GlobalCooldown
		{
			get;
			set;
		}

		public int GroupCooldown
		{
			get;
			set;
		}

		public int MaxPurchases
		{
			get;
			set;
		}

		public string Character1Image
		{
			get;
			set;
		}

		public string Character2Image
		{
			get;
			set;
		}

		public string IconImage
		{
			get;
			set;
		}

		public string IconString
		{
			get;
			set;
		}

		public string OfferImage
		{
			get;
			set;
		}

		public bool AutoPop
		{
			get;
			set;
		}

		public int Priority
		{
			get;
			set;
		}

		public string[] Cost
		{
			get;
			set;
		}

		public bool IgnoreCooldown
		{
			get;
			set;
		}

		public override string ToString()
		{
			return this.Uid;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.Title = row.TryGetString(TargetedBundleVO.COLUMN_title);
			this.Description = row.TryGetString(TargetedBundleVO.COLUMN_description);
			this.ConfirmationString = row.TryGetString(TargetedBundleVO.COLUMN_confirmationString);
			this.Discount = row.TryGetInt(TargetedBundleVO.COLUMN_discount);
			this.Duration = row.TryGetInt(TargetedBundleVO.COLUMN_duration);
			string text = row.TryGetString(TargetedBundleVO.COLUMN_startDate);
			if (!string.IsNullOrEmpty(text))
			{
				this.StartTime = DateTime.ParseExact(text, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
			}
			else
			{
				this.StartTime = DateTime.MaxValue;
				Service.Get<StaRTSLogger>().Warn("TargetedBundle VO Start Date Not Specified");
			}
			string text2 = row.TryGetString(TargetedBundleVO.COLUMN_endDate);
			if (!string.IsNullOrEmpty(text2))
			{
				this.EndTime = DateTime.ParseExact(text2, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
			}
			else
			{
				this.EndTime = DateTime.MaxValue;
			}
			this.ExpirationCooldown = row.TryGetInt(TargetedBundleVO.COLUMN_expirationCooldown);
			this.RepurchaseCooldown = row.TryGetInt(TargetedBundleVO.COLUMN_repurchaseCooldown);
			this.GlobalCooldown = row.TryGetInt(TargetedBundleVO.COLUMN_globalCooldown);
			this.GroupCooldown = row.TryGetInt(TargetedBundleVO.COLUMN_groupCooldown);
			this.MaxPurchases = row.TryGetInt(TargetedBundleVO.COLUMN_maxPurchases);
			this.Character1Image = row.TryGetString(TargetedBundleVO.COLUMN_character1Image);
			this.Character2Image = row.TryGetString(TargetedBundleVO.COLUMN_character2Image);
			this.IconImage = row.TryGetString(TargetedBundleVO.COLUMN_iconImage);
			this.IconString = row.TryGetString(TargetedBundleVO.COLUMN_iconString);
			this.OfferImage = row.TryGetString(TargetedBundleVO.COLUMN_offerImage);
			this.AutoPop = row.TryGetBool(TargetedBundleVO.COLUMN_autoPop);
			this.Priority = row.TryGetInt(TargetedBundleVO.COLUMN_priority);
			this.Cost = row.TryGetStringArray(TargetedBundleVO.COLUMN_cost);
			this.IgnoreCooldown = row.TryGetBool(TargetedBundleVO.COLUMN_ignoreCooldown);
			this.AudienceConditions = new List<AudienceCondition>();
			string[] array = row.TryGetStringArray(TargetedBundleVO.COLUMN_audienceConditions);
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
			this.Groups = new List<string>();
			string[] array2 = row.TryGetStringArray(TargetedBundleVO.COLUMN_groups);
			if (array2 != null)
			{
				int j = 0;
				int num2 = array2.Length;
				while (j < num2)
				{
					this.Groups.Add(array2[j]);
					j++;
				}
			}
			this.LinkedIAPs = new List<string>();
			string[] array3 = row.TryGetStringArray(TargetedBundleVO.COLUMN_linkedPack);
			if (array3 != null)
			{
				int k = 0;
				int num3 = array3.Length;
				while (k < num3)
				{
					this.LinkedIAPs.Add(array3[k]);
					k++;
				}
			}
			this.RewardUIDs = new List<string>();
			string[] array4 = row.TryGetStringArray(TargetedBundleVO.COLUMN_rewards);
			if (array4 != null)
			{
				int l = 0;
				int num4 = array4.Length;
				while (l < num4)
				{
					this.RewardUIDs.Add(array4[l]);
					l++;
				}
			}
		}

		public TargetedBundleVO()
		{
		}

		protected internal TargetedBundleVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).AutoPop);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Character1Image);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Character2Image);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_audienceConditions);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_autoPop);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_character1Image);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_character2Image);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_confirmationString);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_cost);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_description);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_discount);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_duration);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_endDate);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_expirationCooldown);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_globalCooldown);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_groupCooldown);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_groups);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_iconImage);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_iconString);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_ignoreCooldown);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_linkedPack);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_maxPurchases);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_offerImage);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_priority);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_repurchaseCooldown);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_rewards);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_startDate);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_title);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TargetedBundleVO.COLUMN_uid);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).ConfirmationString);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Cost);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Discount);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Duration);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).EndTime);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).ExpirationCooldown);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).GlobalCooldown);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).GroupCooldown);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).IconImage);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).IconString);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).IgnoreCooldown);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).MaxPurchases);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).OfferImage);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Priority);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).RepurchaseCooldown);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Reward);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).StartTime);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Title);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).AutoPop = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Character1Image = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Character2Image = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_audienceConditions = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_autoPop = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_character1Image = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_character2Image = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_confirmationString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_cost = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_description = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_discount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_duration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_endDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_expirationCooldown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_globalCooldown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_groupCooldown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_groups = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_iconImage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_iconString = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_ignoreCooldown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_linkedPack = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_maxPurchases = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_offerImage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_priority = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_repurchaseCooldown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_rewards = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_startDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_title = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			TargetedBundleVO.COLUMN_uid = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).ConfirmationString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Cost = (string[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Description = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Discount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Duration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).EndTime = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).ExpirationCooldown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).GlobalCooldown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).GroupCooldown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).IconImage = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).IconString = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).IgnoreCooldown = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).MaxPurchases = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).OfferImage = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Priority = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).RepurchaseCooldown = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Reward = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).StartTime = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Title = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TargetedBundleVO)GCHandledObjects.GCHandleToObject(instance)).ToString());
		}
	}
}
