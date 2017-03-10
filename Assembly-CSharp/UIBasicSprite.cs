using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

public abstract class UIBasicSprite : UIWidget, IUnitySerializable
{
	public enum Type
	{
		Simple,
		Sliced,
		Tiled,
		Filled,
		Advanced
	}

	public enum FillDirection
	{
		Horizontal,
		Vertical,
		Radial90,
		Radial180,
		Radial360
	}

	public enum AdvancedType
	{
		Invisible,
		Sliced,
		Tiled
	}

	public enum Flip
	{
		Nothing,
		Horizontally,
		Vertically,
		Both
	}

	[HideInInspector, SerializeField]
	protected UIBasicSprite.Type mType;

	[HideInInspector, SerializeField]
	protected UIBasicSprite.FillDirection mFillDirection;

	[HideInInspector, Range(0f, 1f), SerializeField]
	protected float mFillAmount;

	[HideInInspector, SerializeField]
	protected bool mInvert;

	[HideInInspector, SerializeField]
	protected UIBasicSprite.Flip mFlip;

	[System.NonSerialized]
	private Rect mInnerUV;

	[System.NonSerialized]
	private Rect mOuterUV;

	public UIBasicSprite.AdvancedType centerType;

	public UIBasicSprite.AdvancedType leftType;

	public UIBasicSprite.AdvancedType rightType;

	public UIBasicSprite.AdvancedType bottomType;

	public UIBasicSprite.AdvancedType topType;

	protected static Vector2[] mTempPos = new Vector2[4];

	protected static Vector2[] mTempUVs = new Vector2[4];

	public virtual UIBasicSprite.Type type
	{
		get
		{
			return this.mType;
		}
		set
		{
			if (this.mType != value)
			{
				this.mType = value;
				this.MarkAsChanged();
			}
		}
	}

	public UIBasicSprite.Flip flip
	{
		get
		{
			return this.mFlip;
		}
		set
		{
			if (this.mFlip != value)
			{
				this.mFlip = value;
				this.MarkAsChanged();
			}
		}
	}

	public UIBasicSprite.FillDirection fillDirection
	{
		get
		{
			return this.mFillDirection;
		}
		set
		{
			if (this.mFillDirection != value)
			{
				this.mFillDirection = value;
				this.mChanged = true;
			}
		}
	}

