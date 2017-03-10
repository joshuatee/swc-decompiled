using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class BuildingAnimationComponent : ComponentBase
	{
		public bool BuildingUpgrading
		{
			get;
			set;
		}

		public bool Manufacturing
		{
			get;
			set;
		}

		public Animation Anim
		{
			get;
			set;
		}

		public List<ParticleSystem> ListOfParticleSystems
		{
			get;
			set;
		}

		public BuildingAnimationComponent(Animation anim, List<ParticleSystem> listOfParticleSystems)
		{
			this.Anim = anim;
			this.ListOfParticleSystems = listOfParticleSystems;
			this.BuildingUpgrading = false;
			this.Manufacturing = false;
		}

		protected internal BuildingAnimationComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingAnimationComponent)GCHandledObjects.GCHandleToObject(instance)).Anim);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingAnimationComponent)GCHandledObjects.GCHandleToObject(instance)).BuildingUpgrading);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingAnimationComponent)GCHandledObjects.GCHandleToObject(instance)).ListOfParticleSystems);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuildingAnimationComponent)GCHandledObjects.GCHandleToObject(instance)).Manufacturing);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((BuildingAnimationComponent)GCHandledObjects.GCHandleToObject(instance)).Anim = (Animation)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((BuildingAnimationComponent)GCHandledObjects.GCHandleToObject(instance)).BuildingUpgrading = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((BuildingAnimationComponent)GCHandledObjects.GCHandleToObject(instance)).ListOfParticleSystems = (List<ParticleSystem>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((BuildingAnimationComponent)GCHandledObjects.GCHandleToObject(instance)).Manufacturing = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
