using StaRTS.Externals.FileManagement;
using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.MetaData;
using System;

namespace StaRTS.Main.Controllers.Startup
{
	public class PlayerContentStartupTask : StartupTask
	{
		private const string FAILED_MESSAGE = "FMS failed to initialize.";

		private const string STRINGS_PATH_PREFIX = "strings/";

		private GetContentResponse response;

		private Catalog catalog;

		private int currentPatchIndex;

		public PlayerContentStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			EventManager eventManager = Service.Get<EventManager>();
			eventManager.SendEvent(EventId.InitialLoadStart, null);
			GetContentCommand getContentCommand = new GetContentCommand(new GetContentRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId
			});
			getContentCommand.AddSuccessCallback(new AbstractCommand<GetContentRequest, GetContentResponse>.OnSuccessCallback(this.OnCommandComplete));
			Service.Get<ServerAPI>().Async(getContentCommand);
		}

		private void OnCommandComplete(GetContentResponse response, object cookie)
		{
			this.response = response;
			FMS fMS = new FMS();
			fMS.Init(new FmsOptions
			{
				CodeName = response.AppCode,
				Env = StringUtils.ParseEnum<FmsEnvironment>(response.Environment),
				ManifestVersion = response.ManifestVersion,
				Mode = FmsMode.Versioned,
				LocalRootUrl = "http://localhost/",
				RemoteRootUrl = response.CdnRoots[0],
				Engine = Service.Get<Engine>()
			}, new FmsCallback(this.OnFmsComplete), new FmsCallback(this.OnFmsError));
			Service.Get<CurrentPlayer>().Patches = response.Patches;
		}

		private void OnFmsComplete()
		{
			Service.Get<EventManager>().SendEvent(EventId.MetaDataLoadStart, null);
			this.catalog = new Catalog();
			this.currentPatchIndex = -1;
			this.ProcessNextPatch();
		}

		private void OnFmsError()
		{
			AlertScreen.ShowModalWithBI(true, null, "FMS failed to initialize.", "FMS ERROR");
		}

		private void ProcessNextPatch()
		{
			this.currentPatchIndex++;
			this.catalog.PatchData(this.response.Patches[this.currentPatchIndex], new Catalog.CatalogDelegate(this.OnPatchComplete));
		}

		private void OnPatchComplete(bool success, string file)
		{
			while (this.currentPatchIndex < this.response.Patches.Count - 1)
			{
				string text = this.response.Patches[this.currentPatchIndex + 1];
				if (!text.StartsWith("strings/"))
				{
					this.ProcessNextPatch();
					return;
				}
				this.currentPatchIndex++;
			}
			this.OnAllPatchesComplete();
		}

		private void SetupStaticDataController()
		{
			new ValueObjectController();
			StaticDataController staticDataController = new StaticDataController();
			staticDataController.Load<AudioTypeVO>(this.catalog, "AudioData");
			staticDataController.Load<BattleTypeVO>(this.catalog, "BattleData");
			staticDataController.Load<BuffTypeVO>(this.catalog, "BuffData");
			staticDataController.Load<ProjectileTypeVO>(this.catalog, "ProjectileData");
			staticDataController.Load<TroopAbilityVO>(this.catalog, "HeroAbilities");
			staticDataController.Load<BuildingConnectorTypeVO>(this.catalog, "BuildingConnectorData");
			staticDataController.Load<AchievementVO>(this.catalog, "AchievementData");
			staticDataController.Load<CommonAssetVO>(this.catalog, "CommonAssetData");
			staticDataController.Load<PlanetVO>(this.catalog, "PlanetData");
			staticDataController.Load<BuildingTypeVO>(this.catalog, "BuildingData");
			staticDataController.Load<TrapTypeVO>(this.catalog, "TrapData");
			staticDataController.Load<EffectsTypeVO>(this.catalog, "EffectsData");
			staticDataController.Load<ShaderTypeVO>(this.catalog, "ShaderData");
			staticDataController.Load<AssetTypeVO>(this.catalog, "AssetData");
			staticDataController.Load<UITypeVO>(this.catalog, "UIData");
			staticDataController.Load<GameConstantsVO>(this.catalog, "GameConstants");
			staticDataController.Load<ProfanityVO>(this.catalog, "Profanity");
			staticDataController.Load<TroopTypeVO>(this.catalog, "TroopData");
			staticDataController.Load<DefaultLightingVO>(this.catalog, "PlanetaryLighting");
			staticDataController.Load<CivilianTypeVO>(this.catalog, "CivilianData");
			staticDataController.Load<TransportTypeVO>(this.catalog, "TransportData");
			staticDataController.Load<SpecialAttackTypeVO>(this.catalog, "SpecialAttackData");
			staticDataController.Load<StoryActionVO>(this.catalog, "StoryActions");
			staticDataController.Load<StoryTriggerVO>(this.catalog, "StoryTriggers");
			staticDataController.Load<ConditionVO>(this.catalog, "Conditions");
			staticDataController.Load<DefenseEncounterVO>(this.catalog, "DefenseEncounters");
			staticDataController.Load<CampaignVO>(this.catalog, "CampaignData");
			staticDataController.Load<CampaignMissionVO>(this.catalog, "CampaignMissionData");
			staticDataController.Load<CharacterVO>(this.catalog, "CharacterData");
			staticDataController.Load<TextureVO>(this.catalog, "TextureData");
			staticDataController.Load<TournamentVO>(this.catalog, "TournamentData");
			staticDataController.Load<TournamentTierVO>(this.catalog, "TournamentTierData");
			staticDataController.Load<NotificationTypeVO>(this.catalog, "Notifications");
			staticDataController.Load<InAppPurchaseTypeVO>(this.catalog, "InAppPurchases");
			staticDataController.Load<SaleItemTypeVO>(this.catalog, "SaleItems");
			staticDataController.Load<SaleTypeVO>(this.catalog, "Sales");
			staticDataController.Load<RewardVO>(this.catalog, "RewardData");
			staticDataController.Load<TournamentRewardsVO>(this.catalog, "TournamentRewards");
			staticDataController.Load<TurretTypeVO>(this.catalog, "TurretData");
			staticDataController.Load<EncounterProfileVO>(this.catalog, "DefenseEncountersProfiles");
			staticDataController.Load<MobilizationHologramVO>(this.catalog, "MobilizationHologram");
			staticDataController.Load<BattleScriptVO>(this.catalog, "BattleScripts");
			staticDataController.Load<DevNoteEntryVO>(this.catalog, "DevNotes");
			staticDataController.Load<CommandCenterVO>(this.catalog, "CommandCenterEntries");
			staticDataController.Load<TransmissionVO>(this.catalog, "Transmissions");
			staticDataController.Load<TransmissionCharacterVO>(this.catalog, "TransmissionCharacters");
			staticDataController.Load<LimitedTimeRewardVO>(this.catalog, "LimitedTimeRewards");
			staticDataController.Load<IconUpgradeVO>(this.catalog, "FactionIcons");
			staticDataController.Load<ObjectiveVO>(this.catalog, "ObjTable");
			staticDataController.Load<ObjectiveSeriesVO>(this.catalog, "ObjSeries");
			staticDataController.Load<CrateTierVO>(this.catalog, "CrateTiers");
			staticDataController.Load<DataCardTierVO>(this.catalog, "DataCardTiers");
			staticDataController.Load<CrateFlyoutItemVO>(this.catalog, "CrateFlyoutItem");
			staticDataController.Load<CurrencyIconVO>(this.catalog, "CurrencyType");
			staticDataController.Load<RaidMissionPoolVO>(this.catalog, "RaidMissionPool");
			staticDataController.Load<RaidVO>(this.catalog, "Raid");
			staticDataController.Load<WarBuffVO>(this.catalog, "WarBuffData");
			staticDataController.Load<WarScheduleVO>(this.catalog, "WarSchedule");
			staticDataController.Load<SquadLevelVO>(this.catalog, "SquadLevel");
			staticDataController.Load<PerkEffectVO>(this.catalog, "PerkEffects");
			staticDataController.Load<PerkVO>(this.catalog, "Perks");
			staticDataController.Load<TargetedBundleVO>(this.catalog, "SpecialPromo");
			staticDataController.Load<EquipmentVO>(this.catalog, "EquipmentData");
			staticDataController.Load<EquipmentEffectVO>(this.catalog, "EquipmentEffectData");
			staticDataController.Load<SkinOverrideTypeVO>(this.catalog, "SkinOverrideData");
			staticDataController.Load<SkinTypeVO>(this.catalog, "SkinData");
			staticDataController.Load<CrateSupplyVO>(this.catalog, "CrateSupply");
			staticDataController.Load<CrateVO>(this.catalog, "Crate");
			staticDataController.Load<CrateSupplyScaleVO>(this.catalog, "CrateSupplyScale");
			staticDataController.Load<ShardVO>(this.catalog, "Shard");
			staticDataController.Load<PlanetLootVO>(this.catalog, "PlanetLoot");
			staticDataController.Load<PlanetLootEntryVO>(this.catalog, "PlanetLootEntry");
			staticDataController.Load<LimitedEditionItemVO>(this.catalog, "LimitedEditionItemData");
			staticDataController.Load<TroopUniqueAbilityDescVO>(this.catalog, "UISupplemental");
			this.catalog = null;
		}

		private void OnAllPatchesComplete()
		{
			this.SetupStaticDataController();
			new AchievementController();
			GameConstants.Initialize();
			Service.Get<ServerAPI>().SetKeepAlive(new KeepAliveCommand(new KeepAliveRequest()), GameConstants.KEEP_ALIVE_DISPATCH_WAIT_TIME);
			Service.Get<EventManager>().SendEvent(EventId.MetaDataLoadEnd, null);
			Service.Get<LeaderboardController>().InitLeaderBoardListForPlanet();
			Service.Get<CurrentPlayer>().DoPostContentInitialization();
			base.Complete();
		}
	}
}
