using StaRTS.Main.Controllers;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Animations;
using StaRTS.Main.Views.Projectors;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.UX.Controls
{
	public class DeployableTroopControl : IEventObserver, IViewFrameTimeObserver
	{
		private UXSprite cardDimmer;

		private UXElement glowElement;

		private UXElement abilityActivateFxSprite;

		private GeometryProjector geometry;

		private bool observingHeroEvents;

		private bool observingTroopEquipmentEvents;

		private bool observingStarshipEvents;

		private float coolDownTime;

		private float totalCoolDownTime;

		private UXElement EquipmentFX;

		public bool Enabled
		{
			get;
			private set;
		}

		public uint HeroEntityID
		{
			get;
			private set;
		}

		public string DeployableUid
		{
			get;
			private set;
		}

		public bool IsHero
		{
			get;
			private set;
		}

		public bool IsStarship
		{
			get;
			private set;
		}

		public UXCheckbox TroopCheckbox
		{
			get;
			private set;
		}

		public UXLabel TroopCountLabel
		{
			get;
			private set;
		}

		public HeroAbilityState AbilityState
		{
			get;
			private set;
		}

		public bool DisableDueToBuildingDestruction
		{
			get;
			set;
		}

		public DeployableTroopControl(UXCheckbox troopCheckbox, UXLabel troopCountLabel, UXSprite dimmerSprite, UXElement optionalGlowSprite, UXElement optionalActivateFxSprite, GeometryProjector optionalGeometry, bool isHero, bool isStarship, string uid, UXElement equipmentFX)
		{
			this.IsHero = isHero;
			this.IsStarship = isStarship;
			this.DeployableUid = uid;
			this.TroopCheckbox = troopCheckbox;
			this.TroopCountLabel = troopCountLabel;
			this.AbilityState = HeroAbilityState.Dormant;
			this.cardDimmer = dimmerSprite;
			this.glowElement = optionalGlowSprite;
			this.abilityActivateFxSprite = optionalActivateFxSprite;
			this.geometry = optionalGeometry;
			this.EquipmentFX = equipmentFX;
			if (this.abilityActivateFxSprite != null)
			{
				this.abilityActivateFxSprite.Visible = false;
			}
			this.Enable();
			if (this.EquipmentFX == null)
			{
				return;
			}
			IDataController dataController = Service.Get<IDataController>();
			if (this.IsEquipmentActiveOnDeployable(dataController, this.DeployableUid))
			{
				EventManager eventManager = Service.Get<EventManager>();
				eventManager.RegisterObserver(this, EventId.WorldInTransitionComplete);
				eventManager.RegisterObserver(this, EventId.HoloCommScreenDestroyed, EventPriority.AfterDefault);
				this.observingStarshipEvents = this.IsStarship;
				this.observingTroopEquipmentEvents = !this.IsStarship;
			}
			if (this.IsHero)
			{
				EventManager eventManager2 = Service.Get<EventManager>();
				string ability = dataController.Get<TroopTypeVO>(this.DeployableUid).Ability;
				if (!string.IsNullOrEmpty(ability))
				{
					TroopAbilityVO troopAbilityVO = dataController.Get<TroopAbilityVO>(ability);
					if (!troopAbilityVO.Auto)
					{
						this.totalCoolDownTime = (troopAbilityVO.CoolDownTime + troopAbilityVO.Duration) * 0.001f;
						eventManager2.RegisterObserver(this, EventId.HeroDeployed);
						eventManager2.RegisterObserver(this, EventId.HeroKilled);
						eventManager2.RegisterObserver(this, EventId.TroopAbilityDeactivate);
						eventManager2.RegisterObserver(this, EventId.TroopAbilityCoolDownComplete);
						this.observingHeroEvents = true;
					}
				}
			}
		}

		private bool IsEquipmentActiveOnDeployable(IDataController sdc, string deployableUid)
		{
			CurrentBattle currentBattle = Service.Get<BattleController>().GetCurrentBattle();
			List<string> list = (currentBattle.Type == BattleType.PveDefend) ? currentBattle.DefenderEquipment : currentBattle.AttackerEquipment;
			if (list == null)
			{
				return false;
			}
			if (deployableUid.Equals("squadTroops"))
			{
				return false;
			}
			string text = this.IsStarship ? sdc.Get<SpecialAttackTypeVO>(deployableUid).SpecialAttackID : sdc.Get<TroopTypeVO>(deployableUid).TroopID;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EquipmentVO equipmentVO = sdc.Get<EquipmentVO>(list[i]);
				for (int j = 0; j < equipmentVO.EffectUids.Length; j++)
				{
					EquipmentEffectVO equipmentEffectVO = sdc.Get<EquipmentEffectVO>(equipmentVO.EffectUids[j]);
					string[] array = this.IsStarship ? equipmentEffectVO.AffectedSpecialAttackIds : equipmentEffectVO.AffectedTroopIds;
					if (array != null)
					{
						for (int k = 0; k < array.Length; k++)
						{
							if (array[k] == text)
							{
								return true;
							}
						}
					}
				}
				i++;
			}
			return false;
		}

		private void PlayActiveEquipmentAnimation()
		{
			if (this.EquipmentFX != null)
			{
				this.EquipmentFX.Visible = true;
				this.EquipmentFX.Root.GetComponent<Animation>().Play();
			}
		}

		private void StopActiveEquipmentAnimation()
		{
			if (this.EquipmentFX != null)
			{
				this.EquipmentFX.Visible = false;
				this.EquipmentFX.Root.GetComponent<Animation>().Stop();
			}
		}

		public void StopObserving()
		{
			if (this.IsHero)
			{
				this.AbilityState = HeroAbilityState.Dormant;
				this.StopCoolDown();
				if (this.observingHeroEvents)
				{
					EventManager eventManager = Service.Get<EventManager>();
					eventManager.UnregisterObserver(this, EventId.HeroDeployed);
					eventManager.UnregisterObserver(this, EventId.HeroKilled);
					eventManager.UnregisterObserver(this, EventId.TroopAbilityDeactivate);
					eventManager.UnregisterObserver(this, EventId.TroopAbilityCoolDownComplete);
					this.observingHeroEvents = false;
				}
			}
			if (this.observingTroopEquipmentEvents)
			{
				EventManager eventManager2 = Service.Get<EventManager>();
				eventManager2.UnregisterObserver(this, EventId.WorldInTransitionComplete);
				eventManager2.UnregisterObserver(this, EventId.HoloCommScreenDestroyed);
				this.observingTroopEquipmentEvents = false;
				return;
			}
			if (this.observingStarshipEvents)
			{
				EventManager eventManager3 = Service.Get<EventManager>();
				eventManager3.UnregisterObserver(this, EventId.WorldInTransitionComplete);
				eventManager3.UnregisterObserver(this, EventId.HoloCommScreenDestroyed);
				this.observingStarshipEvents = false;
			}
		}

		public void Enable()
		{
			this.Enabled = true;
			this.cardDimmer.Visible = false;
			this.TroopCountLabel.TextColor = UXUtils.GetCostColor(this.TroopCountLabel, true, false);
		}

		public void Disable()
		{
			this.Disable(true);
			IDataController sdc = Service.Get<IDataController>();
			if (this.IsEquipmentActiveOnDeployable(sdc, this.DeployableUid))
			{
				this.StopActiveEquipmentAnimation();
			}
		}

		public void Disable(bool clearTroopCountLabel)
		{
			this.Enabled = false;
			this.cardDimmer.Visible = true;
			this.cardDimmer.FillAmount = 1f;
			if (this.glowElement != null)
			{
				this.glowElement.Visible = false;
			}
			if (this.abilityActivateFxSprite != null)
			{
				this.abilityActivateFxSprite.Visible = false;
			}
			this.TroopCheckbox.Selected = false;
			this.TroopCountLabel.TextColor = UXUtils.GetCostColor(this.TroopCountLabel, false, false);
			if (clearTroopCountLabel)
			{
				this.TroopCountLabel.Text = "";
			}
			if (this.IsHero && this.AbilityState == HeroAbilityState.InUse)
			{
				this.StopCoolDown();
			}
		}

		public void PrepareHeroAbility()
		{
			this.Enable();
			if (this.glowElement != null)
			{
				this.glowElement.Visible = true;
			}
			if (this.abilityActivateFxSprite != null)
			{
				this.abilityActivateFxSprite.Visible = true;
				this.abilityActivateFxSprite.Root.GetComponent<Animation>().Play();
			}
			if (this.geometry != null)
			{
				this.geometry.Config.AnimState = AnimState.AbilityPose;
				this.geometry.Renderer.Render(this.geometry.Config);
			}
			this.AbilityState = HeroAbilityState.Prepared;
		}

		public void UseHeroAbility()
		{
			this.Disable();
			this.AbilityState = HeroAbilityState.InUse;
			this.coolDownTime = this.totalCoolDownTime;
			Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
		}

		private void PutHeroAbilityOnCoolDown()
		{
			this.AbilityState = HeroAbilityState.CoolingDown;
		}

		private void StopCoolDown()
		{
			this.coolDownTime = 0f;
			Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
		}

		public void OnViewFrameTime(float dt)
		{
			this.coolDownTime -= dt;
			if (this.coolDownTime < 0f)
			{
				this.StopCoolDown();
				if (this.abilityActivateFxSprite != null)
				{
					this.abilityActivateFxSprite.Visible = true;
					this.abilityActivateFxSprite.Root.GetComponent<Animation>().Play();
				}
			}
			this.cardDimmer.FillAmount = this.coolDownTime / this.totalCoolDownTime;
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if ((id == EventId.WorldInTransitionComplete || id == EventId.HoloCommScreenDestroyed) && !Service.Get<BattleController>().GetCurrentBattle().Canceled)
			{
				this.PlayActiveEquipmentAnimation();
				return EatResponse.NotEaten;
			}
			SmartEntity smartEntity = (SmartEntity)cookie;
			string uid = smartEntity.TroopComp.TroopType.Uid;
			if (uid != this.DeployableUid)
			{
				return EatResponse.NotEaten;
			}
			switch (id)
			{
			case EventId.HeroDeployed:
				this.HeroEntityID = smartEntity.ID;
				break;
			case EventId.TroopAbilityDeactivate:
				this.PutHeroAbilityOnCoolDown();
				break;
			case EventId.TroopAbilityCoolDownComplete:
				this.StopCoolDown();
				this.PrepareHeroAbility();
				break;
			case EventId.HeroKilled:
				this.Disable();
				this.StopObserving();
				break;
			}
			return EatResponse.NotEaten;
		}

		protected internal DeployableTroopControl(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).Disable();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).Disable(*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).Enable();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).AbilityState);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).DeployableUid);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).DisableDueToBuildingDestruction);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).Enabled);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).IsHero);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).IsStarship);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).TroopCheckbox);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).TroopCountLabel);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).IsEquipmentActiveOnDeployable((IDataController)GCHandledObjects.GCHandleToObject(*args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1))));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).PlayActiveEquipmentAnimation();
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).PrepareHeroAbility();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).PutHeroAbilityOnCoolDown();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).AbilityState = (HeroAbilityState)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).DeployableUid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).DisableDueToBuildingDestruction = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).Enabled = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).IsHero = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).IsStarship = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).TroopCheckbox = (UXCheckbox)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).TroopCountLabel = (UXLabel)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).StopActiveEquipmentAnimation();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).StopCoolDown();
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).StopObserving();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((DeployableTroopControl)GCHandledObjects.GCHandleToObject(instance)).UseHeroAbility();
			return -1L;
		}
	}
}
