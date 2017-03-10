using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/NGUI Sprite"), ExecuteInEditMode]
public class UISprite : UIBasicSprite, IUnitySerializable
{
	[HideInInspector, SerializeField]
	protected internal UIAtlas mAtlas;

	[HideInInspector, SerializeField]
	protected internal string mSpriteName;

	[HideInInspector, SerializeField]
	protected internal bool mFillCenter;

	[System.NonSerialized]
	protected UISpriteData mSprite;

	[System.NonSerialized]
	private bool mSpriteSet;

	public override Material material
	{
		get
		{
			if (!(this.mAtlas != null))
			{
				return null;
			}
			return this.mAtlas.spriteMaterial;
		}
	}

	public UIAtlas atlas
	{
		get
		{
			return this.mAtlas;
		}
		set
		{
			if (this.mAtlas != value)
			{
				base.RemoveFromPanel();
				this.mAtlas = value;
				this.mSpriteSet = false;
				this.mSprite = null;
				if (string.IsNullOrEmpty(this.mSpriteName) && this.mAtlas != null && this.mAtlas.spriteList.Count > 0)
				{
					this.SetAtlasSprite(this.mAtlas.spriteList[0]);
					this.mSpriteName = this.mSprite.name;
				}
				if (!string.IsNullOrEmpty(this.mSpriteName))
				{
					string spriteName = this.mSpriteName;
					this.mSpriteName = "";
					this.spriteName = spriteName;
					this.MarkAsChanged();
				}
			}
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
			if (!string.IsNullOrEmpty(value))
			{
				if (this.mSpriteName != value)
				{
					this.mSpriteName = value;
					this.mSprite = null;
					this.mChanged = true;
					this.mSpriteSet = false;
				}
				return;
			}
			if (string.IsNullOrEmpty(this.mSpriteName))
			{
				return;
			}
			this.mSpriteName = "";
			this.mSprite = null;
			this.mChanged = true;
			this.mSpriteSet = false;
		}
	}

	public bool isValid
	{
		get
		{
			return this.GetAtlasSprite() != null;
		}
	}

	[Obsolete("Use 'centerType' instead")]
	public bool fillCenter
	{
		get
		{
			return this.centerType > UIBasicSprite.AdvancedType.Invisible;
		}
		set
		{
			if (value != this.centerType > UIBasicSprite.AdvancedType.Invisible)
			{
				this.centerType = (value ? UIBasicSprite.AdvancedType.Sliced : UIBasicSprite.AdvancedType.Invisible);
				this.MarkAsChanged();
			}
		}
	}

