using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour, IUnitySerializable
{
	[System.Serializable]
	private class Sprite : IUnitySerializable
	{
		public string name = "Unity Bug";

		public Rect outer = new Rect(0f, 0f, 1f, 1f);

		public Rect inner = new Rect(0f, 0f, 1f, 1f);

		public bool rotated;

		public float paddingLeft;

		public float paddingRight;

		public float paddingTop;

		public float paddingBottom;

		public bool hasPadding
		{
			get
			{
				return this.paddingLeft != 0f || this.paddingRight != 0f || this.paddingTop != 0f || this.paddingBottom != 0f;
			}
		}

		public override void Unity_Serialize(int depth)
		{
			SerializedStateWriter.Instance.WriteString(this.name);
			if (depth <= 7)
			{
				this.outer.Unity_Serialize(depth + 1);
			}
			SerializedStateWriter.Instance.Align();
			if (depth <= 7)
			{
				this.inner.Unity_Serialize(depth + 1);
			}
			SerializedStateWriter.Instance.Align();
			SerializedStateWriter.Instance.WriteBoolean(this.rotated);
			SerializedStateWriter.Instance.Align();
			SerializedStateWriter.Instance.WriteSingle(this.paddingLeft);
			SerializedStateWriter.Instance.WriteSingle(this.paddingRight);
			SerializedStateWriter.Instance.WriteSingle(this.paddingTop);
			SerializedStateWriter.Instance.WriteSingle(this.paddingBottom);
		}

		public override void Unity_Deserialize(int depth)
		{
			this.name = (SerializedStateReader.Instance.ReadString() as string);
			if (depth <= 7)
			{
				this.outer.Unity_Deserialize(depth + 1);
			}
			SerializedStateReader.Instance.Align();
			if (depth <= 7)
			{
				this.inner.Unity_Deserialize(depth + 1);
			}
			SerializedStateReader.Instance.Align();
			this.rotated = SerializedStateReader.Instance.ReadBoolean();
			SerializedStateReader.Instance.Align();
			this.paddingLeft = SerializedStateReader.Instance.ReadSingle();
			this.paddingRight = SerializedStateReader.Instance.ReadSingle();
			this.paddingTop = SerializedStateReader.Instance.ReadSingle();
			this.paddingBottom = SerializedStateReader.Instance.ReadSingle();
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
			if (depth <= 7)
			{
				SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2958);
				this.outer.Unity_NamedSerialize(depth + 1);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			SerializedNamedStateWriter.Instance.Align();
			if (depth <= 7)
			{
				SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2964);
				this.inner.Unity_NamedSerialize(depth + 1);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			SerializedNamedStateWriter.Instance.Align();
			SerializedNamedStateWriter.Instance.WriteBoolean(this.rotated, &var_0_cp_0[var_0_cp_1] + 2970);
			SerializedNamedStateWriter.Instance.Align();
			SerializedNamedStateWriter.Instance.WriteSingle(this.paddingLeft, &var_0_cp_0[var_0_cp_1] + 2978);
			SerializedNamedStateWriter.Instance.WriteSingle(this.paddingRight, &var_0_cp_0[var_0_cp_1] + 2990);
			SerializedNamedStateWriter.Instance.WriteSingle(this.paddingTop, &var_0_cp_0[var_0_cp_1] + 3003);
			SerializedNamedStateWriter.Instance.WriteSingle(this.paddingBottom, &var_0_cp_0[var_0_cp_1] + 3014);
		}

		public unsafe override void Unity_NamedDeserialize(int depth)
		{
			ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
			byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			int var_0_cp_1 = 0;
			this.name = (arg_1A_0.ReadString(&var_0_cp_0[var_0_cp_1] + 2953) as string);
			if (depth <= 7)
			{
				SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2958);
				this.outer.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
			}
			SerializedNamedStateReader.Instance.Align();
			if (depth <= 7)
			{
				SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 2964);
				this.inner.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
			}
			SerializedNamedStateReader.Instance.Align();
			this.rotated = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2970);
			SerializedNamedStateReader.Instance.Align();
			this.paddingLeft = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2978);
			this.paddingRight = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2990);
			this.paddingTop = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 3003);
			this.paddingBottom = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 3014);
		}
	}

	private enum Coordinates
	{
		Pixels,
		TexCoords
	}

	[CompilerGenerated]
	[System.Serializable]
	private sealed class <>c
	{
		public static readonly UIAtlas.<>c <>9 = new UIAtlas.<>c();

		public static Comparison<UISpriteData> <>9__29_0;

		internal int <SortAlphabetically>b__29_0(UISpriteData s1, UISpriteData s2)
		{
			return s1.name.CompareTo(s2.name);
		}
	}

	[HideInInspector, SerializeField]
	protected internal Material material;

	[HideInInspector, SerializeField]
	protected internal List<UISpriteData> mSprites;

	[HideInInspector, SerializeField]
	protected internal float mPixelSize;

	[HideInInspector, SerializeField]
	protected internal UIAtlas mReplacement;

	[HideInInspector, SerializeField]
	protected internal UIAtlas.Coordinates mCoordinates;

	[HideInInspector, SerializeField]
	protected internal List<UIAtlas.Sprite> sprites;

	private int mPMA;

	private Dictionary<string, int> mSpriteIndices;

	public Material spriteMaterial
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.material;
			}
			return this.mReplacement.spriteMaterial;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteMaterial = value;
				return;
			}
			if (this.material == null)
			{
				this.mPMA = 0;
				this.material = value;
				return;
			}
			this.MarkAsChanged();
			this.mPMA = -1;
			this.material = value;
			this.MarkAsChanged();
		}
	}

	public bool premultipliedAlpha
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material spriteMaterial = this.spriteMaterial;
				this.mPMA = ((spriteMaterial != null && spriteMaterial.shader != null && spriteMaterial.shader.name.Contains("Premultiplied")) ? 1 : 0);
			}
			return this.mPMA == 1;
		}
	}

	public List<UISpriteData> spriteList
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.spriteList;
			}
			if (this.mSprites.Count == 0)
			{
				this.Upgrade();
			}
			return this.mSprites;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteList = value;
				return;
			}
			this.mSprites = value;
		}
	}

	public Texture texture
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.texture;
			}
			if (!(this.material != null))
			{
				return null;
			}
			return this.material.mainTexture;
		}
	}

	public float pixelSize
	{
		get
		{
			if (!(this.mReplacement != null))
			{
				return this.mPixelSize;
			}
			return this.mReplacement.pixelSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.pixelSize = value;
				return;
			}
			float num = Mathf.Clamp(value, 0.25f, 4f);
			if (this.mPixelSize != num)
			{
				this.mPixelSize = num;
				this.MarkAsChanged();
			}
		}
	}

	public UIAtlas replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIAtlas uIAtlas = value;
			if (uIAtlas == this)
			{
				uIAtlas = null;
			}
			if (this.mReplacement != uIAtlas)
			{
				if (uIAtlas != null && uIAtlas.replacement == this)
				{
					uIAtlas.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uIAtlas;
				if (uIAtlas != null)
				{
					this.material = null;
				}
				this.MarkAsChanged();
			}
		}
	}

	public UISpriteData GetSprite(string name)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetSprite(name);
		}
		if (!string.IsNullOrEmpty(name))
		{
			if (this.mSprites.Count == 0)
			{
				this.Upgrade();
			}
			if (this.mSprites.Count == 0)
			{
				return null;
			}
			if (this.mSpriteIndices.Count != this.mSprites.Count)
			{
				this.MarkSpriteListAsChanged();
			}
			int num;
			if (this.mSpriteIndices.TryGetValue(name, out num))
			{
				if (num > -1 && num < this.mSprites.Count)
				{
					return this.mSprites[num];
				}
				this.MarkSpriteListAsChanged();
				if (!this.mSpriteIndices.TryGetValue(name, out num))
				{
					return null;
				}
				return this.mSprites[num];
			}
			else
			{
				int i = 0;
				int count = this.mSprites.Count;
				while (i < count)
				{
					UISpriteData uISpriteData = this.mSprites[i];
					if (!string.IsNullOrEmpty(uISpriteData.name) && name == uISpriteData.name)
					{
						this.MarkSpriteListAsChanged();
						return uISpriteData;
					}
					i++;
				}
			}
		}
		return null;
	}

	public string GetRandomSprite(string startsWith)
	{
		if (this.GetSprite(startsWith) != null)
		{
			return startsWith;
		}
		List<UISpriteData> spriteList = this.spriteList;
		List<string> list = new List<string>();
		foreach (UISpriteData current in spriteList)
		{
			if (current.name.StartsWith(startsWith))
			{
				list.Add(current.name);
			}
		}
		if (list.Count <= 0)
		{
			return null;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public void MarkSpriteListAsChanged()
	{
		this.mSpriteIndices.Clear();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			this.mSpriteIndices[this.mSprites[i].name] = i;
			i++;
		}
	}

	public void SortAlphabetically()
	{
		List<UISpriteData> arg_25_0 = this.mSprites;
		Comparison<UISpriteData> arg_25_1;
		if ((arg_25_1 = UIAtlas.<>c.<>9__29_0) == null)
		{
			arg_25_1 = (UIAtlas.<>c.<>9__29_0 = new Comparison<UISpriteData>(UIAtlas.<>c.<>9.<SortAlphabetically>b__29_0));
		}
		arg_25_0.Sort(arg_25_1);
	}

	public BetterList<string> GetListOfSprites()
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetListOfSprites();
		}
		if (this.mSprites.Count == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			UISpriteData uISpriteData = this.mSprites[i];
			if (uISpriteData != null && !string.IsNullOrEmpty(uISpriteData.name))
			{
				betterList.Add(uISpriteData.name);
			}
			i++;
		}
		return betterList;
	}

	public BetterList<string> GetListOfSprites(string match)
	{
		if (this.mReplacement)
		{
			return this.mReplacement.GetListOfSprites(match);
		}
		if (string.IsNullOrEmpty(match))
		{
			return this.GetListOfSprites();
		}
		if (this.mSprites.Count == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			UISpriteData uISpriteData = this.mSprites[i];
			if (uISpriteData != null && !string.IsNullOrEmpty(uISpriteData.name) && string.Equals(match, uISpriteData.name, 5))
			{
				betterList.Add(uISpriteData.name);
				return betterList;
			}
			i++;
		}
		string[] array = match.Split(new char[]
		{
			' '
		}, 1);
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = array[j].ToLower();
		}
		int k = 0;
		int count2 = this.mSprites.Count;
		while (k < count2)
		{
			UISpriteData uISpriteData2 = this.mSprites[k];
			if (uISpriteData2 != null && !string.IsNullOrEmpty(uISpriteData2.name))
			{
				string text = uISpriteData2.name.ToLower();
				int num = 0;
				for (int l = 0; l < array.Length; l++)
				{
					if (text.Contains(array[l]))
					{
						num++;
					}
				}
				if (num == array.Length)
				{
					betterList.Add(uISpriteData2.name);
				}
			}
			k++;
		}
		return betterList;
	}

	private bool References(UIAtlas atlas)
	{
		return !(atlas == null) && (atlas == this || (this.mReplacement != null && this.mReplacement.References(atlas)));
	}

	public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
	{
		return !(a == null) && !(b == null) && (a == b || a.References(b) || b.References(a));
	}

	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		UISprite[] array = NGUITools.FindActive<UISprite>();
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			UISprite uISprite = array[i];
			if (UIAtlas.CheckIfRelated(this, uISprite.atlas))
			{
				UIAtlas atlas = uISprite.atlas;
				uISprite.atlas = null;
				uISprite.atlas = atlas;
			}
			i++;
		}
		UIFont[] array2 = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
		int j = 0;
		int num2 = array2.Length;
		while (j < num2)
		{
			UIFont uIFont = array2[j];
			if (UIAtlas.CheckIfRelated(this, uIFont.atlas))
			{
				UIAtlas atlas2 = uIFont.atlas;
				uIFont.atlas = null;
				uIFont.atlas = atlas2;
			}
			j++;
		}
		UILabel[] array3 = NGUITools.FindActive<UILabel>();
		int k = 0;
		int num3 = array3.Length;
		while (k < num3)
		{
			UILabel uILabel = array3[k];
			if (uILabel.bitmapFont != null && UIAtlas.CheckIfRelated(this, uILabel.bitmapFont.atlas))
			{
				UIFont bitmapFont = uILabel.bitmapFont;
				uILabel.bitmapFont = null;
				uILabel.bitmapFont = bitmapFont;
			}
			k++;
		}
	}

	private bool Upgrade()
	{
		if (this.mReplacement)
		{
			return this.mReplacement.Upgrade();
		}
		if (this.mSprites.Count == 0 && this.sprites.Count > 0 && this.material)
		{
			Texture mainTexture = this.material.mainTexture;
			int width = (mainTexture != null) ? mainTexture.width : 512;
			int height = (mainTexture != null) ? mainTexture.height : 512;
			for (int i = 0; i < this.sprites.Count; i++)
			{
				UIAtlas.Sprite sprite = this.sprites[i];
				Rect outer = sprite.outer;
				Rect inner = sprite.inner;
				if (this.mCoordinates == UIAtlas.Coordinates.TexCoords)
				{
					NGUIMath.ConvertToPixels(outer, width, height, true);
					NGUIMath.ConvertToPixels(inner, width, height, true);
				}
				UISpriteData uISpriteData = new UISpriteData();
				uISpriteData.name = sprite.name;
				uISpriteData.x = Mathf.RoundToInt(outer.xMin);
				uISpriteData.y = Mathf.RoundToInt(outer.yMin);
				uISpriteData.width = Mathf.RoundToInt(outer.width);
				uISpriteData.height = Mathf.RoundToInt(outer.height);
				uISpriteData.paddingLeft = Mathf.RoundToInt(sprite.paddingLeft * outer.width);
				uISpriteData.paddingRight = Mathf.RoundToInt(sprite.paddingRight * outer.width);
				uISpriteData.paddingBottom = Mathf.RoundToInt(sprite.paddingBottom * outer.height);
				uISpriteData.paddingTop = Mathf.RoundToInt(sprite.paddingTop * outer.height);
				uISpriteData.borderLeft = Mathf.RoundToInt(inner.xMin - outer.xMin);
				uISpriteData.borderRight = Mathf.RoundToInt(outer.xMax - inner.xMax);
				uISpriteData.borderBottom = Mathf.RoundToInt(outer.yMax - inner.yMax);
				uISpriteData.borderTop = Mathf.RoundToInt(inner.yMin - outer.yMin);
				this.mSprites.Add(uISpriteData);
			}
			this.sprites.Clear();
			return true;
		}
		return false;
	}

	public UIAtlas()
	{
		this.mSprites = new List<UISpriteData>();
		this.mPixelSize = 1f;
		this.sprites = new List<UIAtlas.Sprite>();
		this.mPMA = -1;
		this.mSpriteIndices = new Dictionary<string, int>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.material);
		}
		if (depth <= 7)
		{
			if (this.mSprites == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.mSprites.Count);
				for (int i = 0; i < this.mSprites.Count; i++)
				{
					((this.mSprites[i] != null) ? this.mSprites[i] : new UISpriteData()).Unity_Serialize(depth + 1);
				}
			}
		}
		SerializedStateWriter.Instance.WriteSingle(this.mPixelSize);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mReplacement);
		}
		SerializedStateWriter.Instance.WriteInt32((int)this.mCoordinates);
		if (depth <= 7)
		{
			if (this.sprites == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.sprites.Count);
				for (int i = 0; i < this.sprites.Count; i++)
				{
					((this.sprites[i] != null) ? this.sprites[i] : new UIAtlas.Sprite()).Unity_Serialize(depth + 1);
				}
			}
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.material = (SerializedStateReader.Instance.ReadUnityEngineObject() as Material);
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.mSprites = new List<UISpriteData>(num);
			for (int i = 0; i < num; i++)
			{
				UISpriteData uISpriteData = new UISpriteData();
				uISpriteData.Unity_Deserialize(depth + 1);
				this.mSprites.Add(uISpriteData);
			}
		}
		this.mPixelSize = SerializedStateReader.Instance.ReadSingle();
		if (depth <= 7)
		{
			this.mReplacement = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIAtlas);
		}
		this.mCoordinates = (UIAtlas.Coordinates)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.sprites = new List<UIAtlas.Sprite>(num);
			for (int i = 0; i < num; i++)
			{
				UIAtlas.Sprite sprite = new UIAtlas.Sprite();
				sprite.Unity_Deserialize(depth + 1);
				this.sprites.Add(sprite);
			}
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.material != null)
		{
			this.material = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.material) as Material);
		}
		if (depth <= 7)
		{
			if (this.mSprites != null)
			{
				for (int i = 0; i < this.mSprites.Count; i++)
				{
					UISpriteData uISpriteData = this.mSprites[i];
					if (uISpriteData != null)
					{
						uISpriteData.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (this.mReplacement != null)
		{
			this.mReplacement = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mReplacement) as UIAtlas);
		}
		if (depth <= 7)
		{
			if (this.sprites != null)
			{
				for (int i = 0; i < this.sprites.Count; i++)
				{
					UIAtlas.Sprite sprite = this.sprites[i];
					if (sprite != null)
					{
						sprite.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.material;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 3028);
		}
		if (depth <= 7)
		{
			if (this.mSprites == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 3037, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 3037, this.mSprites.Count);
				for (int i = 0; i < this.mSprites.Count; i++)
				{
					UISpriteData arg_AF_0 = (this.mSprites[i] != null) ? this.mSprites[i] : new UISpriteData();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_AF_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		SerializedNamedStateWriter.Instance.WriteSingle(this.mPixelSize, &var_0_cp_0[var_0_cp_1] + 2830);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mReplacement, &var_0_cp_0[var_0_cp_1] + 3046);
		}
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.mCoordinates, &var_0_cp_0[var_0_cp_1] + 3059);
		if (depth <= 7)
		{
			if (this.sprites == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 3072, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 3072, this.sprites.Count);
				for (int i = 0; i < this.sprites.Count; i++)
				{
					UIAtlas.Sprite arg_1B3_0 = (this.sprites[i] != null) ? this.sprites[i] : new UIAtlas.Sprite();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_1B3_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
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
			this.material = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3028) as Material);
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 3037);
			this.mSprites = new List<UISpriteData>(num);
			for (int i = 0; i < num; i++)
			{
				UISpriteData uISpriteData = new UISpriteData();
				UISpriteData arg_6C_0 = uISpriteData;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_6C_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.mSprites.Add(uISpriteData);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		this.mPixelSize = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2830);
		if (depth <= 7)
		{
			this.mReplacement = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3046) as UIAtlas);
		}
		this.mCoordinates = (UIAtlas.Coordinates)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3059);
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 3072);
			this.sprites = new List<UIAtlas.Sprite>(num);
			for (int i = 0; i < num; i++)
			{
				UIAtlas.Sprite sprite = new UIAtlas.Sprite();
				UIAtlas.Sprite arg_130_0 = sprite;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_130_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.sprites.Add(sprite);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
	}

	protected internal UIAtlas(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)instance).material);
	}

	public static void $Set0(object instance, long value)
	{
		((UIAtlas)instance).material = (Material)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get1(object instance)
	{
		return ((UIAtlas)instance).mPixelSize;
	}

	public static void $Set1(object instance, float value)
	{
		((UIAtlas)instance).mPixelSize = value;
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)instance).mReplacement);
	}

	public static void $Set2(object instance, long value)
	{
		((UIAtlas)instance).mReplacement = (UIAtlas)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIAtlas.CheckIfRelated((UIAtlas)GCHandledObjects.GCHandleToObject(*args), (UIAtlas)GCHandledObjects.GCHandleToObject(args[1])));
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).pixelSize);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).premultipliedAlpha);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).replacement);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).spriteList);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).spriteMaterial);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).texture);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).GetListOfSprites());
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).GetListOfSprites(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).GetRandomSprite(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).GetSprite(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).MarkAsChanged();
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).MarkSpriteListAsChanged();
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).References((UIAtlas)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).pixelSize = *(float*)args;
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).replacement = (UIAtlas)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).spriteList = (List<UISpriteData>)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).spriteMaterial = (Material)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).SortAlphabetically();
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIAtlas)GCHandledObjects.GCHandleToObject(instance)).Upgrade());
	}
}
