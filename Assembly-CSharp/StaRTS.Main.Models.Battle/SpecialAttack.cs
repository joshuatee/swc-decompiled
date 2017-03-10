using StaRTS.Assets;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Views.World;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Battle
{
	public class SpecialAttack
	{
		public AssetHandle Handle;

		public AssetHandle AttachmentHandle;

		private const string ATTACHED_SHADOW_TRIGGER = "Stored";

		private const string FALLING_SHADOW_TRIGGER = "Drop";

		private const string GROUND_SHADOW_TRIGGER = "Idle";

		private int shotIndex;

		public GameObject StarshipGameObject
		{
			get;
			set;
		}

		public GameObject StarshipDetachableGameObject
		{
			get;
			set;
		}

		public ShadowAnim ShadowAnimator
		{
			get;
			set;
		}

		public ShadowAnim DetachableObjectShadowAnimator
		{
			get;
			private set;
		}

		public Animation DetachableShadowAnimation
		{
			get;
			set;
		}

		public List<GameObject> GunLocators
		{
			get;
			set;
		}

		public LandingTakeOffEffectAnim EffectAnimator
		{
			get;
			set;
		}

		public SpecialAttackTypeVO VO
		{
			get;
			private set;
		}

		public TeamType TeamType
		{
			get;
			private set;
		}

		public Vector3 TargetWorldPos
		{
			get;
			set;
		}

		public int TargetBoardX
		{
			get;
			set;
		}

		public int TargetBoardZ
		{
			get;
			set;
		}

		public ShieldGeneratorComponent TargetShield
		{
			get;
			set;
		}

		public List<Buff> SpecialAttackBuffs
		{
			get;
			private set;
		}

		public uint AttackerIndex
		{
			get;
			set;
		}

		public uint SpawnDelay
		{
			get;
			set;
		}

		public SpecialAttack(SpecialAttackTypeVO vo, TeamType teamType, Vector3 targetWorldPos, int targetBoardX, int targetBoardZ)
		{
			this.VO = vo;
			this.TeamType = teamType;
			this.TargetWorldPos = targetWorldPos;
			this.TargetBoardX = targetBoardX;
			this.TargetBoardZ = targetBoardZ;
			this.SpecialAttackBuffs = new List<Buff>();
			this.StarshipGameObject = null;
			this.GunLocators = null;
			this.StarshipDetachableGameObject = null;
			this.DetachableShadowAnimation = null;
			this.TargetShield = null;
		}

		public void SetupDetachableShadowAnimator()
		{
			if (this.StarshipDetachableGameObject != null)
			{
				this.DetachableShadowAnimation = this.StarshipDetachableGameObject.GetComponent<Animation>();
				this.DetachableObjectShadowAnimator = new ShadowAnim(this.StarshipDetachableGameObject, 0f);
				this.DetachableObjectShadowAnimator.PlayShadowAnim(true);
				return;
			}
			this.DetachableShadowAnimation = null;
		}

		public void UpdateDetachableShadowAnimator(SpecialAttackDetachableObjectState state)
		{
			if (this.DetachableShadowAnimation != null)
			{
				switch (state)
				{
				case SpecialAttackDetachableObjectState.Attached:
					this.DetachableShadowAnimation.Play("Stored");
					return;
				case SpecialAttackDetachableObjectState.Falling:
					this.DetachableShadowAnimation.Play("Drop");
					return;
				case SpecialAttackDetachableObjectState.OnGround:
					this.DetachableShadowAnimation.Play("Idle");
					break;
				default:
					return;
				}
			}
		}

		public GameObject GetGunLocator()
		{
			if (this.GunLocators == null || this.GunLocators.Count < 1)
			{
				return null;
			}
			this.shotIndex = (this.shotIndex + 1) % this.GunLocators.Count;
			if (this.GunLocators == null)
			{
				return null;
			}
			return this.GunLocators[this.shotIndex];
		}

		private int FindSpecialAttackBuff(string buffID)
		{
			int count = this.SpecialAttackBuffs.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.SpecialAttackBuffs[i].BuffType.BuffID == buffID)
				{
					return i;
				}
			}
			return -1;
		}

		public void ApplySpecialAttackBuffs(ref int modifyValue)
		{
			int i = 0;
			int count = this.SpecialAttackBuffs.Count;
			while (i < count)
			{
				Buff buff = this.SpecialAttackBuffs[i];
				buff.ApplyStacks(ref modifyValue, modifyValue);
				i++;
			}
		}

		public void AddAppliedBuff(BuffTypeVO buffVO, BuffVisualPriority visualPriority)
		{
			int num = this.FindSpecialAttackBuff(buffVO.BuffID);
			Buff buff;
			if (num < 0)
			{
				buff = new Buff(buffVO, ArmorType.FlierVehicle, visualPriority);
				buff.AddStack();
				this.SpecialAttackBuffs.Add(buff);
				return;
			}
			buff = this.SpecialAttackBuffs[num];
			if (buffVO.Lvl > buff.BuffType.Lvl)
			{
				buff.UpgradeBuff(buffVO);
			}
			buff.AddStack();
		}

		public void ClearAppliedBuffs()
		{
			this.SpecialAttackBuffs = null;
		}

		protected internal SpecialAttack(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).AddAppliedBuff((BuffTypeVO)GCHandledObjects.GCHandleToObject(*args), (BuffVisualPriority)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).ClearAppliedBuffs();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).FindSpecialAttackBuff(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).DetachableObjectShadowAnimator);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).DetachableShadowAnimation);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).EffectAnimator);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).GunLocators);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).ShadowAnimator);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackBuffs);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).StarshipDetachableGameObject);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).StarshipGameObject);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).TargetBoardX);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).TargetBoardZ);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).TargetShield);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).TargetWorldPos);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).TeamType);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).VO);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).GetGunLocator());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).DetachableObjectShadowAnimator = (ShadowAnim)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).DetachableShadowAnimation = (Animation)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).EffectAnimator = (LandingTakeOffEffectAnim)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).GunLocators = (List<GameObject>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).ShadowAnimator = (ShadowAnim)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).SpecialAttackBuffs = (List<Buff>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).StarshipDetachableGameObject = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).StarshipGameObject = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).TargetBoardX = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).TargetBoardZ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).TargetShield = (ShieldGeneratorComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).TargetWorldPos = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).TeamType = (TeamType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).VO = (SpecialAttackTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).SetupDetachableShadowAnimator();
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((SpecialAttack)GCHandledObjects.GCHandleToObject(instance)).UpdateDetachableShadowAnimator((SpecialAttackDetachableObjectState)(*(int*)args));
			return -1L;
		}
	}
}
