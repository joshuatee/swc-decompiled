using Game.Behaviors;
using StaRTS.FX;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.Entities.Projectiles;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.World
{
	public class ProjectileView
	{
		private const string GAMEOBJECT_NAME = "Projectile";

		private bool isMultipleEmitter;

		private GameObject root;

		private GameObject DefaultTrackerObject;

		private TrailRenderer trailToClear;

		public ProjectileTypeVO ProjectileType
		{
			get;
			private set;
		}

		public Bullet Bullet
		{
			get;
			private set;
		}

		public bool IsOnGround
		{
			get;
			private set;
		}

		public Vector3 StartLocation
		{
			get;
			private set;
		}

		public float LifetimeSeconds
		{
			get;
			set;
		}

		public Transform EmitterTracker
		{
			get;
			private set;
		}

		public Transform MeshTracker
		{
			get;
			private set;
		}

		public Transform TargetTracker
		{
			get;
			private set;
		}

		public Transform DefaultTracker
		{
			get;
			private set;
		}

		public ParticleSystem Emitter
		{
			get;
			private set;
		}

		public Vector3 TargetLocation
		{
			get;
			private set;
		}

		public ProjectileViewPath ViewPath
		{
			get;
			private set;
		}

		public ProjectileView()
		{
			this.OnReturnToPool();
		}

		public void Init(ProjectileTypeVO projectileType, Bullet bullet, bool isOnGround)
		{
			this.ProjectileType = projectileType;
			this.Bullet = bullet;
			this.IsOnGround = isOnGround;
		}

		public void InitWithoutBullet(float lifeSeconds, Vector3 start, Vector3 target, Transform targetTracker)
		{
			this.DefaultTrackerObject = new GameObject();
			this.DefaultTracker = this.DefaultTrackerObject.transform;
			this.DefaultTracker.position = start;
			this.InternalInit(lifeSeconds, start, target, targetTracker);
		}

		public void InitWithMesh(float lifeSeconds, Vector3 start, Vector3 target, GameObject mesh, Transform targetTracker)
		{
			this.InternalInit(lifeSeconds, start, target, targetTracker);
			this.InitMeshTracker(mesh);
		}

		public void InitWithEmitter(float lifeSeconds, Vector3 start, Vector3 target, ParticleSystem emitter, Transform targetTracker)
		{
			this.InternalInit(lifeSeconds, start, target, targetTracker);
			this.InitEmitterTracker(emitter);
		}

		public void InitWithEmitters(float lifeSeconds, Vector3 start, Vector3 target, GameObject rootNode, GameObject mesh, Transform targetTracker)
		{
			this.InternalInit(lifeSeconds, start, target, targetTracker);
			if (mesh != null)
			{
				this.InitMeshTracker(mesh);
			}
			this.InitMultipleEmitterTracker(rootNode);
		}

		public void InitWithMeshAndEmitter(float lifeSeconds, Vector3 start, Vector3 target, GameObject mesh, ParticleSystem emitter, Transform targetTracker)
		{
			this.InternalInit(lifeSeconds, start, target, targetTracker);
			this.InitMeshTracker(mesh);
			this.InitEmitterTracker(emitter);
		}

		public void SetTargetLocation(Vector3 target)
		{
			this.TargetLocation = target;
		}

		public Transform GetTransform()
		{
			Transform result = null;
			if (this.root != null)
			{
				result = this.root.transform;
			}
			else if (this.MeshTracker != null)
			{
				result = this.MeshTracker;
			}
			else if (this.EmitterTracker != null)
			{
				result = this.EmitterTracker;
			}
			else if (this.DefaultTracker != null)
			{
				result = this.DefaultTracker;
			}
			return result;
		}

		private void InternalInit(float lifeSeconds, Vector3 start, Vector3 target, Transform targetTransform)
		{
			this.LifetimeSeconds = lifeSeconds;
			this.StartLocation = start;
			this.SetTargetLocation(target);
			this.TargetTracker = targetTransform;
			if (this.ProjectileType.IsMultiStage)
			{
				this.ViewPath = new ProjectileViewMultiStagePath(this);
				return;
			}
			if (this.ProjectileType.Arcs)
			{
				this.ViewPath = new ProjectileViewArcPath(this);
				return;
			}
			this.ViewPath = new ProjectileViewLinearPath(this);
		}

		private void InitMeshTracker(GameObject mesh)
		{
			this.MeshTracker = mesh.transform;
			this.MeshTracker.position = this.StartLocation;
			if (!this.ProjectileType.Arcs)
			{
				this.MeshTracker.LookAt(this.TargetLocation);
			}
			mesh.SetActive(true);
		}

		private void InitMultipleEmitterTracker(GameObject rootNode)
		{
			this.root = rootNode;
			this.EmitterTracker = rootNode.transform;
			this.EmitterTracker.position = this.StartLocation;
			this.EmitterTracker.LookAt(this.TargetLocation);
			this.isMultipleEmitter = true;
			this.root.SetActive(true);
			ParticleSystem[] allEmitters = MultipleEmittersPool.GetAllEmitters(this.root);
			ParticleSystem[] array = allEmitters;
			for (int i = 0; i < array.Length; i++)
			{
				ParticleSystem particle = array[i];
				this.PlayEmitter(particle);
				this.PlayTrails(particle);
			}
		}

		private void InitEmitterTracker(ParticleSystem emitter)
		{
			this.EmitterTracker = emitter.transform;
			this.EmitterTracker.position = this.StartLocation;
			this.EmitterTracker.LookAt(this.TargetLocation);
			this.Emitter = emitter;
			this.PlayEmitter(this.Emitter);
			this.Emitter.gameObject.SetActive(true);
			this.PlayTrails(this.Emitter);
		}

		private void PlayEmitter(ParticleSystem particle)
		{
			if (Mathf.Approximately(particle.emission.rate.constantMax, 0f))
			{
				particle.startLifetime = this.LifetimeSeconds;
			}
			UnityUtils.PlayParticleEmitter(particle);
		}

		private void PlayTrails(ParticleSystem particle)
		{
			TrailRenderer component = particle.gameObject.GetComponent<TrailRenderer>();
			if (component != null)
			{
				component.enabled = true;
				component.time = -component.time;
				this.trailToClear = component;
			}
			WeaponTrail component2 = particle.gameObject.GetComponent<WeaponTrail>();
			if (component2 != null)
			{
				component2.Restart();
			}
		}

		private void ResetParticle(ParticleSystem particle)
		{
			if (particle != null)
			{
				particle.Stop(false);
				TrailRenderer component = particle.gameObject.GetComponent<TrailRenderer>();
				if (component != null)
				{
					component.enabled = false;
					if (component.time < 0f)
					{
						component.time = -component.time;
					}
				}
			}
		}

		public bool Update(float dt, out float distSquared)
		{
			this.ViewPath.AgeSeconds += dt;
			bool flag = this.ViewPath.AgeSeconds >= this.LifetimeSeconds;
			if (flag)
			{
				Service.Get<EventManager>().SendEvent(EventId.ProjectileViewPathComplete, this.Bullet);
				this.Reset();
				distSquared = 0f;
			}
			else
			{
				this.ViewPath.OnUpdate(dt);
				distSquared = (this.TargetLocation - this.ViewPath.CurrentLocation).sqrMagnitude;
				if (this.trailToClear != null && this.ViewPath.AgeSeconds > dt)
				{
					this.trailToClear.time = -this.trailToClear.time;
					this.trailToClear = null;
				}
			}
			return flag;
		}

		public void Reset()
		{
			if (this.DefaultTrackerObject != null)
			{
				this.DefaultTracker.DetachChildren();
				this.DefaultTracker = null;
				UnityEngine.Object.Destroy(this.DefaultTrackerObject);
				this.DefaultTrackerObject = null;
			}
			this.ResetParticle(this.Emitter);
			if (this.root != null)
			{
				ParticleSystem[] allEmitters = MultipleEmittersPool.GetAllEmitters(this.root);
				ParticleSystem[] array = allEmitters;
				for (int i = 0; i < array.Length; i++)
				{
					ParticleSystem particle = array[i];
					this.ResetParticle(particle);
				}
			}
			if (this.MeshTracker != null && this.MeshTracker.gameObject != null)
			{
				this.MeshTracker.gameObject.SetActive(false);
			}
			if (this.MeshTracker != null)
			{
				this.MeshTracker.DetachChildren();
			}
			if (this.EmitterTracker != null)
			{
				if (this.isMultipleEmitter)
				{
					this.root.SetActive(false);
				}
				else
				{
					this.EmitterTracker.DetachChildren();
				}
			}
			this.root = null;
		}

		public void OnReturnToPool()
		{
			this.Emitter = null;
			this.MeshTracker = null;
			this.EmitterTracker = null;
			this.DefaultTracker = null;
			this.trailToClear = null;
			this.Bullet = null;
			if (this.DefaultTrackerObject != null)
			{
				UnityEngine.Object.Destroy(this.DefaultTrackerObject);
				this.DefaultTrackerObject = null;
			}
			this.ViewPath = null;
		}

		protected internal ProjectileView(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).Bullet);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).DefaultTracker);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).Emitter);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).EmitterTracker);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).IsOnGround);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).LifetimeSeconds);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).MeshTracker);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).ProjectileType);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).StartLocation);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).TargetLocation);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).TargetTracker);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).ViewPath);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).GetTransform());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).Init((ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args), (Bullet)GCHandledObjects.GCHandleToObject(args[1]), *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).InitEmitterTracker((ParticleSystem)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).InitMeshTracker((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).InitMultipleEmitterTracker((GameObject)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).InitWithEmitter(*(float*)args, *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2)), (ParticleSystem)GCHandledObjects.GCHandleToObject(args[3]), (Transform)GCHandledObjects.GCHandleToObject(args[4]));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).InitWithEmitters(*(float*)args, *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2)), (GameObject)GCHandledObjects.GCHandleToObject(args[3]), (GameObject)GCHandledObjects.GCHandleToObject(args[4]), (Transform)GCHandledObjects.GCHandleToObject(args[5]));
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).InitWithMesh(*(float*)args, *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2)), (GameObject)GCHandledObjects.GCHandleToObject(args[3]), (Transform)GCHandledObjects.GCHandleToObject(args[4]));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).InitWithMeshAndEmitter(*(float*)args, *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2)), (GameObject)GCHandledObjects.GCHandleToObject(args[3]), (ParticleSystem)GCHandledObjects.GCHandleToObject(args[4]), (Transform)GCHandledObjects.GCHandleToObject(args[5]));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).InitWithoutBullet(*(float*)args, *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2)), (Transform)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).InternalInit(*(float*)args, *(*(IntPtr*)(args + 1)), *(*(IntPtr*)(args + 2)), (Transform)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).OnReturnToPool();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).PlayEmitter((ParticleSystem)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).PlayTrails((ParticleSystem)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).Reset();
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).ResetParticle((ParticleSystem)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).Bullet = (Bullet)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).DefaultTracker = (Transform)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).Emitter = (ParticleSystem)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).EmitterTracker = (Transform)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).IsOnGround = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).LifetimeSeconds = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).MeshTracker = (Transform)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).ProjectileType = (ProjectileTypeVO)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).StartLocation = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).TargetLocation = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).TargetTracker = (Transform)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).ViewPath = (ProjectileViewPath)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			((ProjectileView)GCHandledObjects.GCHandleToObject(instance)).SetTargetLocation(*(*(IntPtr*)args));
			return -1L;
		}
	}
}