	public override Vector4 border
	{
		get
		{
			UISpriteData atlasSprite = this.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return base.border;
			}
			return new Vector4((float)atlasSprite.borderLeft, (float)atlasSprite.borderBottom, (float)atlasSprite.borderRight, (float)atlasSprite.borderTop);
		}
	}

	public override float pixelSize
	{
		get
		{
			if (!(this.mAtlas != null))
			{
				return 1f;
			}
			return this.mAtlas.pixelSize;
		}
	}

	public override int minWidth
	{
		get
		{
			if (this.type == UIBasicSprite.Type.Sliced || this.type == UIBasicSprite.Type.Advanced)
			{
				float pixelSize = this.pixelSize;
				Vector4 vector = this.border * this.pixelSize;
				int num = Mathf.RoundToInt(vector.x + vector.z);
				UISpriteData atlasSprite = this.GetAtlasSprite();
				if (atlasSprite != null)
				{
					num += Mathf.RoundToInt(pixelSize * (float)(atlasSprite.paddingLeft + atlasSprite.paddingRight));
				}
				return Mathf.Max(base.minWidth, ((num & 1) == 1) ? (num + 1) : num);
			}
			return base.minWidth;
		}
	}

	public override int minHeight
	{
		get
		{
			if (this.type == UIBasicSprite.Type.Sliced || this.type == UIBasicSprite.Type.Advanced)
			{
				float pixelSize = this.pixelSize;
				Vector4 vector = this.border * this.pixelSize;
				int num = Mathf.RoundToInt(vector.y + vector.w);
				UISpriteData atlasSprite = this.GetAtlasSprite();
				if (atlasSprite != null)
				{
					num += Mathf.RoundToInt(pixelSize * (float)(atlasSprite.paddingTop + atlasSprite.paddingBottom));
				}
				return Mathf.Max(base.minHeight, ((num & 1) == 1) ? (num + 1) : num);
			}
			return base.minHeight;
		}
	}

	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = base.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			if (this.GetAtlasSprite() != null && this.mType != UIBasicSprite.Type.Tiled)
			{
				int num5 = this.mSprite.paddingLeft;
				int num6 = this.mSprite.paddingBottom;
				int num7 = this.mSprite.paddingRight;
				int num8 = this.mSprite.paddingTop;
				if (this.mType != UIBasicSprite.Type.Simple)
				{
					float pixelSize = this.pixelSize;
					if (pixelSize != 1f)
					{
						num5 = Mathf.RoundToInt(pixelSize * (float)num5);
						num6 = Mathf.RoundToInt(pixelSize * (float)num6);
						num7 = Mathf.RoundToInt(pixelSize * (float)num7);
						num8 = Mathf.RoundToInt(pixelSize * (float)num8);
					}
				}
				int num9 = this.mSprite.width + num5 + num7;
				int num10 = this.mSprite.height + num6 + num8;
				float num11 = 1f;
				float num12 = 1f;
				if (num9 > 0 && num10 > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
				{
					if ((num9 & 1) != 0)
					{
						num7++;
					}
					if ((num10 & 1) != 0)
					{
						num8++;
					}
					num11 = 1f / (float)num9 * (float)this.mWidth;
					num12 = 1f / (float)num10 * (float)this.mHeight;
				}
				if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num += (float)num7 * num11;
					num3 -= (float)num5 * num11;
				}
				else
				{
					num += (float)num5 * num11;
					num3 -= (float)num7 * num11;
				}
				if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num2 += (float)num8 * num12;
					num4 -= (float)num6 * num12;
				}
				else
				{
					num2 += (float)num6 * num12;
					num4 -= (float)num8 * num12;
				}
			}
			Vector4 vector = (this.mAtlas != null) ? (this.border * this.pixelSize) : Vector4.zero;
			float num13 = vector.x + vector.z;
			float num14 = vector.y + vector.w;
			float x = Mathf.Lerp(num, num3 - num13, this.mDrawRegion.x);
			float y = Mathf.Lerp(num2, num4 - num14, this.mDrawRegion.y);
			float z = Mathf.Lerp(num + num13, num3, this.mDrawRegion.z);
			float w = Mathf.Lerp(num2 + num14, num4, this.mDrawRegion.w);
			return new Vector4(x, y, z, w);
		}
	}

	public override bool premultipliedAlpha
	{
		get
		{
			return this.mAtlas != null && this.mAtlas.premultipliedAlpha;
		}
	}

	public UISpriteData GetAtlasSprite()
	{
		if (!this.mSpriteSet)
		{
			this.mSprite = null;
		}
		if (this.mSprite == null && this.mAtlas != null)
		{
			if (!string.IsNullOrEmpty(this.mSpriteName))
			{
				UISpriteData sprite = this.mAtlas.GetSprite(this.mSpriteName);
				if (sprite == null)
				{
					return null;
				}
				this.SetAtlasSprite(sprite);
			}
			if (this.mSprite == null && this.mAtlas.spriteList.Count > 0)
			{
				UISpriteData uISpriteData = this.mAtlas.spriteList[0];
				if (uISpriteData == null)
				{
					return null;
				}
				this.SetAtlasSprite(uISpriteData);
				if (this.mSprite == null)
				{
					Debug.LogError(this.mAtlas.name + " seems to have a null sprite!");
					return null;
				}
				this.mSpriteName = this.mSprite.name;
			}
		}
		return this.mSprite;
	}

	protected void SetAtlasSprite(UISpriteData sp)
	{
		this.mChanged = true;
		this.mSpriteSet = true;
		if (sp != null)
		{
			this.mSprite = sp;
			this.mSpriteName = this.mSprite.name;
			return;
		}
		this.mSpriteName = ((this.mSprite != null) ? this.mSprite.name : "");
		this.mSprite = sp;
	}

	public override void MakePixelPerfect()
	{
		if (!this.isValid)
		{
			return;
		}
		base.MakePixelPerfect();
		if (this.mType == UIBasicSprite.Type.Tiled)
		{
			return;
		}
		UISpriteData atlasSprite = this.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return;
		}
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		if ((this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled || !atlasSprite.hasBorder) && mainTexture != null)
		{
			int num = Mathf.RoundToInt(this.pixelSize * (float)(atlasSprite.width + atlasSprite.paddingLeft + atlasSprite.paddingRight));
			int num2 = Mathf.RoundToInt(this.pixelSize * (float)(atlasSprite.height + atlasSprite.paddingTop + atlasSprite.paddingBottom));
			if ((num & 1) == 1)
			{
				num++;
			}
			if ((num2 & 1) == 1)
			{
				num2++;
			}
			base.width = num;
			base.height = num2;
		}
	}

	protected override void OnInit()
	{
		if (!this.mFillCenter)
		{
			this.mFillCenter = true;
			this.centerType = UIBasicSprite.AdvancedType.Invisible;
		}
		base.OnInit();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.mChanged || !this.mSpriteSet)
		{
			this.mSpriteSet = true;
			this.mSprite = null;
			this.mChanged = true;
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		if (this.mSprite == null)
		{
			this.mSprite = this.atlas.GetSprite(this.spriteName);
		}
		if (this.mSprite == null)
		{
			return;
		}
		Rect rect = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
		Rect rect2 = new Rect((float)(this.mSprite.x + this.mSprite.borderLeft), (float)(this.mSprite.y + this.mSprite.borderTop), (float)(this.mSprite.width - this.mSprite.borderLeft - this.mSprite.borderRight), (float)(this.mSprite.height - this.mSprite.borderBottom - this.mSprite.borderTop));
		rect = NGUIMath.ConvertToTexCoords(rect, mainTexture.width, mainTexture.height);
		rect2 = NGUIMath.ConvertToTexCoords(rect2, mainTexture.width, mainTexture.height);
		int size = verts.size;
		base.Fill(verts, uvs, cols, rect, rect2);
		if (this.onPostFill != null)
		{
			this.onPostFill(this, size, verts, uvs, cols);
		}
	}

	public UISprite()
	{
		this.mFillCenter = true;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			if (this.leftAnchor == null)
			{
				this.leftAnchor = new UIRect.AnchorPoint();
			}
			this.leftAnchor.Unity_Serialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.rightAnchor == null)
			{
				this.rightAnchor = new UIRect.AnchorPoint();
			}
			this.rightAnchor.Unity_Serialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.bottomAnchor == null)
			{
				this.bottomAnchor = new UIRect.AnchorPoint();
			}
			this.bottomAnchor.Unity_Serialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.topAnchor == null)
			{
				this.topAnchor = new UIRect.AnchorPoint();
			}
			this.topAnchor.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.WriteInt32((int)this.updateAnchors);
		if (depth <= 7)
		{
			this.mColor.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32((int)this.mPivot);
		SerializedStateWriter.Instance.WriteInt32(this.mWidth);
		SerializedStateWriter.Instance.WriteInt32(this.mHeight);
		SerializedStateWriter.Instance.WriteInt32(this.mDepth);
		SerializedStateWriter.Instance.WriteBoolean(this.autoResizeBoxCollider);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.hideIfOffScreen);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.skipBoundsCalculations);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32((int)this.keepAspectRatio);
		SerializedStateWriter.Instance.WriteSingle(this.aspectRatio);
		SerializedStateWriter.Instance.WriteInt32((int)this.mType);
		SerializedStateWriter.Instance.WriteInt32((int)this.mFillDirection);
		SerializedStateWriter.Instance.WriteSingle(this.mFillAmount);
		SerializedStateWriter.Instance.WriteBoolean(this.mInvert);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32((int)this.mFlip);
		SerializedStateWriter.Instance.WriteInt32((int)this.centerType);
		SerializedStateWriter.Instance.WriteInt32((int)this.leftType);
		SerializedStateWriter.Instance.WriteInt32((int)this.rightType);
		SerializedStateWriter.Instance.WriteInt32((int)this.bottomType);
		SerializedStateWriter.Instance.WriteInt32((int)this.topType);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mAtlas);
		}
		SerializedStateWriter.Instance.WriteString(this.mSpriteName);
		SerializedStateWriter.Instance.WriteBoolean(this.mFillCenter);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			if (this.leftAnchor == null)
			{
				this.leftAnchor = new UIRect.AnchorPoint();
			}
			this.leftAnchor.Unity_Deserialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.rightAnchor == null)
			{
				this.rightAnchor = new UIRect.AnchorPoint();
			}
			this.rightAnchor.Unity_Deserialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.bottomAnchor == null)
			{
				this.bottomAnchor = new UIRect.AnchorPoint();
			}
			this.bottomAnchor.Unity_Deserialize(depth + 1);
		}
		if (depth <= 7)
		{
			if (this.topAnchor == null)
			{
				this.topAnchor = new UIRect.AnchorPoint();
			}
			this.topAnchor.Unity_Deserialize(depth + 1);
		}
		this.updateAnchors = (UIRect.AnchorUpdate)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.mColor.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.mPivot = (UIWidget.Pivot)SerializedStateReader.Instance.ReadInt32();
		this.mWidth = SerializedStateReader.Instance.ReadInt32();
		this.mHeight = SerializedStateReader.Instance.ReadInt32();
		this.mDepth = SerializedStateReader.Instance.ReadInt32();
		this.autoResizeBoxCollider = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.hideIfOffScreen = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.skipBoundsCalculations = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.keepAspectRatio = (UIWidget.AspectRatioSource)SerializedStateReader.Instance.ReadInt32();
		this.aspectRatio = SerializedStateReader.Instance.ReadSingle();
		this.mType = (UIBasicSprite.Type)SerializedStateReader.Instance.ReadInt32();
		this.mFillDirection = (UIBasicSprite.FillDirection)SerializedStateReader.Instance.ReadInt32();
		this.mFillAmount = SerializedStateReader.Instance.ReadSingle();
		this.mInvert = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.mFlip = (UIBasicSprite.Flip)SerializedStateReader.Instance.ReadInt32();
		this.centerType = (UIBasicSprite.AdvancedType)SerializedStateReader.Instance.ReadInt32();
		this.leftType = (UIBasicSprite.AdvancedType)SerializedStateReader.Instance.ReadInt32();
		this.rightType = (UIBasicSprite.AdvancedType)SerializedStateReader.Instance.ReadInt32();
		this.bottomType = (UIBasicSprite.AdvancedType)SerializedStateReader.Instance.ReadInt32();
		this.topType = (UIBasicSprite.AdvancedType)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.mAtlas = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIAtlas);
		}
		this.mSpriteName = (SerializedStateReader.Instance.ReadString() as string);
		this.mFillCenter = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (depth <= 7)
		{
			if (this.leftAnchor != null)
			{
				this.leftAnchor.Unity_RemapPPtrs(depth + 1);
			}
		}
		if (depth <= 7)
		{
			if (this.rightAnchor != null)
			{
				this.rightAnchor.Unity_RemapPPtrs(depth + 1);
			}
		}
		if (depth <= 7)
		{
			if (this.bottomAnchor != null)
			{
				this.bottomAnchor.Unity_RemapPPtrs(depth + 1);
			}
		}
		if (depth <= 7)
		{
			if (this.topAnchor != null)
			{
				this.topAnchor.Unity_RemapPPtrs(depth + 1);
			}
		}
		if (this.mAtlas != null)
		{
			this.mAtlas = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mAtlas) as UIAtlas);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			if (this.leftAnchor == null)
			{
				this.leftAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_3F_0 = this.leftAnchor;
			ISerializedNamedStateWriter arg_37_0 = SerializedNamedStateWriter.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_37_0.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2296);
			arg_3F_0.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.rightAnchor == null)
			{
				this.rightAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_82_0 = this.rightAnchor;
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2307);
			arg_82_0.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.bottomAnchor == null)
			{
				this.bottomAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_C5_0 = this.bottomAnchor;
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2319);
			arg_C5_0.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.topAnchor == null)
			{
				this.topAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_108_0 = this.topAnchor;
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2332);
			arg_108_0.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.updateAnchors, &var_0_cp_0[var_0_cp_1] + 741);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2342);
			this.mColor.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mPivot, &var_0_cp_0[var_0_cp_1] + 2349);
		SerializedNamedStateWriter.Instance.WriteInt32(this.mWidth, &var_0_cp_0[var_0_cp_1] + 2098);
		SerializedNamedStateWriter.Instance.WriteInt32(this.mHeight, &var_0_cp_0[var_0_cp_1] + 2105);
		SerializedNamedStateWriter.Instance.WriteInt32(this.mDepth, &var_0_cp_0[var_0_cp_1] + 2356);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.autoResizeBoxCollider, &var_0_cp_0[var_0_cp_1] + 2363);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.hideIfOffScreen, &var_0_cp_0[var_0_cp_1] + 2385);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.skipBoundsCalculations, &var_0_cp_0[var_0_cp_1] + 2401);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.keepAspectRatio, &var_0_cp_0[var_0_cp_1] + 2424);
		SerializedNamedStateWriter.Instance.WriteSingle(this.aspectRatio, &var_0_cp_0[var_0_cp_1] + 2440);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mType, &var_0_cp_0[var_0_cp_1] + 2452);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mFillDirection, &var_0_cp_0[var_0_cp_1] + 2458);
		SerializedNamedStateWriter.Instance.WriteSingle(this.mFillAmount, &var_0_cp_0[var_0_cp_1] + 2473);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mInvert, &var_0_cp_0[var_0_cp_1] + 2485);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mFlip, &var_0_cp_0[var_0_cp_1] + 2493);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.centerType, &var_0_cp_0[var_0_cp_1] + 2499);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.leftType, &var_0_cp_0[var_0_cp_1] + 2510);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.rightType, &var_0_cp_0[var_0_cp_1] + 2519);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.bottomType, &var_0_cp_0[var_0_cp_1] + 2529);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.topType, &var_0_cp_0[var_0_cp_1] + 2540);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mAtlas, &var_0_cp_0[var_0_cp_1] + 3531);
		}
		SerializedNamedStateWriter.Instance.WriteString(this.mSpriteName, &var_0_cp_0[var_0_cp_1] + 2113);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mFillCenter, &var_0_cp_0[var_0_cp_1] + 4429);
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			if (this.leftAnchor == null)
			{
				this.leftAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_3F_0 = this.leftAnchor;
			ISerializedNamedStateReader arg_37_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_37_0.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2296);
			arg_3F_0.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.rightAnchor == null)
			{
				this.rightAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_82_0 = this.rightAnchor;
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2307);
			arg_82_0.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.bottomAnchor == null)
			{
				this.bottomAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_C5_0 = this.bottomAnchor;
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2319);
			arg_C5_0.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			if (this.topAnchor == null)
			{
				this.topAnchor = new UIRect.AnchorPoint();
			}
			UIRect.AnchorPoint arg_108_0 = this.topAnchor;
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2332);
			arg_108_0.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		this.updateAnchors = (UIRect.AnchorUpdate)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 741);
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2342);
			this.mColor.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.mPivot = (UIWidget.Pivot)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2349);
		this.mWidth = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2098);
		this.mHeight = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2105);
		this.mDepth = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2356);
		this.autoResizeBoxCollider = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2363);
		SerializedNamedStateReader.Instance.Align();
		this.hideIfOffScreen = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2385);
		SerializedNamedStateReader.Instance.Align();
		this.skipBoundsCalculations = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2401);
		SerializedNamedStateReader.Instance.Align();
		this.keepAspectRatio = (UIWidget.AspectRatioSource)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2424);
		this.aspectRatio = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2440);
		this.mType = (UIBasicSprite.Type)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2452);
		this.mFillDirection = (UIBasicSprite.FillDirection)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2458);
		this.mFillAmount = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2473);
		this.mInvert = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2485);
		SerializedNamedStateReader.Instance.Align();
		this.mFlip = (UIBasicSprite.Flip)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2493);
		this.centerType = (UIBasicSprite.AdvancedType)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2499);
		this.leftType = (UIBasicSprite.AdvancedType)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2510);
		this.rightType = (UIBasicSprite.AdvancedType)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2519);
		this.bottomType = (UIBasicSprite.AdvancedType)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2529);
		this.topType = (UIBasicSprite.AdvancedType)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2540);
		if (depth <= 7)
		{
			this.mAtlas = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3531) as UIAtlas);
		}
		this.mSpriteName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 2113) as string);
		this.mFillCenter = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4429);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UISprite(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)instance).mAtlas);
	}

	public static void $Set0(object instance, long value)
	{
		((UISprite)instance).mAtlas = (UIAtlas)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get1(object instance)
	{
		return ((UISprite)instance).mFillCenter;
	}

	public static void $Set1(object instance, bool value)
	{
		((UISprite)instance).mFillCenter = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)GCHandledObjects.GCHandleToObject(instance)).atlas);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)GCHandledObjects.GCHandleToObject(instance)).border);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)GCHandledObjects.GCHandleToObject(instance)).drawingDimensions);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)GCHandledObjects.GCHandleToObject(instance)).fillCenter);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)GCHandledObjects.GCHandleToObject(instance)).isValid);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)GCHandledObjects.GCHandleToObject(instance)).material);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)GCHandledObjects.GCHandleToObject(instance)).minHeight);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)GCHandledObjects.GCHandleToObject(instance)).minWidth);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)GCHandledObjects.GCHandleToObject(instance)).pixelSize);
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)GCHandledObjects.GCHandleToObject(instance)).premultipliedAlpha);
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)GCHandledObjects.GCHandleToObject(instance)).spriteName);
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISprite)GCHandledObjects.GCHandleToObject(instance)).GetAtlasSprite());
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).MakePixelPerfect();
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).OnFill((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).OnInit();
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).OnUpdate();
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).atlas = (UIAtlas)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).fillCenter = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).spriteName = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).SetAtlasSprite((UISpriteData)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		((UISprite)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
