using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour, IUnitySerializable
{
	public enum Style
	{
		Text,
		Chat
	}

	protected class Paragraph
	{
		public string text;

		public string[] lines;
	}

	public UILabel textLabel;

	public UIProgressBar scrollBar;

	public UITextList.Style style;

	public int paragraphHistory;

	protected char[] mSeparator;

	protected float mScroll;

	protected int mTotalLines;

	protected int mLastWidth;

	protected int mLastHeight;

	private BetterList<UITextList.Paragraph> mParagraphs;

	private static Dictionary<string, BetterList<UITextList.Paragraph>> mHistory = new Dictionary<string, BetterList<UITextList.Paragraph>>();

	protected BetterList<UITextList.Paragraph> paragraphs
	{
		get
		{
			if (this.mParagraphs == null && !UITextList.mHistory.TryGetValue(base.name, out this.mParagraphs))
			{
				this.mParagraphs = new BetterList<UITextList.Paragraph>();
				UITextList.mHistory.Add(base.name, this.mParagraphs);
			}
			return this.mParagraphs;
		}
	}

	public bool isValid
	{
		get
		{
			return this.textLabel != null && this.textLabel.ambigiousFont != null;
		}
	}

	public float scrollValue
	{
		get
		{
			return this.mScroll;
		}
		set
		{
			value = Mathf.Clamp01(value);
			if (this.isValid && this.mScroll != value)
			{
				if (this.scrollBar != null)
				{
					this.scrollBar.value = value;
					return;
				}
				this.mScroll = value;
				this.UpdateVisibleText();
			}
		}
	}

	protected float lineHeight
	{
		get
		{
			if (!(this.textLabel != null))
			{
				return 20f;
			}
			return (float)this.textLabel.fontSize + this.textLabel.effectiveSpacingY;
		}
	}

	protected int scrollHeight
	{
		get
		{
			if (!this.isValid)
			{
				return 0;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / this.lineHeight);
			return Mathf.Max(0, this.mTotalLines - num);
		}
	}

	public void Clear()
	{
		this.paragraphs.Clear();
		this.UpdateVisibleText();
	}

	private void Start()
	{
		if (this.textLabel == null)
		{
			this.textLabel = base.GetComponentInChildren<UILabel>();
		}
		if (this.scrollBar != null)
		{
			EventDelegate.Add(this.scrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
		}
		this.textLabel.overflowMethod = UILabel.Overflow.ClampContent;
		if (this.style == UITextList.Style.Chat)
		{
			this.textLabel.pivot = UIWidget.Pivot.BottomLeft;
			this.scrollValue = 1f;
			return;
		}
		this.textLabel.pivot = UIWidget.Pivot.TopLeft;
		this.scrollValue = 0f;
	}

	private void Update()
	{
		if (this.isValid && (this.textLabel.width != this.mLastWidth || this.textLabel.height != this.mLastHeight))
		{
			this.Rebuild();
		}
	}

	public void OnScroll(float val)
	{
		int scrollHeight = this.scrollHeight;
		if (scrollHeight != 0)
		{
			val *= this.lineHeight;
			this.scrollValue = this.mScroll - val / (float)scrollHeight;
		}
	}

	public void OnDrag(Vector2 delta)
	{
		int scrollHeight = this.scrollHeight;
		if (scrollHeight != 0)
		{
			float num = delta.y / this.lineHeight;
			this.scrollValue = this.mScroll + num / (float)scrollHeight;
		}
	}

	private void OnScrollBar()
	{
		this.mScroll = UIProgressBar.current.value;
		this.UpdateVisibleText();
	}

	public void Add(string text)
	{
		this.Add(text, true);
	}

	protected void Add(string text, bool updateVisible)
	{
		UITextList.Paragraph paragraph;
		if (this.paragraphs.size < this.paragraphHistory)
		{
			paragraph = new UITextList.Paragraph();
		}
		else
		{
			paragraph = this.mParagraphs[0];
			this.mParagraphs.RemoveAt(0);
		}
		paragraph.text = text;
		this.mParagraphs.Add(paragraph);
		this.Rebuild();
	}

	protected void Rebuild()
	{
		if (this.isValid)
		{
			this.mLastWidth = this.textLabel.width;
			this.mLastHeight = this.textLabel.height;
			this.textLabel.UpdateNGUIText();
			NGUIText.rectHeight = 1000000;
			NGUIText.regionHeight = 1000000;
			this.mTotalLines = 0;
			for (int i = 0; i < this.paragraphs.size; i++)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[i];
				string text;
				NGUIText.WrapText(paragraph.text, out text, false, true, false);
				paragraph.lines = text.Split(new char[]
				{
					'\n'
				});
				this.mTotalLines += paragraph.lines.Length;
			}
			this.mTotalLines = 0;
			int j = 0;
			int size = this.mParagraphs.size;
			while (j < size)
			{
				this.mTotalLines += this.mParagraphs.buffer[j].lines.Length;
				j++;
			}
			if (this.scrollBar != null)
			{
				UIScrollBar uIScrollBar = this.scrollBar as UIScrollBar;
				if (uIScrollBar != null)
				{
					uIScrollBar.barSize = ((this.mTotalLines == 0) ? 1f : (1f - (float)this.scrollHeight / (float)this.mTotalLines));
				}
			}
			this.UpdateVisibleText();
		}
	}

	protected void UpdateVisibleText()
	{
		if (this.isValid)
		{
			if (this.mTotalLines == 0)
			{
				this.textLabel.text = "";
				return;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / this.lineHeight);
			int num2 = Mathf.Max(0, this.mTotalLines - num);
			int num3 = Mathf.RoundToInt(this.mScroll * (float)num2);
			if (num3 < 0)
			{
				num3 = 0;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num4 = 0;
			int size = this.paragraphs.size;
			while (num > 0 && num4 < size)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[num4];
				int num5 = 0;
				int num6 = paragraph.lines.Length;
				while (num > 0 && num5 < num6)
				{
					string text = paragraph.lines[num5];
					if (num3 > 0)
					{
						num3--;
					}
					else
					{
						if (stringBuilder.get_Length() > 0)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append(text);
						num--;
					}
					num5++;
				}
				num4++;
			}
			this.textLabel.text = stringBuilder.ToString();
		}
	}

	public UITextList()
	{
		this.paragraphHistory = 100;
		this.mSeparator = new char[]
		{
			'\n'
		};
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.textLabel);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.scrollBar);
		}
		SerializedStateWriter.Instance.WriteInt32((int)this.style);
		SerializedStateWriter.Instance.WriteInt32(this.paragraphHistory);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.textLabel = (SerializedStateReader.Instance.ReadUnityEngineObject() as UILabel);
		}
		if (depth <= 7)
		{
			this.scrollBar = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIProgressBar);
		}
		this.style = (UITextList.Style)SerializedStateReader.Instance.ReadInt32();
		this.paragraphHistory = SerializedStateReader.Instance.ReadInt32();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.textLabel != null)
		{
			this.textLabel = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.textLabel) as UILabel);
		}
		if (this.scrollBar != null)
		{
			this.scrollBar = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.scrollBar) as UIProgressBar);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.textLabel;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 1553);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.scrollBar, &var_0_cp_0[var_0_cp_1] + 4551);
		}
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.style, &var_0_cp_0[var_0_cp_1] + 2693);
		SerializedNamedStateWriter.Instance.WriteInt32(this.paragraphHistory, &var_0_cp_0[var_0_cp_1] + 4561);
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
			this.textLabel = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1553) as UILabel);
		}
		if (depth <= 7)
		{
			this.scrollBar = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 4551) as UIProgressBar);
		}
		this.style = (UITextList.Style)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 2693);
		this.paragraphHistory = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4561);
	}

	protected internal UITextList(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITextList)instance).textLabel);
	}

	public static void $Set0(object instance, long value)
	{
		((UITextList)instance).textLabel = (UILabel)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITextList)instance).scrollBar);
	}

	public static void $Set1(object instance, long value)
	{
		((UITextList)instance).scrollBar = (UIProgressBar)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).Add(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).Add(Marshal.PtrToStringUni(*(IntPtr*)args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITextList)GCHandledObjects.GCHandleToObject(instance)).isValid);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITextList)GCHandledObjects.GCHandleToObject(instance)).lineHeight);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITextList)GCHandledObjects.GCHandleToObject(instance)).paragraphs);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITextList)GCHandledObjects.GCHandleToObject(instance)).scrollHeight);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UITextList)GCHandledObjects.GCHandleToObject(instance)).scrollValue);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).OnScrollBar();
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).Rebuild();
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).scrollValue = *(float*)args;
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UITextList)GCHandledObjects.GCHandleToObject(instance)).UpdateVisibleText();
		return -1L;
	}
}
