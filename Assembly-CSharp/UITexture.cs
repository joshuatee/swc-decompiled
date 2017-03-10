using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/NGUI Texture"), ExecuteInEditMode]
public class UITexture : UIBasicSprite, IUnitySerializable
{
	[HideInInspector, SerializeField]
	protected internal Rect mRect;

	[HideInInspector, SerializeField]
	protected internal Texture mTexture;

	[HideInInspector, SerializeField]
	protected internal Material mMat;

	[HideInInspector, SerializeField]
	protected internal Shader mShader;

	[HideInInspector, SerializeField]
	protected internal Vector4 mBorder;

	[HideInInspector, SerializeField]
	protected internal bool mFixedAspect;

	[System.NonSerialized]
	private int mPMA;

	public override Texture mainTexture
	{
		get
		{
			if (this.mTexture != null)
			{
				return this.mTexture;
			}
			if (this.mMat != null)
			{
				return this.mMat.mainTexture;
			}
			return null;
		}
		set
		{
			if (this.mTexture != value)
			{
				if (this.drawCall != null && this.drawCall.widgetCount == 1 && this.mMat == null)
				{
					this.mTexture = value;
					this.drawCall.mainTexture = value;
					return;
				}
				base.RemoveFromPanel();
				this.mTexture = value;
				this.mPMA = -1;
				this.MarkAsChanged();
			}
		}
	}

	public override Material material
	{
		get
		{
			return this.mMat;
		}
		set
		{
			if (this.mMat != value)
			{
				base.RemoveFromPanel();
				this.mShader = null;
				this.mMat = value;
				this.mPMA = -1;
				this.MarkAsChanged();
			}
		}
	}

	public override Shader shader
	{
		get
		{
			if (this.mMat != null)
			{
				return this.mMat.shader;
			}
			if (this.mShader == null)
			{
				this.mShader = Shader.Find("Unlit/Transparent Colored");
			}
			return this.mShader;
		}
		set
		{
			if (this.mShader != value)
			{
				if (this.drawCall != null && this.drawCall.widgetCount == 1 && this.mMat == null)
				{
					this.mShader = value;
					this.drawCall.shader = value;
					return;
				}
				base.RemoveFromPanel();
				this.mShader = value;
				this.mPMA = -1;
				this.mMat = null;
				this.MarkAsChanged();
			}
		}
	}

