using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[System.Serializable]
public class UISpriteData : IUnitySerializable
{
	public string name;

	public int x;

	public int y;

	public int width;

	public int height;

	public int borderLeft;

	public int borderRight;

	public int borderTop;

	public int borderBottom;

	public int paddingLeft;

	public int paddingRight;

	public int paddingTop;

	public int paddingBottom;

	public bool hasBorder
	{
		get
		{
			return (this.borderLeft | this.borderRight | this.borderTop | this.borderBottom) != 0;
		}
	}

	public bool hasPadding
	{
		get
		{
			return (this.paddingLeft | this.paddingRight | this.paddingTop | this.paddingBottom) != 0;
		}
	}

	public void SetRect(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	public void SetPadding(int left, int bottom, int right, int top)
	{
		this.paddingLeft = left;
		this.paddingBottom = bottom;
		this.paddingRight = right;
		this.paddingTop = top;
	}

	public void SetBorder(int left, int bottom, int right, int top)
	{
		this.borderLeft = left;
		this.borderBottom = bottom;
		this.borderRight = right;
		this.borderTop = top;
	}

	public void CopyFrom(UISpriteData sd)
	{
		this.name = sd.name;
		this.x = sd.x;
		this.y = sd.y;
		this.width = sd.width;
		this.height = sd.height;
		this.borderLeft = sd.borderLeft;
		this.borderRight = sd.borderRight;
		this.borderTop = sd.borderTop;
		this.borderBottom = sd.borderBottom;
		this.paddingLeft = sd.paddingLeft;
		this.paddingRight = sd.paddingRight;
		this.paddingTop = sd.paddingTop;
		this.paddingBottom = sd.paddingBottom;
	}

	public void CopyBorderFrom(UISpriteData sd)
	{
		this.borderLeft = sd.borderLeft;
		this.borderRight = sd.borderRight;
		this.borderTop = sd.borderTop;
		this.borderBottom = sd.borderBottom;
	}

	public UISpriteData()
	{
		this.name = "Sprite";
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteString(this.name);
		SerializedStateWriter.Instance.WriteInt32(this.x);
		SerializedStateWriter.Instance.WriteInt32(this.y);
		SerializedStateWriter.Instance.WriteInt32(this.width);
		SerializedStateWriter.Instance.WriteInt32(this.height);
		SerializedStateWriter.Instance.WriteInt32(this.borderLeft);
		SerializedStateWriter.Instance.WriteInt32(this.borderRight);
		SerializedStateWriter.Instance.WriteInt32(this.borderTop);
		SerializedStateWriter.Instance.WriteInt32(this.borderBottom);
		SerializedStateWriter.Instance.WriteInt32(this.paddingLeft);
		SerializedStateWriter.Instance.WriteInt32(this.paddingRight);
		SerializedStateWriter.Instance.WriteInt32(this.paddingTop);
		SerializedStateWriter.Instance.WriteInt32(this.paddingBottom);
	}

	public override void Unity_Deserialize(int depth)
	{
		this.name = (SerializedStateReader.Instance.ReadString() as string);
		this.x = SerializedStateReader.Instance.ReadInt32();
		this.y = SerializedStateReader.Instance.ReadInt32();
		this.width = SerializedStateReader.Instance.ReadInt32();
		this.height = SerializedStateReader.Instance.ReadInt32();
		this.borderLeft = SerializedStateReader.Instance.ReadInt32();
		this.borderRight = SerializedStateReader.Instance.ReadInt32();
		this.borderTop = SerializedStateReader.Instance.ReadInt32();
		this.borderBottom = SerializedStateReader.Instance.ReadInt32();
		this.paddingLeft = SerializedStateReader.Instance.ReadInt32();
		this.paddingRight = SerializedStateReader.Instance.ReadInt32();
		this.paddingTop = SerializedStateReader.Instance.ReadInt32();
		this.paddingBottom = SerializedStateReader.Instance.ReadInt32();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		string arg_1F_1 = this.name;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteString(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 2953);
		SerializedNamedStateWriter.Instance.WriteInt32(this.x, &var_0_cp_0[var_0_cp_1] + 2138);
		SerializedNamedStateWriter.Instance.WriteInt32(this.y, &var_0_cp_0[var_0_cp_1] + 2140);
		SerializedNamedStateWriter.Instance.WriteInt32(this.width, &var_0_cp_0[var_0_cp_1] + 2142);
		SerializedNamedStateWriter.Instance.WriteInt32(this.height, &var_0_cp_0[var_0_cp_1] + 2148);
		SerializedNamedStateWriter.Instance.WriteInt32(this.borderLeft, &var_0_cp_0[var_0_cp_1] + 4466);
		SerializedNamedStateWriter.Instance.WriteInt32(this.borderRight, &var_0_cp_0[var_0_cp_1] + 4477);
		SerializedNamedStateWriter.Instance.WriteInt32(this.borderTop, &var_0_cp_0[var_0_cp_1] + 4489);
		SerializedNamedStateWriter.Instance.WriteInt32(this.borderBottom, &var_0_cp_0[var_0_cp_1] + 4499);
		SerializedNamedStateWriter.Instance.WriteInt32(this.paddingLeft, &var_0_cp_0[var_0_cp_1] + 2978);
		SerializedNamedStateWriter.Instance.WriteInt32(this.paddingRight, &var_0_cp_0[var_0_cp_1] + 2990);
		SerializedNamedStateWriter.Instance.WriteInt32(this.paddingTop, &var_0_cp_0[var_0_cp_1] + 3003);
		SerializedNamedStateWriter.Instance.WriteInt32(this.paddingBottom, &var_0_cp_0[var_0_cp_1] + 3014);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.name = (arg_1A_0.ReadString(&var_0_cp_0[var_0_cp_1] + 2953) as string);
		this.x = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2138);
		this.y = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2140);
		this.width = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2142);
		this.height = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2148);
		this.borderLeft = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4466);
		this.borderRight = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4477);
		this.borderTop = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4489);
		this.borderBottom = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4499);
		this.paddingLeft = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2978);
		this.paddingRight = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2990);
		this.paddingTop = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3003);
		this.paddingBottom = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3014);
	}

	protected internal UISpriteData(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UISpriteData)GCHandledObjects.GCHandleToObject(instance)).CopyBorderFrom((UISpriteData)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UISpriteData)GCHandledObjects.GCHandleToObject(instance)).CopyFrom((UISpriteData)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISpriteData)GCHandledObjects.GCHandleToObject(instance)).hasBorder);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISpriteData)GCHandledObjects.GCHandleToObject(instance)).hasPadding);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UISpriteData)GCHandledObjects.GCHandleToObject(instance)).SetBorder(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3));
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UISpriteData)GCHandledObjects.GCHandleToObject(instance)).SetPadding(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3));
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UISpriteData)GCHandledObjects.GCHandleToObject(instance)).SetRect(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3));
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UISpriteData)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UISpriteData)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UISpriteData)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UISpriteData)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UISpriteData)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