	public float fillAmount
	{
		get
		{
			return this.mFillAmount;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mFillAmount != num)
			{
				this.mFillAmount = num;
				this.mChanged = true;
			}
		}
	}

	public override int minWidth
	{
		get
		{
			if (this.type == UIBasicSprite.Type.Sliced || this.type == UIBasicSprite.Type.Advanced)
			{
				Vector4 vector = this.border * this.pixelSize;
				int num = Mathf.RoundToInt(vector.x + vector.z);
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
				Vector4 vector = this.border * this.pixelSize;
				int num = Mathf.RoundToInt(vector.y + vector.w);
				return Mathf.Max(base.minHeight, ((num & 1) == 1) ? (num + 1) : num);
			}
			return base.minHeight;
		}
	}

	public bool invert
	{
		get
		{
			return this.mInvert;
		}
		set
		{
			if (this.mInvert != value)
			{
				this.mInvert = value;
				this.mChanged = true;
			}
		}
	}

	public bool hasBorder
	{
		get
		{
			Vector4 border = this.border;
			return border.x != 0f || border.y != 0f || border.z != 0f || border.w != 0f;
		}
	}

	public virtual bool premultipliedAlpha
	{
		get
		{
			return false;
		}
	}

	public virtual float pixelSize
	{
		get
		{
			return 1f;
		}
	}

	private Vector4 drawingUVs
	{
		get
		{
			switch (this.mFlip)
			{
			case UIBasicSprite.Flip.Horizontally:
				return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMin, this.mOuterUV.xMin, this.mOuterUV.yMax);
			case UIBasicSprite.Flip.Vertically:
				return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMax, this.mOuterUV.xMax, this.mOuterUV.yMin);
			case UIBasicSprite.Flip.Both:
				return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMax, this.mOuterUV.xMin, this.mOuterUV.yMin);
			default:
				return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMin, this.mOuterUV.xMax, this.mOuterUV.yMax);
			}
		}
	}

	private Color32 drawingColor
	{
		get
		{
			Color color = base.color;
			color.a = this.finalAlpha;
			if (this.premultipliedAlpha)
			{
				color = NGUITools.ApplyPMA(color);
			}
			if (QualitySettings.activeColorSpace == ColorSpace.Linear)
			{
				color.r = Mathf.GammaToLinearSpace(color.r);
				color.g = Mathf.GammaToLinearSpace(color.g);
				color.b = Mathf.GammaToLinearSpace(color.b);
				color.a = Mathf.GammaToLinearSpace(color.a);
			}
			return color;
		}
	}

	protected void Fill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, Rect outer, Rect inner)
	{
		this.mOuterUV = outer;
		this.mInnerUV = inner;
		switch (this.type)
		{
		case UIBasicSprite.Type.Simple:
			this.SimpleFill(verts, uvs, cols);
			return;
		case UIBasicSprite.Type.Sliced:
			this.SlicedFill(verts, uvs, cols);
			return;
		case UIBasicSprite.Type.Tiled:
			this.TiledFill(verts, uvs, cols);
			return;
		case UIBasicSprite.Type.Filled:
			this.FilledFill(verts, uvs, cols);
			return;
		case UIBasicSprite.Type.Advanced:
			this.AdvancedFill(verts, uvs, cols);
			return;
		default:
			return;
		}
	}

	private void SimpleFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 drawingUVs = this.drawingUVs;
		Color32 drawingColor = this.drawingColor;
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.y));
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.y));
		uvs.Add(new Vector2(drawingUVs.x, drawingUVs.y));
		uvs.Add(new Vector2(drawingUVs.x, drawingUVs.w));
		uvs.Add(new Vector2(drawingUVs.z, drawingUVs.w));
		uvs.Add(new Vector2(drawingUVs.z, drawingUVs.y));
		cols.Add(drawingColor);
		cols.Add(drawingColor);
		cols.Add(drawingColor);
		cols.Add(drawingColor);
	}

	private void SlicedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector4 vector = this.border * this.pixelSize;
		if (vector.x == 0f && vector.y == 0f && vector.z == 0f && vector.w == 0f)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Color32 drawingColor = this.drawingColor;
		Vector4 drawingDimensions = this.drawingDimensions;
		UIBasicSprite.mTempPos[0].x = drawingDimensions.x;
		UIBasicSprite.mTempPos[0].y = drawingDimensions.y;
		UIBasicSprite.mTempPos[3].x = drawingDimensions.z;
		UIBasicSprite.mTempPos[3].y = drawingDimensions.w;
		if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector.z;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector.x;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector.x;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector.z;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMax;
		}
		if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector.w;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector.y;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector.y;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector.w;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMax;
		}
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			for (int j = 0; j < 3; j++)
			{
				if (this.centerType != UIBasicSprite.AdvancedType.Invisible || i != 1 || j != 1)
				{
					int num2 = j + 1;
					verts.Add(new Vector3(UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[j].y));
					verts.Add(new Vector3(UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num2].y));
					verts.Add(new Vector3(UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[num2].y));
					verts.Add(new Vector3(UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[j].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num2].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[num2].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y));
					cols.Add(drawingColor);
					cols.Add(drawingColor);
					cols.Add(drawingColor);
					cols.Add(drawingColor);
				}
			}
		}
	}

	private void TiledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Vector2 vector = new Vector2(this.mInnerUV.width * (float)mainTexture.width, this.mInnerUV.height * (float)mainTexture.height);
		vector *= this.pixelSize;
		if (mainTexture == null || vector.x < 2f || vector.y < 2f)
		{
			return;
		}
		Color32 drawingColor = this.drawingColor;
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 vector2;
		if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
		{
			vector2.x = this.mInnerUV.xMax;
			vector2.z = this.mInnerUV.xMin;
		}
		else
		{
			vector2.x = this.mInnerUV.xMin;
			vector2.z = this.mInnerUV.xMax;
		}
		if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
		{
			vector2.y = this.mInnerUV.yMax;
			vector2.w = this.mInnerUV.yMin;
		}
		else
		{
			vector2.y = this.mInnerUV.yMin;
			vector2.w = this.mInnerUV.yMax;
		}
		float num = drawingDimensions.x;
		float num2 = drawingDimensions.y;
		float x = vector2.x;
		float y = vector2.y;
		while (num2 < drawingDimensions.w)
		{
			num = drawingDimensions.x;
			float num3 = num2 + vector.y;
			float y2 = vector2.w;
			if (num3 > drawingDimensions.w)
			{
				y2 = Mathf.Lerp(vector2.y, vector2.w, (drawingDimensions.w - num2) / vector.y);
				num3 = drawingDimensions.w;
			}
			while (num < drawingDimensions.z)
			{
				float num4 = num + vector.x;
				float x2 = vector2.z;
				if (num4 > drawingDimensions.z)
				{
					x2 = Mathf.Lerp(vector2.x, vector2.z, (drawingDimensions.z - num) / vector.x);
					num4 = drawingDimensions.z;
				}
				verts.Add(new Vector3(num, num2));
				verts.Add(new Vector3(num, num3));
				verts.Add(new Vector3(num4, num3));
				verts.Add(new Vector3(num4, num2));
				uvs.Add(new Vector2(x, y));
				uvs.Add(new Vector2(x, y2));
				uvs.Add(new Vector2(x2, y2));
				uvs.Add(new Vector2(x2, y));
				cols.Add(drawingColor);
				cols.Add(drawingColor);
				cols.Add(drawingColor);
				cols.Add(drawingColor);
				num += vector.x;
			}
			num2 += vector.y;
		}
	}

	private void FilledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (this.mFillAmount < 0.001f)
		{
			return;
		}
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 drawingUVs = this.drawingUVs;
		Color32 drawingColor = this.drawingColor;
		if (this.mFillDirection == UIBasicSprite.FillDirection.Horizontal || this.mFillDirection == UIBasicSprite.FillDirection.Vertical)
		{
			if (this.mFillDirection == UIBasicSprite.FillDirection.Horizontal)
			{
				float num = (drawingUVs.z - drawingUVs.x) * this.mFillAmount;
				if (this.mInvert)
				{
					drawingDimensions.x = drawingDimensions.z - (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
					drawingUVs.x = drawingUVs.z - num;
				}
				else
				{
					drawingDimensions.z = drawingDimensions.x + (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
					drawingUVs.z = drawingUVs.x + num;
				}
			}
			else if (this.mFillDirection == UIBasicSprite.FillDirection.Vertical)
			{
				float num2 = (drawingUVs.w - drawingUVs.y) * this.mFillAmount;
				if (this.mInvert)
				{
					drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
					drawingUVs.y = drawingUVs.w - num2;
				}
				else
				{
					drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
					drawingUVs.w = drawingUVs.y + num2;
				}
			}
		}
		UIBasicSprite.mTempPos[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
		UIBasicSprite.mTempPos[1] = new Vector2(drawingDimensions.x, drawingDimensions.w);
		UIBasicSprite.mTempPos[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
		UIBasicSprite.mTempPos[3] = new Vector2(drawingDimensions.z, drawingDimensions.y);
		UIBasicSprite.mTempUVs[0] = new Vector2(drawingUVs.x, drawingUVs.y);
		UIBasicSprite.mTempUVs[1] = new Vector2(drawingUVs.x, drawingUVs.w);
		UIBasicSprite.mTempUVs[2] = new Vector2(drawingUVs.z, drawingUVs.w);
		UIBasicSprite.mTempUVs[3] = new Vector2(drawingUVs.z, drawingUVs.y);
		if (this.mFillAmount < 1f)
		{
			if (this.mFillDirection == UIBasicSprite.FillDirection.Radial90)
			{
				if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, this.mFillAmount, this.mInvert, 0))
				{
					for (int i = 0; i < 4; i++)
					{
						verts.Add(UIBasicSprite.mTempPos[i]);
						uvs.Add(UIBasicSprite.mTempUVs[i]);
						cols.Add(drawingColor);
					}
				}
				return;
			}
			if (this.mFillDirection == UIBasicSprite.FillDirection.Radial180)
			{
				for (int j = 0; j < 2; j++)
				{
					float t = 0f;
					float t2 = 1f;
					float t3;
					float t4;
					if (j == 0)
					{
						t3 = 0f;
						t4 = 0.5f;
					}
					else
					{
						t3 = 0.5f;
						t4 = 1f;
					}
					UIBasicSprite.mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t3);
					UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x;
					UIBasicSprite.mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t4);
					UIBasicSprite.mTempPos[3].x = UIBasicSprite.mTempPos[2].x;
					UIBasicSprite.mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t);
					UIBasicSprite.mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t2);
					UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[1].y;
					UIBasicSprite.mTempPos[3].y = UIBasicSprite.mTempPos[0].y;
					UIBasicSprite.mTempUVs[0].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, t3);
					UIBasicSprite.mTempUVs[1].x = UIBasicSprite.mTempUVs[0].x;
					UIBasicSprite.mTempUVs[2].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, t4);
					UIBasicSprite.mTempUVs[3].x = UIBasicSprite.mTempUVs[2].x;
					UIBasicSprite.mTempUVs[0].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t);
					UIBasicSprite.mTempUVs[1].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t2);
					UIBasicSprite.mTempUVs[2].y = UIBasicSprite.mTempUVs[1].y;
					UIBasicSprite.mTempUVs[3].y = UIBasicSprite.mTempUVs[0].y;
					float value = (!this.mInvert) ? (this.fillAmount * 2f - (float)j) : (this.mFillAmount * 2f - (float)(1 - j));
					if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, Mathf.Clamp01(value), !this.mInvert, NGUIMath.RepeatIndex(j + 3, 4)))
					{
						for (int k = 0; k < 4; k++)
						{
							verts.Add(UIBasicSprite.mTempPos[k]);
							uvs.Add(UIBasicSprite.mTempUVs[k]);
							cols.Add(drawingColor);
						}
					}
				}
				return;
			}
			if (this.mFillDirection == UIBasicSprite.FillDirection.Radial360)
			{
				for (int l = 0; l < 4; l++)
				{
					float t5;
					float t6;
					if (l < 2)
					{
						t5 = 0f;
						t6 = 0.5f;
					}
					else
					{
						t5 = 0.5f;
						t6 = 1f;
					}
					float t7;
					float t8;
					if (l == 0 || l == 3)
					{
						t7 = 0f;
						t8 = 0.5f;
					}
					else
					{
						t7 = 0.5f;
						t8 = 1f;
					}
					UIBasicSprite.mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t5);
					UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x;
					UIBasicSprite.mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t6);
					UIBasicSprite.mTempPos[3].x = UIBasicSprite.mTempPos[2].x;
					UIBasicSprite.mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t7);
					UIBasicSprite.mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t8);
					UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[1].y;
					UIBasicSprite.mTempPos[3].y = UIBasicSprite.mTempPos[0].y;
					UIBasicSprite.mTempUVs[0].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, t5);
					UIBasicSprite.mTempUVs[1].x = UIBasicSprite.mTempUVs[0].x;
					UIBasicSprite.mTempUVs[2].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, t6);
					UIBasicSprite.mTempUVs[3].x = UIBasicSprite.mTempUVs[2].x;
					UIBasicSprite.mTempUVs[0].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t7);
					UIBasicSprite.mTempUVs[1].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t8);
					UIBasicSprite.mTempUVs[2].y = UIBasicSprite.mTempUVs[1].y;
					UIBasicSprite.mTempUVs[3].y = UIBasicSprite.mTempUVs[0].y;
					float value2 = this.mInvert ? (this.mFillAmount * 4f - (float)NGUIMath.RepeatIndex(l + 2, 4)) : (this.mFillAmount * 4f - (float)(3 - NGUIMath.RepeatIndex(l + 2, 4)));
					if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, Mathf.Clamp01(value2), this.mInvert, NGUIMath.RepeatIndex(l + 2, 4)))
					{
						for (int m = 0; m < 4; m++)
						{
							verts.Add(UIBasicSprite.mTempPos[m]);
							uvs.Add(UIBasicSprite.mTempUVs[m]);
							cols.Add(drawingColor);
						}
					}
				}
				return;
			}
		}
		for (int n = 0; n < 4; n++)
		{
			verts.Add(UIBasicSprite.mTempPos[n]);
			uvs.Add(UIBasicSprite.mTempUVs[n]);
			cols.Add(drawingColor);
		}
	}

	private void AdvancedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Vector4 vector = this.border * this.pixelSize;
		if (vector.x == 0f && vector.y == 0f && vector.z == 0f && vector.w == 0f)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Color32 drawingColor = this.drawingColor;
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector2 vector2 = new Vector2(this.mInnerUV.width * (float)mainTexture.width, this.mInnerUV.height * (float)mainTexture.height);
		vector2 *= this.pixelSize;
		if (vector2.x < 1f)
		{
			vector2.x = 1f;
		}
		if (vector2.y < 1f)
		{
			vector2.y = 1f;
		}
		UIBasicSprite.mTempPos[0].x = drawingDimensions.x;
		UIBasicSprite.mTempPos[0].y = drawingDimensions.y;
		UIBasicSprite.mTempPos[3].x = drawingDimensions.z;
		UIBasicSprite.mTempPos[3].y = drawingDimensions.w;
		if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector.z;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector.x;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector.x;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector.z;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMax;
		}
		if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector.w;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector.y;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector.y;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector.w;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMax;
		}
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			for (int j = 0; j < 3; j++)
			{
				if (this.centerType != UIBasicSprite.AdvancedType.Invisible || i != 1 || j != 1)
				{
					int num2 = j + 1;
					if (i == 1 && j == 1)
					{
						if (this.centerType == UIBasicSprite.AdvancedType.Tiled)
						{
							float x = UIBasicSprite.mTempPos[i].x;
							float x2 = UIBasicSprite.mTempPos[num].x;
							float y = UIBasicSprite.mTempPos[j].y;
							float y2 = UIBasicSprite.mTempPos[num2].y;
							float x3 = UIBasicSprite.mTempUVs[i].x;
							float y3 = UIBasicSprite.mTempUVs[j].y;
							for (float num3 = y; num3 < y2; num3 += vector2.y)
							{
								float num4 = x;
								float num5 = UIBasicSprite.mTempUVs[num2].y;
								float num6 = num3 + vector2.y;
								if (num6 > y2)
								{
									num5 = Mathf.Lerp(y3, num5, (y2 - num3) / vector2.y);
									num6 = y2;
								}
								while (num4 < x2)
								{
									float num7 = num4 + vector2.x;
									float num8 = UIBasicSprite.mTempUVs[num].x;
									if (num7 > x2)
									{
										num8 = Mathf.Lerp(x3, num8, (x2 - num4) / vector2.x);
										num7 = x2;
									}
									UIBasicSprite.Fill(verts, uvs, cols, num4, num7, num3, num6, x3, num8, y3, num5, drawingColor);
									num4 += vector2.x;
								}
							}
						}
						else if (this.centerType == UIBasicSprite.AdvancedType.Sliced)
						{
							UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num2].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num2].y, drawingColor);
						}
					}
					else if (i == 1)
					{
						if ((j == 0 && this.bottomType == UIBasicSprite.AdvancedType.Tiled) || (j == 2 && this.topType == UIBasicSprite.AdvancedType.Tiled))
						{
							float x4 = UIBasicSprite.mTempPos[i].x;
							float x5 = UIBasicSprite.mTempPos[num].x;
							float y4 = UIBasicSprite.mTempPos[j].y;
							float y5 = UIBasicSprite.mTempPos[num2].y;
							float x6 = UIBasicSprite.mTempUVs[i].x;
							float y6 = UIBasicSprite.mTempUVs[j].y;
							float y7 = UIBasicSprite.mTempUVs[num2].y;
							for (float num9 = x4; num9 < x5; num9 += vector2.x)
							{
								float num10 = num9 + vector2.x;
								float num11 = UIBasicSprite.mTempUVs[num].x;
								if (num10 > x5)
								{
									num11 = Mathf.Lerp(x6, num11, (x5 - num9) / vector2.x);
									num10 = x5;
								}
								UIBasicSprite.Fill(verts, uvs, cols, num9, num10, y4, y5, x6, num11, y6, y7, drawingColor);
							}
						}
						else if ((j == 0 && this.bottomType != UIBasicSprite.AdvancedType.Invisible) || (j == 2 && this.topType != UIBasicSprite.AdvancedType.Invisible))
						{
							UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num2].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num2].y, drawingColor);
						}
					}
					else if (j == 1)
					{
						if ((i == 0 && this.leftType == UIBasicSprite.AdvancedType.Tiled) || (i == 2 && this.rightType == UIBasicSprite.AdvancedType.Tiled))
						{
							float x7 = UIBasicSprite.mTempPos[i].x;
							float x8 = UIBasicSprite.mTempPos[num].x;
							float y8 = UIBasicSprite.mTempPos[j].y;
							float y9 = UIBasicSprite.mTempPos[num2].y;
							float x9 = UIBasicSprite.mTempUVs[i].x;
							float x10 = UIBasicSprite.mTempUVs[num].x;
							float y10 = UIBasicSprite.mTempUVs[j].y;
							for (float num12 = y8; num12 < y9; num12 += vector2.y)
							{
								float num13 = UIBasicSprite.mTempUVs[num2].y;
								float num14 = num12 + vector2.y;
								if (num14 > y9)
								{
									num13 = Mathf.Lerp(y10, num13, (y9 - num12) / vector2.y);
									num14 = y9;
								}
								UIBasicSprite.Fill(verts, uvs, cols, x7, x8, num12, num14, x9, x10, y10, num13, drawingColor);
							}
						}
						else if ((i == 0 && this.leftType != UIBasicSprite.AdvancedType.Invisible) || (i == 2 && this.rightType != UIBasicSprite.AdvancedType.Invisible))
						{
							UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num2].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num2].y, drawingColor);
						}
					}
					else if ((j == 0 && this.bottomType != UIBasicSprite.AdvancedType.Invisible) || (j == 2 && this.topType != UIBasicSprite.AdvancedType.Invisible) || (i == 0 && this.leftType != UIBasicSprite.AdvancedType.Invisible) || (i == 2 && this.rightType != UIBasicSprite.AdvancedType.Invisible))
					{
						UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num2].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num2].y, drawingColor);
					}
				}
			}
		}
	}

	private static bool RadialCut(Vector2[] xy, Vector2[] uv, float fill, bool invert, int corner)
	{
		if (fill < 0.001f)
		{
			return false;
		}
		if ((corner & 1) == 1)
		{
			invert = !invert;
		}
		if (!invert && fill > 0.999f)
		{
			return true;
		}
		float num = Mathf.Clamp01(fill);
		if (invert)
		{
			num = 1f - num;
		}
		num *= 1.57079637f;
		float cos = Mathf.Cos(num);
		float sin = Mathf.Sin(num);
		UIBasicSprite.RadialCut(xy, cos, sin, invert, corner);
		UIBasicSprite.RadialCut(uv, cos, sin, invert, corner);
		return true;
	}

	private static void RadialCut(Vector2[] xy, float cos, float sin, bool invert, int corner)
	{
		int num = NGUIMath.RepeatIndex(corner + 1, 4);
		int num2 = NGUIMath.RepeatIndex(corner + 2, 4);
		int num3 = NGUIMath.RepeatIndex(corner + 3, 4);
		if ((corner & 1) == 1)
		{
			if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num2].x = xy[num].x;
				}
			}
			else if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num2].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num3].y = xy[num2].y;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (!invert)
			{
				xy[num3].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
				return;
			}
			xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
			return;
		}
		else
		{
			if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num2].y = xy[num].y;
				}
			}
			else if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num2].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num3].x = xy[num2].x;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (invert)
			{
				xy[num3].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
				return;
			}
			xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
			return;
		}
	}

	private static void Fill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, float v0x, float v1x, float v0y, float v1y, float u0x, float u1x, float u0y, float u1y, Color col)
	{
		verts.Add(new Vector3(v0x, v0y));
		verts.Add(new Vector3(v0x, v1y));
		verts.Add(new Vector3(v1x, v1y));
		verts.Add(new Vector3(v1x, v0y));
		uvs.Add(new Vector2(u0x, u0y));
		uvs.Add(new Vector2(u0x, u1y));
		uvs.Add(new Vector2(u1x, u1y));
		uvs.Add(new Vector2(u1x, u0y));
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
	}

	public UIBasicSprite()
	{
		this.mFillDirection = UIBasicSprite.FillDirection.Radial360;
		this.mFillAmount = 1f;
		this.centerType = UIBasicSprite.AdvancedType.Sliced;
		this.leftType = UIBasicSprite.AdvancedType.Sliced;
		this.rightType = UIBasicSprite.AdvancedType.Sliced;
		this.bottomType = UIBasicSprite.AdvancedType.Sliced;
		this.topType = UIBasicSprite.AdvancedType.Sliced;
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
	}

	protected internal UIBasicSprite(UIntPtr dummy) : base(dummy)
	{
	}

	public static float $Get0(object instance)
	{
		return ((UIBasicSprite)instance).mFillAmount;
	}

	public static void $Set0(object instance, float value)
	{
		((UIBasicSprite)instance).mFillAmount = value;
	}

	public static bool $Get1(object instance)
	{
		return ((UIBasicSprite)instance).mInvert;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIBasicSprite)instance).mInvert = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).AdvancedFill((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).Fill((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]), *(*(IntPtr*)(args + 3)), *(*(IntPtr*)(args + 4)));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		UIBasicSprite.Fill((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]), *(float*)(args + 3), *(float*)(args + 4), *(float*)(args + 5), *(float*)(args + 6), *(float*)(args + 7), *(float*)(args + 8), *(float*)(args + 9), *(float*)(args + 10), *(*(IntPtr*)(args + 11)));
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).FilledFill((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).drawingColor);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).drawingUVs);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).fillAmount);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).fillDirection);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).flip);
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).hasBorder);
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).invert);
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).minHeight);
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).minWidth);
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).pixelSize);
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).premultipliedAlpha);
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).type);
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		UIBasicSprite.RadialCut((Vector2[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), *(float*)(args + 1), *(float*)(args + 2), *(sbyte*)(args + 3) != 0, *(int*)(args + 4));
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIBasicSprite.RadialCut((Vector2[])GCHandledObjects.GCHandleToPinnedArrayObject(*args), (Vector2[])GCHandledObjects.GCHandleToPinnedArrayObject(args[1]), *(float*)(args + 2), *(sbyte*)(args + 3) != 0, *(int*)(args + 4)));
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).fillAmount = *(float*)args;
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).fillDirection = (UIBasicSprite.FillDirection)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).flip = (UIBasicSprite.Flip)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).invert = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).type = (UIBasicSprite.Type)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).SimpleFill((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).SlicedFill((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).TiledFill((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		((UIBasicSprite)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
