using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

public class AssetMeshDataMonoBehaviour : MonoBehaviour, IUnitySerializable
{
	public List<GameObject> SelectableGameObjects;

	public GameObject ShadowGameObject;

	public List<GameObject> GunLocatorGameObjects;

	public GameObject TickerGameObject;

	public GameObject MeterGameObject;

	public List<GameObject> OtherGameObjects;

	public List<AnimationClip> ListOfAnimationTracks;

	public List<ParticleSystem> ListOfParticleSystems;

	public float WalkSpeed;

	public float RunSpeed;

	public AssetMeshDataMonoBehaviour()
	{
		this.SelectableGameObjects = new List<GameObject>();
		this.GunLocatorGameObjects = new List<GameObject>();
		this.OtherGameObjects = new List<GameObject>();
		this.ListOfAnimationTracks = new List<AnimationClip>();
		this.ListOfParticleSystems = new List<ParticleSystem>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			if (this.SelectableGameObjects == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.SelectableGameObjects.Count);
				for (int i = 0; i < this.SelectableGameObjects.Count; i++)
				{
					SerializedStateWriter.Instance.WriteUnityEngineObject(this.SelectableGameObjects[i]);
				}
			}
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.ShadowGameObject);
		}
		if (depth <= 7)
		{
			if (this.GunLocatorGameObjects == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.GunLocatorGameObjects.Count);
				for (int i = 0; i < this.GunLocatorGameObjects.Count; i++)
				{
					SerializedStateWriter.Instance.WriteUnityEngineObject(this.GunLocatorGameObjects[i]);
				}
			}
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.TickerGameObject);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.MeterGameObject);
		}
		if (depth <= 7)
		{
			if (this.OtherGameObjects == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.OtherGameObjects.Count);
				for (int i = 0; i < this.OtherGameObjects.Count; i++)
				{
					SerializedStateWriter.Instance.WriteUnityEngineObject(this.OtherGameObjects[i]);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.ListOfAnimationTracks == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.ListOfAnimationTracks.Count);
				for (int i = 0; i < this.ListOfAnimationTracks.Count; i++)
				{
					SerializedStateWriter.Instance.WriteUnityEngineObject(this.ListOfAnimationTracks[i]);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.ListOfParticleSystems == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.ListOfParticleSystems.Count);
				for (int i = 0; i < this.ListOfParticleSystems.Count; i++)
				{
					SerializedStateWriter.Instance.WriteUnityEngineObject(this.ListOfParticleSystems[i]);
				}
			}
		}
		SerializedStateWriter.Instance.WriteSingle(this.WalkSpeed);
		SerializedStateWriter.Instance.WriteSingle(this.RunSpeed);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.SelectableGameObjects = new List<GameObject>(num);
			for (int i = 0; i < num; i++)
			{
				this.SelectableGameObjects.Add(SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
			}
		}
		if (depth <= 7)
		{
			this.ShadowGameObject = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.GunLocatorGameObjects = new List<GameObject>(num);
			for (int i = 0; i < num; i++)
			{
				this.GunLocatorGameObjects.Add(SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
			}
		}
		if (depth <= 7)
		{
			this.TickerGameObject = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.MeterGameObject = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.OtherGameObjects = new List<GameObject>(num);
			for (int i = 0; i < num; i++)
			{
				this.OtherGameObjects.Add(SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.ListOfAnimationTracks = new List<AnimationClip>(num);
			for (int i = 0; i < num; i++)
			{
				this.ListOfAnimationTracks.Add(SerializedStateReader.Instance.ReadUnityEngineObject() as AnimationClip);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.ListOfParticleSystems = new List<ParticleSystem>(num);
			for (int i = 0; i < num; i++)
			{
				this.ListOfParticleSystems.Add(SerializedStateReader.Instance.ReadUnityEngineObject() as ParticleSystem);
			}
		}
		this.WalkSpeed = SerializedStateReader.Instance.ReadSingle();
		this.RunSpeed = SerializedStateReader.Instance.ReadSingle();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (depth <= 7)
		{
			if (this.SelectableGameObjects != null)
			{
				for (int i = 0; i < this.SelectableGameObjects.Count; i++)
				{
					this.SelectableGameObjects[i] = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.SelectableGameObjects[i]) as GameObject);
				}
			}
		}
		if (this.ShadowGameObject != null)
		{
			this.ShadowGameObject = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.ShadowGameObject) as GameObject);
		}
		if (depth <= 7)
		{
			if (this.GunLocatorGameObjects != null)
			{
				for (int i = 0; i < this.GunLocatorGameObjects.Count; i++)
				{
					this.GunLocatorGameObjects[i] = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.GunLocatorGameObjects[i]) as GameObject);
				}
			}
		}
		if (this.TickerGameObject != null)
		{
			this.TickerGameObject = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.TickerGameObject) as GameObject);
		}
		if (this.MeterGameObject != null)
		{
			this.MeterGameObject = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.MeterGameObject) as GameObject);
		}
		if (depth <= 7)
		{
			if (this.OtherGameObjects != null)
			{
				for (int i = 0; i < this.OtherGameObjects.Count; i++)
				{
					this.OtherGameObjects[i] = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.OtherGameObjects[i]) as GameObject);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.ListOfAnimationTracks != null)
			{
				for (int i = 0; i < this.ListOfAnimationTracks.Count; i++)
				{
					this.ListOfAnimationTracks[i] = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.ListOfAnimationTracks[i]) as AnimationClip);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.ListOfParticleSystems != null)
			{
				for (int i = 0; i < this.ListOfParticleSystems.Count; i++)
				{
					this.ListOfParticleSystems[i] = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.ListOfParticleSystems[i]) as ParticleSystem);
				}
			}
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			if (this.SelectableGameObjects == null)
			{
				ISerializedNamedStateWriter arg_29_0 = SerializedNamedStateWriter.Instance;
				var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
				var_0_cp_1 = 0;
				arg_29_0.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4694, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4694, this.SelectableGameObjects.Count);
				for (int i = 0; i < this.SelectableGameObjects.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.SelectableGameObjects[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.ShadowGameObject, &var_0_cp_0[var_0_cp_1] + 4716);
		}
		if (depth <= 7)
		{
			if (this.GunLocatorGameObjects == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4733, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4733, this.GunLocatorGameObjects.Count);
				for (int i = 0; i < this.GunLocatorGameObjects.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.GunLocatorGameObjects[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.TickerGameObject, &var_0_cp_0[var_0_cp_1] + 4755);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.MeterGameObject, &var_0_cp_0[var_0_cp_1] + 4772);
		}
		if (depth <= 7)
		{
			if (this.OtherGameObjects == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4788, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4788, this.OtherGameObjects.Count);
				for (int i = 0; i < this.OtherGameObjects.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.OtherGameObjects[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.ListOfAnimationTracks == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4805, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4805, this.ListOfAnimationTracks.Count);
				for (int i = 0; i < this.ListOfAnimationTracks.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.ListOfAnimationTracks[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.ListOfParticleSystems == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4827, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4827, this.ListOfParticleSystems.Count);
				for (int i = 0; i < this.ListOfParticleSystems.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.ListOfParticleSystems[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		SerializedNamedStateWriter.Instance.WriteSingle(this.WalkSpeed, &var_0_cp_0[var_0_cp_1] + 4849);
		SerializedNamedStateWriter.Instance.WriteSingle(this.RunSpeed, &var_0_cp_0[var_0_cp_1] + 4859);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateReader arg_1E_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			int num = arg_1E_0.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4694);
			this.SelectableGameObjects = new List<GameObject>(num);
			for (int i = 0; i < num; i++)
			{
				this.SelectableGameObjects.Add(SerializedNamedStateReader.Instance.ReadUnityEngineObject((IntPtr)0) as GameObject);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			this.ShadowGameObject = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 4716) as GameObject);
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4733);
			this.GunLocatorGameObjects = new List<GameObject>(num);
			for (int i = 0; i < num; i++)
			{
				this.GunLocatorGameObjects.Add(SerializedNamedStateReader.Instance.ReadUnityEngineObject((IntPtr)0) as GameObject);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			this.TickerGameObject = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 4755) as GameObject);
		}
		if (depth <= 7)
		{
			this.MeterGameObject = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 4772) as GameObject);
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4788);
			this.OtherGameObjects = new List<GameObject>(num);
			for (int i = 0; i < num; i++)
			{
				this.OtherGameObjects.Add(SerializedNamedStateReader.Instance.ReadUnityEngineObject((IntPtr)0) as GameObject);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4805);
			this.ListOfAnimationTracks = new List<AnimationClip>(num);
			for (int i = 0; i < num; i++)
			{
				this.ListOfAnimationTracks.Add(SerializedNamedStateReader.Instance.ReadUnityEngineObject((IntPtr)0) as AnimationClip);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4827);
			this.ListOfParticleSystems = new List<ParticleSystem>(num);
			for (int i = 0; i < num; i++)
			{
				this.ListOfParticleSystems.Add(SerializedNamedStateReader.Instance.ReadUnityEngineObject((IntPtr)0) as ParticleSystem);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		this.WalkSpeed = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 4849);
		this.RunSpeed = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 4859);
	}

	protected internal AssetMeshDataMonoBehaviour(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((AssetMeshDataMonoBehaviour)instance).ShadowGameObject);
	}

	public static void $Set0(object instance, long value)
	{
		((AssetMeshDataMonoBehaviour)instance).ShadowGameObject = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((AssetMeshDataMonoBehaviour)instance).TickerGameObject);
	}

	public static void $Set1(object instance, long value)
	{
		((AssetMeshDataMonoBehaviour)instance).TickerGameObject = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((AssetMeshDataMonoBehaviour)instance).MeterGameObject);
	}

	public static void $Set2(object instance, long value)
	{
		((AssetMeshDataMonoBehaviour)instance).MeterGameObject = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get3(object instance)
	{
		return ((AssetMeshDataMonoBehaviour)instance).WalkSpeed;
	}

	public static void $Set3(object instance, float value)
	{
		((AssetMeshDataMonoBehaviour)instance).WalkSpeed = value;
	}

	public static float $Get4(object instance)
	{
		return ((AssetMeshDataMonoBehaviour)instance).RunSpeed;
	}

	public static void $Set4(object instance, float value)
	{
		((AssetMeshDataMonoBehaviour)instance).RunSpeed = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((AssetMeshDataMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((AssetMeshDataMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((AssetMeshDataMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((AssetMeshDataMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((AssetMeshDataMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
