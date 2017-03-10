using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/NGUI Unity2D Sprite"), ExecuteInEditMode]
public class UI2DSprite : UIBasicSprite, IUnitySerializable
{
	[HideInInspector, SerializeField]
	protected internal Sprite mSprite;

	[HideInInspector, SerializeField]
	protected internal Material mMat;

	[HideInInspector, SerializeField]
	protected internal Shader mShader;

	[HideInInspector, SerializeField]
	protected internal Vector4 mBorder;

	[HideInInspector, SerializeField]
	protected internal bool mFixedAspect;

	[HideInInspector, SerializeField]
	protected internal float mPixelSize;

	public Sprite nextSprite;

	[System.NonSerialized]
	private int mPMA;

	public Sprite sprite2D
	{
		get
		{
			return this.mSprite;
		}
		set
		{
			if (this.mSprite != value)
			{
				base.RemoveFromPanel();
				this.mSprite = value;
				this.nextSprite = null;
				base.CreatePanel();
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
				base.RemoveFromPanel();
				this.mShader = value;
				if (this.mMat == null)
				{
					this.mPMA = -1;
					this.MarkAsChanged();
				}
			}
		}
	}

	public override Texture mainTexture
	{
		get
		{
			if (this.mSprite != null)
			{
				return this.mSprite.texture;
			}
			if (this.mMat != null)
			{
				return this.mMat.mainTexture;
			}
			return null;
		}
	}

	public override bool premultipliedAlpha
	{
		get
		{
			if (this.mPMA == -1)
			{
				Shader shader = this.shader;
				this.mPMA = ((shader != null && shader.name.Contains("Premultiplied")) ? 1 : 0);
			}
			return this.mPMA == 1;
		}
	}

