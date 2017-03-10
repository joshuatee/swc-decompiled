using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/NGUI Font"), ExecuteInEditMode]
public class UIFont : MonoBehaviour, IUnitySerializable
{
	[HideInInspector, SerializeField]
	protected internal Material mMat;

	[HideInInspector, SerializeField]
	protected internal Rect mUVRect;

	[HideInInspector, SerializeField]
	protected internal BMFont mFont;

	[HideInInspector, SerializeField]
	protected internal UIAtlas mAtlas;

	[HideInInspector, SerializeField]
	protected internal UIFont mReplacement;

	[HideInInspector, SerializeField]
	protected internal List<BMSymbol> mSymbols;

	[HideInInspector, SerializeField]
	protected internal Font mDynamicFont;

	[HideInInspector, SerializeField]
	protected internal int mDynamicFontSize;

	[HideInInspector, SerializeField]
	protected internal FontStyle mDynamicFontStyle;

	[System.NonSerialized]
	private UISpriteData mSprite;

	private int mPMA;

	private int mPacked;

	public BMFont bmFont
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mFont;
			}
			return this.mReplacement.bmFont;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.bmFont = value;
				return;
			}
			this.mFont = value;
		}
	}

	public int texWidth
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.texWidth;
			}
			if (this.mFont == null)
			{
				return 1;
			}
			return this.mFont.texWidth;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.texWidth = value;
				return;
			}
			if (this.mFont != null)
			{
				this.mFont.texWidth = value;
			}
		}
	}

	public int texHeight
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.texHeight;
			}
			if (this.mFont == null)
			{
				return 1;
			}
			return this.mFont.texHeight;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.texHeight = value;
				return;
			}
			if (this.mFont != null)
			{
				this.mFont.texHeight = value;
			}
		}
	}

	public bool hasSymbols
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mSymbols != null && this.mSymbols.Count != 0;
			}
			return this.mReplacement.hasSymbols;
		}
	}

	public List<BMSymbol> symbols
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mSymbols;
			}
			return this.mReplacement.symbols;
		}
	}

	public UIAtlas atlas
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mAtlas;
			}
			return this.mReplacement.atlas;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.atlas = value;
				return;
			}
			if (this.mAtlas != value)
			{
				this.mPMA = -1;
				this.mAtlas = value;
				if (this.mAtlas != null)
				{
					this.mMat = this.mAtlas.spriteMaterial;
					if (this.sprite != null)
					{
						this.mUVRect = this.uvRect;
					}
				}
				this.MarkAsChanged();
			}
		}
	}

	public Material material
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.material;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.spriteMaterial;
			}
			if (this.mMat != null)
			{
				if (this.mDynamicFont != null && this.mMat != this.mDynamicFont.material)
				{
					this.mMat.mainTexture = this.mDynamicFont.material.mainTexture;
				}
				return this.mMat;
			}
			if (this.mDynamicFont != null)
			{
				return this.mDynamicFont.material;
			}
			return null;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.material = value;
				return;
			}
			if (this.mMat != value)
			{
				this.mPMA = -1;
				this.mMat = value;
				this.MarkAsChanged();
			}
		}
	}

	[Obsolete("Use UIFont.premultipliedAlphaShader instead")]
	public bool premultipliedAlpha
	{
		get
		{
			return this.premultipliedAlphaShader;
		}
	}

	public bool premultipliedAlphaShader
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlphaShader;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = ((material != null && material.shader != null && material.shader.name.Contains("Premultiplied")) ? 1 : 0);
			}
			return this.mPMA == 1;
		}
	}

	public bool packedFontShader
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.packedFontShader;
			}
			if (this.mAtlas != null)
			{
				return false;
			}
			if (this.mPacked == -1)
			{
				Material material = this.material;
				this.mPacked = ((material != null && material.shader != null && material.shader.name.Contains("Packed")) ? 1 : 0);
			}
			return this.mPacked == 1;
		}
	}

	public Texture2D texture
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.texture;
			}
			Material material = this.material;
			if (!(material != null))
			{
				return null;
			}
			return material.mainTexture as Texture2D;
		}
	}

	public Rect uvRect
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.uvRect;
			}
			if (!(this.mAtlas != null) || this.sprite == null)
			{
				return new Rect(0f, 0f, 1f, 1f);
			}
			return this.mUVRect;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.uvRect = value;
				return;
			}
			if (this.sprite == null && this.mUVRect != value)
			{
				this.mUVRect = value;
				this.MarkAsChanged();
			}
		}
	}

	public string spriteName
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mFont.spriteName;
			}
			return this.mReplacement.spriteName;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteName = value;
				return;
			}
			if (this.mFont.spriteName != value)
			{
				this.mFont.spriteName = value;
				this.MarkAsChanged();
			}
		}
	}

	public bool isValid
	{
		get
		{
			return this.mDynamicFont != null || this.mFont.isValid;
		}
	}

	[Obsolete("Use UIFont.defaultSize instead")]
	public int size
	{
		get
		{
			return this.defaultSize;
		}
		set
		{
			this.defaultSize = value;
		}
	}

	public int defaultSize
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.defaultSize;
			}
			if (this.isDynamic || this.mFont == null)
			{
				return this.mDynamicFontSize;
			}
			return this.mFont.charSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.defaultSize = value;
				return;
			}
			this.mDynamicFontSize = value;
		}
	}

	public UISpriteData sprite
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.sprite;
			}
			if (this.mSprite == null && this.mAtlas != null && !string.IsNullOrEmpty(this.mFont.spriteName))
			{
				this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
				if (this.mSprite == null)
				{
					this.mSprite = this.mAtlas.GetSprite(base.name);
				}
				if (this.mSprite == null)
				{
					this.mFont.spriteName = null;
				}
				else
				{
					this.UpdateUVRect();
				}
				int i = 0;
				int count = this.mSymbols.Count;
				while (i < count)
				{
					this.symbols[i].MarkAsChanged();
					i++;
				}
			}
			return this.mSprite;
		}
	}

	public UIFont replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIFont uIFont = value;
			if (uIFont == this)
			{
				uIFont = null;
			}
			if (this.mReplacement != uIFont)
			{
				if (uIFont != null && uIFont.replacement == this)
				{
					uIFont.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uIFont;
				if (uIFont != null)
				{
					this.mPMA = -1;
					this.mMat = null;
					this.mFont = null;
					this.mDynamicFont = null;
				}
				this.MarkAsChanged();
			}
		}
	}

	public bool isDynamic
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mDynamicFont != null;
			}
			return this.mReplacement.isDynamic;
		}
	}

	public Font dynamicFont
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mDynamicFont;
			}
			return this.mReplacement.dynamicFont;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFont = value;
				return;
			}
			if (this.mDynamicFont != value)
			{
				if (this.mDynamicFont != null)
				{
					this.material = null;
				}
				this.mDynamicFont = value;
				this.MarkAsChanged();
			}
		}
	}

	public FontStyle dynamicFontStyle
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mDynamicFontStyle;
			}
			return this.mReplacement.dynamicFontStyle;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFontStyle = value;
				return;
			}
			if (this.mDynamicFontStyle != value)
			{
				this.mDynamicFontStyle = value;
				this.MarkAsChanged();
			}
		}
	}

	private Texture dynamicTexture
	{
		get
		{
			if (this.mReplacement)
			{
				return this.mReplacement.dynamicTexture;
			}
			if (this.isDynamic)
			{
				return this.mDynamicFont.material.mainTexture;
			}
			return null;
		}
	}

	private void Trim()
	{
		Texture texture = this.mAtlas.texture;
		if (texture != null && this.mSprite != null)
		{
			Rect rect = NGUIMath.ConvertToPixels(this.mUVRect, this.texture.width, this.texture.height, true);
			Rect rect2 = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
			int xMin = Mathf.RoundToInt(rect2.xMin - rect.xMin);
			int yMin = Mathf.RoundToInt(rect2.yMin - rect.yMin);
			int xMax = Mathf.RoundToInt(rect2.xMax - rect.xMin);
			int yMax = Mathf.RoundToInt(rect2.yMax - rect.yMin);
			this.mFont.Trim(xMin, yMin, xMax, yMax);
		}
	}

	private bool References(UIFont font)
	{
		return !(font == null) && (font == this || (this.mReplacement != null && this.mReplacement.References(font)));
	}

	public static bool CheckIfRelated(UIFont a, UIFont b)
	{
		return !(a == null) && !(b == null) && ((a.isDynamic && b.isDynamic && a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0]) || a == b || a.References(b) || b.References(a));
	}

	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		this.mSprite = null;
		UILabel[] array = NGUITools.FindActive<UILabel>();
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			UILabel uILabel = array[i];
			if (uILabel.enabled && NGUITools.GetActive(uILabel.gameObject) && UIFont.CheckIfRelated(this, uILabel.bitmapFont))
			{
				UIFont bitmapFont = uILabel.bitmapFont;
				uILabel.bitmapFont = null;
				uILabel.bitmapFont = bitmapFont;
			}
			i++;
		}
		int j = 0;
		int count = this.symbols.Count;
		while (j < count)
		{
			this.symbols[j].MarkAsChanged();
			j++;
		}
	}

	public void UpdateUVRect()
	{
		if (this.mAtlas == null)
		{
			return;
		}
		Texture texture = this.mAtlas.texture;
		if (texture != null)
		{
			this.mUVRect = new Rect((float)(this.mSprite.x - this.mSprite.paddingLeft), (float)(this.mSprite.y - this.mSprite.paddingTop), (float)(this.mSprite.width + this.mSprite.paddingLeft + this.mSprite.paddingRight), (float)(this.mSprite.height + this.mSprite.paddingTop + this.mSprite.paddingBottom));
			this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
			if (this.mSprite.hasPadding)
			{
				this.Trim();
			}
		}
	}

	private BMSymbol GetSymbol(string sequence, bool createIfMissing)
	{
		int i = 0;
		int count = this.mSymbols.Count;
		while (i < count)
		{
			BMSymbol bMSymbol = this.mSymbols[i];
			if (bMSymbol.sequence == sequence)
			{
				return bMSymbol;
			}
			i++;
		}
		if (createIfMissing)
		{
			BMSymbol bMSymbol2 = new BMSymbol();
			bMSymbol2.sequence = sequence;
			this.mSymbols.Add(bMSymbol2);
			return bMSymbol2;
		}
		return null;
	}

	public BMSymbol MatchSymbol(string text, int offset, int textLength)
	{
		int count = this.mSymbols.Count;
		if (count == 0)
		{
			return null;
		}
		textLength -= offset;
		for (int i = 0; i < count; i++)
		{
			BMSymbol bMSymbol = this.mSymbols[i];
			int length = bMSymbol.length;
			if (length != 0 && textLength >= length)
			{
				bool flag = true;
				for (int j = 0; j < length; j++)
				{
					if (text.get_Chars(offset + j) != bMSymbol.sequence.get_Chars(j))
					{
						flag = false;
						break;
					}
				}
				if (flag && bMSymbol.Validate(this.atlas))
				{
					return bMSymbol;
				}
			}
		}
		return null;
	}

	public void AddSymbol(string sequence, string spriteName)
	{
		BMSymbol symbol = this.GetSymbol(sequence, true);
		symbol.spriteName = spriteName;
		this.MarkAsChanged();
	}

	public void RemoveSymbol(string sequence)
	{
		BMSymbol symbol = this.GetSymbol(sequence, false);
		if (symbol != null)
		{
			this.symbols.Remove(symbol);
		}
		this.MarkAsChanged();
	}

	public void RenameSymbol(string before, string after)
	{
		BMSymbol symbol = this.GetSymbol(before, false);
		if (symbol != null)
		{
			symbol.sequence = after;
		}
		this.MarkAsChanged();
	}

	public bool UsesSprite(string s)
	{
		if (!string.IsNullOrEmpty(s))
		{
			if (s.Equals(this.spriteName))
			{
				return true;
			}
			int i = 0;
			int count = this.symbols.Count;
			while (i < count)
			{
				BMSymbol bMSymbol = this.symbols[i];
				if (s.Equals(bMSymbol.spriteName))
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	public UIFont()
	{
		this.mUVRect = new Rect(0f, 0f, 1f, 1f);
		this.mFont = new BMFont();
		this.mSymbols = new List<BMSymbol>();
		this.mDynamicFontSize = 16;
		this.mPMA = -1;
		this.mPacked = -1;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mMat);
		}
		if (depth <= 7)
		{
			this.mUVRect.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			if (this.mFont == null)
			{
				this.mFont = new BMFont();
			}
			this.mFont.Unity_Serialize(depth + 1);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mAtlas);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mReplacement);
		}
		if (depth <= 7)
		{
			if (this.mSymbols == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.mSymbols.Count);
				for (int i = 0; i < this.mSymbols.Count; i++)
				{
					((this.mSymbols[i] != null) ? this.mSymbols[i] : new BMSymbol()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mDynamicFont);
		}
		SerializedStateWriter.Instance.WriteInt32(this.mDynamicFontSize);
		SerializedStateWriter.Instance.WriteInt32((int)this.mDynamicFontStyle);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.mMat = (SerializedStateReader.Instance.ReadUnityEngineObject() as Material);
		}
		if (depth <= 7)
		{
			this.mUVRect.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			if (this.mFont == null)
			{
				this.mFont = new BMFont();
			}
			this.mFont.Unity_Deserialize(depth + 1);
		}
		if (depth <= 7)
		{
			this.mAtlas = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIAtlas);
		}
		if (depth <= 7)
		{
			this.mReplacement = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIFont);
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.mSymbols = new List<BMSymbol>(num);
			for (int i = 0; i < num; i++)
			{
				BMSymbol bMSymbol = new BMSymbol();
				bMSymbol.Unity_Deserialize(depth + 1);
				this.mSymbols.Add(bMSymbol);
			}
		}
		if (depth <= 7)
		{
			this.mDynamicFont = (SerializedStateReader.Instance.ReadUnityEngineObject() as Font);
		}
		this.mDynamicFontSize = SerializedStateReader.Instance.ReadInt32();
		this.mDynamicFontStyle = (FontStyle)SerializedStateReader.Instance.ReadInt32();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.mMat != null)
		{
			this.mMat = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mMat) as Material);
		}
		if (depth <= 7)
		{
			if (this.mFont != null)
			{
				this.mFont.Unity_RemapPPtrs(depth + 1);
			}
		}
		if (this.mAtlas != null)
		{
			this.mAtlas = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mAtlas) as UIAtlas);
		}
		if (this.mReplacement != null)
		{
			this.mReplacement = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mReplacement) as UIFont);
		}
		if (depth <= 7)
		{
			if (this.mSymbols != null)
			{
				for (int i = 0; i < this.mSymbols.Count; i++)
				{
					BMSymbol bMSymbol = this.mSymbols[i];
					if (bMSymbol != null)
					{
						bMSymbol.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (this.mDynamicFont != null)
		{
			this.mDynamicFont = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mDynamicFont) as Font);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.mMat;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 2796);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3517);
			this.mUVRect.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			if (this.mFont == null)
			{
				this.mFont = new BMFont();
			}
			BMFont arg_95_0 = this.mFont;
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3525);
			arg_95_0.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mAtlas, &var_0_cp_0[var_0_cp_1] + 3531);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mReplacement, &var_0_cp_0[var_0_cp_1] + 3046);
		}
		if (depth <= 7)
		{
			if (this.mSymbols == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 3538, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 3538, this.mSymbols.Count);
				for (int i = 0; i < this.mSymbols.Count; i++)
				{
					BMSymbol arg_165_0 = (this.mSymbols[i] != null) ? this.mSymbols[i] : new BMSymbol();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_165_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mDynamicFont, &var_0_cp_0[var_0_cp_1] + 3547);
		}
		SerializedNamedStateWriter.Instance.WriteInt32(this.mDynamicFontSize, &var_0_cp_0[var_0_cp_1] + 3560);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mDynamicFontStyle, &var_0_cp_0[var_0_cp_1] + 3577);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateReader arg_1E_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			this.mMat = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 2796) as Material);
		}
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3517);
			this.mUVRect.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			if (this.mFont == null)
			{
				this.mFont = new BMFont();
			}
			BMFont arg_9A_0 = this.mFont;
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3525);
			arg_9A_0.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			this.mAtlas = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3531) as UIAtlas);
		}
		if (depth <= 7)
		{
			this.mReplacement = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3046) as UIFont);
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 3538);
			this.mSymbols = new List<BMSymbol>(num);
			for (int i = 0; i < num; i++)
			{
				BMSymbol bMSymbol = new BMSymbol();
				BMSymbol arg_12C_0 = bMSymbol;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_12C_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.mSymbols.Add(bMSymbol);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			this.mDynamicFont = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3547) as Font);
		}
		this.mDynamicFontSize = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3560);
		this.mDynamicFontStyle = (FontStyle)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3577);
	}

	protected internal UIFont(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)instance).mMat);
	}

	public static void $Set0(object instance, long value)
	{
		((UIFont)instance).mMat = (Material)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)instance).mAtlas);
	}

	public static void $Set1(object instance, long value)
	{
		((UIFont)instance).mAtlas = (UIAtlas)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)instance).mReplacement);
	}

	public static void $Set2(object instance, long value)
	{
		((UIFont)instance).mReplacement = (UIFont)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get3(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)instance).mDynamicFont);
	}

	public static void $Set3(object instance, long value)
	{
		((UIFont)instance).mDynamicFont = (Font)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).AddSymbol(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIFont.CheckIfRelated((UIFont)GCHandledObjects.GCHandleToObject(*args), (UIFont)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).atlas);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).bmFont);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).defaultSize);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).dynamicFont);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).dynamicFontStyle);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).dynamicTexture);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).hasSymbols);
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).isDynamic);
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).isValid);
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).material);
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).packedFontShader);
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).premultipliedAlpha);
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).premultipliedAlphaShader);
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).replacement);
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).size);
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).sprite);
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).spriteName);
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).symbols);
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).texHeight);
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).texture);
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).texWidth);
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).uvRect);
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).GetSymbol(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0));
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).MarkAsChanged();
		return -1L;
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).MatchSymbol(Marshal.PtrToStringUni(*(IntPtr*)args), *(int*)(args + 1), *(int*)(args + 2)));
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).References((UIFont)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).RemoveSymbol(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).RenameSymbol(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).atlas = (UIAtlas)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke31(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).bmFont = (BMFont)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke32(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).defaultSize = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke33(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).dynamicFont = (Font)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke34(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).dynamicFontStyle = (FontStyle)(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke35(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).material = (Material)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke36(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).replacement = (UIFont)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke37(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).size = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke38(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).spriteName = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke39(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).texHeight = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke40(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).texWidth = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke41(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).uvRect = *(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke42(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).Trim();
		return -1L;
	}

	public unsafe static long $Invoke43(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke44(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke45(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke46(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke47(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke48(long instance, long* args)
	{
		((UIFont)GCHandledObjects.GCHandleToObject(instance)).UpdateUVRect();
		return -1L;
	}

	public unsafe static long $Invoke49(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIFont)GCHandledObjects.GCHandleToObject(instance)).UsesSprite(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}
}
