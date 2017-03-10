using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[System.Serializable]
public class BMGlyph : IUnitySerializable
{
	public int index;

	public int x;

	public int y;

	public int width;

	public int height;

	public int offsetX;

	public int offsetY;

	public int advance;

	public int channel;

	public List<int> kerning;

	public int GetKerning(int previousChar)
	{
		if (this.kerning != null && previousChar != 0)
		{
			int i = 0;
			int count = this.kerning.Count;
			while (i < count)
			{
				if (this.kerning[i] == previousChar)
				{
					return this.kerning[i + 1];
				}
				i += 2;
			}
		}
		return 0;
	}

	public void SetKerning(int previousChar, int amount)
	{
		if (this.kerning == null)
		{
			this.kerning = new List<int>();
		}
		for (int i = 0; i < this.kerning.Count; i += 2)
		{
			if (this.kerning[i] == previousChar)
			{
				this.kerning[i + 1] = amount;
				return;
			}
		}
		this.kerning.Add(previousChar);
		this.kerning.Add(amount);
	}

	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		int num = this.x + this.width;
		int num2 = this.y + this.height;
		if (this.x < xMin)
		{
			int num3 = xMin - this.x;
			this.x += num3;
			this.width -= num3;
			this.offsetX += num3;
		}
		if (this.y < yMin)
		{
			int num4 = yMin - this.y;
			this.y += num4;
			this.height -= num4;
			this.offsetY += num4;
		}
		if (num > xMax)
		{
			this.width -= num - xMax;
		}
		if (num2 > yMax)
		{
			this.height -= num2 - yMax;
		}
	}

	public BMGlyph()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32(this.index);
		SerializedStateWriter.Instance.WriteInt32(this.x);
		SerializedStateWriter.Instance.WriteInt32(this.y);
		SerializedStateWriter.Instance.WriteInt32(this.width);
		SerializedStateWriter.Instance.WriteInt32(this.height);
		SerializedStateWriter.Instance.WriteInt32(this.offsetX);
		SerializedStateWriter.Instance.WriteInt32(this.offsetY);
		SerializedStateWriter.Instance.WriteInt32(this.advance);
		SerializedStateWriter.Instance.WriteInt32(this.channel);
		if (depth <= 7)
		{
			if (this.kerning == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.kerning.Count);
				for (int i = 0; i < this.kerning.Count; i++)
				{
					SerializedStateWriter.Instance.WriteInt32(this.kerning[i]);
				}
			}
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		this.index = SerializedStateReader.Instance.ReadInt32();
		this.x = SerializedStateReader.Instance.ReadInt32();
		this.y = SerializedStateReader.Instance.ReadInt32();
		this.width = SerializedStateReader.Instance.ReadInt32();
		this.height = SerializedStateReader.Instance.ReadInt32();
		this.offsetX = SerializedStateReader.Instance.ReadInt32();
		this.offsetY = SerializedStateReader.Instance.ReadInt32();
		this.advance = SerializedStateReader.Instance.ReadInt32();
		this.channel = SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.kerning = new List<int>(num);
			for (int i = 0; i < num; i++)
			{
				this.kerning.Add(SerializedStateReader.Instance.ReadInt32());
			}
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = this.index;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 2132);
		SerializedNamedStateWriter.Instance.WriteInt32(this.x, &var_0_cp_0[var_0_cp_1] + 2138);
		SerializedNamedStateWriter.Instance.WriteInt32(this.y, &var_0_cp_0[var_0_cp_1] + 2140);
		SerializedNamedStateWriter.Instance.WriteInt32(this.width, &var_0_cp_0[var_0_cp_1] + 2142);
		SerializedNamedStateWriter.Instance.WriteInt32(this.height, &var_0_cp_0[var_0_cp_1] + 2148);
		SerializedNamedStateWriter.Instance.WriteInt32(this.offsetX, &var_0_cp_0[var_0_cp_1] + 2155);
		SerializedNamedStateWriter.Instance.WriteInt32(this.offsetY, &var_0_cp_0[var_0_cp_1] + 2163);
		SerializedNamedStateWriter.Instance.WriteInt32(this.advance, &var_0_cp_0[var_0_cp_1] + 2171);
		SerializedNamedStateWriter.Instance.WriteInt32(this.channel, &var_0_cp_0[var_0_cp_1] + 2179);
		if (depth <= 7)
		{
			if (this.kerning == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2187, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2187, this.kerning.Count);
				for (int i = 0; i < this.kerning.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteInt32(this.kerning[i], (IntPtr)0);
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
		this.index = arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2132);
		this.x = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2138);
		this.y = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2140);
		this.width = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2142);
		this.height = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2148);
		this.offsetX = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2155);
		this.offsetY = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2163);
		this.advance = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2171);
		this.channel = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2179);
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2187);
			this.kerning = new List<int>(num);
			for (int i = 0; i < num; i++)
			{
				this.kerning.Add(SerializedNamedStateReader.Instance.ReadInt32((IntPtr)0));
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
	}

	protected internal BMGlyph(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMGlyph)GCHandledObjects.GCHandleToObject(instance)).GetKerning(*(int*)args));
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((BMGlyph)GCHandledObjects.GCHandleToObject(instance)).SetKerning(*(int*)args, *(int*)(args + 1));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((BMGlyph)GCHandledObjects.GCHandleToObject(instance)).Trim(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3));
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((BMGlyph)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((BMGlyph)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((BMGlyph)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((BMGlyph)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((BMGlyph)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