	public override bool premultipliedAlpha
	{
		get
		{
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = ((material != null && material.shader != null && material.shader.name.Contains("Premultiplied")) ? 1 : 0);
			}
			return this.mPMA == 1;
		}
	}

	public override Vector4 border
	{
		get
		{
			return this.mBorder;
		}
		set
		{
			if (this.mBorder != value)
			{
				this.mBorder = value;
				this.MarkAsChanged();
			}
		}
	}

	public Rect uvRect
	{
		get
		{
			return this.mRect;
		}
		set
		{
			if (this.mRect != value)
			{
				this.mRect = value;
				this.MarkAsChanged();
			}
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
			if (this.mTexture != null && this.mType != UIBasicSprite.Type.Tiled)
			{
				int width = this.mTexture.width;
				int height = this.mTexture.height;
				int num5 = 0;
				int num6 = 0;
				float num7 = 1f;
				float num8 = 1f;
				if (width > 0 && height > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
				{
					if ((width & 1) != 0)
					{
						num5++;
					}
					if ((height & 1) != 0)
					{
						num6++;
					}
					num7 = 1f / (float)width * (float)this.mWidth;
					num8 = 1f / (float)height * (float)this.mHeight;
				}
				if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num += (float)num5 * num7;
				}
				else
				{
					num3 -= (float)num5 * num7;
				}
				if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num2 += (float)num6 * num8;
				}
				else
				{
					num4 -= (float)num6 * num8;
				}
			}
			float num9;
			float num10;
			if (this.mFixedAspect)
			{
				num9 = 0f;
				num10 = 0f;
			}
			else
			{
				Vector4 border = this.border;
				num9 = border.x + border.z;
				num10 = border.y + border.w;
			}
			float x = Mathf.Lerp(num, num3 - num9, this.mDrawRegion.x);
			float y = Mathf.Lerp(num2, num4 - num10, this.mDrawRegion.y);
			float z = Mathf.Lerp(num + num9, num3, this.mDrawRegion.z);
			float w = Mathf.Lerp(num2 + num10, num4, this.mDrawRegion.w);
			return new Vector4(x, y, z, w);
		}
	}

	public bool fixedAspect
	{
		get
		{
			return this.mFixedAspect;
		}
		set
		{
			if (this.mFixedAspect != value)
			{
				this.mFixedAspect = value;
				this.mDrawRegion = new Vector4(0f, 0f, 1f, 1f);
				this.MarkAsChanged();
			}
		}
	}

	public override void MakePixelPerfect()
	{
		base.MakePixelPerfect();
		if (this.mType == UIBasicSprite.Type.Tiled)
		{
			return;
		}
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		if ((this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled || !base.hasBorder) && mainTexture != null)
		{
			int num = mainTexture.width;
			int num2 = mainTexture.height;
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

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.mFixedAspect)
		{
			Texture mainTexture = this.mainTexture;
			if (mainTexture != null)
			{
				int num = mainTexture.width;
				int num2 = mainTexture.height;
				if ((num & 1) == 1)
				{
					num++;
				}
				if ((num2 & 1) == 1)
				{
					num2++;
				}
				float num3 = (float)this.mWidth;
				float num4 = (float)this.mHeight;
				float num5 = num3 / num4;
				float num6 = (float)num / (float)num2;
				if (num6 < num5)
				{
					float num7 = (num3 - num4 * num6) / num3 * 0.5f;
					base.drawRegion = new Vector4(num7, 0f, 1f - num7, 1f);
					return;
				}
				float num8 = (num4 - num3 / num6) / num4 * 0.5f;
				base.drawRegion = new Vector4(0f, num8, 1f, 1f - num8);
			}
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Rect rect = new Rect(this.mRect.x * (float)mainTexture.width, this.mRect.y * (float)mainTexture.height, (float)mainTexture.width * this.mRect.width, (float)mainTexture.height * this.mRect.height);
		Rect inner = rect;
		Vector4 border = this.border;
		inner.xMin += border.x;
		inner.yMin += border.y;
		inner.xMax -= border.z;
		inner.yMax -= border.w;
		float num = 1f / (float)mainTexture.width;
		float num2 = 1f / (float)mainTexture.height;
		rect.xMin *= num;
		rect.xMax *= num;
		rect.yMin *= num2;
		rect.yMax *= num2;
		inner.xMin *= num;
		inner.xMax *= num;
		inner.yMin *= num2;
		inner.yMax *= num2;
		int size = verts.size;
		base.Fill(verts, uvs, cols, rect, inner);
		if (this.onPostFill != null)
		{
			this.onPostFill(this, size, verts, uvs, cols);
		}
	}

	public UITexture()
	{
		this.mRect = new Rect(0f, 0f, 1f, 1f);
		this.mBorder = Vector4.zero;
		this.mPMA = -1;
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
			this.mRect.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mTexture);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mMat);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mShader);
		}
		if (depth <= 7)
		{
			this.mBorder.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.mFixedAspect);
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
			this.mRect.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.mTexture = (SerializedStateReader.Instance.ReadUnityEngineObject() as Texture);
		}
		if (depth <= 7)
		{
			this.mMat = (SerializedStateReader.Instance.ReadUnityEngineObject() as Material);
		}
		if (depth <= 7)
		{
			this.mShader = (SerializedStateReader.Instance.ReadUnityEngineObject() as Shader);
		}
		if (depth <= 7)
		{
			this.mBorder.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.mFixedAspect = SerializedStateReader.Instance.ReadBoolean();
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
		if (this.mTexture != null)
		{
			this.mTexture = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mTexture) as Texture);
		}
		if (this.mMat != null)
		{
			this.mMat = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mMat) as Material);
		}
		if (this.mShader != null)
		{
			this.mShader = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mShader) as Shader);
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
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4578);
			this.mRect.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mTexture, &var_0_cp_0[var_0_cp_1] + 4584);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mMat, &var_0_cp_0[var_0_cp_1] + 2796);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mShader, &var_0_cp_0[var_0_cp_1] + 2801);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2809);
			this.mBorder.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mFixedAspect, &var_0_cp_0[var_0_cp_1] + 2817);
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
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 4578);
			this.mRect.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.mTexture = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 4584) as Texture);
		}
		if (depth <= 7)
		{
			this.mMat = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2796) as Material);
		}
		if (depth <= 7)
		{
			this.mShader = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2801) as Shader);
		}
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2809);
			this.mBorder.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.mFixedAspect = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2817);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UITexture(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITexture)instance).mTexture);
	}

	public static void $Set0(object instance, long value)
	{
		((UITexture)instance).mTexture = (Texture)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITexture)instance).mMat);
	}

	public static void $Set1(object instance, long value)
	{
		((UITexture)instance).mMat = (Material)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITexture)instance).mShader);
	}

	public static void $Set2(object instance, long value)
	{
		((UITexture)instance).mShader = (Shader)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get3(object instance, int index)
	{
		UITexture expr_06_cp_0 = (UITexture)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.mBorder.x;
		case 1:
			return expr_06_cp_0.mBorder.y;
		case 2:
			return expr_06_cp_0.mBorder.z;
		case 3:
			return expr_06_cp_0.mBorder.w;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set3(object instance, float value, int index)
	{
		UITexture expr_06_cp_0 = (UITexture)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.mBorder.x = value;
			return;
		case 1:
			expr_06_cp_0.mBorder.y = value;
			return;
		case 2:
			expr_06_cp_0.mBorder.z = value;
			return;
		case 3:
			expr_06_cp_0.mBorder.w = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static bool $Get4(object instance)
	{
		return ((UITexture)instance).mFixedAspect;
	}

	public static void $Set4(object instance, bool value)
	{
		((UITexture)instance).mFixedAspect = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITexture)GCHandledObjects.GCHandleToObject(instance)).border);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITexture)GCHandledObjects.GCHandleToObject(instance)).drawingDimensions);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITexture)GCHandledObjects.GCHandleToObject(instance)).fixedAspect);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITexture)GCHandledObjects.GCHandleToObject(instance)).mainTexture);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITexture)GCHandledObjects.GCHandleToObject(instance)).material);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITexture)GCHandledObjects.GCHandleToObject(instance)).premultipliedAlpha);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITexture)GCHandledObjects.GCHandleToObject(instance)).shader);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITexture)GCHandledObjects.GCHandleToObject(instance)).uvRect);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).MakePixelPerfect();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).OnFill((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).OnUpdate();
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).border = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).fixedAspect = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).mainTexture = (Texture)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).material = (Material)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).shader = (Shader)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).uvRect = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UITexture)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
