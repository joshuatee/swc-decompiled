using StaRTS.Externals.Maker.MRSS;
using StaRTS.Externals.Maker.Player;
using StaRTS.FX;
using StaRTS.Main.Configs;
using StaRTS.Main.Controllers.Holonet;
using StaRTS.Main.Controllers.Notifications;
using StaRTS.Main.Controllers.Objectives;
using StaRTS.Main.Controllers.Squads;
using StaRTS.Main.Controllers.VictoryConditions;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Animation;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Controllers.Startup
{
	public class GeneralStartupTask : StartupTask
	{
		public GeneralStartupTask(float startPercentage) : base(startPercentage)
		{
		}

		public override void Start()
		{
			Service.Get<EventManager>().SendEvent(EventId.InitializeGeneralSystemsStart, null);
			new WorldTransitioner();
			new VictoryConditionController();
			new DefensiveBattleController();
			new RaidDefenseController();
			new CombatTriggerManager();
			new TrapViewController();
			new TrapController();
			new BattleController();
			new BattleRecordController();
			new BattlePlaybackController();
			new PostBattleRepairController();
			new ShooterController();
			new TargetingController();
			new TurretAttackController();
			new TroopAttackController();
			new BuffController();
			new TroopAbilityController();
			new ArmoryController();
			new DeployableShardUnlockController();
			new FXManager();
			new AnimationEventManager();
			new StarportDecalManager();
			if (!HardwareProfile.IsLowEndDevice())
			{
				new TerrainBlendController();
			}
			new BuildingTooltipController();
			new CurrencyEffects();
			new CurrencyController();
			new StorageEffects();
			new AnimController();
			new UnlockController();
			new RewardManager();
			new CampaignController();
			new TournamentController();
			new PvpManager();
			new NeighborVisitManager();
			new TransportController();
			new ShuttleController();
			new ShieldEffects();
			new ShieldController();
			new MobilizationEffectsManager();
			new SocialPushNotificationController();
			new FactionIconUpgradeController();
			new TroopDonationTrackController();
			new LimitedEditionItemController();
			VideoPlayerKeepAlive.Init();
			new VideoDataManager();
			if (GameConstants.IsMakerVideoEnabled())
			{
				new ThumbnailManager();
			}
			Service.Get<NotificationController>().Init();
			Service.Get<EventManager>().SendEvent(EventId.InitializeGeneralSystemsEnd, null);
			new TargetedBundleController();
			HolonetController holonetController = new HolonetController();
			holonetController.PrepareContent(new HolonetController.HolonetPreparedDelegate(this.OnHolonetPrepared));
			new InventoryCrateRewardController();
			new ObjectiveManager();
			new ObjectiveController();
		}

		private void OnHolonetPrepared()
		{
			base.Complete();
		}
	}
}