	public override float pixelSize
	{
		get
		{
			return this.mPixelSize;
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
			if (this.mSprite != null && this.mType != UIBasicSprite.Type.Tiled)
			{
				int num5 = Mathf.RoundToInt(this.mSprite.rect.width);
				int num6 = Mathf.RoundToInt(this.mSprite.rect.height);
				int num7 = Mathf.RoundToInt(this.mSprite.textureRectOffset.x);
				int num8 = Mathf.RoundToInt(this.mSprite.textureRectOffset.y);
				int num9 = Mathf.RoundToInt(this.mSprite.rect.width - this.mSprite.textureRect.width - this.mSprite.textureRectOffset.x);
				int num10 = Mathf.RoundToInt(this.mSprite.rect.height - this.mSprite.textureRect.height - this.mSprite.textureRectOffset.y);
				float num11 = 1f;
				float num12 = 1f;
				if (num5 > 0 && num6 > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
				{
					if ((num5 & 1) != 0)
					{
						num9++;
					}
					if ((num6 & 1) != 0)
					{
						num10++;
					}
					num11 = 1f / (float)num5 * (float)this.mWidth;
					num12 = 1f / (float)num6 * (float)this.mHeight;
				}
				if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num += (float)num9 * num11;
					num3 -= (float)num7 * num11;
				}
				else
				{
					num += (float)num7 * num11;
					num3 -= (float)num9 * num11;
				}
				if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num2 += (float)num10 * num12;
					num4 -= (float)num8 * num12;
				}
				else
				{
					num2 += (float)num8 * num12;
					num4 -= (float)num10 * num12;
				}
			}
			float num13;
			float num14;
			if (this.mFixedAspect)
			{
				num13 = 0f;
				num14 = 0f;
			}
			else
			{
				Vector4 vector = this.border * this.pixelSize;
				num13 = vector.x + vector.z;
				num14 = vector.y + vector.w;
			}
			float x = Mathf.Lerp(num, num3 - num13, this.mDrawRegion.x);
			float y = Mathf.Lerp(num2, num4 - num14, this.mDrawRegion.y);
			float z = Mathf.Lerp(num + num13, num3, this.mDrawRegion.z);
			float w = Mathf.Lerp(num2 + num14, num4, this.mDrawRegion.w);
			return new Vector4(x, y, z, w);
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

	protected override void OnUpdate()
	{
		if (this.nextSprite != null)
		{
			if (this.nextSprite != this.mSprite)
			{
				this.sprite2D = this.nextSprite;
			}
			this.nextSprite = null;
		}
		base.OnUpdate();
		if (this.mFixedAspect)
		{
			Texture mainTexture = this.mainTexture;
			if (mainTexture != null)
			{
				int num = Mathf.RoundToInt(this.mSprite.rect.width);
				int num2 = Mathf.RoundToInt(this.mSprite.rect.height);
				int num3 = Mathf.RoundToInt(this.mSprite.textureRectOffset.x);
				int num4 = Mathf.RoundToInt(this.mSprite.textureRectOffset.y);
				int num5 = Mathf.RoundToInt(this.mSprite.rect.width - this.mSprite.textureRect.width - this.mSprite.textureRectOffset.x);
				int num6 = Mathf.RoundToInt(this.mSprite.rect.height - this.mSprite.textureRect.height - this.mSprite.textureRectOffset.y);
				num += num3 + num5;
				num2 += num6 + num4;
				float num7 = (float)this.mWidth;
				float num8 = (float)this.mHeight;
				float num9 = num7 / num8;
				float num10 = (float)num / (float)num2;
				if (num10 < num9)
				{
					float num11 = (num7 - num8 * num10) / num7 * 0.5f;
					base.drawRegion = new Vector4(num11, 0f, 1f - num11, 1f);
					return;
				}
				float num12 = (num8 - num7 / num10) / num8 * 0.5f;
				base.drawRegion = new Vector4(0f, num12, 1f, 1f - num12);
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
			Rect rect = this.mSprite.rect;
			int num = Mathf.RoundToInt(rect.width);
			int num2 = Mathf.RoundToInt(rect.height);
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

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Rect rect = (this.mSprite != null) ? this.mSprite.textureRect : new Rect(0f, 0f, (float)mainTexture.width, (float)mainTexture.height);
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

	public UI2DSprite()
	{
		this.mBorder = Vector4.zero;
		this.mPixelSize = 1f;
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
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mSprite);
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
		SerializedStateWriter.Instance.WriteSingle(this.mPixelSize);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.nextSprite);
		}
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
			this.mSprite = (SerializedStateReader.Instance.ReadUnityEngineObject() as Sprite);
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
		this.mPixelSize = SerializedStateReader.Instance.ReadSingle();
		if (depth <= 7)
		{
			this.nextSprite = (SerializedStateReader.Instance.ReadUnityEngineObject() as Sprite);
		}
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
		if (this.mSprite != null)
		{
			this.mSprite = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mSprite) as Sprite);
		}
		if (this.mMat != null)
		{
			this.mMat = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mMat) as Material);
		}
		if (this.mShader != null)
		{
			this.mShader = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mShader) as Shader);
		}
		if (this.nextSprite != null)
		{
			this.nextSprite = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.nextSprite) as Sprite);
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
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mSprite, &var_0_cp_0[var_0_cp_1] + 2788);
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
		SerializedNamedStateWriter.Instance.WriteSingle(this.mPixelSize, &var_0_cp_0[var_0_cp_1] + 2830);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.nextSprite, &var_0_cp_0[var_0_cp_1] + 2841);
		}
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
			this.mSprite = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2788) as Sprite);
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
		this.mPixelSize = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2830);
		if (depth <= 7)
		{
			this.nextSprite = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2841) as Sprite);
		}
	}

	protected internal UI2DSprite(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSprite)instance).mSprite);
	}

	public static void $Set0(object instance, long value)
	{
		((UI2DSprite)instance).mSprite = (Sprite)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSprite)instance).mMat);
	}

	public static void $Set1(object instance, long value)
	{
		((UI2DSprite)instance).mMat = (Material)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSprite)instance).mShader);
	}

	public static void $Set2(object instance, long value)
	{
		((UI2DSprite)instance).mShader = (Shader)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get3(object instance, int index)
	{
		UI2DSprite expr_06_cp_0 = (UI2DSprite)instance;
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
		UI2DSprite expr_06_cp_0 = (UI2DSprite)instance;
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
		return ((UI2DSprite)instance).mFixedAspect;
	}

	public static void $Set4(object instance, bool value)
	{
		((UI2DSprite)instance).mFixedAspect = value;
	}

	public static float $Get5(object instance)
	{
		return ((UI2DSprite)instance).mPixelSize;
	}

	public static void $Set5(object instance, float value)
	{
		((UI2DSprite)instance).mPixelSize = value;
	}

	public static long $Get6(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSprite)instance).nextSprite);
	}

	public static void $Set6(object instance, long value)
	{
		((UI2DSprite)instance).nextSprite = (Sprite)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).border);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).drawingDimensions);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).mainTexture);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).material);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).pixelSize);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).premultipliedAlpha);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).shader);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).sprite2D);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).MakePixelPerfect();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).OnFill((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).OnUpdate();
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).border = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).material = (Material)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).shader = (Shader)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).sprite2D = (Sprite)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UI2DSprite)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
