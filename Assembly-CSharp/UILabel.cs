using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/NGUI Label"), ExecuteInEditMode]
public class UILabel : UIWidget, IUnitySerializable
{
	public enum Effect
	{
		None,
		Shadow,
		Outline,
		Outline8
	}

	public enum Overflow
	{
		ShrinkContent,
		ClampContent,
		ResizeFreely,
		ResizeHeight
	}

	public enum Crispness
	{
		Never,
		OnDesktop,
		Always
	}

	public UILabel.Crispness keepCrispWhenShrunk;

	[HideInInspector, SerializeField]
	protected internal Font mTrueTypeFont;

	[HideInInspector, SerializeField]
	protected internal UIFont mFont;

	[HideInInspector, Multiline(6), SerializeField]
	protected internal string mText;

	[HideInInspector, SerializeField]
	protected internal int mFontSize;

	[HideInInspector, SerializeField]
	protected internal FontStyle mFontStyle;

	[HideInInspector, SerializeField]
	protected internal NGUIText.Alignment mAlignment;

	[HideInInspector, SerializeField]
	protected internal bool mEncoding;

	[HideInInspector, SerializeField]
	protected internal int mMaxLineCount;

	[HideInInspector, SerializeField]
	protected internal UILabel.Effect mEffectStyle;

	[HideInInspector, SerializeField]
	protected internal Color mEffectColor;

	[HideInInspector, SerializeField]
	protected internal NGUIText.SymbolStyle mSymbols;

	[HideInInspector, SerializeField]
	protected internal Vector2 mEffectDistance;

	[HideInInspector, SerializeField]
	protected internal UILabel.Overflow mOverflow;

	[HideInInspector, SerializeField]
	protected internal Material mMaterial;

	[HideInInspector, SerializeField]
	protected internal bool mApplyGradient;

	[HideInInspector, SerializeField]
	protected internal Color mGradientTop;

	[HideInInspector, SerializeField]
	protected internal Color mGradientBottom;

	[HideInInspector, SerializeField]
	protected internal int mSpacingX;

	[HideInInspector, SerializeField]
	protected internal int mSpacingY;

	[HideInInspector, SerializeField]
	protected internal bool mUseFloatSpacing;

	[HideInInspector, SerializeField]
	protected internal float mFloatSpacingX;

	[HideInInspector, SerializeField]
	protected internal float mFloatSpacingY;

	[HideInInspector, SerializeField]
	protected internal bool mOverflowEllipsis;

	[HideInInspector, SerializeField]
	protected internal bool mShrinkToFit;

	[HideInInspector, SerializeField]
	protected internal int mMaxLineWidth;

	[HideInInspector, SerializeField]
	protected internal int mMaxLineHeight;

	[HideInInspector, SerializeField]
	protected internal float mLineWidth;

	[HideInInspector, SerializeField]
	protected internal bool mMultiline;

	[System.NonSerialized]
	private Font mActiveTTF;

	[System.NonSerialized]
	private float mDensity;

	[System.NonSerialized]
	private bool mShouldBeProcessed;

	[System.NonSerialized]
	private string mProcessedText;

	[System.NonSerialized]
	private bool mPremultiply;

	[System.NonSerialized]
	private Vector2 mCalculatedSize;

	[System.NonSerialized]
	private float mScale;

	[System.NonSerialized]
	private int mFinalFontSize;

	[System.NonSerialized]
	private int mLastWidth;

	[System.NonSerialized]
	private int mLastHeight;

	[System.NonSerialized]
	public bool UseFontSharpening;

	private static BetterList<UILabel> mList = new BetterList<UILabel>();

	private static Dictionary<Font, int> mFontUsage = new Dictionary<Font, int>();

	private static List<UIPanel> mTempPanelList;

	private static bool mTexRebuildAdded = false;

	private static BetterList<Vector3> mTempVerts = new BetterList<Vector3>();

	private static BetterList<int> mTempIndices = new BetterList<int>();

	public int finalFontSize
	{
		get
		{
			if (this.trueTypeFont)
			{
				return Mathf.RoundToInt(this.mScale * (float)this.mFinalFontSize);
			}
			return Mathf.RoundToInt((float)this.mFinalFontSize * this.mScale);
		}
	}

	private bool shouldBeProcessed
	{
		get
		{
			return this.mShouldBeProcessed;
		}
		set
		{
			if (value)
			{
				this.mChanged = true;
				this.mShouldBeProcessed = true;
				return;
			}
			this.mShouldBeProcessed = false;
		}
	}

	public override bool isAnchoredHorizontally
	{
		get
		{
			return base.isAnchoredHorizontally || this.mOverflow == UILabel.Overflow.ResizeFreely;
		}
	}

	public override bool isAnchoredVertically
	{
		get
		{
			return base.isAnchoredVertically || this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight;
		}
	}

	public override Material material
	{
		get
		{
			if (this.mMaterial != null)
			{
				return this.mMaterial;
			}
			if (this.mFont != null)
			{
				return this.mFont.material;
			}
			if (this.mTrueTypeFont != null)
			{
				return this.mTrueTypeFont.material;
			}
			return null;
		}
		set
		{
			if (this.mMaterial != value)
			{
				base.RemoveFromPanel();
				this.mMaterial = value;
				this.MarkAsChanged();
			}
		}
	}

	[Obsolete("Use UILabel.bitmapFont instead")]
	public UIFont font
	{
		get
		{
			return this.bitmapFont;
		}
		set
		{
			this.bitmapFont = value;
		}
	}

	public UIFont bitmapFont
	{
		get
		{
			return this.mFont;
		}
		set
		{
			if (this.mFont != value)
			{
				base.RemoveFromPanel();
				this.mFont = value;
				this.mTrueTypeFont = null;
				this.MarkAsChanged();
			}
		}
	}

	public Font trueTypeFont
	{
		get
		{
			if (this.mTrueTypeFont != null)
			{
				return this.mTrueTypeFont;
			}
			if (!(this.mFont != null))
			{
				return null;
			}
			return this.mFont.dynamicFont;
		}
		set
		{
			if (this.mTrueTypeFont != value)
			{
				this.SetActiveFont(null);
				base.RemoveFromPanel();
				this.mTrueTypeFont = value;
				this.shouldBeProcessed = true;
				this.mFont = null;
				this.SetActiveFont(value);
				this.ProcessAndRequest();
				if (this.mActiveTTF != null)
				{
					base.MarkAsChanged();
				}
			}
		}
	}

	public UnityEngine.Object ambigiousFont
	{
		get
		{
			return this.mFont ?? this.mTrueTypeFont;
		}
		set
		{
			UIFont uIFont = value as UIFont;
			if (uIFont != null)
			{
				this.bitmapFont = uIFont;
				return;
			}
			this.trueTypeFont = (value as Font);
		}
	}

