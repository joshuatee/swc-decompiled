using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Holonet;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers.Holonet;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Controllers.Holonet
{
	public class TransmissionsHolonetController : IHolonetContoller
	{
		private const string TRANSM_TUT_DAILY_CRATE_REBEL_UID = "transm_tut_daily_crate_r";

		private const string TRANSM_TUT_DAILY_CRATE_EMPIRE_UID = "transm_tut_daily_crate_e";

		private TransmissionsHolonetPopupView popupView;

		private TransmissionVO battleVO;

		private int transmissionIndex;

		private List<TransmissionVO> incomingTransmissions;

		private int lastTransmissionTimeViewed;

		public HolonetControllerType ControllerType
		{
			get
			{
				return HolonetControllerType.Transmissions;
			}
		}

		public List<TransmissionVO> Transmissions
		{
			get;
			private set;
		}

		public TransmissionsHolonetController()
		{
			this.Transmissions = new List<TransmissionVO>();
			this.incomingTransmissions = new List<TransmissionVO>();
			this.battleVO = new TransmissionVO();
			this.battleVO.Type = TransmissionType.Battle;
			this.lastTransmissionTimeViewed = 0;
		}

		public void PrepareContent(int lastTimeViewed)
		{
			this.lastTransmissionTimeViewed = lastTimeViewed;
			HolonetGetMessagesCommand holonetGetMessagesCommand = new HolonetGetMessagesCommand(new PlayerIdRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId
			});
			holonetGetMessagesCommand.AddSuccessCallback(new AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>.OnSuccessCallback(this.OnGetMessagesSuccess));
			holonetGetMessagesCommand.AddFailureCallback(new AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>.OnFailureCallback(this.OnGetMessagesFailure));
			Service.Get<ServerAPI>().Sync(holonetGetMessagesCommand);
		}

		private void OnGetMessagesFailure(uint status, object cookie)
		{
			Service.Get<HolonetController>().ContentPrepared(this, 0);
		}

		private void OnGetMessagesSuccess(HolonetGetMessagesResponse response, object cookie)
		{
			this.FinishPreparingTransmissions(response.MessageVOs);
		}

		private void FinishPreparingTransmissions(List<TransmissionVO> msgVOs)
		{
			int num = 0;
			this.Transmissions.Clear();
			this.incomingTransmissions.Clear();
			this.transmissionIndex = 0;
			SquadWarManager warManager = Service.Get<SquadController>().WarManager;
			SharedPlayerPrefs sharedPlayerPrefs = Service.Get<SharedPlayerPrefs>();
			int pref = sharedPlayerPrefs.GetPref<int>("DailyCrateTransTutorialViewed");
			int serverTime = (int)Service.Get<ServerAPI>().ServerTime;
			IDataController dataController = Service.Get<IDataController>();
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			foreach (TransmissionVO current in dataController.GetAll<TransmissionVO>())
			{
				if (current.Faction == currentPlayer.Faction && current.StartTime > 0 && current.StartTime <= serverTime && serverTime < current.EndTime && (!this.IsDailyCrateTutorialTransmission(current) || pref != 1))
				{
					this.Transmissions.Add(current);
					if (current.StartTime > this.lastTransmissionTimeViewed)
					{
						num++;
					}
				}
			}
			HolonetController holonetController = Service.Get<HolonetController>();
			msgVOs.Sort(new Comparison<TransmissionVO>(holonetController.CompareTimestamps));
			int hOLONET_EVENT_MESSAGE_MAX_COUNT = GameConstants.HOLONET_EVENT_MESSAGE_MAX_COUNT;
			int count = msgVOs.Count;
			int num2 = 0;
			while (num2 < hOLONET_EVENT_MESSAGE_MAX_COUNT && num2 < count)
			{
				TransmissionVO transmissionVO = msgVOs[num2];
				if (!this.DuplicateTransmission(transmissionVO))
				{
					this.Transmissions.Add(transmissionVO);
					if (transmissionVO.StartTime > this.lastTransmissionTimeViewed)
					{
						num++;
						if (this.IsWarTransmission(transmissionVO) && !warManager.IsTimeWithinCurrentSquadWarPhase(transmissionVO.StartTime))
						{
							num--;
						}
					}
				}
				else
				{
					Service.Get<StaRTSLogger>().Error("Duplicate entry in transmission event messages repsonse: " + transmissionVO.Uid);
				}
				num2++;
			}
			this.Transmissions.Sort(new Comparison<TransmissionVO>(holonetController.CompareTimestamps));
			int pref2 = sharedPlayerPrefs.GetPref<int>("HighestViewedSquadLvlUP");
			int hOLONET_MAX_INCOMING_TRANSMISSIONS = GameConstants.HOLONET_MAX_INCOMING_TRANSMISSIONS;
			int count2 = this.Transmissions.Count;
			int num3 = 0;
			TransmissionVO transmissionVO2 = null;
			int num4 = 0;
			while (num3 < hOLONET_MAX_INCOMING_TRANSMISSIONS && num4 < count2)
			{
				TransmissionVO transmissionVO3 = this.Transmissions[num4];
				if (this.IsIncomingTransmission(transmissionVO3) && (!this.IsWarTransmission(transmissionVO3) || warManager.IsTimeWithinCurrentSquadWarPhase(transmissionVO3.StartTime)))
				{
					if (this.IsSquadLevelUpTransmission(transmissionVO3))
					{
						if (!Service.Get<PerkManager>().HasPlayerSeenPerkTutorial())
						{
							goto IL_2A7;
						}
						bool flag = transmissionVO2 == null || transmissionVO3.SquadLevel > transmissionVO2.SquadLevel;
						if (!flag || pref2 >= transmissionVO3.SquadLevel)
						{
							goto IL_2A7;
						}
						if (transmissionVO2 != null)
						{
							num3--;
							this.incomingTransmissions.Remove(transmissionVO2);
						}
						transmissionVO2 = transmissionVO3;
					}
					if (pref == 0)
					{
						if (this.IsDailyCrateTransmission(transmissionVO3))
						{
							goto IL_2A7;
						}
						if (this.IsDailyCrateTutorialTransmission(transmissionVO3))
						{
							sharedPlayerPrefs.SetPref("DailyCrateTransTutorialViewed", "1");
						}
					}
					num3++;
					this.incomingTransmissions.Add(transmissionVO3);
				}
				IL_2A7:
				num4++;
			}
			this.incomingTransmissions.Sort(new Comparison<TransmissionVO>(this.CompareIncommingPriorites));
			holonetController.ContentPrepared(this, num);
		}

		private bool IsDailyCrateTransmission(TransmissionVO vo)
		{
			return vo.Type == TransmissionType.DailyCrateReward;
		}

		private bool IsDailyCrateTutorialTransmission(TransmissionVO vo)
		{
			return vo.Uid == "transm_tut_daily_crate_r" || vo.Uid == "transm_tut_daily_crate_e";
		}

		private bool IsSquadLevelUpTransmission(TransmissionVO vo)
		{
			return vo.Type == TransmissionType.GuildLevelUp;
		}

		private bool IsWarTransmission(TransmissionVO vo)
		{
			return vo.Type == TransmissionType.WarPreparation || vo.Type == TransmissionType.WarStart || vo.Type == TransmissionType.WarEnded;
		}

		public void InitTransmissionsTest(List<TransmissionVO> testTransmissions)
		{
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			IDataController dataController = Service.Get<IDataController>();
			this.incomingTransmissions.Clear();
			if (testTransmissions != null)
			{
				foreach (TransmissionVO current in testTransmissions)
				{
					if (current.Faction == currentPlayer.Faction)
					{
						this.incomingTransmissions.Add(current);
					}
				}
			}
			foreach (TransmissionVO current2 in dataController.GetAll<TransmissionVO>())
			{
				if (current2.Faction == currentPlayer.Faction)
				{
					this.incomingTransmissions.Add(current2);
				}
			}
			HolonetController holonetController = Service.Get<HolonetController>();
			holonetController.SetTabCount(this, this.incomingTransmissions.Count);
		}

		private bool DuplicateTransmission(TransmissionVO vo)
		{
			int count = this.Transmissions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.Transmissions[i].Uid == vo.Uid)
				{
					return true;
				}
			}
			return false;
		}

		private int CompareIncommingPriorites(TransmissionVO a, TransmissionVO b)
		{
			return b.Priority - a.Priority;
		}

		private bool IsIncomingTransmission(TransmissionVO vo)
		{
			return this.IsDailyCrateTutorialTransmission(vo) || (vo.StartTime > this.lastTransmissionTimeViewed && (vo.Type == TransmissionType.Conflict || vo.Type == TransmissionType.WarPreparation || vo.Type == TransmissionType.WarStart || vo.Type == TransmissionType.WarEnded || vo.Type == TransmissionType.Generic || vo.Type == TransmissionType.GuildLevelUp || vo.Type == TransmissionType.DailyCrateReward));
		}

		public int GetInCommingTransmissionCount()
		{
			return this.incomingTransmissions.Count;
		}

		public bool HasNewBattles()
		{
			return this.battleVO.AttackerData != null && this.battleVO.AttackerData.Count > 0;
		}

		public void InitBattlesTransmission(List<BattleEntry> battles)
		{
			this.battleVO.InitBattleData(battles);
			if (this.HasNewBattles())
			{
				this.incomingTransmissions.Add(this.battleVO);
				this.incomingTransmissions.Sort(new Comparison<TransmissionVO>(this.CompareIncommingPriorites));
				int hOLONET_MAX_INCOMING_TRANSMISSIONS = GameConstants.HOLONET_MAX_INCOMING_TRANSMISSIONS;
				if (this.incomingTransmissions.Count > hOLONET_MAX_INCOMING_TRANSMISSIONS)
				{
					this.incomingTransmissions.RemoveAt(this.incomingTransmissions.Count - 1);
				}
			}
		}

		public void UpdateIncomingTransmission(int index)
		{
			if (index >= 1)
			{
				this.transmissionIndex = index - 1;
				if (this.transmissionIndex >= this.incomingTransmissions.Count)
				{
					this.transmissionIndex = this.incomingTransmissions.Count - 1;
				}
				else if (this.transmissionIndex < 0)
				{
					this.transmissionIndex = 0;
				}
				this.popupView.RefreshView(this.incomingTransmissions[this.transmissionIndex]);
			}
		}

		public void DismissIncomingTransmission(int index)
		{
			if (index >= 1)
			{
				this.transmissionIndex = index - 1;
				if (this.transmissionIndex < this.incomingTransmissions.Count)
				{
					this.incomingTransmissions.RemoveAt(this.transmissionIndex);
					if (this.transmissionIndex > this.incomingTransmissions.Count)
					{
						this.transmissionIndex = this.incomingTransmissions.Count - 1;
					}
				}
				if (this.transmissionIndex < this.incomingTransmissions.Count)
				{
					this.popupView.RefreshView(this.incomingTransmissions[this.transmissionIndex]);
				}
			}
		}

		public bool IsTransmissionPopupOpened()
		{
			return this.popupView != null;
		}

		public void OnTransmissionPopupClosed()
		{
			if (this.popupView != null)
			{
				this.battleVO.ResetAttackerData();
				this.popupView = null;
				this.incomingTransmissions.Clear();
				this.transmissionIndex = 0;
				Service.Get<HolonetController>().EnableAllHolonetTabButtons();
			}
		}

		public void OnTransmissionPopupIntialized(TransmissionsHolonetPopupView popup)
		{
			Service.Get<HolonetController>().DisableAllHolonetTabButtons();
			this.popupView = popup;
			this.popupView.SetMaxTransmissionIndex(this.incomingTransmissions.Count);
			if (this.HasNewBattles())
			{
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				this.battleVO.CharacterID = GameUtils.GetTransmissionHoloId(currentPlayer.Faction, currentPlayer.Planet.Uid);
				this.battleVO.Faction = currentPlayer.Faction;
			}
			this.popupView.RefreshView(this.incomingTransmissions[this.transmissionIndex]);
		}

		protected internal TransmissionsHolonetController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).CompareIncommingPriorites((TransmissionVO)GCHandledObjects.GCHandleToObject(*args), (TransmissionVO)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).DismissIncomingTransmission(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).DuplicateTransmission((TransmissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).FinishPreparingTransmissions((List<TransmissionVO>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).ControllerType);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).Transmissions);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).GetInCommingTransmissionCount());
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).HasNewBattles());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).InitBattlesTransmission((List<BattleEntry>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).InitTransmissionsTest((List<TransmissionVO>)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).IsDailyCrateTransmission((TransmissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).IsDailyCrateTutorialTransmission((TransmissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).IsIncomingTransmission((TransmissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).IsSquadLevelUpTransmission((TransmissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).IsTransmissionPopupOpened());
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).IsWarTransmission((TransmissionVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).OnGetMessagesSuccess((HolonetGetMessagesResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).OnTransmissionPopupClosed();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).OnTransmissionPopupIntialized((TransmissionsHolonetPopupView)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).PrepareContent(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).Transmissions = (List<TransmissionVO>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((TransmissionsHolonetController)GCHandledObjects.GCHandleToObject(instance)).UpdateIncomingTransmission(*(int*)args);
			return -1L;
		}
	}
}
