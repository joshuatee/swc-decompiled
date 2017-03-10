using Source.StaRTS.Main.Models.Commands.Player;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Commands.Campaign;
using StaRTS.Main.Models.Commands.Cheats;
using StaRTS.Main.Models.Commands.Crates;
using StaRTS.Main.Models.Commands.Equipment;
using StaRTS.Main.Models.Commands.Holonet;
using StaRTS.Main.Models.Commands.Missions;
using StaRTS.Main.Models.Commands.Objectives;
using StaRTS.Main.Models.Commands.Perks;
using StaRTS.Main.Models.Commands.Planets;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Commands.Player.Account.External;
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
using StaRTS.Main.Models.Commands.Squads;
using StaRTS.Main.Models.Commands.Squads.Requests;
using StaRTS.Main.Models.Commands.Squads.Responses;
using StaRTS.Main.Models.Commands.TargetedBundleOffers;
using StaRTS.Main.Models.Commands.Test.Config;
using StaRTS.Main.Models.Commands.Tournament;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands
{
	public abstract class GameCommand<TRequest, TResponse> : AbstractCommand<TRequest, TResponse> where TRequest : AbstractRequest where TResponse : AbstractResponse
	{
		public GameCommand(string action, TRequest request, TResponse response) : base(action, request, response)
		{
		}

		public override void OnSuccess()
		{
		}

		public override OnCompleteAction OnFailure(uint status, object data)
		{
			return this.HandleFailure(status, data);
		}

		private OnCompleteAction HandleFailure(uint status, object data)
		{
			bool flag = true;
			string text = null;
			if (status != 917u)
			{
				if (status != 1900u)
				{
					if (status == 1999u)
					{
						string playerId = Service.Get<CurrentPlayer>().PlayerId;
						text = Service.Get<Lang>().Get("DESYNC_BANNED", new object[]
						{
							playerId
						});
					}
				}
				else
				{
					Service.Get<PlayerIdentityController>().HandleInactiveIdentityError(data as string);
					flag = false;
				}
			}
			else
			{
				text = Service.Get<Lang>().Get("DESYNC_DUPLICATE_SESSION", new object[0]);
			}
			if (!flag)
			{
				return OnCompleteAction.Ok;
			}
			if (text != null)
			{
				string biMessage = text + " Status : " + status;
				AlertScreen.ShowModalWithBI(true, null, text, biMessage);
			}
			return OnCompleteAction.Desync;
		}

		protected OnCompleteAction EatFailure(uint status, object data)
		{
			this.HandleFailure(status, data);
			return OnCompleteAction.Ok;
		}

		protected internal GameCommand(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((GameCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((GameCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((GameCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((GameCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((GameCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((GameCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((GameCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((GameCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((GameCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((GameCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((GameCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((GameCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((GameCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((GameCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((GameCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((GameCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((GameCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((GameCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((GameCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((GameCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((GameCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((GameCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((GameCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((GameCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((GameCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			((GameCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((GameCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((GameCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((GameCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((GameCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((GameCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((GameCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((GameCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((GameCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((GameCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			((GameCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((GameCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((GameCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((GameCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			((GameCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((GameCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((GameCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((GameCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((GameCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((GameCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((GameCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((GameCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((GameCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((GameCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			((GameCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			((GameCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((GameCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((GameCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			((GameCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			((GameCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			((GameCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((GameCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((GameCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((GameCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((GameCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((GameCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((GameCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((GameCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((GameCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((GameCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((GameCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((GameCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((GameCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((GameCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((GameCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((GameCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((GameCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((GameCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			((GameCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			((GameCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			((GameCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			((GameCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			((GameCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			((GameCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			((GameCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			((GameCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			((GameCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			((GameCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			((GameCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			((GameCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			((GameCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			((GameCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			((GameCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			((GameCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			((GameCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			((GameCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			((GameCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			((GameCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			((GameCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			((GameCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			((GameCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			((GameCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			((GameCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			((GameCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			((GameCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			((GameCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			((GameCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			((GameCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			((GameCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			((GameCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			((GameCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			((GameCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			((GameCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			((GameCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			((GameCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			((GameCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			((GameCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			((GameCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			((GameCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			((GameCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			((GameCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			((GameCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			((GameCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			((GameCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			((GameCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			((GameCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			((GameCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			((GameCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}
	}
}
