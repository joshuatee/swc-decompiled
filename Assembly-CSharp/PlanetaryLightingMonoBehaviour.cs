using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

public class PlanetaryLightingMonoBehaviour : MonoBehaviour, IUnitySerializable
{
	public string colorDataVersionSting;

	public List<Gradient> buildingsDark;

	public List<Gradient> buildingsLight;

	public List<Gradient> terrainDark;

	public List<Gradient> terrainLight;

	public List<Gradient> shadow;

	public List<Gradient> units;

	public List<Gradient> gridColor;

	public List<Gradient> buildingGridColor;

	public List<Gradient> wallGridColor;

	public PlanetaryLightingMonoBehaviour()
	{
		this.colorDataVersionSting = "";
		this.buildingsDark = new List<Gradient>();
		this.buildingsLight = new List<Gradient>();
		this.terrainDark = new List<Gradient>();
		this.terrainLight = new List<Gradient>();
		this.shadow = new List<Gradient>();
		this.units = new List<Gradient>();
		this.gridColor = new List<Gradient>();
		this.buildingGridColor = new List<Gradient>();
		this.wallGridColor = new List<Gradient>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteString(this.colorDataVersionSting);
		if (depth <= 7)
		{
			if (this.buildingsDark == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.buildingsDark.Count);
				for (int i = 0; i < this.buildingsDark.Count; i++)
				{
					SerializedStateWriter.Instance.WriteGradient(this.buildingsDark[i]);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.buildingsLight == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.buildingsLight.Count);
				for (int i = 0; i < this.buildingsLight.Count; i++)
				{
					SerializedStateWriter.Instance.WriteGradient(this.buildingsLight[i]);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.terrainDark == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.terrainDark.Count);
				for (int i = 0; i < this.terrainDark.Count; i++)
				{
					SerializedStateWriter.Instance.WriteGradient(this.terrainDark[i]);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.terrainLight == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.terrainLight.Count);
				for (int i = 0; i < this.terrainLight.Count; i++)
				{
					SerializedStateWriter.Instance.WriteGradient(this.terrainLight[i]);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.shadow == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.shadow.Count);
				for (int i = 0; i < this.shadow.Count; i++)
				{
					SerializedStateWriter.Instance.WriteGradient(this.shadow[i]);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.units == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.units.Count);
				for (int i = 0; i < this.units.Count; i++)
				{
					SerializedStateWriter.Instance.WriteGradient(this.units[i]);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.gridColor == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.gridColor.Count);
				for (int i = 0; i < this.gridColor.Count; i++)
				{
					SerializedStateWriter.Instance.WriteGradient(this.gridColor[i]);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.buildingGridColor == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.buildingGridColor.Count);
				for (int i = 0; i < this.buildingGridColor.Count; i++)
				{
					SerializedStateWriter.Instance.WriteGradient(this.buildingGridColor[i]);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.wallGridColor == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.wallGridColor.Count);
				for (int i = 0; i < this.wallGridColor.Count; i++)
				{
					SerializedStateWriter.Instance.WriteGradient(this.wallGridColor[i]);
				}
			}
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		this.colorDataVersionSting = (SerializedStateReader.Instance.ReadString() as string);
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.buildingsDark = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.buildingsDark.Add(SerializedStateReader.Instance.ReadGradient() as Gradient);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.buildingsLight = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.buildingsLight.Add(SerializedStateReader.Instance.ReadGradient() as Gradient);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.terrainDark = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.terrainDark.Add(SerializedStateReader.Instance.ReadGradient() as Gradient);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.terrainLight = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.terrainLight.Add(SerializedStateReader.Instance.ReadGradient() as Gradient);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.shadow = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.shadow.Add(SerializedStateReader.Instance.ReadGradient() as Gradient);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.units = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.units.Add(SerializedStateReader.Instance.ReadGradient() as Gradient);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.gridColor = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.gridColor.Add(SerializedStateReader.Instance.ReadGradient() as Gradient);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.buildingGridColor = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.buildingGridColor.Add(SerializedStateReader.Instance.ReadGradient() as Gradient);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.wallGridColor = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.wallGridColor.Add(SerializedStateReader.Instance.ReadGradient() as Gradient);
			}
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		string arg_1F_1 = this.colorDataVersionSting;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteString(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 4868);
		if (depth <= 7)
		{
			if (this.buildingsDark == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4890, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4890, this.buildingsDark.Count);
				for (int i = 0; i < this.buildingsDark.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteGradient(this.buildingsDark[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.buildingsLight == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4904, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4904, this.buildingsLight.Count);
				for (int i = 0; i < this.buildingsLight.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteGradient(this.buildingsLight[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.terrainDark == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4919, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4919, this.terrainDark.Count);
				for (int i = 0; i < this.terrainDark.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteGradient(this.terrainDark[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.terrainLight == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4931, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4931, this.terrainLight.Count);
				for (int i = 0; i < this.terrainLight.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteGradient(this.terrainLight[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.shadow == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4944, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4944, this.shadow.Count);
				for (int i = 0; i < this.shadow.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteGradient(this.shadow[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.units == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4951, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4951, this.units.Count);
				for (int i = 0; i < this.units.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteGradient(this.units[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.gridColor == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4957, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4957, this.gridColor.Count);
				for (int i = 0; i < this.gridColor.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteGradient(this.gridColor[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.buildingGridColor == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4967, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4967, this.buildingGridColor.Count);
				for (int i = 0; i < this.buildingGridColor.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteGradient(this.buildingGridColor[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.wallGridColor == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4985, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4985, this.wallGridColor.Count);
				for (int i = 0; i < this.wallGridColor.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteGradient(this.wallGridColor[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.colorDataVersionSting = (arg_1A_0.ReadString(&var_0_cp_0[var_0_cp_1] + 4868) as string);
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4890);
			this.buildingsDark = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.buildingsDark.Add(SerializedNamedStateReader.Instance.ReadGradient((IntPtr)0) as Gradient);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4904);
			this.buildingsLight = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.buildingsLight.Add(SerializedNamedStateReader.Instance.ReadGradient((IntPtr)0) as Gradient);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4919);
			this.terrainDark = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.terrainDark.Add(SerializedNamedStateReader.Instance.ReadGradient((IntPtr)0) as Gradient);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4931);
			this.terrainLight = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.terrainLight.Add(SerializedNamedStateReader.Instance.ReadGradient((IntPtr)0) as Gradient);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4944);
			this.shadow = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.shadow.Add(SerializedNamedStateReader.Instance.ReadGradient((IntPtr)0) as Gradient);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4951);
			this.units = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.units.Add(SerializedNamedStateReader.Instance.ReadGradient((IntPtr)0) as Gradient);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4957);
			this.gridColor = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.gridColor.Add(SerializedNamedStateReader.Instance.ReadGradient((IntPtr)0) as Gradient);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4967);
			this.buildingGridColor = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.buildingGridColor.Add(SerializedNamedStateReader.Instance.ReadGradient((IntPtr)0) as Gradient);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 4985);
			this.wallGridColor = new List<Gradient>(num);
			for (int i = 0; i < num; i++)
			{
				this.wallGridColor.Add(SerializedNamedStateReader.Instance.ReadGradient((IntPtr)0) as Gradient);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
	}

	protected internal PlanetaryLightingMonoBehaviour(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((PlanetaryLightingMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((PlanetaryLightingMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((PlanetaryLightingMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((PlanetaryLightingMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((PlanetaryLightingMonoBehaviour)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
