using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[System.Serializable]
public class BMSymbol : IUnitySerializable
{
	public string sequence;

	public string spriteName;

	private UISpriteData mSprite;

	private bool mIsValid;

	private int mLength;

	private int mOffsetX;

	private int mOffsetY;

	private int mWidth;

	private int mHeight;

	private int mAdvance;

	private Rect mUV;

	public int length
	{
		get
		{
			if (this.mLength == 0)
			{
				this.mLength = this.sequence.get_Length();
			}
			return this.mLength;
		}
	}

	public int offsetX
	{
		get
		{
			return this.mOffsetX;
		}
	}

	public int offsetY
	{
		get
		{
			return this.mOffsetY;
		}
	}

	public int width
	{
		get
		{
			return this.mWidth;
		}
	}

	public int height
	{
		get
		{
			return this.mHeight;
		}
	}

	public int advance
	{
		get
		{
			return this.mAdvance;
		}
	}

	public Rect uvRect
	{
		get
		{
			return this.mUV;
		}
	}

	public void MarkAsChanged()
	{
		this.mIsValid = false;
	}

	public bool Validate(UIAtlas atlas)
	{
		if (atlas == null)
		{
			return false;
		}
		if (!this.mIsValid)
		{
			if (string.IsNullOrEmpty(this.spriteName))
			{
				return false;
			}
			this.mSprite = ((atlas != null) ? atlas.GetSprite(this.spriteName) : null);
			if (this.mSprite != null)
			{
				Texture texture = atlas.texture;
				if (texture == null)
				{
					this.mSprite = null;
				}
				else
				{
					this.mUV = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
					this.mUV = NGUIMath.ConvertToTexCoords(this.mUV, texture.width, texture.height);
					this.mOffsetX = this.mSprite.paddingLeft;
					this.mOffsetY = this.mSprite.paddingTop;
					this.mWidth = this.mSprite.width;
					this.mHeight = this.mSprite.height;
					this.mAdvance = this.mSprite.width + (this.mSprite.paddingLeft + this.mSprite.paddingRight);
					this.mIsValid = true;
				}
			}
		}
		return this.mSprite != null;
	}

	public BMSymbol()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteString(this.sequence);
		SerializedStateWriter.Instance.WriteString(this.spriteName);
	}

	public override void Unity_Deserialize(int depth)
	{
		this.sequence = (SerializedStateReader.Instance.ReadString() as string);
		this.spriteName = (SerializedStateReader.Instance.ReadString() as string);
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		string arg_1F_1 = this.sequence;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteString(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 2195);
		SerializedNamedStateWriter.Instance.WriteString(this.spriteName, &var_0_cp_0[var_0_cp_1] + 2204);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.sequence = (arg_1A_0.ReadString(&var_0_cp_0[var_0_cp_1] + 2195) as string);
		this.spriteName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 2204) as string);
	}

	protected internal BMSymbol(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).advance);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).height);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).length);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).offsetX);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).offsetY);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).uvRect);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).width);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).MarkAsChanged();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((BMSymbol)GCHandledObjects.GCHandleToObject(instance)).Validate((UIAtlas)GCHandledObjects.GCHandleToObject(*args)));
	}
}
