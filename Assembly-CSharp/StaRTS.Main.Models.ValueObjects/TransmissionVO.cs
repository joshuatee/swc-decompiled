using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Utils;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class TransmissionVO : ITimestamped, IValueObject, ICallToAction
	{
		private const string SERVER_START_DATE = "postingDate";

		private const string SERVER_SOURCE_TYPE = "eventType";

		private const string SERVER_SOURCE_LEVEL = "level";

		private const string SERVER_SOURCE_UID = "uid";

		private const string SERVER_SOURCE_TIER = "tier";

		private const string SERVER_SOURCE_EMPIRE_NAME = "empireName";

		private const string SERVER_SOURCE_EMPIRE_SCORE = "empireScore";

		private const string SERVER_SOURCE_REBEL_NAME = "rebelName";

		private const string SERVER_SOURCE_REBEL_SCORE = "rebelScore";

		private const string SERVER_END_DATE = "_expirationDate";

		private const string CONFLICT_ACTION_TYPE = "conflictEnd";

		private const string WAR_BOARD_CTA = "gotowarboard";

		private const string CURRENT_SQUAD_NAME = "guildName";

		private const string SQUAD_LEVEL = "guildLevel";

		private const string SQUAD_LEVELUP_CTA = "squadlevelup";

		private const string CRATE_ID = "crateId";

		private const string DAILY_CRATE_REWARD_CTA = "dailycratereward";

		private const int DEFAULT_PRIORITY = 0;

		private const int BATTLE_PRIORITY = 1;

		private const int SQUAD_LEVEL_UP_PRIORITY = 2;

		private const int CONFLICT_PRIORITY = 3;

		private const int SQUAD_WAR_PRIORITY = 4;

		private const int DAILY_CRATE_REWARD_PRIORITY = 5;

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

		public static int COLUMN_faction
		{
			get;
			private set;
		}

		public static int COLUMN_npc
		{
			get;
			private set;
		}

		public static int COLUMN_actionType
		{
			get;
			private set;
		}

		public static int COLUMN_actionData
		{
			get;
			private set;
		}

		public static int COLUMN_actionDisplay
		{
			get;
			private set;
		}

		public static int COLUMN_image
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

		public static int COLUMN_transType
		{
			get;
			private set;
		}

		public string Uid
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

		public FactionType Faction
		{
			get;
			set;
		}

		public string CharacterID
		{
			get;
			set;
		}

		public string Image
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

		public TransmissionType Type
		{
			get;
			set;
		}

		public int Priority
		{
			get;
			private set;
		}

		public string TransData
		{
			get;
			set;
		}

		public List<BattleEntry> AttackerData
		{
			get;
			private set;
		}

		public int TotalPvpRatingDelta
		{
			get;
			private set;
		}

		public string CurrentSquadName
		{
			get;
			private set;
		}

		public int SquadLevel
		{
			get;
			private set;
		}

		public string CrateId
		{
			get;
			set;
		}

		public string EmpireSquadName
		{
			get;
			set;
		}

		public int EmpireScore
		{
			get;
			set;
		}

		public string RebelSquadName
		{
			get;
			set;
		}

		public int RebelScore
		{
			get;
			set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.TitleText = row.TryGetString(TransmissionVO.COLUMN_titleText);
			this.BodyText = row.TryGetString(TransmissionVO.COLUMN_bodyText);
			string text = row.TryGetString(TransmissionVO.COLUMN_startDate);
			if (!string.IsNullOrEmpty(text))
			{
				try
				{
					DateTime date = DateTime.ParseExact(text, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
					this.StartTime = DateUtils.GetSecondsFromEpoch(date);
					goto IL_A6;
				}
				catch (Exception)
				{
					this.StartTime = 0;
					Service.Get<StaRTSLogger>().Warn("TransmissionVO:: Transmission Holonet CMS Start Date Format Error: " + this.Uid);
					goto IL_A6;
				}
			}
			this.StartTime = 0;
			Service.Get<StaRTSLogger>().Warn("TransmissionVO:: Transmission Holonet CMS Start Date Not Specified For: " + this.Uid);
			IL_A6:
			string text2 = row.TryGetString(TransmissionVO.COLUMN_endDate);
			if (!string.IsNullOrEmpty(text2))
			{
				try
				{
					DateTime date2 = DateTime.ParseExact(text2, "HH:mm,dd-MM-yyyy", CultureInfo.InvariantCulture);
					this.EndTime = DateUtils.GetSecondsFromEpoch(date2);
					goto IL_10C;
				}
				catch (Exception)
				{
					this.EndTime = 2147483647;
					Service.Get<StaRTSLogger>().Warn("TransmissionVO:: Transmission Holonet CMS End Date Format Error: " + this.Uid);
					goto IL_10C;
				}
			}
			this.EndTime = 2147483647;
			IL_10C:
			this.Faction = StringUtils.ParseEnum<FactionType>(row.TryGetString(TransmissionVO.COLUMN_faction));
			this.CharacterID = row.TryGetString(TransmissionVO.COLUMN_npc);
			this.Image = row.TryGetString(TransmissionVO.COLUMN_image);
			this.Btn1 = row.TryGetString(TransmissionVO.COLUMN_btn1);
			this.Btn1Action = row.TryGetString(TransmissionVO.COLUMN_btn1action);
			this.Btn1Data = row.TryGetString(TransmissionVO.COLUMN_btn1data);
			this.Btn2 = row.TryGetString(TransmissionVO.COLUMN_btn2);
			this.Btn2Action = row.TryGetString(TransmissionVO.COLUMN_btn2action);
			this.Btn2Data = row.TryGetString(TransmissionVO.COLUMN_btn2data);
			this.Type = StringUtils.ParseEnum<TransmissionType>(row.TryGetString(TransmissionVO.COLUMN_transType));
			this.Priority = 0;
		}

		public void InitBattleData(List<BattleEntry> battles)
		{
			this.Priority = 1;
			this.AttackerData = battles;
			this.TotalPvpRatingDelta = 0;
			int count = battles.Count;
			for (int i = 0; i < count; i++)
			{
				BattleEntry battleEntry = battles[i];
				BattleParticipant defender = battleEntry.Defender;
				int num = GameUtils.CalcuateMedals(defender.AttackRating, defender.DefenseRating);
				int num2 = GameUtils.CalcuateMedals(defender.AttackRating + defender.AttackRatingDelta, defender.DefenseRating + defender.DefenseRatingDelta);
				this.TotalPvpRatingDelta += num2 - num;
			}
		}

		public void ResetAttackerData()
		{
			if (this.AttackerData != null)
			{
				this.AttackerData.Clear();
				this.AttackerData = null;
			}
		}

		public static TransmissionVO CreateFromServerObject(object data)
		{
			TransmissionVO transmissionVO = new TransmissionVO();
			Dictionary<string, object> dictionary = data as Dictionary<string, object>;
			transmissionVO.Priority = 0;
			StringBuilder stringBuilder = new StringBuilder();
			if (dictionary.ContainsKey("postingDate"))
			{
				transmissionVO.StartTime = Convert.ToInt32((string)dictionary["postingDate"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("eventType"))
			{
				string text = (string)dictionary["eventType"];
				transmissionVO.Type = StringUtils.ParseEnum<TransmissionType>(text);
				stringBuilder.Append(text);
			}
			if (dictionary.ContainsKey("tier"))
			{
				transmissionVO.Priority = 3;
				string text2 = (string)dictionary["tier"];
				transmissionVO.Btn1Data = text2;
				transmissionVO.Btn1Action = "conflictEnd";
				stringBuilder.Append(text2);
			}
			else if (dictionary.ContainsKey("level"))
			{
				string text3 = (string)dictionary["level"];
				transmissionVO.Btn1Data = text3;
				stringBuilder.Append(text3);
			}
			if (dictionary.ContainsKey("uid"))
			{
				string text4 = (string)dictionary["uid"];
				transmissionVO.TransData = text4;
				stringBuilder.Append(text4);
			}
			if (dictionary.ContainsKey("empireName"))
			{
				string text5 = (string)dictionary["empireName"];
				transmissionVO.EmpireSquadName = WWW.UnEscapeURL(text5);
				stringBuilder.Append(text5);
			}
			if (dictionary.ContainsKey("empireScore"))
			{
				int num = Convert.ToInt32(dictionary["empireScore"], CultureInfo.InvariantCulture);
				transmissionVO.EmpireScore = num;
				stringBuilder.Append(num);
			}
			if (dictionary.ContainsKey("rebelName"))
			{
				string text6 = (string)dictionary["rebelName"];
				transmissionVO.RebelSquadName = WWW.UnEscapeURL(text6);
				stringBuilder.Append(text6);
			}
			if (dictionary.ContainsKey("rebelScore"))
			{
				int num2 = Convert.ToInt32(dictionary["rebelScore"], CultureInfo.InvariantCulture);
				transmissionVO.RebelScore = num2;
				stringBuilder.Append(num2);
			}
			if (transmissionVO.Type == TransmissionType.WarStart || transmissionVO.Type == TransmissionType.WarEnded || transmissionVO.Type == TransmissionType.WarPreparation)
			{
				transmissionVO.Priority = 4;
				transmissionVO.Btn1Action = "gotowarboard";
			}
			if (dictionary.ContainsKey("guildName"))
			{
				string s = (string)dictionary["guildName"];
				transmissionVO.CurrentSquadName = WWW.UnEscapeURL(s);
			}
			if (dictionary.ContainsKey("guildLevel"))
			{
				transmissionVO.SquadLevel = Convert.ToInt32(dictionary["guildLevel"], CultureInfo.InvariantCulture);
				transmissionVO.Btn1Action = "squadlevelup";
				transmissionVO.Priority = 2;
			}
			if (dictionary.ContainsKey("crateId"))
			{
				transmissionVO.CrateId = (string)dictionary["crateId"];
				transmissionVO.Btn1Action = "dailycratereward";
				transmissionVO.Priority = 5;
			}
			transmissionVO.Uid = stringBuilder.ToString();
			transmissionVO.EndTime = 2147483647;
			return transmissionVO;
		}

		public TransmissionVO()
		{
		}

		protected internal TransmissionVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.CreateFromServerObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).AttackerData);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).BodyText);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Btn1);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Btn1Action);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Btn1Data);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Btn2);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Btn2Action);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Btn2Data);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).CharacterID);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_actionData);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_actionDisplay);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_actionType);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_bodyText);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_btn1);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_btn1action);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_btn1data);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_btn2);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_btn2action);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_btn2data);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_endDate);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_faction);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_image);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_npc);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_startDate);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_titleText);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(TransmissionVO.COLUMN_transType);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).CrateId);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).CurrentSquadName);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).EmpireScore);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).EmpireSquadName);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).EndTime);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Image);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Priority);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).RebelScore);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).RebelSquadName);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).SquadLevel);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).StartTime);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).TitleText);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).TotalPvpRatingDelta);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).TransData);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Type);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).InitBattleData((List<BattleEntry>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).ResetAttackerData();
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).AttackerData = (List<BattleEntry>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).BodyText = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Btn1 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Btn1Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Btn1Data = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Btn2 = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Btn2Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Btn2Data = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).CharacterID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			TransmissionVO.COLUMN_actionData = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			TransmissionVO.COLUMN_actionDisplay = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			TransmissionVO.COLUMN_actionType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			TransmissionVO.COLUMN_bodyText = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			TransmissionVO.COLUMN_btn1 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			TransmissionVO.COLUMN_btn1action = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			TransmissionVO.COLUMN_btn1data = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			TransmissionVO.COLUMN_btn2 = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			TransmissionVO.COLUMN_btn2action = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			TransmissionVO.COLUMN_btn2data = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			TransmissionVO.COLUMN_endDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			TransmissionVO.COLUMN_faction = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			TransmissionVO.COLUMN_image = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			TransmissionVO.COLUMN_npc = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			TransmissionVO.COLUMN_startDate = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			TransmissionVO.COLUMN_titleText = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			TransmissionVO.COLUMN_transType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).CrateId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).CurrentSquadName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).EmpireScore = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).EmpireSquadName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).EndTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Image = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Priority = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).RebelScore = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).RebelSquadName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).SquadLevel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).StartTime = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).TitleText = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).TotalPvpRatingDelta = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).TransData = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Type = (TransmissionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((TransmissionVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
