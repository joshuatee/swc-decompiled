using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[System.Serializable]
public class BMFont : IUnitySerializable
{
	[HideInInspector, SerializeField]
	protected internal int mSize;

	[HideInInspector, SerializeField]
	protected internal int mBase;

	[HideInInspector, SerializeField]
	protected internal int mWidth;

	[HideInInspector, SerializeField]
	protected internal int mHeight;

	[HideInInspector, SerializeField]
	protected internal string mSpriteName;

	[HideInInspector, SerializeField]
	protected internal List<BMGlyph> mSaved;

	private Dictionary<int, BMGlyph> mDict;

	public bool isValid
	{
		get
		{
			return this.mSaved.Count > 0;
		}
	}

	public int charSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			this.mSize = value;
		}
	}

	public int baseOffset
	{
		get
		{
			return this.mBase;
		}
		set
		{
			this.mBase = value;
		}
	}

	public int texWidth
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			this.mWidth = value;
		}
	}

	public int texHeight
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			this.mHeight = value;
		}
	}

	public int glyphCount
	{
		get
		{
			if (!this.isValid)
			{
				return 0;
			}
			return this.mSaved.Count;
		}
	}

	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			this.mSpriteName = value;
		}
	}

	public List<BMGlyph> glyphs
	{
		get
		{
			return this.mSaved;
		}
	}

	public BMGlyph GetGlyph(int index, bool createIfMissing)
	{
		BMGlyph bMGlyph = null;
		if (this.mDict.Count == 0)
		{
			int i = 0;
			int count = this.mSaved.Count;
			while (i < count)
			{
				BMGlyph bMGlyph2 = this.mSaved[i];
				this.mDict.Add(bMGlyph2.index, bMGlyph2);
				i++;
			}
		}
		if (!this.mDict.TryGetValue(index, out bMGlyph) & createIfMissing)
		{
			bMGlyph = new BMGlyph();
			bMGlyph.index = index;
			this.mSaved.Add(bMGlyph);
			this.mDict.Add(index, bMGlyph);
		}
		return bMGlyph;
	}

	public BMGlyph GetGlyph(int index)
	{
		return this.GetGlyph(index, false);
	}

	public void Clear()
	{
		this.mDict.Clear();
		this.mSaved.Clear();
	}

	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		if (this.isValid)
		{
			int i = 0;
			int count = this.mSaved.Count;
			while (i < count)
			{
				BMGlyph bMGlyph = this.mSaved[i];
				if (bMGlyph != null)
				{
					bMGlyph.Trim(xMin, yMin, xMax, yMax);
				}
				i++;
			}
		}
	}

	public BMFont()
	{
		this.mSize = 16;
		this.mSaved = new List<BMGlyph>();
		this.mDict = new Dictionary<int, BMGlyph>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32(this.mSize);
		SerializedStateWriter.Instance.WriteInt32(this.mBase);
		SerializedStateWriter.Instance.WriteInt32(this.mWidth);
		SerializedStateWriter.Instance.WriteInt32(this.mHeight);
		SerializedStateWriter.Instance.WriteString(this.mSpriteName);
		if (depth <= 7)
		{
			if (this.mSaved == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.mSaved.Count);
				for (int i = 0; i < this.mSaved.Count; i++)
				{
					((this.mSaved[i] != null) ? this.mSaved[i] : new BMGlyph()).Unity_Serialize(depth + 1);
				}
			}
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		this.mSize = SerializedStateReader.Instance.ReadInt32();
		this.mBase = SerializedStateReader.Instance.ReadInt32();
		this.mWidth = SerializedStateReader.Instance.ReadInt32();
		this.mHeight = SerializedStateReader.Instance.ReadInt32();
		this.mSpriteName = (SerializedStateReader.Instance.ReadString() as string);
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.mSaved = new List<BMGlyph>(num);
			for (int i = 0; i < num; i++)
			{
				BMGlyph bMGlyph = new BMGlyph();
				bMGlyph.Unity_Deserialize(depth + 1);
				this.mSaved.Add(bMGlyph);
			}
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (depth <= 7)
		{
			if (this.mSaved != null)
			{
				for (int i = 0; i < this.mSaved.Count; i++)
				{
					BMGlyph bMGlyph = this.mSaved[i];
					if (bMGlyph != null)
					{
						bMGlyph.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = this.mSize;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 1659);
		SerializedNamedStateWriter.Instance.WriteInt32(this.mBase, &var_0_cp_0[var_0_cp_1] + 2092);
		SerializedNamedStateWriter.Instance.WriteInt32(this.mWidth, &var_0_cp_0[var_0_cp_1] + 2098);
		SerializedNamedStateWriter.Instance.WriteInt32(this.mHeight, &var_0_cp_0[var_0_cp_1] + 2105);
		SerializedNamedStateWriter.Instance.WriteString(this.mSpriteName, &var_0_cp_0[var_0_cp_1] + 2113);
		if (depth <= 7)
		{
			if (this.mSaved == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2125, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2125, this.mSaved.Count);
				for (int i = 0; i < this.mSaved.Count; i++)
				{
					BMGlyph arg_10A_0 = (this.mSaved[i] != null) ? this.mSaved[i] : new BMGlyph();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_10A_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
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
		this.mSize = arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1659);
		this.mBase = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2092);
		this.mWidth = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2098);
		this.mHeight = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2105);
		this.mSpriteName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 2113) as string);
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 2125);
			this.mSaved = new List<BMGlyph>(num);
			for (int i = 0; i < num; i++)
			{
				BMGlyph bMGlyph = new BMGlyph();
				BMGlyph arg_C7_0 = bMGlyph;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_C7_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.mSaved.Add(bMGlyph);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
	}

	protected internal BMFont(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((BMFont)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMFont)GCHandledObjects.GCHandleToObject(instance)).baseOffset);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMFont)GCHandledObjects.GCHandleToObject(instance)).charSize);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMFont)GCHandledObjects.GCHandleToObject(instance)).glyphCount);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMFont)GCHandledObjects.GCHandleToObject(instance)).glyphs);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMFont)GCHandledObjects.GCHandleToObject(instance)).isValid);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMFont)GCHandledObjects.GCHandleToObject(instance)).spriteName);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMFont)GCHandledObjects.GCHandleToObject(instance)).texHeight);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMFont)GCHandledObjects.GCHandleToObject(instance)).texWidth);
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMFont)GCHandledObjects.GCHandleToObject(instance)).GetGlyph(*(int*)args));
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMFont)GCHandledObjects.GCHandleToObject(instance)).GetGlyph(*(int*)args, *(sbyte*)(args + 1) != 0));
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((BMFont)GCHandledObjects.GCHandleToObject(instance)).baseOffset = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((BMFont)GCHandledObjects.GCHandleToObject(instance)).charSize = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((BMFont)GCHandledObjects.GCHandleToObject(instance)).spriteName = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((BMFont)GCHandledObjects.GCHandleToObject(instance)).texHeight = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((BMFont)GCHandledObjects.GCHandleToObject(instance)).texWidth = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((BMFont)GCHandledObjects.GCHandleToObject(instance)).Trim(*(int*)args, *(int*)(args + 1), *(int*)(args + 2), *(int*)(args + 3));
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((BMFont)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((BMFont)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((BMFont)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((BMFont)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((BMFont)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
