using Game.Behaviors;
using StaRTS.FX;
using StaRTS.Main.Models.Entities;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class DeployedTroop
	{
		public string Uid
		{
			get;
			private set;
		}

		public SmartEntity Entity
		{
			get;
			private set;
		}

		public uint AbilityTimer
		{
			get;
			set;
		}

		public uint CoolDownTimer
		{
			get;
			set;
		}

		public int AbilityClipCount
		{
			get;
			set;
		}

		public bool Activated
		{
			get;
			set;
		}

		public LightSaberHitEffect LightSaberHitFx
		{
			get;
			set;
		}

		public WeaponTrail WeaponTrail
		{
			get;
			set;
		}

		public float WeaponTrailActivateLifetime
		{
			get;
			set;
		}

		public float WeaponTrailDeactivateLifetime
		{
			get;
			set;
		}

		public bool EffectsSetup
		{
			get;
			set;
		}

		public DeployedTroop(string uid, SmartEntity entity)
		{
			this.Uid = uid;
			this.Entity = entity;
			this.AbilityTimer = 0u;
			this.CoolDownTimer = 0u;
			this.AbilityClipCount = 0;
			this.Activated = false;
		}

		protected internal DeployedTroop(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).AbilityClipCount);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).Activated);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).EffectsSetup);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).LightSaberHitFx);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).WeaponTrail);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).WeaponTrailActivateLifetime);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).WeaponTrailDeactivateLifetime);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).AbilityClipCount = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).Activated = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).EffectsSetup = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).Entity = (SmartEntity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).LightSaberHitFx = (LightSaberHitEffect)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).WeaponTrail = (WeaponTrail)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).WeaponTrailActivateLifetime = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((DeployedTroop)GCHandledObjects.GCHandleToObject(instance)).WeaponTrailDeactivateLifetime = *(float*)args;
			return -1L;
		}
	}
}
