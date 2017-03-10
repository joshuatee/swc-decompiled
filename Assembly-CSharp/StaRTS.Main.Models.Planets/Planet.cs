using StaRTS.Assets;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils;
using StaRTS.Main.Views.UX.Screens.ScreenHelpers;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Models.Planets
{
	public class Planet
	{
		private float THRASH_TIME;

		private float THRASH_DELTA_PERCENT;

		private const int THRASH_CAP = 20;

		public AssetHandle Handle;

		public AssetHandle EventEffectsHandle;

		public AssetHandle ParticleFXHandle;

		public AssetHandle PlanetGlowHandle;

		public PlanetFXState CurrentPlanetFXState;

		public Material planetMaterial;

		private TimedEventState tournamentState;

		private TournamentVO tournamentVO;

		private int actualPopulation;

		private int thrashValue;

		private float thrashTimer;

		private int thrashDelta;

		public GameObject PlanetGameObject
		{
			get;
			set;
		}

		public GameObject EventEffect
		{
			get;
			set;
		}

		public GameObject ParticleFX
		{
			get;
			set;
		}

		public GameObject PlanetGlowEffect
		{
			get;
			set;
		}

		public GameObject PlanetExplosions
		{
			get;
			set;
		}

		public ParticleSystem ParticleRings
		{
			get;
			set;
		}

		public float OriginalRingSize
		{
			get;
			set;
		}

		public PlanetVO VO
		{
			get;
			set;
		}

		public Vector3 ObjectExtents
		{
			get;
			set;
		}

		public TournamentVO Tournament
		{
			get
			{
				return this.tournamentVO;
			}
			set
			{
				this.tournamentVO = value;
				this.tournamentState = ((this.tournamentVO != null) ? TimedEventUtils.GetState(this.tournamentVO) : TimedEventState.Invalid);
			}
		}

		public TimedEventState TournamentState
		{
			get
			{
				return this.tournamentState;
			}
		}

		public TimedEventCountdownHelper TournamentCountdown
		{
			get;
			set;
		}

		public bool IsCurrentPlanet
		{
			get;
			set;
		}

		public bool CurrentBackUIShown
		{
			get;
			set;
		}

		public bool IsForegrounded
		{
			get;
			set;
		}

		public List<SocialFriendData> FriendsOnPlanet
		{
			get;
			set;
		}

		public int NumSquadmatesOnPlanet
		{
			get;
			set;
		}

		public int ThrashingPopulation
		{
			get
			{
				return this.actualPopulation + this.thrashValue;
			}
			set
			{
				this.actualPopulation = value;
			}
		}

		public Planet(PlanetVO vo)
		{
			this.THRASH_TIME = GameConstants.GALAXY_PLANET_POPULATION_UPDATE_TIME;
			this.THRASH_DELTA_PERCENT = GameConstants.GALAXY_PLANET_POPULATION_COUNT_PERCENTAGE;
			this.VO = vo;
			this.thrashTimer = 0f;
			this.ThrashingPopulation = 0;
			this.thrashValue = 0;
			this.IsCurrentPlanet = false;
			this.CurrentBackUIShown = false;
			this.FriendsOnPlanet = new List<SocialFriendData>();
			this.CurrentPlanetFXState = PlanetFXState.Unknown;
			this.tournamentState = TimedEventState.Invalid;
		}

		public bool UpdatePlanetTournamentState()
		{
			bool result = false;
			TournamentVO activeTournamentOnPlanet = TournamentController.GetActiveTournamentOnPlanet(this.VO.Uid);
			if (activeTournamentOnPlanet != this.tournamentVO)
			{
				result = true;
			}
			else if (activeTournamentOnPlanet != null)
			{
				TimedEventState state = TimedEventUtils.GetState(this.tournamentVO);
				if (state != this.tournamentState)
				{
					result = true;
				}
			}
			this.Tournament = activeTournamentOnPlanet;
			return result;
		}

		public bool IsCurrentAndNeedsAnim()
		{
			return this.IsCurrentPlanet && !this.CurrentBackUIShown;
		}

		public void UpdateThrashingPopulation(float dt)
		{
			this.thrashTimer += dt;
			if (this.thrashTimer > this.THRASH_TIME)
			{
				this.thrashTimer = 0f;
				this.thrashDelta = (int)(this.THRASH_DELTA_PERCENT * (float)this.actualPopulation);
				this.thrashValue += Service.Get<Rand>().ViewRangeInt(-this.thrashDelta, this.thrashDelta);
				if (this.thrashValue > 20)
				{
					this.thrashValue = 20;
					return;
				}
				if (this.thrashValue < -20)
				{
					this.thrashValue = -20;
					this.thrashValue = Mathf.Max(this.thrashValue, -this.actualPopulation);
				}
			}
		}

		public void DestroyParticles()
		{
			if (this.ParticleFX != null)
			{
				UnityEngine.Object.Destroy(this.ParticleFX);
				this.ParticleFX = null;
			}
			this.ParticleRings = null;
			this.PlanetExplosions = null;
		}

		public void Destroy()
		{
			if (this.planetMaterial != null)
			{
				UnityUtils.DestroyMaterial(this.planetMaterial);
				this.planetMaterial = null;
			}
			if (this.PlanetGameObject != null)
			{
				UnityEngine.Object.Destroy(this.PlanetGameObject);
				this.PlanetGameObject = null;
			}
			if (this.EventEffect != null)
			{
				UnityEngine.Object.Destroy(this.EventEffect);
				this.EventEffect = null;
			}
			if (this.PlanetGlowEffect != null)
			{
				UnityEngine.Object.Destroy(this.PlanetGlowEffect);
				this.PlanetGlowEffect = null;
			}
			this.DestroyParticles();
			this.Handle = AssetHandle.Invalid;
			this.PlanetGameObject = null;
			this.VO = null;
			this.ObjectExtents = Vector3.zero;
		}

		protected internal Planet(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).DestroyParticles();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).CurrentBackUIShown);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).EventEffect);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).FriendsOnPlanet);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).IsCurrentPlanet);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).IsForegrounded);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).NumSquadmatesOnPlanet);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).ObjectExtents);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).OriginalRingSize);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).ParticleFX);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).ParticleRings);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).PlanetExplosions);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).PlanetGameObject);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).PlanetGlowEffect);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).ThrashingPopulation);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).Tournament);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).TournamentCountdown);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).TournamentState);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).VO);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).IsCurrentAndNeedsAnim());
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).CurrentBackUIShown = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).EventEffect = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).FriendsOnPlanet = (List<SocialFriendData>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).IsCurrentPlanet = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).IsForegrounded = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).NumSquadmatesOnPlanet = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).ObjectExtents = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).OriginalRingSize = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).ParticleFX = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).ParticleRings = (ParticleSystem)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).PlanetExplosions = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).PlanetGameObject = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).PlanetGlowEffect = (GameObject)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).ThrashingPopulation = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).Tournament = (TournamentVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).TournamentCountdown = (TimedEventCountdownHelper)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).VO = (PlanetVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Planet)GCHandledObjects.GCHandleToObject(instance)).UpdatePlanetTournamentState());
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((Planet)GCHandledObjects.GCHandleToObject(instance)).UpdateThrashingPopulation(*(float*)args);
			return -1L;
		}
	}
}
