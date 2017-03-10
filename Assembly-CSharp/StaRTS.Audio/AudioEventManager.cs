using Net.RichardLord.Ash.Core;
using StaRTS.Main.Controllers;
using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Controllers.World;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Main.Views.World;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using StaRTS.Utils.State;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Audio
{
	public class AudioEventManager : IEventObserver
	{
		private const float FIXED_MUSIC_LOOP_DELAY = 7f;

		private AudioManager audioManager;

		private EventManager eventManager;

		private uint timerId;

		private List<StrIntPair> droidClips;

		private float galaxyUIPlanetFocusThrottle;

		public AudioEventManager(AudioManager audioManager)
		{
			this.audioManager = audioManager;
			this.eventManager = Service.Get<EventManager>();
			this.droidClips = new List<StrIntPair>();
			this.droidClips.Add(new StrIntPair("sfx_ui_droid_1", 25));
			this.droidClips.Add(new StrIntPair("sfx_ui_droid_2", 25));
			this.droidClips.Add(new StrIntPair("sfx_ui_droid_3", 25));
			this.droidClips.Add(new StrIntPair("sfx_ui_droid_4", 25));
			this.eventManager.RegisterObserver(this, EventId.BattleLogScreenTabSelected);
			this.eventManager.RegisterObserver(this, EventId.BattleLogScreenRevengeButtonClicked);
			this.eventManager.RegisterObserver(this, EventId.BattleLogScreenReplayButtonClicked);
			this.eventManager.RegisterObserver(this, EventId.BattleLogScreenShareButtonClicked);
			this.eventManager.RegisterObserver(this, EventId.ShooterWarmingUp);
			this.eventManager.RegisterObserver(this, EventId.EntityAttackedTarget);
			this.eventManager.RegisterObserver(this, EventId.ProjectileViewImpacted);
			this.eventManager.RegisterObserver(this, EventId.EntityKilled);
			this.eventManager.RegisterObserver(this, EventId.TroopPlacedOnBoard);
			this.eventManager.RegisterObserver(this, EventId.ButtonClicked);
			this.eventManager.RegisterObserver(this, EventId.ContextButtonClicked);
			this.eventManager.RegisterObserver(this, EventId.GameStateChanged);
			this.eventManager.RegisterObserver(this, EventId.MusicUnmuted);
			this.eventManager.RegisterObserver(this, EventId.TroopNotPlacedInvalidArea);
			this.eventManager.RegisterObserver(this, EventId.TroopNotPlacedInvalidTroop);
			this.eventManager.RegisterObserver(this, EventId.HeroNotActivated);
			this.eventManager.RegisterObserver(this, EventId.TroopAbilityActivate);
			this.eventManager.RegisterObserver(this, EventId.ProjectileViewDeflected);
			this.eventManager.RegisterObserver(this, EventId.ScreenClosing);
			this.eventManager.RegisterObserver(this, EventId.ScreenLoaded);
			this.eventManager.RegisterObserver(this, EventId.UserLiftedBuildingAudio);
			this.eventManager.RegisterObserver(this, EventId.UserGridMovedBuildingAudio);
			this.eventManager.RegisterObserver(this, EventId.UserLoweredBuildingAudio);
			this.eventManager.RegisterObserver(this, EventId.BuildingPurchaseCanceled);
			this.eventManager.RegisterObserver(this, EventId.BuildingPurchaseConfirmed);
			this.eventManager.RegisterObserver(this, EventId.BuildingSelectedFromStore);
			this.eventManager.RegisterObserver(this, EventId.BuildingSelected);
			this.eventManager.RegisterObserver(this, EventId.StoreCategorySelected);
			this.eventManager.RegisterObserver(this, EventId.BackButtonClicked);
			this.eventManager.RegisterObserver(this, EventId.HoloEvent);
			this.eventManager.RegisterObserver(this, EventId.PlayHoloGreet);
			this.eventManager.RegisterObserver(this, EventId.StoryTranscriptDisplayed);
			this.eventManager.RegisterObserver(this, EventId.SpecialAttackDeployed);
			this.eventManager.RegisterObserver(this, EventId.SpecialAttackDropshipFlyingAway);
			this.eventManager.RegisterObserver(this, EventId.SpecialAttackFired);
			this.eventManager.RegisterObserver(this, EventId.StoryAttackButtonClicked);
			this.eventManager.RegisterObserver(this, EventId.StoryNextButtonClicked);
			this.eventManager.RegisterObserver(this, EventId.StorySkipButtonClicked);
			this.eventManager.RegisterObserver(this, EventId.HolonetChangeTabs);
			this.eventManager.RegisterObserver(this, EventId.HolonetNextPrevTransmision);
			this.eventManager.RegisterObserver(this, EventId.StarEarned);
			this.eventManager.RegisterObserver(this, EventId.BattleEndVictoryStarDisplayed);
			this.eventManager.RegisterObserver(this, EventId.IntroStarted);
			this.eventManager.RegisterObserver(this, EventId.CurrencyCollected);
			this.eventManager.RegisterObserver(this, EventId.AudibleCurrencySpent);
			this.eventManager.RegisterObserver(this, EventId.BuildingLevelUpgraded);
			this.eventManager.RegisterObserver(this, EventId.StarshipLevelUpgraded);
			this.eventManager.RegisterObserver(this, EventId.TroopLevelUpgraded);
			this.eventManager.RegisterObserver(this, EventId.EquipmentUpgraded);
			this.eventManager.RegisterObserver(this, EventId.TroopRecruited);
			this.eventManager.RegisterObserver(this, EventId.StarshipMobilized);
			this.eventManager.RegisterObserver(this, EventId.StarshipMobilizedFromPrize);
			this.eventManager.RegisterObserver(this, EventId.HeroMobilized);
			this.eventManager.RegisterObserver(this, EventId.HeroMobilizedFromPrize);
			this.eventManager.RegisterObserver(this, EventId.TransportArrived);
			this.eventManager.RegisterObserver(this, EventId.TransportDeparted);
			this.eventManager.RegisterObserver(this, EventId.InitiatedBuyout);
			this.eventManager.RegisterObserver(this, EventId.ContractAdded);
			this.eventManager.RegisterObserver(this, EventId.BuildingConstructed);
			this.eventManager.RegisterObserver(this, EventId.ShuttleAnimStateChanged);
			this.eventManager.RegisterObserver(this, EventId.ShieldBorderDestroyed);
			this.eventManager.RegisterObserver(this, EventId.ShieldStarted);
			this.eventManager.RegisterObserver(this, EventId.ChampionShieldActivated);
			this.eventManager.RegisterObserver(this, EventId.ChampionShieldDeactivated);
			this.eventManager.RegisterObserver(this, EventId.ChampionShieldDestroyed);
			this.eventManager.RegisterObserver(this, EventId.SquadEdited);
			this.eventManager.RegisterObserver(this, EventId.SquadSelect);
			this.eventManager.RegisterObserver(this, EventId.SquadSend);
			this.eventManager.RegisterObserver(this, EventId.SquadNext);
			this.eventManager.RegisterObserver(this, EventId.SquadMore);
			this.eventManager.RegisterObserver(this, EventId.SquadFB);
			this.eventManager.RegisterObserver(this, EventId.SquadCredits);
			this.eventManager.RegisterObserver(this, EventId.InfoButtonClicked);
			this.eventManager.RegisterObserver(this, EventId.MissionActionButtonClicked);
			this.eventManager.RegisterObserver(this, EventId.InventoryResourceUpdated);
			this.eventManager.RegisterObserver(this, EventId.LongPressStarted);
			this.eventManager.RegisterObserver(this, EventId.ApplicationPauseToggled);
			this.eventManager.RegisterObserver(this, EventId.DeviceMusicPlayerStateChanged);
			this.eventManager.RegisterObserver(this, EventId.WorldInTransitionComplete);
			this.eventManager.RegisterObserver(this, EventId.MuteEvent);
			this.eventManager.RegisterObserver(this, EventId.UnmuteEvent);
			this.eventManager.RegisterObserver(this, EventId.SimulateAudioEvent);
			this.eventManager.RegisterObserver(this, EventId.HQCelebrationPlayed);
			this.eventManager.RegisterObserver(this, EventId.TrapTriggered);
			this.eventManager.RegisterObserver(this, EventId.TrapDestroyed);
			this.eventManager.RegisterObserver(this, EventId.DroidPurchaseCancelled);
			this.eventManager.RegisterObserver(this, EventId.TroopLoadingIntoStarport);
			this.eventManager.RegisterObserver(this, EventId.TroopPlacedInsideShieldError);
			this.eventManager.RegisterObserver(this, EventId.TroopPlacedInsideBuildingError);
			this.eventManager.RegisterObserver(this, EventId.TextCrawlStarted);
			this.eventManager.RegisterObserver(this, EventId.TextCrawlComplete);
			this.eventManager.RegisterObserver(this, EventId.PlanetRelocateStarted);
			this.eventManager.RegisterObserver(this, EventId.GalaxyGoToGalaxyView);
			this.eventManager.RegisterObserver(this, EventId.GalaxyGoToPlanetView);
			this.eventManager.RegisterObserver(this, EventId.GalaxyPlanetTapped);
			this.eventManager.RegisterObserver(this, EventId.PlanetConfirmRelocate);
			this.eventManager.RegisterObserver(this, EventId.GalaxyTransitionToNextPlanet);
			this.eventManager.RegisterObserver(this, EventId.GalaxyTransitionToPreviousPlanet);
			this.eventManager.RegisterObserver(this, EventId.GalaxyNotEnoughRelocateStarsClose);
			this.eventManager.RegisterObserver(this, EventId.GalaxyUIPlanetFocus);
			this.eventManager.RegisterObserver(this, EventId.ObjectiveDetailsClicked);
			this.eventManager.RegisterObserver(this, EventId.ObjectiveCrateInfoScreenOpened);
			this.eventManager.RegisterObserver(this, EventId.ObjectiveCompleted);
			this.eventManager.RegisterObserver(this, EventId.ObjectiveRewardDataCardRevealed);
			this.eventManager.RegisterObserver(this, EventId.UIFilterSelected);
			this.eventManager.RegisterObserver(this, EventId.PerkSelected);
			this.eventManager.RegisterObserver(this, EventId.TroopDonationTrackRewardReceived);
			this.eventManager.RegisterObserver(this, EventId.SquadLeveledUpCelebration);
			this.eventManager.RegisterObserver(this, EventId.PerkActivated);
			this.eventManager.RegisterObserver(this, EventId.PerkInvested);
			this.eventManager.RegisterObserver(this, EventId.PerkCelebStarted);
			this.eventManager.RegisterObserver(this, EventId.DeployableUnlockCelebrationPlayed);
			this.eventManager.RegisterObserver(this, EventId.EquipmentUnlockCelebrationPlayed);
			this.eventManager.RegisterObserver(this, EventId.EquipmentActivated);
			this.eventManager.RegisterObserver(this, EventId.EquipmentDataFragmentEarned);
			this.eventManager.RegisterObserver(this, EventId.SupplyCrateProgressBar);
			this.eventManager.RegisterObserver(this, EventId.InventoryCrateAnimationStateChange);
			this.eventManager.RegisterObserver(this, EventId.InventoryCrateAnimVFXTriggered);
			this.eventManager.RegisterObserver(this, EventId.CrateRewardIdleHop);
		}

		private void PlayInventoryCrateAnimSFXBasedOnState(InventoryCrateAnimationState animState)
		{
			switch (animState)
			{
			case InventoryCrateAnimationState.Landed:
				this.audioManager.PlayAudio("sfx_rewards_crate_land");
				return;
			case InventoryCrateAnimationState.Open:
				this.audioManager.PlayAudio("sfx_rewards_crate_open");
				return;
			case InventoryCrateAnimationState.ShowPBar:
				this.audioManager.PlayAudio("sfx_rewards_progressbar");
				return;
			case InventoryCrateAnimationState.Hop:
				this.audioManager.PlayAudio("sfx_rewards_crate_hop");
				return;
			default:
				return;
			}
		}

		private void PlayInventoryCrateAnimVfxSfx(string sfxName)
		{
			this.audioManager.PlayAudio(sfxName);
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.PlayHoloGreet)
			{
				if (id <= EventId.GameStateChanged)
				{
					if (id <= EventId.EntityKilled)
					{
						if (id <= EventId.AudibleCurrencySpent)
						{
							if (id == EventId.ApplicationPauseToggled)
							{
								this.HandleApplicationPause((bool)cookie);
								return EatResponse.NotEaten;
							}
							switch (id)
							{
							case EventId.BuildingPurchaseCanceled:
								this.audioManager.PlayAudio("sfx_ui_placement_stop");
								return EatResponse.NotEaten;
							case EventId.BuildingPurchaseConfirmed:
								this.audioManager.PlayAudio("sfx_ui_placement_confirm");
								return EatResponse.NotEaten;
							case EventId.BuildingPurchaseSuccess:
							case EventId.BuildingPurchaseModeStarted:
							case EventId.BuildingPurchaseModeEnded:
							case EventId.BuildingStartedUpgrading:
							case EventId.BuildingSelected:
							case EventId.BuildingDeselected:
							case EventId.BuildingQuickStashed:
							case EventId.DroidPurchaseAnimationComplete:
							case EventId.DroidPurchaseCompleted:
								return EatResponse.NotEaten;
							case EventId.BuildingSelectedFromStore:
								goto IL_8E6;
							case EventId.BuildingSelectedSound:
								this.audioManager.PlayAudio("sfx_button_selectbuilding");
								return EatResponse.NotEaten;
							case EventId.StoreCategorySelected:
								break;
							case EventId.BackButtonClicked:
								this.audioManager.PlayAudio("sfx_button_back");
								return EatResponse.NotEaten;
							case EventId.DroidPurchaseCancelled:
								this.audioManager.PlayAudio("sfx_ui_droid_purchase_cancel");
								return EatResponse.NotEaten;
							case EventId.DeviceMusicPlayerStateChanged:
								this.HandleDeviceMusicPlayerStateChanged((bool)cookie);
								return EatResponse.NotEaten;
							case EventId.CurrencyCollected:
								this.PlayCurrencyCollectionEffect(((CurrencyCollectionTag)cookie).Type);
								return EatResponse.NotEaten;
							case EventId.AudibleCurrencySpent:
								switch ((CurrencyType)cookie)
								{
								case CurrencyType.Credits:
									this.audioManager.PlayAudio("sfx_button_usecredits");
									return EatResponse.NotEaten;
								case CurrencyType.Materials:
									this.audioManager.PlayAudio("sfx_button_usematerials");
									return EatResponse.NotEaten;
								case CurrencyType.Contraband:
									this.audioManager.PlayAudio("sfx_button_usematerials");
									return EatResponse.NotEaten;
								default:
									return EatResponse.NotEaten;
								}
								break;
							default:
								return EatResponse.NotEaten;
							}
						}
						else if (id != EventId.EntityAttackedTarget)
						{
							if (id != EventId.EntityKilled)
							{
								return EatResponse.NotEaten;
							}
							SmartEntity smartEntity = cookie as SmartEntity;
							if (smartEntity.TroopComp == null || smartEntity.TroopComp.TroopType.Type == TroopType.Vehicle || LangUtils.ShouldPlayVOClips())
							{
								this.PlayBattleSound(smartEntity, AudioCollectionType.Death);
								return EatResponse.NotEaten;
							}
							return EatResponse.NotEaten;
						}
						else
						{
							SmartEntity smartEntity = cookie as SmartEntity;
							if (smartEntity != null && smartEntity.TroopComp != null && smartEntity.TroopComp.IsAbilityModeActive)
							{
								TroopAbilityVO abilityVO = smartEntity.TroopComp.AbilityVO;
								string randomClip = this.audioManager.GetRandomClip(abilityVO.AudioAbilityAttack);
								this.audioManager.PlayAudio(randomClip);
								return EatResponse.NotEaten;
							}
							this.PlayBattleSound(smartEntity, AudioCollectionType.Attack);
							return EatResponse.NotEaten;
						}
					}
					else if (id <= EventId.WorldInTransitionComplete)
					{
						switch (id)
						{
						case EventId.ProjectileViewImpacted:
						{
							SmartEntity smartEntity = (SmartEntity)cookie;
							this.PlayBattleSound(smartEntity, AudioCollectionType.Impact);
							return EatResponse.NotEaten;
						}
						case EventId.ProjectileViewPathComplete:
						case EventId.DamagePercentUpdated:
						case EventId.StarportMeterUpdated:
						case EventId.SquadLeaderboardUpdated:
						case EventId.HolonetLeaderBoardUpdated:
						case EventId.ClearableCleared:
						case EventId.ClearableStarted:
						case EventId.StartupTasksCompleted:
						case EventId.TroopDeployed:
						case EventId.ChampionStartedRepairing:
						case EventId.ChampionRepaired:
						case EventId.TroopDonationTrackProgressUpdated:
							return EatResponse.NotEaten;
						case EventId.HolonetChangeTabs:
						case EventId.HolonetNextPrevTransmision:
							goto IL_99A;
						case EventId.TransportArrived:
							this.audioManager.PlayAudio("sfx_ui_vehiclepickup");
							return EatResponse.NotEaten;
						case EventId.TransportDeparted:
							this.audioManager.PlayAudio("sfx_ui_vehicletransport");
							return EatResponse.NotEaten;
						case EventId.TroopPlacedOnBoard:
						{
							SmartEntity smartEntity = cookie as SmartEntity;
							TroopComponent troopComp = smartEntity.TroopComp;
							if (smartEntity.ShooterComp != null && smartEntity.ShooterComp.isSkinned)
							{
								this.audioManager.PlayAudio("sfx_placement_skinned_notify_01");
							}
							if (troopComp.TroopType.Type == TroopType.Vehicle || LangUtils.ShouldPlayVOClips())
							{
								this.PlayBattleSound(troopComp.AudioVO, AudioCollectionType.Placement);
								return EatResponse.NotEaten;
							}
							return EatResponse.NotEaten;
						}
						case EventId.TroopRecruited:
						{
							ContractEventData contractEventData = (ContractEventData)cookie;
							if (contractEventData.Silent)
							{
								return EatResponse.NotEaten;
							}
							BuildingType type = contractEventData.BuildingVO.Type;
							if (type == BuildingType.Factory)
							{
								this.audioManager.PlayAudio("sfx_ui_vehiclecompleted_1");
								return EatResponse.NotEaten;
							}
							if ((type == BuildingType.Barracks || type == BuildingType.Cantina) && LangUtils.ShouldPlayVOClips())
							{
								TroopTypeVO audioType = Service.Get<IDataController>().Get<TroopTypeVO>(contractEventData.Contract.ProductUid);
								this.PlayRandomClip(audioType, AudioCollectionType.Train);
								return EatResponse.NotEaten;
							}
							return EatResponse.NotEaten;
						}
						case EventId.TroopLoadingIntoStarport:
							this.audioManager.PlayAudio("sfx_ui_troopload_1");
							return EatResponse.NotEaten;
						case EventId.StarshipMobilized:
						case EventId.HeroMobilized:
						case EventId.StarshipMobilizedFromPrize:
						case EventId.HeroMobilizedFromPrize:
							goto IL_A47;
						case EventId.TroopNotPlacedInvalidArea:
						case EventId.TroopNotPlacedInvalidTroop:
						case EventId.TroopPlacedInsideShieldError:
						case EventId.TroopPlacedInsideBuildingError:
						case EventId.HeroNotActivated:
							this.audioManager.PlayAudio("sfx_ui_placement_error");
							return EatResponse.NotEaten;
						case EventId.TroopDonationTrackRewardReceived:
							this.audioManager.PlayAudio("sfx_stinger_perk_donation_rep_reward");
							return EatResponse.NotEaten;
						default:
							switch (id)
							{
							case EventId.SquadEdited:
								this.audioManager.PlayAudio("sfx_button_squad_edit");
								return EatResponse.NotEaten;
							case EventId.SquadSelect:
								this.audioManager.PlayAudio("sfx_button_squad_select");
								return EatResponse.NotEaten;
							case EventId.SquadSend:
								this.audioManager.PlayAudio("sfx_button_squad_chat");
								return EatResponse.NotEaten;
							case EventId.SquadNext:
								this.audioManager.PlayAudio("sfx_button_next");
								return EatResponse.NotEaten;
							case EventId.SquadMore:
								this.audioManager.PlayAudio("sfx_button_more_info");
								return EatResponse.NotEaten;
							case EventId.SquadFB:
								this.audioManager.PlayAudio("sfx_button_facebook");
								return EatResponse.NotEaten;
							case EventId.SquadCredits:
								this.audioManager.PlayAudio("sfx_button_usecredits");
								return EatResponse.NotEaten;
							case EventId.VisitPlayer:
							case EventId.TroopViewReady:
							case EventId.BuildingSwapped:
							case EventId.BuildingReplaced:
							case EventId.SpecialAttackSpawned:
							case EventId.HudComplete:
							case EventId.IntroComplete:
							case EventId.InventoryCapacityChanged:
							case EventId.MapDataProcessingStart:
							case EventId.MapDataProcessingEnd:
							case EventId.WorldLoadComplete:
								return EatResponse.NotEaten;
							case EventId.TroopLevelUpgraded:
							case EventId.StarshipLevelUpgraded:
								goto IL_A5D;
							case EventId.BuildingLevelUpgraded:
								this.PlayBuildingUpgradedSound(cookie as ContractEventData);
								return EatResponse.NotEaten;
							case EventId.BuildingConstructed:
								this.PlayBuildingUpgradedSound(cookie as ContractEventData);
								return EatResponse.NotEaten;
							case EventId.SpecialAttackDeployed:
							{
								SpecialAttack specialAttack = (SpecialAttack)cookie;
								this.PlayBattleSound(specialAttack.VO, AudioCollectionType.Movement);
								return EatResponse.NotEaten;
							}
							case EventId.SpecialAttackDropshipFlyingAway:
							{
								SpecialAttack specialAttack2 = (SpecialAttack)cookie;
								this.PlayBattleSound(specialAttack2.VO, AudioCollectionType.MovementAway);
								return EatResponse.NotEaten;
							}
							case EventId.SpecialAttackFired:
								this.PlayBattleSound((IAudioVO)cookie, AudioCollectionType.Attack);
								return EatResponse.NotEaten;
							case EventId.IntroStarted:
								goto IL_96D;
							case EventId.WorldInTransitionComplete:
								this.PlayStateMusicOrEffect();
								return EatResponse.NotEaten;
							default:
								return EatResponse.NotEaten;
							}
							break;
						}
					}
					else if (id != EventId.UIFilterSelected)
					{
						if (id != EventId.GameStateChanged)
						{
							return EatResponse.NotEaten;
						}
						goto IL_726;
					}
					this.audioManager.PlayAudio("sfx_button_store_selectcategory");
					return EatResponse.NotEaten;
				}
				if (id <= EventId.InventoryResourceUpdated)
				{
					if (id <= EventId.ContractAdded)
					{
						if (id == EventId.ShooterWarmingUp)
						{
							this.PlayBattleSound((SmartEntity)cookie, AudioCollectionType.Charge);
							return EatResponse.NotEaten;
						}
						switch (id)
						{
						case EventId.ButtonClicked:
							this.PlayButtonEffect(cookie.ToString());
							return EatResponse.NotEaten;
						case EventId.ContextButtonClicked:
						{
							string text = (string)cookie;
							string id2 = (text == "Move") ? "sfx_button_editmode" : "sfx_button_context";
							this.audioManager.PlayAudio(id2);
							return EatResponse.NotEaten;
						}
						case EventId.InfoButtonClicked:
							this.audioManager.PlayAudio("sfx_button_more_info");
							return EatResponse.NotEaten;
						case EventId.UserWantedEditBaseState:
						case EventId.UserLiftedBuilding:
						case EventId.UserMovedLiftedBuilding:
						case EventId.UserLoweredBuilding:
						case EventId.UserStashedBuilding:
							return EatResponse.NotEaten;
						case EventId.UserLiftedBuildingAudio:
							this.audioManager.PlayAudio("sfx_ui_placement_start");
							return EatResponse.NotEaten;
						case EventId.UserGridMovedBuildingAudio:
							this.audioManager.PlayAudio("sfx_ui_placement_move");
							return EatResponse.NotEaten;
						case EventId.UserLoweredBuildingAudio:
							this.audioManager.PlayAudio("sfx_ui_placement_drop");
							return EatResponse.NotEaten;
						case EventId.LongPressStarted:
							this.audioManager.PlayAudio("sfx_button_editfill");
							return EatResponse.NotEaten;
						case EventId.ContractAdded:
						{
							ContractEventData contractEventData2 = (ContractEventData)cookie;
							this.PlayContractSound(contractEventData2.Contract);
							return EatResponse.NotEaten;
						}
						default:
							return EatResponse.NotEaten;
						}
					}
					else
					{
						if (id == EventId.InitiatedBuyout)
						{
							this.audioManager.PlayAudio("sfx_button_finishnow");
							return EatResponse.NotEaten;
						}
						if (id != EventId.InventoryResourceUpdated)
						{
							return EatResponse.NotEaten;
						}
						this.PlayInventoryUpdatedSound((string)cookie);
						return EatResponse.NotEaten;
					}
				}
				else if (id <= EventId.ScreenLoaded)
				{
					if (id == EventId.ScreenClosing)
					{
						this.audioManager.PlayAudio("sfx_button_close");
						return EatResponse.NotEaten;
					}
					if (id != EventId.ScreenLoaded)
					{
						return EatResponse.NotEaten;
					}
					this.PlayScreenLoadedSound(cookie as ScreenBase);
					return EatResponse.NotEaten;
				}
				else
				{
					switch (id)
					{
					case EventId.HoloEvent:
						this.audioManager.PlayAudio(cookie as string);
						return EatResponse.NotEaten;
					case EventId.StoryTranscriptDisplayed:
						this.audioManager.PlayAudio(cookie as string);
						return EatResponse.NotEaten;
					case EventId.HolocommScreenLoadComplete:
					case EventId.HoloCommScreenDestroyed:
					case EventId.StoryChainCompleted:
					case EventId.LogStoryActionExecuted:
					case EventId.HeroDeployed:
						return EatResponse.NotEaten;
					case EventId.StoryNextButtonClicked:
					case EventId.StoryAttackButtonClicked:
					case EventId.StorySkipButtonClicked:
						goto IL_99A;
					case EventId.TextCrawlStarted:
						break;
					case EventId.TextCrawlComplete:
						this.PlayStateMusicOrEffect();
						return EatResponse.NotEaten;
					case EventId.TroopAbilityActivate:
					{
						SmartEntity smartEntity2 = (SmartEntity)cookie;
						TroopAbilityVO abilityVO2 = smartEntity2.TroopComp.AbilityVO;
						this.audioManager.PlayAudio(this.audioManager.GetRandomClip(abilityVO2.AudioAbilityActivate));
						new AudioEffectLoop(abilityVO2.Duration * 0.001f, abilityVO2.AudioAbilityLoop);
						return EatResponse.NotEaten;
					}
					default:
						if (id == EventId.ProjectileViewDeflected)
						{
							BuffTypeVO buffTypeVO = (BuffTypeVO)cookie;
							this.audioManager.PlayAudio(this.audioManager.GetRandomClip(buffTypeVO.AudioAbilityEvent));
							return EatResponse.NotEaten;
						}
						if (id != EventId.PlayHoloGreet)
						{
							return EatResponse.NotEaten;
						}
						this.audioManager.PlayAudio(cookie as string);
						return EatResponse.NotEaten;
					}
				}
				IL_96D:
				this.audioManager.Stop(AudioCategory.Ambience);
				this.audioManager.PlayAudio("music_intro");
				return EatResponse.NotEaten;
			}
			if (id <= EventId.PlanetRelocateStarted)
			{
				if (id <= EventId.ChampionShieldDestroyed)
				{
					if (id <= EventId.BattleEndVictoryStarDisplayed)
					{
						if (id != EventId.StarEarned && id != EventId.BattleEndVictoryStarDisplayed)
						{
							return EatResponse.NotEaten;
						}
						this.PlayStarSound((int)cookie);
						return EatResponse.NotEaten;
					}
					else
					{
						if (id == EventId.MissionActionButtonClicked)
						{
							this.PlayMissionActionSound(cookie as CampaignMissionVO);
							return EatResponse.NotEaten;
						}
						switch (id)
						{
						case EventId.ShuttleAnimStateChanged:
						{
							ShuttleState state = ((ShuttleAnim)cookie).State;
							if (state == ShuttleState.Landing)
							{
								this.audioManager.PlayAudio("sfx_ui_shuttle_arrive");
								return EatResponse.NotEaten;
							}
							if (state != ShuttleState.LiftOff)
							{
								return EatResponse.NotEaten;
							}
							this.audioManager.PlayAudio("sfx_ui_shuttle_full");
							return EatResponse.NotEaten;
						}
						case EventId.ShieldBorderDestroyed:
							this.audioManager.PlayAudio("sfx_shields_power_down");
							return EatResponse.NotEaten;
						case EventId.ShieldStarted:
							this.audioManager.PlayAudio("sfx_shields_power_up");
							return EatResponse.NotEaten;
						case EventId.ShieldDisabled:
							return EatResponse.NotEaten;
						case EventId.ChampionShieldDeactivated:
							this.audioManager.PlayAudio("sfx_champion_shield_deactivate");
							return EatResponse.NotEaten;
						case EventId.ChampionShieldActivated:
							this.audioManager.PlayAudio("sfx_champion_shield_activate");
							return EatResponse.NotEaten;
						case EventId.ChampionShieldDestroyed:
							this.audioManager.PlayAudio("sfx_champion_shield_destroyed");
							return EatResponse.NotEaten;
						default:
							return EatResponse.NotEaten;
						}
					}
				}
				else if (id <= EventId.GalaxyUIPlanetFocus)
				{
					if (id != EventId.HQCelebrationPlayed)
					{
						switch (id)
						{
						case EventId.GalaxyPlanetTapped:
							this.audioManager.PlayAudio("sfx_swipe_planet");
							return EatResponse.NotEaten;
						case EventId.GalaxyPlanetInfoButton:
							return EatResponse.NotEaten;
						case EventId.GalaxyGoToPlanetView:
							this.audioManager.Stop(AudioCategory.Ambience, AudioCategory.Music);
							this.audioManager.PlayAudio("sfx_trans_base_to_playscreen");
							this.audioManager.PlayAudio("music_galaxy_map_01");
							return EatResponse.NotEaten;
						case EventId.GalaxyGoToGalaxyView:
							this.audioManager.Stop(AudioCategory.Ambience, AudioCategory.Music);
							this.audioManager.PlayAudio("sfx_trans_base_to_galaxy");
							this.audioManager.PlayAudio("music_galaxy_map_01");
							return EatResponse.NotEaten;
						case EventId.GalaxyTransitionToNextPlanet:
							this.audioManager.PlayAudio("sfx_swipe_planet");
							return EatResponse.NotEaten;
						case EventId.GalaxyTransitionToPreviousPlanet:
							this.audioManager.PlayAudio("sfx_swipe_planet");
							return EatResponse.NotEaten;
						case EventId.GalaxyNotEnoughRelocateStarsClose:
							this.audioManager.PlayAudio("sfx_button_no_relocate");
							return EatResponse.NotEaten;
						case EventId.GalaxyUIPlanetFocus:
							if (Time.time - this.galaxyUIPlanetFocusThrottle > GameConstants.GALAXY_UI_PLANET_FOCUS_THROTTLE)
							{
								this.galaxyUIPlanetFocusThrottle = Time.time;
								this.audioManager.PlayAudio("sfx_ui_planet_focus");
								return EatResponse.NotEaten;
							}
							return EatResponse.NotEaten;
						default:
							return EatResponse.NotEaten;
						}
					}
				}
				else
				{
					switch (id)
					{
					case EventId.BattleLogScreenTabSelected:
						this.audioManager.PlayAudio("sfx_button_squad_select");
						return EatResponse.NotEaten;
					case EventId.BattleLogScreenShareButtonClicked:
						this.audioManager.PlayAudio("sfx_button_more_info");
						return EatResponse.NotEaten;
					case EventId.BattleLogScreenRevengeButtonClicked:
					case EventId.BattleLogScreenReplayButtonClicked:
						this.audioManager.PlayAudio("sfx_button_startbattle");
						return EatResponse.NotEaten;
					case EventId.ServerAdminMessage:
					case EventId.GameServicesSignedIn:
					case EventId.GameServicesSignedOut:
					case EventId.TournamentEntered:
					case EventId.TournamentRedeeming:
					case EventId.TournamentRedeemed:
						return EatResponse.NotEaten;
					case EventId.SimulateAudioEvent:
					{
						AudioEventData audioEventData = (AudioEventData)cookie;
						this.OnEvent(audioEventData.EventId, audioEventData.EventCookie);
						return EatResponse.NotEaten;
					}
					case EventId.MuteEvent:
						this.eventManager.UnregisterObserver(this, (EventId)cookie);
						return EatResponse.NotEaten;
					case EventId.UnmuteEvent:
						this.eventManager.RegisterObserver(this, (EventId)cookie, EventPriority.Default);
						return EatResponse.NotEaten;
					case EventId.MusicUnmuted:
						goto IL_726;
					default:
						switch (id)
						{
						case EventId.TrapTriggered:
							this.PlayTrapSound((TrapComponent)cookie);
							return EatResponse.NotEaten;
						case EventId.TrapDisarmed:
						case EventId.PlanetRelocateButtonPressed:
							return EatResponse.NotEaten;
						case EventId.TrapDestroyed:
							this.PlayTrapDestroySound((TrapComponent)cookie);
							return EatResponse.NotEaten;
						case EventId.PlanetConfirmRelocate:
							this.audioManager.Stop(AudioCategory.Ambience, AudioCategory.Music);
							this.audioManager.PlayAudio("sfx_trans_planet_to_hyperspace");
							return EatResponse.NotEaten;
						case EventId.PlanetRelocateStarted:
							this.audioManager.PlayAudio("sfx_trans_hyperspace");
							return EatResponse.NotEaten;
						default:
							return EatResponse.NotEaten;
						}
						break;
					}
				}
			}
			else if (id <= EventId.SupplyCrateProgressBar)
			{
				if (id <= EventId.ObjectiveRewardDataCardRevealed)
				{
					if (id == EventId.HolonetDevNotes)
					{
						goto IL_99A;
					}
					switch (id)
					{
					case EventId.ObjectiveDetailsClicked:
					case EventId.ObjectiveCrateInfoScreenOpened:
						this.audioManager.PlayAudio("sfx_button_context");
						return EatResponse.NotEaten;
					case EventId.ObjectiveCompleted:
						this.audioManager.PlayAudio("sfx_ui_collectcredits_1");
						return EatResponse.NotEaten;
					case EventId.ObjectiveRewardDataCardRevealed:
						break;
					default:
						return EatResponse.NotEaten;
					}
				}
				else
				{
					switch (id)
					{
					case EventId.PerkSelected:
						goto IL_8E6;
					case EventId.SquadLeveledUpCelebration:
						this.audioManager.PlayAudio("sfx_stinger_perk_squad_level_up");
						return EatResponse.NotEaten;
					case EventId.PerkActivated:
						this.audioManager.PlayAudio("sfx_button_perk_activate");
						return EatResponse.NotEaten;
					case EventId.PerkInvested:
						this.audioManager.PlayAudio("sfx_button_perk_rep_invest");
						return EatResponse.NotEaten;
					case EventId.PerkCelebStarted:
						this.audioManager.PlayAudio("sfx_stinger_perk_upgrade");
						return EatResponse.NotEaten;
					case EventId.SquadAdvancementTabSelected:
					case EventId.TargetedBundleChampionRedeemed:
					case EventId.TargetedBundleRewardRedeemed:
					case EventId.TargetedBundleReserve:
					case EventId.ShardsEarned:
					case EventId.EquipmentUnlocked:
						return EatResponse.NotEaten;
					case EventId.EquipmentUpgraded:
						goto IL_A5D;
					case EventId.EquipmentActivated:
						this.audioManager.PlayAudio("sfx_button_trainunit");
						return EatResponse.NotEaten;
					default:
						switch (id)
						{
						case EventId.EquipmentUnlockCelebrationPlayed:
							break;
						case EventId.EquipmentBuffShaderRemove:
						case EventId.EquipmentBuffShaderApply:
							return EatResponse.NotEaten;
						case EventId.EquipmentDataFragmentEarned:
							goto IL_A47;
						case EventId.SupplyCrateProgressBar:
							this.audioManager.PlayAudio("sfx_button_editmode");
							return EatResponse.NotEaten;
						default:
							return EatResponse.NotEaten;
						}
						break;
					}
				}
			}
			else if (id <= EventId.DeployableUnlockCelebrationPlayed)
			{
				if (id == EventId.ShardUnitUpgraded)
				{
					goto IL_A5D;
				}
				if (id != EventId.DeployableUnlockCelebrationPlayed)
				{
					return EatResponse.NotEaten;
				}
			}
			else
			{
				if (id == EventId.CrateRewardIdleHop)
				{
					this.audioManager.PlayAudio("sfx_rewards_crate_hop");
					return EatResponse.NotEaten;
				}
				if (id == EventId.InventoryCrateAnimationStateChange)
				{
					InventoryCrateAnimationState animState = (InventoryCrateAnimationState)cookie;
					this.PlayInventoryCrateAnimSFXBasedOnState(animState);
					return EatResponse.NotEaten;
				}
				if (id != EventId.InventoryCrateAnimVFXTriggered)
				{
					return EatResponse.NotEaten;
				}
				string sfxName = Convert.ToString(cookie);
				this.PlayInventoryCrateAnimVfxSfx(sfxName);
				return EatResponse.NotEaten;
			}
			this.audioManager.PlayAudio("sfx_ui_hq_celebration");
			return EatResponse.NotEaten;
			IL_726:
			this.PlayStateMusicOrEffect();
			return EatResponse.NotEaten;
			IL_8E6:
			this.audioManager.PlayAudio("sfx_button_store_selectbuilding");
			return EatResponse.NotEaten;
			IL_99A:
			this.audioManager.Stop(AudioCategory.Dialogue);
			this.audioManager.PlayAudio("sfx_button_next");
			return EatResponse.NotEaten;
			IL_A47:
			this.audioManager.PlayAudio("sfx_button_readyhero");
			return EatResponse.NotEaten;
			IL_A5D:
			this.audioManager.PlayAudio("sfx_button_readystarship");
			return EatResponse.NotEaten;
		}

		private void HandleApplicationPause(bool paused)
		{
			if (paused)
			{
				AudioManager audioManager = Service.Get<AudioManager>();
				if (audioManager != null)
				{
					audioManager.SetVolume(AudioCategory.Music, 0f);
					audioManager.SetVolume(AudioCategory.Ambience, 0f);
				}
				return;
			}
			float volume = 1f;
			AudioManager audioManager2 = Service.Get<AudioManager>();
			if (audioManager2 != null)
			{
				audioManager2.SetVolume(AudioCategory.Music, volume);
				audioManager2.SetVolume(AudioCategory.Ambience, volume);
			}
			if (this.audioManager.IsThirdPartyNativePluginActive())
			{
				return;
			}
			if (Service.Get<GameStateMachine>().CurrentState is VideoPlayBackState)
			{
				return;
			}
			this.audioManager.RefreshMusic();
		}

		private void HandleDeviceMusicPlayerStateChanged(bool isDeviceMusicPlaying)
		{
			if (Service.Get<GameStateMachine>().CurrentState is VideoPlayBackState)
			{
				return;
			}
			this.audioManager.RefreshMusic();
		}

		private void PlayBattleSound(Entity entity, AudioCollectionType clipType)
		{
			if (this.audioManager.GetBattleAudioFlag(clipType))
			{
				return;
			}
			this.PlayRandomClip(entity, clipType);
			this.audioManager.SetBattleAudioFlag(clipType);
		}

		private void PlayBattleSound(IAudioVO audioType, AudioCollectionType clipType)
		{
			if (this.audioManager.GetBattleAudioFlag(clipType))
			{
				return;
			}
			this.PlayRandomClip(audioType, clipType);
			this.audioManager.SetBattleAudioFlag(clipType);
		}

		private void PlayStarSound(int starCount)
		{
			if (Service.Get<CurrentPlayer>().CampaignProgress.FueInProgress)
			{
				return;
			}
			switch (starCount)
			{
			case 1:
				this.audioManager.PlayAudio("sfx_stinger_victory_onestar");
				return;
			case 2:
				this.audioManager.PlayAudio("sfx_stinger_victory_twostar");
				return;
			case 3:
				this.audioManager.PlayAudio("sfx_stinger_victory_threestar");
				return;
			default:
				return;
			}
		}

		private void PlayScreenLoadedSound(ScreenBase screen)
		{
			if (screen is AlertScreen && !(screen is ConfirmRelocateScreen))
			{
				this.audioManager.PlayAudio("sfx_ui_alert");
			}
		}

		private void PlayInventoryUpdatedSound(string resourceType)
		{
			string text = (resourceType == "droids") ? "sfx_ui_droid_purchase" : "";
			if (text != "")
			{
				this.audioManager.PlayAudio(text);
			}
		}

		private void PlayContractSound(Contract contract)
		{
			string id = "";
			switch (contract.DeliveryType)
			{
			case DeliveryType.Infantry:
			case DeliveryType.Vehicle:
			case DeliveryType.Starship:
			case DeliveryType.Hero:
			case DeliveryType.Champion:
			case DeliveryType.Mercenary:
				id = "sfx_button_trainunit";
				break;
			case DeliveryType.Building:
			case DeliveryType.UpgradeBuilding:
			case DeliveryType.SwapBuilding:
				id = this.audioManager.GetRandomClip(this.droidClips);
				break;
			}
			this.audioManager.PlayAudio(id);
		}

		private void PlayBuildingUpgradedSound(ContractEventData contractData)
		{
			if (contractData.Silent)
			{
				return;
			}
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			string text;
			if (contractData.BuildingVO.Type == BuildingType.HQ)
			{
				if (faction != FactionType.Empire)
				{
					if (faction != FactionType.Rebel)
					{
						text = "sfx_stinger_upgradehq";
					}
					else
					{
						text = "sfx_stinger_rebel_upgradehq";
					}
				}
				else
				{
					text = "sfx_stinger_empire_upgradehq";
				}
			}
			else if (faction != FactionType.Empire)
			{
				if (faction != FactionType.Rebel)
				{
					text = "sfx_stinger_upgradebuilding";
				}
				else
				{
					text = "sfx_stinger_rebel_upgradebuilding";
				}
			}
			else
			{
				text = "sfx_stinger_empire_upgradebuilding";
			}
			if (text != null)
			{
				this.audioManager.PlayAudio(text);
			}
		}

		private void PlayMissionActionSound(CampaignMissionVO vo)
		{
			string id = "";
			switch (vo.MissionType)
			{
			case MissionType.Attack:
			case MissionType.Defend:
			case MissionType.RaidDefend:
				id = "sfx_button_startbattle";
				break;
			case MissionType.Collect:
				id = "sfx_ui_collectreward_1";
				break;
			}
			this.audioManager.PlayAudio(id);
		}

		private void PlayRandomClip(Entity entity, AudioCollectionType clipType)
		{
			if (entity == null)
			{
				return;
			}
			IAudioVO audioTypeFromEntity = this.GetAudioTypeFromEntity(entity);
			this.PlayRandomClip(audioTypeFromEntity, clipType);
		}

		private void PlayRandomClip(IAudioVO audioType, AudioCollectionType clipType)
		{
			if (audioType == null)
			{
				return;
			}
			List<StrIntPair> list = null;
			switch (clipType)
			{
			case AudioCollectionType.Charge:
				list = audioType.AudioCharge;
				break;
			case AudioCollectionType.Attack:
				list = audioType.AudioAttack;
				break;
			case AudioCollectionType.Death:
				list = audioType.AudioDeath;
				break;
			case AudioCollectionType.Placement:
				list = audioType.AudioPlacement;
				break;
			case AudioCollectionType.Movement:
				list = audioType.AudioMovement;
				break;
			case AudioCollectionType.MovementAway:
				list = audioType.AudioMovementAway;
				break;
			case AudioCollectionType.Impact:
				list = audioType.AudioImpact;
				break;
			case AudioCollectionType.Train:
				list = audioType.AudioTrain;
				break;
			}
			if (list == null)
			{
				return;
			}
			string randomClip = this.audioManager.GetRandomClip(list);
			this.audioManager.PlayAudio(randomClip);
		}

		private IAudioVO GetAudioTypeFromEntity(Entity entity)
		{
			IAudioVO result = null;
			if (entity.Has<BuildingComponent>())
			{
				result = entity.Get<BuildingComponent>().BuildingType;
			}
			else if (entity.Has<TroopComponent>())
			{
				result = entity.Get<TroopComponent>().AudioVO;
			}
			return result;
		}

		private void PlayStateMusicOrEffect()
		{
			IState currentState = Service.Get<GameStateMachine>().CurrentState;
			CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
			FactionType faction = currentPlayer.Faction;
			PlanetVO planet = currentPlayer.Planet;
			string text = null;
			if (currentState is EditBaseState)
			{
				this.audioManager.PlayAudio("sfx_button_editmode");
				return;
			}
			if (currentState is HomeState || currentState is FueBattleStartState)
			{
				this.PlayPlanetBaseMusic(planet, faction);
				return;
			}
			if (currentState is NeighborVisitState)
			{
				PlanetVO planet2 = Service.Get<NeighborVisitManager>().NeighborPlayer.Map.Planet;
				this.PlayPlanetBaseMusic(planet2, faction);
				return;
			}
			if (currentState is WarBoardState)
			{
				this.PlayPlanetBaseMusic(planet, faction);
				return;
			}
			if (currentState is BattleStartState)
			{
				if (!Service.Get<WorldTransitioner>().IsTransitioning())
				{
					this.audioManager.Stop(AudioCategory.Ambience);
					this.PlayPreBattleMusic();
					return;
				}
				this.audioManager.Stop(AudioCategory.Ambience, AudioCategory.Music);
				return;
			}
			else
			{
				if (!(currentState is BattlePlaybackState) && !(currentState is BattlePlayState))
				{
					if (currentState is BattleEndPlaybackState || currentState is BattleEndState)
					{
						this.audioManager.Stop(AudioCategory.Music);
						Service.Get<ViewTimerManager>().KillViewTimer(this.timerId);
						string ambientMusic = Service.Get<BattleController>().GetCurrentBattle().AmbientMusic;
						if (!string.IsNullOrEmpty(ambientMusic))
						{
							text = ambientMusic;
						}
						else
						{
							text = planet.AmbientMusic;
						}
						this.audioManager.PlayAudio(text);
						bool isReplay = Service.Get<BattleController>().GetCurrentBattle().IsReplay;
						BattleEntry battleEntry;
						if (isReplay)
						{
							battleEntry = Service.Get<BattlePlaybackController>().CurrentBattleEntry;
						}
						else
						{
							battleEntry = Service.Get<BattleController>().GetCurrentBattle();
						}
						bool won = battleEntry.Won;
						string id;
						if (faction == FactionType.Empire)
						{
							id = (won ? "sfx_stinger_empire_victory" : "sfx_stinger_empire_defeat");
						}
						else
						{
							id = (won ? "sfx_stinger_rebel_victory" : "sfx_stinger_rebel_defeat");
						}
						this.audioManager.PlayAudio(id);
					}
					return;
				}
				this.audioManager.Stop(AudioCategory.Ambience);
				if (Service.Get<WorldTransitioner>().IsTransitioning())
				{
					this.PlayPreBattleMusic();
					return;
				}
				string text2 = null;
				if (faction != FactionType.Empire)
				{
					if (faction == FactionType.Rebel)
					{
						text2 = "sfx_stinger_rebel_battle";
					}
				}
				else
				{
					text2 = "sfx_stinger_empire_battle";
				}
				CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
				string battleMusic = currentBattle.BattleMusic;
				if (!string.IsNullOrEmpty(battleMusic))
				{
					text = battleMusic;
				}
				if (string.IsNullOrEmpty(text))
				{
					PlanetVO planetVO = Service.Get<IDataController>().Get<PlanetVO>(currentBattle.PlanetId);
					text = planetVO.BattleMusic;
				}
				if (string.IsNullOrEmpty(text))
				{
					text = planet.BattleMusic;
				}
				if (text2 != null)
				{
					this.audioManager.PlayAudio(text2);
					float delay = 7f;
					this.timerId = Service.Get<ViewTimerManager>().CreateViewTimer(delay, false, new TimerDelegate(this.StartMusicOnTimer), text);
					return;
				}
				this.audioManager.PlayAudio(text);
				return;
			}
		}

		private void PlayPlanetBaseMusic(PlanetVO planet, FactionType faction)
		{
			string id = "";
			if (!Service.Get<WorldTransitioner>().IsTransitioning())
			{
				string text = planet.AmbientMusic;
				if (string.IsNullOrEmpty(text))
				{
					text = "sfx_ambient_tatooine";
				}
				if (!this.audioManager.IsPlaying(AudioCategory.Ambience, text))
				{
					this.audioManager.Stop(AudioCategory.Ambience);
					this.audioManager.PlayAudio(text);
				}
			}
			else
			{
				this.audioManager.Stop(AudioCategory.Ambience);
			}
			switch (faction)
			{
			case FactionType.Empire:
				id = planet.EmpireMusic;
				break;
			case FactionType.Rebel:
				id = planet.RebelMusic;
				break;
			case FactionType.Smuggler:
				id = "music_rebel_base_1";
				break;
			}
			if (!this.audioManager.IsPlaying(AudioCategory.Music, id))
			{
				this.audioManager.Stop(AudioCategory.Music);
				this.audioManager.PlayAudio(id);
			}
		}

		private void PlayPreBattleMusic()
		{
			this.audioManager.Stop(AudioCategory.Music);
			string ambientMusic = Service.Get<BattleController>().GetCurrentBattle().AmbientMusic;
			string id;
			if (!string.IsNullOrEmpty(ambientMusic))
			{
				id = ambientMusic;
			}
			else
			{
				CurrentPlayer currentPlayer = Service.Get<CurrentPlayer>();
				PlanetVO planet = currentPlayer.Planet;
				id = planet.AmbientMusic;
			}
			this.audioManager.PlayAudio(id);
		}

		private void StartMusicOnTimer(uint timerId, object cookie)
		{
			this.audioManager.PlayAudio(cookie as string);
		}

		private void PlayTrapSound(TrapComponent trapComp)
		{
			this.audioManager.PlayAudio(trapComp.Type.RevealAudio);
		}

		private void PlayTrapDestroySound(TrapComponent trapComp)
		{
			this.audioManager.PlayAudio(trapComp.Type.RevealAudio);
		}

		private void PlayCurrencyCollectionEffect(CurrencyType currencyType)
		{
			string id;
			switch (currencyType)
			{
			case CurrencyType.Credits:
				id = "sfx_ui_collectcredits_1";
				break;
			case CurrencyType.Materials:
				id = "sfx_ui_collectmaterials_1";
				break;
			case CurrencyType.Contraband:
				id = "sfx_ui_collectmaterials_1";
				break;
			case CurrencyType.Reputation:
				return;
			case CurrencyType.Crystals:
				id = "sfx_ui_collecthardcurrency_1";
				break;
			default:
				return;
			}
			this.audioManager.PlayAudio(id);
		}

		private void PlayButtonEffect(string elementName)
		{
			string id = "";
			uint num = <PrivateImplementationDetails>.ComputeStringHash(elementName);
			if (num <= 2022222315u)
			{
				if (num <= 1573806145u)
				{
					if (num <= 350364697u)
					{
						if (num <= 252390500u)
						{
							if (num != 226083189u)
							{
								if (num != 252390500u)
								{
									goto IL_58A;
								}
								if (!(elementName == "BtnAbout"))
								{
									goto IL_58A;
								}
								goto IL_519;
							}
							else
							{
								if (!(elementName == "TabUpgradePerks"))
								{
									goto IL_58A;
								}
								goto IL_582;
							}
						}
						else if (num != 268391939u)
						{
							if (num != 350364697u)
							{
								goto IL_58A;
							}
							if (!(elementName == "BtnLanguage"))
							{
								goto IL_58A;
							}
							goto IL_519;
						}
						else if (!(elementName == "ButtonBattle"))
						{
							goto IL_58A;
						}
					}
					else if (num <= 1150718760u)
					{
						if (num != 944478957u)
						{
							if (num != 1150718760u)
							{
								goto IL_58A;
							}
							if (!(elementName == "Newspaper"))
							{
								goto IL_58A;
							}
							id = "sfx_button_more_info";
							goto IL_5BF;
						}
						else
						{
							if (!(elementName == "ButtonLog"))
							{
								goto IL_58A;
							}
							goto IL_542;
						}
					}
					else if (num != 1458528444u)
					{
						if (num != 1573806145u)
						{
							goto IL_58A;
						}
						if (!(elementName == "ButtonWar"))
						{
							goto IL_58A;
						}
					}
					else
					{
						if (!(elementName == "ButtonSettings"))
						{
							goto IL_58A;
						}
						goto IL_542;
					}
					id = "sfx_button_mission";
					goto IL_5BF;
				}
				if (num <= 1785103917u)
				{
					if (num <= 1686051900u)
					{
						if (num != 1622808274u)
						{
							if (num != 1686051900u)
							{
								goto IL_58A;
							}
							if (!(elementName == "ButtonEndBattle"))
							{
								goto IL_58A;
							}
							id = "sfx_button_endbattle";
							goto IL_5BF;
						}
						else
						{
							if (!(elementName == "TabActivatePerks"))
							{
								goto IL_58A;
							}
							goto IL_582;
						}
					}
					else if (num != 1711916995u)
					{
						if (num != 1785103917u)
						{
							goto IL_58A;
						}
						if (!(elementName == "Medals"))
						{
							goto IL_58A;
						}
					}
					else
					{
						if (!(elementName == "BtnFacebook"))
						{
							goto IL_58A;
						}
						goto IL_519;
					}
				}
				else if (num <= 2010720957u)
				{
					if (num != 1789335184u)
					{
						if (num != 2010720957u)
						{
							goto IL_58A;
						}
						if (!(elementName == "BaseRating"))
						{
							goto IL_58A;
						}
					}
					else
					{
						if (!(elementName == "ButtonLeaderboard"))
						{
							goto IL_58A;
						}
						id = "sfx_button_squad";
						goto IL_5BF;
					}
				}
				else if (num != 2012714738u)
				{
					if (num != 2022222315u)
					{
						goto IL_58A;
					}
					if (!(elementName == "BtnPrivacyPolicy"))
					{
						goto IL_58A;
					}
					goto IL_519;
				}
				else
				{
					if (!(elementName == "ButtonHome"))
					{
						goto IL_58A;
					}
					goto IL_52F;
				}
				IL_542:
				id = "sfx_button_more_info";
				goto IL_5BF;
				IL_582:
				id = "sfx_button_squad_select";
				goto IL_5BF;
			}
			if (num > 2842717329u)
			{
				if (num <= 3398130181u)
				{
					if (num <= 3024925430u)
					{
						if (num != 2877101259u)
						{
							if (num != 3024925430u)
							{
								goto IL_58A;
							}
							if (!(elementName == "ButtonNextBattle"))
							{
								goto IL_58A;
							}
							id = "sfx_button_startbattle";
							goto IL_5BF;
						}
						else if (!(elementName == "BtnOption2Top"))
						{
							goto IL_58A;
						}
					}
					else if (num != 3374403082u)
					{
						if (num != 3398130181u)
						{
							goto IL_58A;
						}
						if (!(elementName == "ButtonExitEdit"))
						{
							goto IL_58A;
						}
						goto IL_52F;
					}
					else
					{
						if (!(elementName == "ButtonStore"))
						{
							goto IL_58A;
						}
						goto IL_53A;
					}
				}
				else if (num <= 3650851718u)
				{
					if (num != 3521080394u)
					{
						if (num != 3650851718u)
						{
							goto IL_58A;
						}
						if (!(elementName == "BtnOption1Bottom"))
						{
							goto IL_58A;
						}
					}
					else
					{
						if (!(elementName == "BtnMusic"))
						{
							goto IL_58A;
						}
						goto IL_50E;
					}
				}
				else if (num != 3789427652u)
				{
					if (num != 3804014611u)
					{
						if (num != 3886812856u)
						{
							goto IL_58A;
						}
						if (!(elementName == "BtnOption1Top"))
						{
							goto IL_58A;
						}
					}
					else if (!(elementName == "BtnOption2Bottom"))
					{
						goto IL_58A;
					}
				}
				else
				{
					if (!(elementName == "BtnHelp"))
					{
						goto IL_58A;
					}
					goto IL_519;
				}
				id = "sfx_button_next";
				goto IL_5BF;
			}
			if (num <= 2486775183u)
			{
				if (num <= 2084477219u)
				{
					if (num != 2039097040u)
					{
						if (num != 2084477219u)
						{
							goto IL_58A;
						}
						if (!(elementName == "BtnTOS"))
						{
							goto IL_58A;
						}
						goto IL_519;
					}
					else
					{
						if (!(elementName == "Shield"))
						{
							goto IL_58A;
						}
						goto IL_53A;
					}
				}
				else if (num != 2265190446u)
				{
					if (num != 2486775183u)
					{
						goto IL_58A;
					}
					if (!(elementName == "ButtonPrimaryAction"))
					{
						goto IL_58A;
					}
					id = "sfx_button_restartbattle";
					goto IL_5BF;
				}
				else
				{
					if (!(elementName == "ButtonClans"))
					{
						goto IL_58A;
					}
					id = "sfx_button_squad";
					goto IL_5BF;
				}
			}
			else if (num <= 2789677294u)
			{
				if (num != 2647020400u)
				{
					if (num != 2789677294u)
					{
						goto IL_58A;
					}
					if (!(elementName == "Crystals"))
					{
						goto IL_58A;
					}
					goto IL_53A;
				}
				else if (!(elementName == "BtnSoundEffects"))
				{
					goto IL_58A;
				}
			}
			else if (num != 2839587760u)
			{
				if (num != 2842717329u)
				{
					goto IL_58A;
				}
				if (!(elementName == "BtnCancel"))
				{
					goto IL_58A;
				}
				id = "sfx_button_back";
				goto IL_5BF;
			}
			else
			{
				if (!(elementName == "BtnSwap"))
				{
					goto IL_58A;
				}
				id = "sfx_button_more_info";
				goto IL_5BF;
			}
			IL_50E:
			id = "sfx_button_next";
			goto IL_5BF;
			IL_53A:
			id = "sfx_button_store";
			goto IL_5BF;
			IL_519:
			id = "sfx_button_facebook";
			goto IL_5BF;
			IL_52F:
			id = "sfx_button_home";
			goto IL_5BF;
			IL_58A:
			if (elementName.StartsWith("CheckboxTroop"))
			{
				id = "sfx_button_selecttroop";
			}
			else if (elementName.StartsWith("ButtonCampaignCard") || elementName.StartsWith("ButtonObjectiveCard"))
			{
				id = "sfx_button_campaign";
			}
			IL_5BF:
			this.audioManager.PlayAudio(id);
		}

		protected internal AudioEventManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).GetAudioTypeFromEntity((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).HandleApplicationPause(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).HandleDeviceMusicPlayerStateChanged(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayBattleSound((Entity)GCHandledObjects.GCHandleToObject(*args), (AudioCollectionType)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayBattleSound((IAudioVO)GCHandledObjects.GCHandleToObject(*args), (AudioCollectionType)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayBuildingUpgradedSound((ContractEventData)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayButtonEffect(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayContractSound((Contract)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayCurrencyCollectionEffect((CurrencyType)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayInventoryCrateAnimSFXBasedOnState((InventoryCrateAnimationState)(*(int*)args));
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayInventoryCrateAnimVfxSfx(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayInventoryUpdatedSound(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayMissionActionSound((CampaignMissionVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayPlanetBaseMusic((PlanetVO)GCHandledObjects.GCHandleToObject(*args), (FactionType)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayPreBattleMusic();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayRandomClip((Entity)GCHandledObjects.GCHandleToObject(*args), (AudioCollectionType)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayRandomClip((IAudioVO)GCHandledObjects.GCHandleToObject(*args), (AudioCollectionType)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayScreenLoadedSound((ScreenBase)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayStarSound(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayStateMusicOrEffect();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayTrapDestroySound((TrapComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((AudioEventManager)GCHandledObjects.GCHandleToObject(instance)).PlayTrapSound((TrapComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