	public string text
	{
		get
		{
			return this.mText;
		}
		set
		{
			if (this.mText == value)
			{
				return;
			}
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(this.mText))
				{
					this.mText = "";
					this.MarkAsChanged();
					this.ProcessAndRequest();
				}
			}
			else if (this.mText != value)
			{
				this.mText = value;
				this.MarkAsChanged();
				this.ProcessAndRequest();
			}
			if (this.autoResizeBoxCollider)
			{
				base.ResizeCollider();
			}
		}
	}

	public int defaultFontSize
	{
		get
		{
			if (this.trueTypeFont != null)
			{
				return this.mFontSize;
			}
			if (!(this.mFont != null))
			{
				return 16;
			}
			return this.mFont.defaultSize;
		}
	}

	public int fontSize
	{
		get
		{
			return this.mFontSize;
		}
		set
		{
			value = Mathf.Clamp(value, 0, 256);
			if (this.mFontSize != value)
			{
				this.mFontSize = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	public FontStyle fontStyle
	{
		get
		{
			return this.mFontStyle;
		}
		set
		{
			if (this.mFontStyle != value)
			{
				this.mFontStyle = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	public NGUIText.Alignment alignment
	{
		get
		{
			return this.mAlignment;
		}
		set
		{
			if (this.mAlignment != value)
			{
				this.mAlignment = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	public bool applyGradient
	{
		get
		{
			return this.mApplyGradient;
		}
		set
		{
			if (this.mApplyGradient != value)
			{
				this.mApplyGradient = value;
				this.MarkAsChanged();
			}
		}
	}

	public Color gradientTop
	{
		get
		{
			return this.mGradientTop;
		}
		set
		{
			if (this.mGradientTop != value)
			{
				this.mGradientTop = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	public Color gradientBottom
	{
		get
		{
			return this.mGradientBottom;
		}
		set
		{
			if (this.mGradientBottom != value)
			{
				this.mGradientBottom = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	public int spacingX
	{
		get
		{
			return this.mSpacingX;
		}
		set
		{
			if (this.mSpacingX != value)
			{
				this.mSpacingX = value;
				this.MarkAsChanged();
			}
		}
	}

	public int spacingY
	{
		get
		{
			return this.mSpacingY;
		}
		set
		{
			if (this.mSpacingY != value)
			{
				this.mSpacingY = value;
				this.MarkAsChanged();
			}
		}
	}

	public bool useFloatSpacing
	{
		get
		{
			return this.mUseFloatSpacing;
		}
		set
		{
			if (this.mUseFloatSpacing != value)
			{
				this.mUseFloatSpacing = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	public float floatSpacingX
	{
		get
		{
			return this.mFloatSpacingX;
		}
		set
		{
			if (!Mathf.Approximately(this.mFloatSpacingX, value))
			{
				this.mFloatSpacingX = value;
				this.MarkAsChanged();
			}
		}
	}

	public float floatSpacingY
	{
		get
		{
			return this.mFloatSpacingY;
		}
		set
		{
			if (!Mathf.Approximately(this.mFloatSpacingY, value))
			{
				this.mFloatSpacingY = value;
				this.MarkAsChanged();
			}
		}
	}

	public float effectiveSpacingY
	{
		get
		{
			if (!this.mUseFloatSpacing)
			{
				return (float)this.mSpacingY;
			}
			return this.mFloatSpacingY;
		}
	}

	public float effectiveSpacingX
	{
		get
		{
			if (!this.mUseFloatSpacing)
			{
				return (float)this.mSpacingX;
			}
			return this.mFloatSpacingX;
		}
	}

	public bool overflowEllipsis
	{
		get
		{
			return this.mOverflowEllipsis;
		}
		set
		{
			if (this.mOverflowEllipsis != value)
			{
				this.mOverflowEllipsis = value;
				this.MarkAsChanged();
			}
		}
	}

	private bool keepCrisp
	{
		get
		{
			return this.trueTypeFont != null && this.keepCrispWhenShrunk != UILabel.Crispness.Never;
		}
	}

	public bool supportEncoding
	{
		get
		{
			return this.mEncoding;
		}
		set
		{
			if (this.mEncoding != value)
			{
				this.mEncoding = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	public NGUIText.SymbolStyle symbolStyle
	{
		get
		{
			return this.mSymbols;
		}
		set
		{
			if (this.mSymbols != value)
			{
				this.mSymbols = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	public UILabel.Overflow overflowMethod
	{
		get
		{
			return this.mOverflow;
		}
		set
		{
			if (this.mOverflow != value)
			{
				this.mOverflow = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	[Obsolete("Use 'width' instead")]
	public int lineWidth
	{
		get
		{
			return base.width;
		}
		set
		{
			base.width = value;
		}
	}

	[Obsolete("Use 'height' instead")]
	public int lineHeight
	{
		get
		{
			return base.height;
		}
		set
		{
			base.height = value;
		}
	}

	public bool multiLine
	{
		get
		{
			return this.mMaxLineCount != 1;
		}
		set
		{
			if (this.mMaxLineCount != 1 != value)
			{
				this.mMaxLineCount = (value ? 0 : 1);
				this.shouldBeProcessed = true;
			}
		}
	}

	public override Vector3[] localCorners
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.localCorners;
		}
	}

	public override Vector3[] worldCorners
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.worldCorners;
		}
	}

	public override Vector4 drawingDimensions
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.drawingDimensions;
		}
	}

	public int maxLineCount
	{
		get
		{
			return this.mMaxLineCount;
		}
		set
		{
			if (this.mMaxLineCount != value)
			{
				this.mMaxLineCount = Mathf.Max(value, 0);
				this.shouldBeProcessed = true;
				if (this.overflowMethod == UILabel.Overflow.ShrinkContent)
				{
					this.MakePixelPerfect();
				}
			}
		}
	}

	public UILabel.Effect effectStyle
	{
		get
		{
			return this.mEffectStyle;
		}
		set
		{
			if (this.mEffectStyle != value)
			{
				this.mEffectStyle = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	public Color effectColor
	{
		get
		{
			return this.mEffectColor;
		}
		set
		{
			if (this.mEffectColor != value)
			{
				this.mEffectColor = value;
				if (this.mEffectStyle != UILabel.Effect.None)
				{
					this.shouldBeProcessed = true;
				}
			}
		}
	}

	public Vector2 effectDistance
	{
		get
		{
			return this.mEffectDistance;
		}
		set
		{
			if (this.mEffectDistance != value)
			{
				this.mEffectDistance = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	[Obsolete("Use 'overflowMethod == UILabel.Overflow.ShrinkContent' instead")]
	public bool shrinkToFit
	{
		get
		{
			return this.mOverflow == UILabel.Overflow.ShrinkContent;
		}
		set
		{
			if (value)
			{
				this.overflowMethod = UILabel.Overflow.ShrinkContent;
			}
		}
	}

	public string processedText
	{
		get
		{
			if (this.mLastWidth != this.mWidth || this.mLastHeight != this.mHeight)
			{
				this.mLastWidth = this.mWidth;
				this.mLastHeight = this.mHeight;
				this.mShouldBeProcessed = true;
			}
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return this.mProcessedText;
		}
	}

	public Vector2 printedSize
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return this.mCalculatedSize;
		}
	}

	public override Vector2 localSize
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.localSize;
		}
	}

	private bool isValid
	{
		get
		{
			return this.mFont != null || this.mTrueTypeFont != null;
		}
	}

	protected override void OnInit()
	{
		base.OnInit();
		UILabel.mList.Add(this);
		this.SetActiveFont(this.trueTypeFont);
	}

	protected override void OnDisable()
	{
		this.SetActiveFont(null);
		UILabel.mList.Remove(this);
		base.OnDisable();
	}

	protected void SetActiveFont(Font fnt)
	{
		if (this.mActiveTTF != fnt)
		{
			Font font = this.mActiveTTF;
			int num;
			if (font != null && UILabel.mFontUsage.TryGetValue(font, out num))
			{
				num = Mathf.Max(0, --num);
				if (num == 0)
				{
					UILabel.mFontUsage.Remove(font);
				}
				else
				{
					UILabel.mFontUsage[font] = num;
				}
			}
			this.mActiveTTF = fnt;
			if (fnt != null)
			{
				int num2 = 0;
				UILabel.mFontUsage[fnt] = num2 + 1;
			}
		}
	}

	private static void OnFontChanged(Font font)
	{
		for (int i = 0; i < UILabel.mList.size; i++)
		{
			UILabel uILabel = UILabel.mList[i];
			if (uILabel != null)
			{
				Font trueTypeFont = uILabel.trueTypeFont;
				if (trueTypeFont == font)
				{
					trueTypeFont.RequestCharactersInTexture(uILabel.mText, uILabel.mFinalFontSize, uILabel.mFontStyle);
					uILabel.RemoveFromPanel();
					uILabel.CreatePanel();
					if (UILabel.mTempPanelList == null)
					{
						UILabel.mTempPanelList = new List<UIPanel>();
					}
					if (!UILabel.mTempPanelList.Contains(uILabel.panel))
					{
						UILabel.mTempPanelList.Add(uILabel.panel);
					}
				}
			}
		}
		if (UILabel.mTempPanelList != null)
		{
			for (int j = 0; j < UILabel.mTempPanelList.Count; j++)
			{
				UIPanel uIPanel = UILabel.mTempPanelList[j];
				uIPanel.Refresh();
			}
			UILabel.mTempPanelList.Clear();
		}
	}

	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.shouldBeProcessed)
		{
			this.ProcessText();
		}
		return base.GetSides(relativeTo);
	}

	protected override void UpgradeFrom265()
	{
		this.ProcessText(true, true);
		if (this.mShrinkToFit)
		{
			this.overflowMethod = UILabel.Overflow.ShrinkContent;
			this.mMaxLineCount = 0;
		}
		if (this.mMaxLineWidth != 0)
		{
			base.width = this.mMaxLineWidth;
			this.overflowMethod = ((this.mMaxLineCount > 0) ? UILabel.Overflow.ResizeHeight : UILabel.Overflow.ShrinkContent);
		}
		else
		{
			this.overflowMethod = UILabel.Overflow.ResizeFreely;
		}
		if (this.mMaxLineHeight != 0)
		{
			base.height = this.mMaxLineHeight;
		}
		if (this.mFont != null)
		{
			int defaultSize = this.mFont.defaultSize;
			if (base.height < defaultSize)
			{
				base.height = defaultSize;
			}
			this.fontSize = defaultSize;
		}
		this.mMaxLineWidth = 0;
		this.mMaxLineHeight = 0;
		this.mShrinkToFit = false;
		NGUITools.UpdateWidgetCollider(base.gameObject, true);
	}

	protected override void OnAnchor()
	{
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			if (base.isFullyAnchored)
			{
				this.mOverflow = UILabel.Overflow.ShrinkContent;
			}
		}
		else if (this.mOverflow == UILabel.Overflow.ResizeHeight && this.topAnchor.target != null && this.bottomAnchor.target != null)
		{
			this.mOverflow = UILabel.Overflow.ShrinkContent;
		}
		base.OnAnchor();
	}

	private void ProcessAndRequest()
	{
		if (this.ambigiousFont != null)
		{
			this.ProcessText();
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (!UILabel.mTexRebuildAdded)
		{
			UILabel.mTexRebuildAdded = true;
			Font.textureRebuilt += new Action<Font>(UILabel.OnFontChanged);
		}
	}

	protected override void OnStart()
	{
		base.OnStart();
		if (this.mLineWidth > 0f)
		{
			this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
			this.mLineWidth = 0f;
		}
		if (!this.mMultiline)
		{
			this.mMaxLineCount = 1;
			this.mMultiline = true;
		}
		this.mPremultiply = (this.material != null && this.material.shader != null && this.material.shader.name.Contains("Premultiplied"));
		this.ProcessAndRequest();
	}

	public override void MarkAsChanged()
	{
		this.shouldBeProcessed = true;
		base.MarkAsChanged();
	}

	public void ProcessText()
	{
		this.ProcessText(false, true);
	}

	private void ProcessText(bool legacyMode, bool full)
	{
		if (!this.isValid)
		{
			return;
		}
		this.mChanged = true;
		this.shouldBeProcessed = false;
		float num = this.mDrawRegion.z - this.mDrawRegion.x;
		float num2 = this.mDrawRegion.w - this.mDrawRegion.y;
		NGUIText.rectWidth = (legacyMode ? ((this.mMaxLineWidth != 0) ? this.mMaxLineWidth : 1000000) : base.width);
		NGUIText.rectHeight = (legacyMode ? ((this.mMaxLineHeight != 0) ? this.mMaxLineHeight : 1000000) : base.height);
		NGUIText.regionWidth = ((num != 1f) ? Mathf.RoundToInt((float)NGUIText.rectWidth * num) : NGUIText.rectWidth);
		NGUIText.regionHeight = ((num2 != 1f) ? Mathf.RoundToInt((float)NGUIText.rectHeight * num2) : NGUIText.rectHeight);
		this.mFinalFontSize = Mathf.Abs(legacyMode ? Mathf.RoundToInt(base.cachedTransform.localScale.x) : this.defaultFontSize);
		this.mScale = 1f;
		if (NGUIText.regionWidth < 1 || NGUIText.regionHeight < 0)
		{
			this.mProcessedText = "";
			return;
		}
		bool flag = this.trueTypeFont != null;
		if (flag && this.keepCrisp)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				this.mDensity = ((root != null) ? root.pixelSizeAdjustment : 1f);
			}
		}
		else
		{
			this.mDensity = 1f;
		}
		if (full)
		{
			this.UpdateNGUIText();
		}
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			NGUIText.rectWidth = 1000000;
			NGUIText.regionWidth = 1000000;
		}
		if (this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight)
		{
			NGUIText.rectHeight = 1000000;
			NGUIText.regionHeight = 1000000;
		}
		if (this.mFinalFontSize > 0)
		{
			bool keepCrisp = this.keepCrisp;
			int i = this.mFinalFontSize;
			while (i > 0)
			{
				if (keepCrisp)
				{
					this.mFinalFontSize = i;
					NGUIText.fontSize = this.mFinalFontSize;
				}
				else
				{
					this.mScale = (float)i / (float)this.mFinalFontSize;
					NGUIText.fontScale = (flag ? this.mScale : ((float)this.mFontSize / (float)this.mFont.defaultSize * this.mScale));
				}
				NGUIText.Update(false, this.UseFontSharpening);
				bool flag2 = NGUIText.WrapText(this.mText, out this.mProcessedText, true, false, this.mOverflowEllipsis && this.mOverflow == UILabel.Overflow.ClampContent);
				if (NGUIText.dynamicFont == null)
				{
					this.ProcessText(legacyMode, full);
					return;
				}
				if (this.mOverflow == UILabel.Overflow.ShrinkContent && !flag2)
				{
					if (--i <= 1)
					{
						break;
					}
					i--;
				}
				else
				{
					if (this.mOverflow == UILabel.Overflow.ResizeFreely)
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
						this.mWidth = Mathf.Max(this.minWidth, Mathf.RoundToInt(this.mCalculatedSize.x));
						if (num != 1f)
						{
							this.mWidth = Mathf.RoundToInt((float)this.mWidth / num);
						}
						this.mHeight = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
						if (num2 != 1f)
						{
							this.mHeight = Mathf.RoundToInt((float)this.mHeight / num2);
						}
						if ((this.mWidth & 1) == 1)
						{
							this.mWidth++;
						}
						if ((this.mHeight & 1) == 1)
						{
							this.mHeight++;
						}
					}
					else if (this.mOverflow == UILabel.Overflow.ResizeHeight)
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
						this.mHeight = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
						if (num2 != 1f)
						{
							this.mHeight = Mathf.RoundToInt((float)this.mHeight / num2);
						}
						if ((this.mHeight & 1) == 1)
						{
							this.mHeight++;
						}
					}
					else
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
					}
					if (legacyMode)
					{
						base.width = Mathf.RoundToInt(this.mCalculatedSize.x);
						base.height = Mathf.RoundToInt(this.mCalculatedSize.y);
						base.cachedTransform.localScale = Vector3.one;
						break;
					}
					break;
				}
			}
		}
		else
		{
			base.cachedTransform.localScale = Vector3.one;
			this.mProcessedText = "";
			this.mScale = 1f;
		}
		if (full)
		{
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
		}
	}

	public override void MakePixelPerfect()
	{
		if (!(this.ambigiousFont != null))
		{
			base.MakePixelPerfect();
			return;
		}
		Vector3 localPosition = base.cachedTransform.localPosition;
		localPosition.x = (float)Mathf.RoundToInt(localPosition.x);
		localPosition.y = (float)Mathf.RoundToInt(localPosition.y);
		localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
		base.cachedTransform.localPosition = localPosition;
		base.cachedTransform.localScale = Vector3.one;
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			this.AssumeNaturalSize();
			return;
		}
		int width = base.width;
		int height = base.height;
		UILabel.Overflow overflow = this.mOverflow;
		if (overflow != UILabel.Overflow.ResizeHeight)
		{
			this.mWidth = 100000;
		}
		this.mHeight = 100000;
		this.mOverflow = UILabel.Overflow.ShrinkContent;
		this.ProcessText(false, true);
		this.mOverflow = overflow;
		int num = Mathf.RoundToInt(this.mCalculatedSize.x);
		int num2 = Mathf.RoundToInt(this.mCalculatedSize.y);
		num = Mathf.Max(num, base.minWidth);
		num2 = Mathf.Max(num2, base.minHeight);
		if ((num & 1) == 1)
		{
			num++;
		}
		if ((num2 & 1) == 1)
		{
			num2++;
		}
		this.mWidth = Mathf.Max(width, num);
		this.mHeight = Mathf.Max(height, num2);
		this.MarkAsChanged();
	}

	public void AssumeNaturalSize()
	{
		if (this.ambigiousFont != null)
		{
			this.mWidth = 100000;
			this.mHeight = 100000;
			this.ProcessText(false, true);
			this.mWidth = Mathf.RoundToInt(this.mCalculatedSize.x);
			this.mHeight = Mathf.RoundToInt(this.mCalculatedSize.y);
			if ((this.mWidth & 1) == 1)
			{
				this.mWidth++;
			}
			if ((this.mHeight & 1) == 1)
			{
				this.mHeight++;
			}
			this.MarkAsChanged();
		}
	}

	[Obsolete("Use UILabel.GetCharacterAtPosition instead")]
	public int GetCharacterIndex(Vector3 worldPos)
	{
		return this.GetCharacterIndexAtPosition(worldPos, false);
	}

	[Obsolete("Use UILabel.GetCharacterAtPosition instead")]
	public int GetCharacterIndex(Vector2 localPos)
	{
		return this.GetCharacterIndexAtPosition(localPos, false);
	}

	public int GetCharacterIndexAtPosition(Vector3 worldPos, bool precise)
	{
		Vector2 localPos = base.cachedTransform.InverseTransformPoint(worldPos);
		return this.GetCharacterIndexAtPosition(localPos, precise);
	}

	public int GetCharacterIndexAtPosition(Vector2 localPos, bool precise)
	{
		if (this.isValid)
		{
			string processedText = this.processedText;
			if (string.IsNullOrEmpty(processedText))
			{
				return 0;
			}
			this.UpdateNGUIText();
			if (precise)
			{
				NGUIText.PrintExactCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			}
			else
			{
				NGUIText.PrintApproximateCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			}
			if (UILabel.mTempVerts.size > 0)
			{
				this.ApplyOffset(UILabel.mTempVerts, 0);
				int result = precise ? NGUIText.GetExactCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, localPos) : NGUIText.GetApproximateCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, localPos);
				UILabel.mTempVerts.Clear();
				UILabel.mTempIndices.Clear();
				NGUIText.bitmapFont = null;
				NGUIText.dynamicFont = null;
				return result;
			}
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
		}
		return 0;
	}

	public string GetWordAtPosition(Vector3 worldPos)
	{
		int characterIndexAtPosition = this.GetCharacterIndexAtPosition(worldPos, true);
		return this.GetWordAtCharacterIndex(characterIndexAtPosition);
	}

	public string GetWordAtPosition(Vector2 localPos)
	{
		int characterIndexAtPosition = this.GetCharacterIndexAtPosition(localPos, true);
		return this.GetWordAtCharacterIndex(characterIndexAtPosition);
	}

	public string GetWordAtCharacterIndex(int characterIndex)
	{
		if (characterIndex != -1 && characterIndex < this.mText.get_Length())
		{
			int num = this.mText.LastIndexOfAny(new char[]
			{
				' ',
				'\n'
			}, characterIndex) + 1;
			int num2 = this.mText.IndexOfAny(new char[]
			{
				' ',
				'\n',
				',',
				'.'
			}, characterIndex);
			if (num2 == -1)
			{
				num2 = this.mText.get_Length();
			}
			if (num != num2)
			{
				int num3 = num2 - num;
				if (num3 > 0)
				{
					string text = this.mText.Substring(num, num3);
					return NGUIText.StripSymbols(text);
				}
			}
		}
		return null;
	}

	public string GetUrlAtPosition(Vector3 worldPos)
	{
		return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(worldPos, true));
	}

	public string GetUrlAtPosition(Vector2 localPos)
	{
		return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(localPos, true));
	}

	public string GetUrlAtCharacterIndex(int characterIndex)
	{
		if (characterIndex != -1 && characterIndex < this.mText.get_Length() - 6)
		{
			int num;
			if (this.mText.get_Chars(characterIndex) == '[' && this.mText.get_Chars(characterIndex + 1) == 'u' && this.mText.get_Chars(characterIndex + 2) == 'r' && this.mText.get_Chars(characterIndex + 3) == 'l' && this.mText.get_Chars(characterIndex + 4) == '=')
			{
				num = characterIndex;
			}
			else
			{
				num = this.mText.LastIndexOf("[url=", characterIndex);
			}
			if (num == -1)
			{
				return null;
			}
			num += 5;
			int num2 = this.mText.IndexOf("]", num);
			if (num2 == -1)
			{
				return null;
			}
			int num3 = this.mText.IndexOf("[/url]", num2);
			if (num3 == -1 || characterIndex <= num3)
			{
				return this.mText.Substring(num, num2 - num);
			}
		}
		return null;
	}

	public int GetCharacterIndex(int currentIndex, KeyCode key)
	{
		if (this.isValid)
		{
			string processedText = this.processedText;
			if (string.IsNullOrEmpty(processedText))
			{
				return 0;
			}
			int defaultFontSize = this.defaultFontSize;
			this.UpdateNGUIText();
			NGUIText.PrintApproximateCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			if (UILabel.mTempVerts.size > 0)
			{
				this.ApplyOffset(UILabel.mTempVerts, 0);
				int i = 0;
				while (i < UILabel.mTempIndices.size)
				{
					if (UILabel.mTempIndices[i] == currentIndex)
					{
						Vector2 pos = UILabel.mTempVerts[i];
						if (key == KeyCode.UpArrow)
						{
							pos.y += (float)defaultFontSize + this.effectiveSpacingY;
						}
						else if (key == KeyCode.DownArrow)
						{
							pos.y -= (float)defaultFontSize + this.effectiveSpacingY;
						}
						else if (key == KeyCode.Home)
						{
							pos.x -= 1000f;
						}
						else if (key == KeyCode.End)
						{
							pos.x += 1000f;
						}
						int approximateCharacterIndex = NGUIText.GetApproximateCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, pos);
						if (approximateCharacterIndex != currentIndex)
						{
							UILabel.mTempVerts.Clear();
							UILabel.mTempIndices.Clear();
							return approximateCharacterIndex;
						}
						break;
					}
					else
					{
						i++;
					}
				}
				UILabel.mTempVerts.Clear();
				UILabel.mTempIndices.Clear();
			}
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
			if (key == KeyCode.UpArrow || key == KeyCode.Home)
			{
				return 0;
			}
			if (key == KeyCode.DownArrow || key == KeyCode.End)
			{
				return processedText.get_Length();
			}
		}
		return currentIndex;
	}

	public void PrintOverlay(int start, int end, UIGeometry caret, UIGeometry highlight, Color caretColor, Color highlightColor)
	{
		if (caret != null)
		{
			caret.Clear();
		}
		if (highlight != null)
		{
			highlight.Clear();
		}
		if (!this.isValid)
		{
			return;
		}
		string processedText = this.processedText;
		this.UpdateNGUIText();
		int size = caret.verts.size;
		Vector2 item = new Vector2(0.5f, 0.5f);
		float finalAlpha = this.finalAlpha;
		if (highlight != null && start != end)
		{
			int size2 = highlight.verts.size;
			NGUIText.PrintCaretAndSelection(processedText, start, end, caret.verts, highlight.verts);
			if (highlight.verts.size > size2)
			{
				this.ApplyOffset(highlight.verts, size2);
				Color32 item2 = new Color(highlightColor.r, highlightColor.g, highlightColor.b, highlightColor.a * finalAlpha);
				for (int i = size2; i < highlight.verts.size; i++)
				{
					highlight.uvs.Add(item);
					highlight.cols.Add(item2);
				}
			}
		}
		else
		{
			NGUIText.PrintCaretAndSelection(processedText, start, end, caret.verts, null);
		}
		this.ApplyOffset(caret.verts, size);
		Color32 item3 = new Color(caretColor.r, caretColor.g, caretColor.b, caretColor.a * finalAlpha);
		for (int j = size; j < caret.verts.size; j++)
		{
			caret.uvs.Add(item);
			caret.cols.Add(item3);
		}
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (!this.isValid)
		{
			return;
		}
		int num = verts.size;
		Color color = base.color;
		color.a = this.finalAlpha;
		if (this.mFont != null && this.mFont.premultipliedAlphaShader)
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
		string processedText = this.processedText;
		int size = verts.size;
		this.UpdateNGUIText();
		NGUIText.tint = color;
		NGUIText.Print(processedText, verts, uvs, cols);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		Vector2 vector = this.ApplyOffset(verts, size);
		if (this.mFont != null && this.mFont.packedFontShader)
		{
			return;
		}
		if (this.effectStyle != UILabel.Effect.None)
		{
			int size2 = verts.size;
			vector.x = this.mEffectDistance.x;
			vector.y = this.mEffectDistance.y;
			this.ApplyShadow(verts, uvs, cols, num, size2, vector.x, -vector.y);
			if (this.effectStyle == UILabel.Effect.Outline || this.effectStyle == UILabel.Effect.Outline8)
			{
				num = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, size2, -vector.x, vector.y);
				num = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, size2, vector.x, vector.y);
				num = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, size2, -vector.x, -vector.y);
				if (this.effectStyle == UILabel.Effect.Outline8)
				{
					num = size2;
					size2 = verts.size;
					this.ApplyShadow(verts, uvs, cols, num, size2, -vector.x, 0f);
					num = size2;
					size2 = verts.size;
					this.ApplyShadow(verts, uvs, cols, num, size2, vector.x, 0f);
					num = size2;
					size2 = verts.size;
					this.ApplyShadow(verts, uvs, cols, num, size2, 0f, vector.y);
					num = size2;
					size2 = verts.size;
					this.ApplyShadow(verts, uvs, cols, num, size2, 0f, -vector.y);
				}
			}
		}
		if (this.onPostFill != null)
		{
			this.onPostFill(this, num, verts, uvs, cols);
		}
	}

	public Vector2 ApplyOffset(BetterList<Vector3> verts, int start)
	{
		Vector2 pivotOffset = base.pivotOffset;
		float num = Mathf.Lerp(0f, (float)(-(float)this.mWidth), pivotOffset.x);
		float num2 = Mathf.Lerp((float)this.mHeight, 0f, pivotOffset.y) + Mathf.Lerp(this.mCalculatedSize.y - (float)this.mHeight, 0f, pivotOffset.y);
		num = Mathf.Round(num);
		num2 = Mathf.Round(num2);
		for (int i = start; i < verts.size; i++)
		{
			Vector3[] expr_7F_cp_0_cp_0 = verts.buffer;
			int expr_7F_cp_0_cp_1 = i;
			expr_7F_cp_0_cp_0[expr_7F_cp_0_cp_1].x = expr_7F_cp_0_cp_0[expr_7F_cp_0_cp_1].x + num;
			Vector3[] expr_95_cp_0_cp_0 = verts.buffer;
			int expr_95_cp_0_cp_1 = i;
			expr_95_cp_0_cp_0[expr_95_cp_0_cp_1].y = expr_95_cp_0_cp_0[expr_95_cp_0_cp_1].y + num2;
		}
		return new Vector2(num, num2);
	}

	public void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
	{
		Color color = this.mEffectColor;
		color.a *= this.finalAlpha;
		Color32 color2 = (this.bitmapFont != null && this.bitmapFont.premultipliedAlphaShader) ? NGUITools.ApplyPMA(color) : color;
		for (int i = start; i < end; i++)
		{
			verts.Add(verts.buffer[i]);
			uvs.Add(uvs.buffer[i]);
			cols.Add(cols.buffer[i]);
			Vector3 vector = verts.buffer[i];
			vector.x += x;
			vector.y += y;
			verts.buffer[i] = vector;
			Color32 color3 = cols.buffer[i];
			if (color3.a == 255)
			{
				cols.buffer[i] = color2;
			}
			else
			{
				Color color4 = color;
				color4.a = (float)color3.a / 255f * color.a;
				cols.buffer[i] = ((this.bitmapFont != null && this.bitmapFont.premultipliedAlphaShader) ? NGUITools.ApplyPMA(color4) : color4);
			}
		}
	}

	public int CalculateOffsetToFit(string text)
	{
		this.UpdateNGUIText();
		NGUIText.encoding = false;
		NGUIText.symbolStyle = NGUIText.SymbolStyle.None;
		int result = NGUIText.CalculateOffsetToFit(text);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		return result;
	}

	public void SetCurrentProgress()
	{
		if (UIProgressBar.current != null)
		{
			this.text = UIProgressBar.current.value.ToString("F");
		}
	}

	public void SetCurrentPercent()
	{
		if (UIProgressBar.current != null)
		{
			this.text = Mathf.RoundToInt(UIProgressBar.current.value * 100f) + "%";
		}
	}

	public void SetCurrentSelection()
	{
		if (UIPopupList.current != null)
		{
			this.text = (UIPopupList.current.isLocalized ? Localization.Get(UIPopupList.current.value) : UIPopupList.current.value);
		}
	}

	public bool Wrap(string text, out string final)
	{
		return this.Wrap(text, out final, 1000000);
	}

	public bool Wrap(string text, out string final, int height)
	{
		this.UpdateNGUIText();
		NGUIText.rectHeight = height;
		NGUIText.regionHeight = height;
		bool result = NGUIText.WrapText(text, out final, false);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		return result;
	}

	public void UpdateNGUIText()
	{
		Font trueTypeFont = this.trueTypeFont;
		bool flag = trueTypeFont != null;
		NGUIText.fontSize = this.mFinalFontSize;
		NGUIText.fontStyle = this.mFontStyle;
		NGUIText.rectWidth = this.mWidth;
		NGUIText.rectHeight = this.mHeight;
		NGUIText.regionWidth = Mathf.RoundToInt((float)this.mWidth * (this.mDrawRegion.z - this.mDrawRegion.x));
		NGUIText.regionHeight = Mathf.RoundToInt((float)this.mHeight * (this.mDrawRegion.w - this.mDrawRegion.y));
		NGUIText.gradient = (this.mApplyGradient && (this.mFont == null || !this.mFont.packedFontShader));
		NGUIText.gradientTop = this.mGradientTop;
		NGUIText.gradientBottom = this.mGradientBottom;
		NGUIText.encoding = this.mEncoding;
		NGUIText.premultiply = this.mPremultiply;
		NGUIText.symbolStyle = this.mSymbols;
		NGUIText.maxLines = this.mMaxLineCount;
		NGUIText.spacingX = this.effectiveSpacingX;
		NGUIText.spacingY = this.effectiveSpacingY;
		NGUIText.fontScale = (flag ? this.mScale : ((float)this.mFontSize / (float)this.mFont.defaultSize * this.mScale));
		if (this.mFont != null)
		{
			NGUIText.bitmapFont = this.mFont;
			while (true)
			{
				UIFont replacement = NGUIText.bitmapFont.replacement;
				if (replacement == null)
				{
					break;
				}
				NGUIText.bitmapFont = replacement;
			}
			if (NGUIText.bitmapFont.isDynamic)
			{
				NGUIText.dynamicFont = NGUIText.bitmapFont.dynamicFont;
				NGUIText.bitmapFont = null;
			}
			else
			{
				NGUIText.dynamicFont = null;
			}
		}
		else
		{
			NGUIText.dynamicFont = trueTypeFont;
			NGUIText.bitmapFont = null;
		}
		if (flag && this.keepCrisp)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				NGUIText.pixelDensity = ((root != null) ? root.pixelSizeAdjustment : 1f);
			}
		}
		else
		{
			NGUIText.pixelDensity = 1f;
		}
		if (this.mDensity != NGUIText.pixelDensity)
		{
			this.ProcessText(false, false);
			NGUIText.rectWidth = this.mWidth;
			NGUIText.rectHeight = this.mHeight;
			NGUIText.regionWidth = Mathf.RoundToInt((float)this.mWidth * (this.mDrawRegion.z - this.mDrawRegion.x));
			NGUIText.regionHeight = Mathf.RoundToInt((float)this.mHeight * (this.mDrawRegion.w - this.mDrawRegion.y));
		}
		if (this.alignment == NGUIText.Alignment.Automatic)
		{
			UIWidget.Pivot pivot = base.pivot;
			if (pivot == UIWidget.Pivot.Left || pivot == UIWidget.Pivot.TopLeft || pivot == UIWidget.Pivot.BottomLeft)
			{
				NGUIText.alignment = NGUIText.Alignment.Left;
			}
			else if (pivot == UIWidget.Pivot.Right || pivot == UIWidget.Pivot.TopRight || pivot == UIWidget.Pivot.BottomRight)
			{
				NGUIText.alignment = NGUIText.Alignment.Right;
			}
			else
			{
				NGUIText.alignment = NGUIText.Alignment.Center;
			}
		}
		else
		{
			NGUIText.alignment = this.alignment;
		}
		NGUIText.Update(true, this.UseFontSharpening);
	}

	private void OnApplicationPause(bool paused)
	{
		if (!paused && this.mTrueTypeFont != null)
		{
			this.Invalidate(false);
		}
	}

	public UILabel()
	{
		this.keepCrispWhenShrunk = UILabel.Crispness.OnDesktop;
		this.mText = "";
		this.mFontSize = 16;
		this.mEncoding = true;
		this.mEffectColor = Color.black;
		this.mSymbols = NGUIText.SymbolStyle.Normal;
		this.mEffectDistance = Vector2.one;
		this.mGradientTop = Color.white;
		this.mGradientBottom = new Color(0.7f, 0.7f, 0.7f);
		this.mMultiline = true;
		this.mDensity = 1f;
		this.mShouldBeProcessed = true;
		this.mCalculatedSize = Vector2.zero;
		this.mScale = 1f;
		this.UseFontSharpening = true;
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
		SerializedStateWriter.Instance.WriteInt32((int)this.keepCrispWhenShrunk);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mTrueTypeFont);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mFont);
		}
		SerializedStateWriter.Instance.WriteString(this.mText);
		SerializedStateWriter.Instance.WriteInt32(this.mFontSize);
		SerializedStateWriter.Instance.WriteInt32((int)this.mFontStyle);
		SerializedStateWriter.Instance.WriteInt32((int)this.mAlignment);
		SerializedStateWriter.Instance.WriteBoolean(this.mEncoding);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32(this.mMaxLineCount);
		SerializedStateWriter.Instance.WriteInt32((int)this.mEffectStyle);
		if (depth <= 7)
		{
			this.mEffectColor.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32((int)this.mSymbols);
		if (depth <= 7)
		{
			this.mEffectDistance.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32((int)this.mOverflow);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mMaterial);
		}
		SerializedStateWriter.Instance.WriteBoolean(this.mApplyGradient);
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.mGradientTop.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.mGradientBottom.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32(this.mSpacingX);
		SerializedStateWriter.Instance.WriteInt32(this.mSpacingY);
		SerializedStateWriter.Instance.WriteBoolean(this.mUseFloatSpacing);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.mFloatSpacingX);
		SerializedStateWriter.Instance.WriteSingle(this.mFloatSpacingY);
		SerializedStateWriter.Instance.WriteBoolean(this.mOverflowEllipsis);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.mShrinkToFit);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32(this.mMaxLineWidth);
		SerializedStateWriter.Instance.WriteInt32(this.mMaxLineHeight);
		SerializedStateWriter.Instance.WriteSingle(this.mLineWidth);
		SerializedStateWriter.Instance.WriteBoolean(this.mMultiline);
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
		this.keepCrispWhenShrunk = (UILabel.Crispness)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.mTrueTypeFont = (SerializedStateReader.Instance.ReadUnityEngineObject() as Font);
		}
		if (depth <= 7)
		{
			this.mFont = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIFont);
		}
		this.mText = (SerializedStateReader.Instance.ReadString() as string);
		this.mFontSize = SerializedStateReader.Instance.ReadInt32();
		this.mFontStyle = (FontStyle)SerializedStateReader.Instance.ReadInt32();
		this.mAlignment = (NGUIText.Alignment)SerializedStateReader.Instance.ReadInt32();
		this.mEncoding = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.mMaxLineCount = SerializedStateReader.Instance.ReadInt32();
		this.mEffectStyle = (UILabel.Effect)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.mEffectColor.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.mSymbols = (NGUIText.SymbolStyle)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.mEffectDistance.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.mOverflow = (UILabel.Overflow)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			this.mMaterial = (SerializedStateReader.Instance.ReadUnityEngineObject() as Material);
		}
		this.mApplyGradient = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.mGradientTop.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.mGradientBottom.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.mSpacingX = SerializedStateReader.Instance.ReadInt32();
		this.mSpacingY = SerializedStateReader.Instance.ReadInt32();
		this.mUseFloatSpacing = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.mFloatSpacingX = SerializedStateReader.Instance.ReadSingle();
		this.mFloatSpacingY = SerializedStateReader.Instance.ReadSingle();
		this.mOverflowEllipsis = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.mShrinkToFit = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.mMaxLineWidth = SerializedStateReader.Instance.ReadInt32();
		this.mMaxLineHeight = SerializedStateReader.Instance.ReadInt32();
		this.mLineWidth = SerializedStateReader.Instance.ReadSingle();
		this.mMultiline = SerializedStateReader.Instance.ReadBoolean();
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
		if (this.mTrueTypeFont != null)
		{
			this.mTrueTypeFont = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mTrueTypeFont) as Font);
		}
		if (this.mFont != null)
		{
			this.mFont = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mFont) as UIFont);
		}
		if (this.mMaterial != null)
		{
			this.mMaterial = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mMaterial) as Material);
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
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.keepCrispWhenShrunk, &var_0_cp_0[var_0_cp_1] + 3734);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mTrueTypeFont, &var_0_cp_0[var_0_cp_1] + 3754);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mFont, &var_0_cp_0[var_0_cp_1] + 3525);
		}
		SerializedNamedStateWriter.Instance.WriteString(this.mText, &var_0_cp_0[var_0_cp_1] + 3768);
		SerializedNamedStateWriter.Instance.WriteInt32(this.mFontSize, &var_0_cp_0[var_0_cp_1] + 3774);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mFontStyle, &var_0_cp_0[var_0_cp_1] + 3784);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mAlignment, &var_0_cp_0[var_0_cp_1] + 3795);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mEncoding, &var_0_cp_0[var_0_cp_1] + 3806);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32(this.mMaxLineCount, &var_0_cp_0[var_0_cp_1] + 3816);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mEffectStyle, &var_0_cp_0[var_0_cp_1] + 3830);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3843);
			this.mEffectColor.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mSymbols, &var_0_cp_0[var_0_cp_1] + 3538);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3856);
			this.mEffectDistance.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mOverflow, &var_0_cp_0[var_0_cp_1] + 3872);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mMaterial, &var_0_cp_0[var_0_cp_1] + 3882);
		}
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mApplyGradient, &var_0_cp_0[var_0_cp_1] + 3892);
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3907);
			this.mGradientTop.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3920);
			this.mGradientBottom.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32(this.mSpacingX, &var_0_cp_0[var_0_cp_1] + 3936);
		SerializedNamedStateWriter.Instance.WriteInt32(this.mSpacingY, &var_0_cp_0[var_0_cp_1] + 3946);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mUseFloatSpacing, &var_0_cp_0[var_0_cp_1] + 3956);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.mFloatSpacingX, &var_0_cp_0[var_0_cp_1] + 3973);
		SerializedNamedStateWriter.Instance.WriteSingle(this.mFloatSpacingY, &var_0_cp_0[var_0_cp_1] + 3988);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mOverflowEllipsis, &var_0_cp_0[var_0_cp_1] + 4003);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mShrinkToFit, &var_0_cp_0[var_0_cp_1] + 4021);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32(this.mMaxLineWidth, &var_0_cp_0[var_0_cp_1] + 4034);
		SerializedNamedStateWriter.Instance.WriteInt32(this.mMaxLineHeight, &var_0_cp_0[var_0_cp_1] + 4048);
		SerializedNamedStateWriter.Instance.WriteSingle(this.mLineWidth, &var_0_cp_0[var_0_cp_1] + 4063);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.mMultiline, &var_0_cp_0[var_0_cp_1] + 4074);
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
		this.keepCrispWhenShrunk = (UILabel.Crispness)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3734);
		if (depth <= 7)
		{
			this.mTrueTypeFont = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3754) as Font);
		}
		if (depth <= 7)
		{
			this.mFont = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3525) as UIFont);
		}
		this.mText = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 3768) as string);
		this.mFontSize = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3774);
		this.mFontStyle = (FontStyle)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3784);
		this.mAlignment = (NGUIText.Alignment)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3795);
		this.mEncoding = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3806);
		SerializedNamedStateReader.Instance.Align();
		this.mMaxLineCount = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3816);
		this.mEffectStyle = (UILabel.Effect)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3830);
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3843);
			this.mEffectColor.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.mSymbols = (NGUIText.SymbolStyle)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3538);
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3856);
			this.mEffectDistance.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.mOverflow = (UILabel.Overflow)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3872);
		if (depth <= 7)
		{
			this.mMaterial = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3882) as Material);
		}
		this.mApplyGradient = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3892);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3907);
			this.mGradientTop.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3920);
			this.mGradientBottom.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.mSpacingX = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3936);
		this.mSpacingY = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3946);
		this.mUseFloatSpacing = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3956);
		SerializedNamedStateReader.Instance.Align();
		this.mFloatSpacingX = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 3973);
		this.mFloatSpacingY = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 3988);
		this.mOverflowEllipsis = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4003);
		SerializedNamedStateReader.Instance.Align();
		this.mShrinkToFit = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4021);
		SerializedNamedStateReader.Instance.Align();
		this.mMaxLineWidth = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4034);
		this.mMaxLineHeight = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4048);
		this.mLineWidth = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 4063);
		this.mMultiline = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4074);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UILabel(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)instance).mTrueTypeFont);
	}

	public static void $Set0(object instance, long value)
	{
		((UILabel)instance).mTrueTypeFont = (Font)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)instance).mFont);
	}

	public static void $Set1(object instance, long value)
	{
		((UILabel)instance).mFont = (UIFont)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get2(object instance)
	{
		return ((UILabel)instance).mEncoding;
	}

	public static void $Set2(object instance, bool value)
	{
		((UILabel)instance).mEncoding = value;
	}

	public static float $Get3(object instance, int index)
	{
		UILabel expr_06_cp_0 = (UILabel)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.mEffectColor.r;
		case 1:
			return expr_06_cp_0.mEffectColor.g;
		case 2:
			return expr_06_cp_0.mEffectColor.b;
		case 3:
			return expr_06_cp_0.mEffectColor.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set3(object instance, float value, int index)
	{
		UILabel expr_06_cp_0 = (UILabel)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.mEffectColor.r = value;
			return;
		case 1:
			expr_06_cp_0.mEffectColor.g = value;
			return;
		case 2:
			expr_06_cp_0.mEffectColor.b = value;
			return;
		case 3:
			expr_06_cp_0.mEffectColor.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get4(object instance, int index)
	{
		UILabel expr_06_cp_0 = (UILabel)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.mEffectDistance.x;
		case 1:
			return expr_06_cp_0.mEffectDistance.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set4(object instance, float value, int index)
	{
		UILabel expr_06_cp_0 = (UILabel)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.mEffectDistance.x = value;
			return;
		case 1:
			expr_06_cp_0.mEffectDistance.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static long $Get5(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)instance).mMaterial);
	}

	public static void $Set5(object instance, long value)
	{
		((UILabel)instance).mMaterial = (Material)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get6(object instance)
	{
		return ((UILabel)instance).mApplyGradient;
	}

	public static void $Set6(object instance, bool value)
	{
		((UILabel)instance).mApplyGradient = value;
	}

	public static float $Get7(object instance, int index)
	{
		UILabel expr_06_cp_0 = (UILabel)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.mGradientTop.r;
		case 1:
			return expr_06_cp_0.mGradientTop.g;
		case 2:
			return expr_06_cp_0.mGradientTop.b;
		case 3:
			return expr_06_cp_0.mGradientTop.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set7(object instance, float value, int index)
	{
		UILabel expr_06_cp_0 = (UILabel)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.mGradientTop.r = value;
			return;
		case 1:
			expr_06_cp_0.mGradientTop.g = value;
			return;
		case 2:
			expr_06_cp_0.mGradientTop.b = value;
			return;
		case 3:
			expr_06_cp_0.mGradientTop.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get8(object instance, int index)
	{
		UILabel expr_06_cp_0 = (UILabel)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.mGradientBottom.r;
		case 1:
			return expr_06_cp_0.mGradientBottom.g;
		case 2:
			return expr_06_cp_0.mGradientBottom.b;
		case 3:
			return expr_06_cp_0.mGradientBottom.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set8(object instance, float value, int index)
	{
		UILabel expr_06_cp_0 = (UILabel)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.mGradientBottom.r = value;
			return;
		case 1:
			expr_06_cp_0.mGradientBottom.g = value;
			return;
		case 2:
			expr_06_cp_0.mGradientBottom.b = value;
			return;
		case 3:
			expr_06_cp_0.mGradientBottom.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static bool $Get9(object instance)
	{
		return ((UILabel)instance).mUseFloatSpacing;
	}

	public static void $Set9(object instance, bool value)
	{
		((UILabel)instance).mUseFloatSpacing = value;
	}

	public static float $Get10(object instance)
	{
		return ((UILabel)instance).mFloatSpacingX;
	}

	public static void $Set10(object instance, float value)
	{
		((UILabel)instance).mFloatSpacingX = value;
	}

	public static float $Get11(object instance)
	{
		return ((UILabel)instance).mFloatSpacingY;
	}

	public static void $Set11(object instance, float value)
	{
		((UILabel)instance).mFloatSpacingY = value;
	}

	public static bool $Get12(object instance)
	{
		return ((UILabel)instance).mOverflowEllipsis;
	}

	public static void $Set12(object instance, bool value)
	{
		((UILabel)instance).mOverflowEllipsis = value;
	}

	public static bool $Get13(object instance)
	{
		return ((UILabel)instance).mShrinkToFit;
	}

	public static void $Set13(object instance, bool value)
	{
		((UILabel)instance).mShrinkToFit = value;
	}

	public static float $Get14(object instance)
	{
		return ((UILabel)instance).mLineWidth;
	}

	public static void $Set14(object instance, float value)
	{
		((UILabel)instance).mLineWidth = value;
	}

	public static bool $Get15(object instance)
	{
		return ((UILabel)instance).mMultiline;
	}

	public static void $Set15(object instance, bool value)
	{
		((UILabel)instance).mMultiline = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).ApplyOffset((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).ApplyShadow((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]), *(int*)(args + 3), *(int*)(args + 4), *(float*)(args + 5), *(float*)(args + 6));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).AssumeNaturalSize();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).CalculateOffsetToFit(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).alignment);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).ambigiousFont);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).applyGradient);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).bitmapFont);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).defaultFontSize);
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).drawingDimensions);
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).effectColor);
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).effectDistance);
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).effectiveSpacingX);
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).effectiveSpacingY);
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).effectStyle);
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).finalFontSize);
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).floatSpacingX);
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).floatSpacingY);
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).font);
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).fontSize);
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).fontStyle);
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).gradientBottom);
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).gradientTop);
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).isAnchoredHorizontally);
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).isAnchoredVertically);
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).isValid);
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).keepCrisp);
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).lineHeight);
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).lineWidth);
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).localCorners);
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).localSize);
	}

	public unsafe static long $Invoke31(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).material);
	}

	public unsafe static long $Invoke32(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).maxLineCount);
	}

	public unsafe static long $Invoke33(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).multiLine);
	}

	public unsafe static long $Invoke34(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).overflowEllipsis);
	}

	public unsafe static long $Invoke35(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).overflowMethod);
	}

	public unsafe static long $Invoke36(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).printedSize);
	}

	public unsafe static long $Invoke37(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).processedText);
	}

	public unsafe static long $Invoke38(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).shouldBeProcessed);
	}

	public unsafe static long $Invoke39(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).shrinkToFit);
	}

	public unsafe static long $Invoke40(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).spacingX);
	}

	public unsafe static long $Invoke41(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).spacingY);
	}

	public unsafe static long $Invoke42(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).supportEncoding);
	}

	public unsafe static long $Invoke43(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).symbolStyle);
	}

	public unsafe static long $Invoke44(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).text);
	}

	public unsafe static long $Invoke45(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).trueTypeFont);
	}

	public unsafe static long $Invoke46(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).useFloatSpacing);
	}

	public unsafe static long $Invoke47(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).worldCorners);
	}

	public unsafe static long $Invoke48(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).GetCharacterIndex(*(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke49(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).GetCharacterIndex(*(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke50(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).GetCharacterIndex(*(int*)args, (KeyCode)(*(int*)(args + 1))));
	}

	public unsafe static long $Invoke51(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).GetCharacterIndexAtPosition(*(*(IntPtr*)args), *(sbyte*)(args + 1) != 0));
	}

	public unsafe static long $Invoke52(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).GetCharacterIndexAtPosition(*(*(IntPtr*)args), *(sbyte*)(args + 1) != 0));
	}

	public unsafe static long $Invoke53(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).GetSides((Transform)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke54(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).GetUrlAtCharacterIndex(*(int*)args));
	}

	public unsafe static long $Invoke55(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).GetUrlAtPosition(*(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke56(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).GetUrlAtPosition(*(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke57(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).GetWordAtCharacterIndex(*(int*)args));
	}

	public unsafe static long $Invoke58(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).GetWordAtPosition(*(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke59(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UILabel)GCHandledObjects.GCHandleToObject(instance)).GetWordAtPosition(*(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke60(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).MakePixelPerfect();
		return -1L;
	}

	public unsafe static long $Invoke61(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).MarkAsChanged();
		return -1L;
	}

	public unsafe static long $Invoke62(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).OnAnchor();
		return -1L;
	}

	public unsafe static long $Invoke63(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).OnApplicationPause(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke64(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke65(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke66(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).OnFill((BetterList<Vector3>)GCHandledObjects.GCHandleToObject(*args), (BetterList<Vector2>)GCHandledObjects.GCHandleToObject(args[1]), (BetterList<Color32>)GCHandledObjects.GCHandleToObject(args[2]));
		return -1L;
	}

	public unsafe static long $Invoke67(long instance, long* args)
	{
		UILabel.OnFontChanged((Font)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke68(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).OnInit();
		return -1L;
	}

	public unsafe static long $Invoke69(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).OnStart();
		return -1L;
	}

	public unsafe static long $Invoke70(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).PrintOverlay(*(int*)args, *(int*)(args + 1), (UIGeometry)GCHandledObjects.GCHandleToObject(args[2]), (UIGeometry)GCHandledObjects.GCHandleToObject(args[3]), *(*(IntPtr*)(args + 4)), *(*(IntPtr*)(args + 5)));
		return -1L;
	}

	public unsafe static long $Invoke71(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).ProcessAndRequest();
		return -1L;
	}

	public unsafe static long $Invoke72(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).ProcessText();
		return -1L;
	}

	public unsafe static long $Invoke73(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).ProcessText(*(sbyte*)args != 0, *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke74(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).alignment = (NGUIText.Alignment)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke75(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).ambigiousFont = (UnityEngine.Object)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke76(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).applyGradient = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke77(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).bitmapFont = (UIFont)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke78(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).effectColor = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke79(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).effectDistance = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke80(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).effectStyle = (UILabel.Effect)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke81(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).floatSpacingX = *(float*)args;
		return -1L;
	}

	public unsafe static long $Invoke82(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).floatSpacingY = *(float*)args;
		return -1L;
	}

	public unsafe static long $Invoke83(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).font = (UIFont)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke84(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).fontSize = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke85(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).fontStyle = (FontStyle)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke86(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).gradientBottom = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke87(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).gradientTop = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke88(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).lineHeight = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke89(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).lineWidth = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke90(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).material = (Material)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke91(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).maxLineCount = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke92(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).multiLine = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke93(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).overflowEllipsis = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke94(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).overflowMethod = (UILabel.Overflow)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke95(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).shouldBeProcessed = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke96(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).shrinkToFit = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke97(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).spacingX = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke98(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).spacingY = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke99(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).supportEncoding = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke100(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).symbolStyle = (NGUIText.SymbolStyle)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke101(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).text = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke102(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).trueTypeFont = (Font)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke103(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).useFloatSpacing = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke104(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).SetActiveFont((Font)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke105(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).SetCurrentPercent();
		return -1L;
	}

	public unsafe static long $Invoke106(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).SetCurrentProgress();
		return -1L;
	}

	public unsafe static long $Invoke107(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).SetCurrentSelection();
		return -1L;
	}

	public unsafe static long $Invoke108(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke109(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke110(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke111(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke112(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke113(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).UpdateNGUIText();
		return -1L;
	}

	public unsafe static long $Invoke114(long instance, long* args)
	{
		((UILabel)GCHandledObjects.GCHandleToObject(instance)).UpgradeFrom265();
		return -1L;
	}
}
