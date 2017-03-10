using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.FX
{
	public class ShieldBuildingInfo
	{
		public Entity Building
		{
			get;
			private set;
		}

		public List<AssetHandle> AssetHandles
		{
			get;
			set;
		}

		public Dictionary<string, GameObject> EffectAssets
		{
			get;
			private set;
		}

		public List<ShieldReason> Reasons
		{
			get;
			private set;
		}

		public bool LoadComplete
		{
			get;
			set;
		}

		public GameObject Shield
		{
			get;
			set;
		}

		public ParticleSystem Spark
		{
			get;
			set;
		}

		public ParticleSystem Destruction
		{
			get;
			set;
		}

		public GameObject Generator
		{
			get;
			set;
		}

		public GameObject Top
		{
			get;
			set;
		}

		public Material DecalMaterial
		{
			get;
			set;
		}

		public Material ShieldMaterial
		{
			get;
			set;
		}

		public ShieldDissolve ShieldDisolveEffect
		{
			get;
			set;
		}

		public ShieldBuildingInfo(Entity building, List<string> effectUids)
		{
			this.Building = building;
			this.EffectAssets = new Dictionary<string, GameObject>();
			int i = 0;
			int count = effectUids.Count;
			while (i < count)
			{
				this.EffectAssets.Add(effectUids[i], null);
				i++;
			}
			this.Reasons = new List<ShieldReason>();
			this.LoadComplete = false;
			this.ShieldDisolveEffect = new ShieldDissolve();
		}

		public void PlayShieldDisolveEffect(bool turnOn, DissolveCompleteDelegate OnCompleteCallback)
		{
			this.ShieldDisolveEffect.Play(this.Shield, this.ShieldMaterial, turnOn, OnCompleteCallback, this);
		}

		protected internal ShieldBuildingInfo(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).AssetHandles);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Building);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).DecalMaterial);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Destruction);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).EffectAssets);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Generator);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).LoadComplete);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Reasons);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Shield);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).ShieldDisolveEffect);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).ShieldMaterial);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Spark);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Top);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).PlayShieldDisolveEffect(*(sbyte*)args != 0, (DissolveCompleteDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).AssetHandles = (List<AssetHandle>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Building = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).DecalMaterial = (Material)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Destruction = (ParticleSystem)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).EffectAssets = (Dictionary<string, GameObject>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Generator = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).LoadComplete = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Reasons = (List<ShieldReason>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Shield = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).ShieldDisolveEffect = (ShieldDissolve)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).ShieldMaterial = (Material)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Spark = (ParticleSystem)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((ShieldBuildingInfo)GCHandledObjects.GCHandleToObject(instance)).Top = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
