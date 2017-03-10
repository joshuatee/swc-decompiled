using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Commands.Campaign;
using StaRTS.Main.Models.Commands.Cheats;
using StaRTS.Main.Models.Commands.Crates;
using StaRTS.Main.Models.Commands.Equipment;
using StaRTS.Main.Models.Commands.Missions;
using StaRTS.Main.Models.Commands.Perks;
using StaRTS.Main.Models.Commands.Planets;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Commands.Player.Building.Clear;
using StaRTS.Main.Models.Commands.Player.Building.Collect;
using StaRTS.Main.Models.Commands.Player.Building.Construct;
using StaRTS.Main.Models.Commands.Player.Building.Contracts;
using StaRTS.Main.Models.Commands.Player.Building.Move;
using StaRTS.Main.Models.Commands.Player.Building.Rearm;
using StaRTS.Main.Models.Commands.Player.Building.Swap;
using StaRTS.Main.Models.Commands.Player.Deployable;
using StaRTS.Main.Models.Commands.Player.Deployable.Upgrade.Start;
using StaRTS.Main.Models.Commands.Player.Fue;
using StaRTS.Main.Models.Commands.Player.Identity;
using StaRTS.Main.Models.Commands.Player.Raids;
using StaRTS.Main.Models.Commands.Player.Store;
using StaRTS.Main.Models.Commands.Pvp;
using StaRTS.Main.Models.Commands.Squads.Requests;
using StaRTS.Main.Models.Commands.Squads.Responses;
using StaRTS.Main.Models.Commands.TargetedBundleOffers;
using StaRTS.Main.Models.Commands.Tournament;
using StaRTS.Main.Models.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands
{
	public abstract class GameActionCommand<TRequest, TResponse> : GameCommand<TRequest, TResponse> where TRequest : PlayerIdChecksumRequest where TResponse : AbstractResponse
	{
		public const uint UNSYNCHRONIZED = 5000u;

		private uint status;

		public GameActionCommand(string action, TRequest request, TResponse response) : base(action, request, response)
		{
		}

		public override OnCompleteAction OnComplete(Data data, bool success)
		{
			this.status = data.Status;
			return base.OnComplete(data, success);
		}

		public override void OnSuccess()
		{
			if (this.status == 5001u)
			{
				this.SendPlayerError();
			}
			base.OnSuccess();
		}

		public override OnCompleteAction OnFailure(uint status, object data)
		{
			if (status == 5000u)
			{
				Service.Get<StaRTSLogger>().Debug(base.RequestArgs.ChecksumInfoString);
				this.SendPlayerError();
			}
			return base.OnFailure(status, data);
		}

		private void SendPlayerError()
		{
			PlayerErrorCommand command = new PlayerErrorCommand(new PlayerErrorRequest
			{
				Prefix = "DESYNC:",
				PlayerId = Service.Get<CurrentPlayer>().PlayerId,
				ClientCheckSumString = base.RequestArgs.ChecksumInfoString
			});
			Service.Get<ServerAPI>().Async(command);
		}

		protected internal GameActionCommand(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GameActionCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GameActionCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((GameActionCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((GameActionCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((GameActionCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((GameActionCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((GameActionCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((GameActionCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((GameActionCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((GameActionCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((GameActionCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((GameActionCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((GameActionCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((GameActionCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((GameActionCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((GameActionCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((GameActionCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((GameActionCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((GameActionCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((GameActionCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((GameActionCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((GameActionCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((GameActionCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((GameActionCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((GameActionCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((GameActionCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((GameActionCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((GameActionCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((GameActionCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((GameActionCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((GameActionCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((GameActionCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((GameActionCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((GameActionCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((GameActionCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((GameActionCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((GameActionCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((GameActionCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((GameActionCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((GameActionCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((GameActionCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((GameActionCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((GameActionCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((GameActionCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			((GameActionCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((GameActionCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			((GameActionCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			((GameActionCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((GameActionCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((GameActionCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((GameActionCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((GameActionCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((GameActionCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((GameActionCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((GameActionCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((GameActionCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((GameActionCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((GameActionCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((GameActionCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((GameActionCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			((GameActionCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			((GameActionCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			((GameActionCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			((GameActionCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			((GameActionCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			((GameActionCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			((GameActionCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			((GameActionCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			((GameActionCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			((GameActionCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			((GameActionCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			((GameActionCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			((GameActionCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			((GameActionCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			((GameActionCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			((GameActionCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			((GameActionCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			((GameActionCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			((GameActionCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			((GameActionCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			((GameActionCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			((GameActionCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			((GameActionCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			((GameActionCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			((GameActionCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			((GameActionCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			((GameActionCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			((GameActionCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			((GameActionCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			((GameActionCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			((GameActionCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			((GameActionCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GameActionCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			((GameActionCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			((GameActionCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).SendPlayerError();
			return -1L;
		}
	}
}
