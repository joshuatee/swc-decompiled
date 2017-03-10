using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/Input Field")]
public class UIInput : MonoBehaviour, IUnitySerializable
{
	public enum InputType
	{
		Standard,
		AutoCorrect,
		Password
	}

	public enum Validation
	{
		None,
		Integer,
		Float,
		Alphanumeric,
		Username,
		Name,
		Filename
	}

	public enum KeyboardType
	{
		Default,
		ASCIICapable,
		NumbersAndPunctuation,
		URL,
		NumberPad,
		PhonePad,
		NamePhonePad,
		EmailAddress
	}

	public enum OnReturnKey
	{
		Default,
		Submit,
		NewLine
	}

	public delegate char OnValidate(string text, int charIndex, char addedChar);

	public static UIInput current;

	public static UIInput selection;

	public UILabel label;

	public UIInput.InputType inputType;

	public UIInput.OnReturnKey onReturnKey;

	public UIInput.KeyboardType keyboardType;

	public bool hideInput;

	[System.NonSerialized]
	public bool selectAllTextOnFocus;

	public UIInput.Validation validation;

	public int characterLimit;

	public string savedAs;

	[HideInInspector, SerializeField]
	protected internal GameObject selectOnTab;

	public Color activeTextColor;

	public Color caretColor;

	public Color selectionColor;

	public List<EventDelegate> onSubmit;

	public List<EventDelegate> onChange;

	public UIInput.OnValidate onValidate;

	[HideInInspector, SerializeField]
	protected string mValue;

	[System.NonSerialized]
	protected string mDefaultText;

	[System.NonSerialized]
	protected Color mDefaultColor;

	[System.NonSerialized]
	protected float mPosition;

	[System.NonSerialized]
	protected bool mDoInit;

	[System.NonSerialized]
	protected UIWidget.Pivot mPivot;

	[System.NonSerialized]
	protected bool mLoadSavedValue;

	protected static int mDrawStart = 0;

	protected static string mLastIME = "";

	protected static TouchScreenKeyboard mKeyboard;

	private static bool mWaitForKeyboard = false;

	[System.NonSerialized]
	protected int mSelectionStart;

	[System.NonSerialized]
	protected int mSelectionEnd;

	[System.NonSerialized]
	protected UITexture mHighlight;

	[System.NonSerialized]
	protected UITexture mCaret;

	[System.NonSerialized]
	protected Texture2D mBlankTex;

	[System.NonSerialized]
	protected float mNextBlink;

	[System.NonSerialized]
	protected float mLastAlpha;

	[System.NonSerialized]
	protected string mCached;

	[System.NonSerialized]
	protected int mSelectMe;

	[System.NonSerialized]
	protected int mSelectTime;

	[System.NonSerialized]
	private UICamera mCam;

	[System.NonSerialized]
	private bool mEllipsis;

	private static int mIgnoreKey = 0;

	public string defaultText
	{
		get
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			return this.mDefaultText;
		}
		set
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			this.mDefaultText = value;
			this.UpdateLabel();
		}
	}

	public bool inputShouldBeHidden
	{
		get
		{
			return this.hideInput && this.label != null && !this.label.multiLine && this.inputType != UIInput.InputType.Password;
		}
	}

	[Obsolete("Use UIInput.value instead")]
	public string text
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	public string value
	{
		get
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			return this.mValue;
		}
		set
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			UIInput.mDrawStart = 0;
			if (Application.platform == RuntimePlatform.BlackBerryPlayer)
			{
				value = value.Replace("\\b", "\b");
			}
			value = this.Validate(value);
			if (this.isSelected && UIInput.mKeyboard != null && this.mCached != value)
			{
				UIInput.mKeyboard.text = value;
				this.mCached = value;
			}
			if (this.mValue != value)
			{
				this.mValue = value;
				this.mLoadSavedValue = false;
				if (this.isSelected)
				{
					if (string.IsNullOrEmpty(value))
					{
						this.mSelectionStart = 0;
						this.mSelectionEnd = 0;
					}
					else
					{
						this.mSelectionStart = value.get_Length();
						this.mSelectionEnd = this.mSelectionStart;
					}
				}
				else
				{
					this.SaveToPlayerPrefs(value);
				}
				this.UpdateLabel();
				this.ExecuteOnChange();
			}
		}
	}

	[Obsolete("Use UIInput.isSelected instead")]
	public bool selected
	{
		get
		{
			return this.isSelected;
		}
		set
		{
			this.isSelected = value;
		}
	}

	public bool isSelected
	{
		get
		{
			return UIInput.selection == this;
		}
		set
		{
			if (!value)
			{
				if (this.isSelected)
				{
					UICamera.selectedObject = null;
					return;
				}
			}
			else
			{
				UICamera.selectedObject = base.gameObject;
			}
		}
	}

	public int cursorPosition
	{
		get
		{
			if (UIInput.mKeyboard != null && !this.inputShouldBeHidden)
			{
				return this.value.get_Length();
			}
			if (!this.isSelected)
			{
				return this.value.get_Length();
			}
			return this.mSelectionEnd;
		}
		set
		{
			if (this.isSelected)
			{
				if (UIInput.mKeyboard != null && !this.inputShouldBeHidden)
				{
					return;
				}
				this.mSelectionEnd = value;
				this.UpdateLabel();
			}
		}
	}

	public int selectionStart
	{
		get
		{
			if (UIInput.mKeyboard != null && !this.inputShouldBeHidden)
			{
				return 0;
			}
			if (!this.isSelected)
			{
				return this.value.get_Length();
			}
			return this.mSelectionStart;
		}
		set
		{
			if (this.isSelected)
			{
				if (UIInput.mKeyboard != null && !this.inputShouldBeHidden)
				{
					return;
				}
				this.mSelectionStart = value;
				this.UpdateLabel();
			}
		}
	}

	public int selectionEnd
	{
		get
		{
			if (UIInput.mKeyboard != null && !this.inputShouldBeHidden)
			{
				return this.value.get_Length();
			}
			if (!this.isSelected)
			{
				return this.value.get_Length();
			}
			return this.mSelectionEnd;
		}
		set
		{
			if (this.isSelected)
			{
				if (UIInput.mKeyboard != null && !this.inputShouldBeHidden)
				{
					return;
				}
				this.mSelectionEnd = value;
				this.UpdateLabel();
			}
		}
	}

	public UITexture caret
	{
		get
		{
			return this.mCaret;
		}
	}

	public string Validate(string val)
	{
		if (string.IsNullOrEmpty(val))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder(val.get_Length());
		for (int i = 0; i < val.get_Length(); i++)
		{
			char c = val.get_Chars(i);
			if (this.onValidate != null)
			{
				c = this.onValidate(stringBuilder.ToString(), stringBuilder.get_Length(), c);
			}
			else if (this.validation != UIInput.Validation.None)
			{
				c = this.Validate(stringBuilder.ToString(), stringBuilder.get_Length(), c);
			}
			if (c != '\0')
			{
				stringBuilder.Append(c);
			}
		}
		if (this.characterLimit > 0 && stringBuilder.get_Length() > this.characterLimit)
		{
			return stringBuilder.ToString(0, this.characterLimit);
		}
		return stringBuilder.ToString();
	}

	private void Start()
	{
		if (this.selectOnTab != null)
		{
			UIKeyNavigation uIKeyNavigation = base.GetComponent<UIKeyNavigation>();
			if (uIKeyNavigation == null)
			{
				uIKeyNavigation = base.gameObject.AddComponent<UIKeyNavigation>();
				uIKeyNavigation.onDown = this.selectOnTab;
			}
			this.selectOnTab = null;
			NGUITools.SetDirty(this);
		}
		if (this.mLoadSavedValue && !string.IsNullOrEmpty(this.savedAs))
		{
			this.LoadValue();
			return;
		}
		this.value = this.mValue.Replace("\\n", "\n");
	}

	protected void Init()
	{
		if (this.mDoInit && this.label != null)
		{
			this.mDoInit = false;
			this.mDefaultText = this.label.text;
			this.mDefaultColor = this.label.color;
			this.label.supportEncoding = false;
			this.mEllipsis = this.label.overflowEllipsis;
			if (this.label.alignment == NGUIText.Alignment.Justified)
			{
				this.label.alignment = NGUIText.Alignment.Left;
				Debug.LogWarning("Input fields using labels with justified alignment are not supported at this time", this);
			}
			this.mPivot = this.label.pivot;
			this.mPosition = this.label.cachedTransform.localPosition.x;
			this.UpdateLabel();
		}
	}

	protected void SaveToPlayerPrefs(string val)
	{
		if (!string.IsNullOrEmpty(this.savedAs))
		{
			if (string.IsNullOrEmpty(val))
			{
				PlayerPrefs.DeleteKey(this.savedAs);
				return;
			}
			PlayerPrefs.SetString(this.savedAs, val);
		}
	}

	protected virtual void OnSelect(bool isSelected)
	{
		if (isSelected)
		{
			this.OnSelectEvent();
			return;
		}
		this.OnDeselectEvent();
	}

	protected void OnSelectEvent()
	{
		this.mSelectTime = Time.frameCount;
		UIInput.selection = this;
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.label != null)
		{
			this.mEllipsis = this.label.overflowEllipsis;
			this.label.overflowEllipsis = false;
		}
		if (this.label != null && NGUITools.GetActive(this))
		{
			this.mSelectMe = Time.frameCount;
		}
	}

	protected void OnDeselectEvent()
	{
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.label != null)
		{
			this.label.overflowEllipsis = this.mEllipsis;
		}
		if (this.label != null && NGUITools.GetActive(this))
		{
			this.mValue = this.value;
			if (UIInput.mKeyboard != null)
			{
				UIInput.mWaitForKeyboard = false;
				UIInput.mKeyboard.active = false;
				UIInput.mKeyboard = null;
			}
			if (string.IsNullOrEmpty(this.mValue))
			{
				this.label.text = this.mDefaultText;
				this.label.color = this.mDefaultColor;
			}
			else
			{
				this.label.text = this.mValue;
			}
			Input.imeCompositionMode = IMECompositionMode.Auto;
			this.RestoreLabelPivot();
		}
		UIInput.selection = null;
		this.UpdateLabel();
	}

	protected virtual void Update()
	{
		if (!this.isSelected || this.mSelectTime == Time.frameCount)
		{
			return;
		}
		if (this.mDoInit)
		{
			this.Init();
		}
		if (UIInput.mWaitForKeyboard)
		{
			if (UIInput.mKeyboard != null && !UIInput.mKeyboard.active)
			{
				return;
			}
			UIInput.mWaitForKeyboard = false;
		}
		if (this.mSelectMe != -1 && this.mSelectMe != Time.frameCount)
		{
			this.mSelectMe = -1;
			this.mSelectionEnd = (string.IsNullOrEmpty(this.mValue) ? 0 : this.mValue.get_Length());
			UIInput.mDrawStart = 0;
			this.mSelectionStart = (this.selectAllTextOnFocus ? 0 : this.mSelectionEnd);
			this.label.color = this.activeTextColor;
			RuntimePlatform platform = Application.platform;
			if (platform == RuntimePlatform.IPhonePlayer || platform == RuntimePlatform.Android || platform == RuntimePlatform.WP8Player || platform == RuntimePlatform.BlackBerryPlayer || platform == RuntimePlatform.MetroPlayerARM || platform == RuntimePlatform.MetroPlayerX64 || platform == RuntimePlatform.MetroPlayerX86)
			{
				TouchScreenKeyboardType touchScreenKeyboardType;
				string text;
				if (this.inputShouldBeHidden)
				{
					TouchScreenKeyboard.hideInput = true;
					touchScreenKeyboardType = (TouchScreenKeyboardType)this.keyboardType;
					text = "|";
				}
				else if (this.inputType == UIInput.InputType.Password)
				{
					TouchScreenKeyboard.hideInput = false;
					touchScreenKeyboardType = (TouchScreenKeyboardType)this.keyboardType;
					text = this.mValue;
					this.mSelectionStart = this.mSelectionEnd;
				}
				else
				{
					TouchScreenKeyboard.hideInput = false;
					touchScreenKeyboardType = (TouchScreenKeyboardType)this.keyboardType;
					text = this.mValue;
					this.mSelectionStart = this.mSelectionEnd;
				}
				UIInput.mWaitForKeyboard = true;
				UIInput.mKeyboard = ((this.inputType == UIInput.InputType.Password) ? TouchScreenKeyboard.Open(text, touchScreenKeyboardType, false, false, true) : TouchScreenKeyboard.Open(text, touchScreenKeyboardType, !this.inputShouldBeHidden && this.inputType == UIInput.InputType.AutoCorrect, this.label.multiLine && !this.hideInput, false, false, this.defaultText));
				UIInput.mKeyboard.active = true;
			}
			else
			{
				Vector2 vector = (UICamera.current != null && UICamera.current.cachedCamera != null) ? UICamera.current.cachedCamera.WorldToScreenPoint(this.label.worldCorners[0]) : this.label.worldCorners[0];
				vector.y = (float)Screen.height - vector.y;
				Input.imeCompositionMode = IMECompositionMode.On;
				Input.compositionCursorPos = vector;
			}
			this.UpdateLabel();
			if (string.IsNullOrEmpty(Input.inputString))
			{
				return;
			}
		}
		if (UIInput.mKeyboard != null)
		{
			string text2 = (UIInput.mKeyboard.done || !UIInput.mKeyboard.active) ? this.mCached : UIInput.mKeyboard.text;
			if (this.inputShouldBeHidden)
			{
				if (text2 != "|")
				{
					if (!string.IsNullOrEmpty(text2))
					{
						this.Insert(text2.Substring(1));
					}
					else if (!UIInput.mKeyboard.done && UIInput.mKeyboard.active)
					{
						this.DoBackspace();
					}
					UIInput.mKeyboard.text = "|";
				}
			}
			else if (this.mCached != text2)
			{
				this.mCached = text2;
				if (!UIInput.mKeyboard.done && UIInput.mKeyboard.active)
				{
					this.value = text2;
				}
			}
			if (UIInput.mKeyboard.done || !UIInput.mKeyboard.active)
			{
				if (!UIInput.mKeyboard.wasCanceled)
				{
					this.Submit();
				}
				UIInput.mKeyboard = null;
				this.isSelected = false;
				this.mCached = "";
			}
		}
		else
		{
			string compositionString = Input.compositionString;
			if (string.IsNullOrEmpty(compositionString) && !string.IsNullOrEmpty(Input.inputString))
			{
				string inputString = Input.inputString;
				for (int i = 0; i < inputString.get_Length(); i++)
				{
					char c = inputString.get_Chars(i);
					if (c >= ' ' && c != '' && c != '' && c != '' && c != '')
					{
						this.Insert(c.ToString());
					}
				}
			}
			if (UIInput.mLastIME != compositionString)
			{
				this.mSelectionEnd = (string.IsNullOrEmpty(compositionString) ? this.mSelectionStart : (this.mValue.get_Length() + compositionString.get_Length()));
				UIInput.mLastIME = compositionString;
				this.UpdateLabel();
				this.ExecuteOnChange();
			}
		}
		if (this.mCaret != null && this.mNextBlink < RealTime.time)
		{
			this.mNextBlink = RealTime.time + 0.5f;
			this.mCaret.enabled = !this.mCaret.enabled;
		}
		if (this.isSelected && this.mLastAlpha != this.label.finalAlpha)
		{
			this.UpdateLabel();
		}
		if (this.mCam == null)
		{
			this.mCam = UICamera.FindCameraForLayer(base.gameObject.layer);
		}
		if (this.mCam != null)
		{
			if (UICamera.GetKeyDown(this.mCam.submitKey0))
			{
				bool flag = this.onReturnKey == UIInput.OnReturnKey.NewLine || (this.onReturnKey == UIInput.OnReturnKey.Default && this.label.multiLine && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl) && this.label.overflowMethod != UILabel.Overflow.ClampContent && this.validation == UIInput.Validation.None);
				if (flag)
				{
					this.Insert("\n");
				}
				else
				{
					if (UICamera.controller.current != null)
					{
						UICamera.controller.clickNotification = UICamera.ClickNotification.None;
					}
					UICamera.currentKey = this.mCam.submitKey0;
					this.Submit();
				}
			}
			if (UICamera.GetKeyDown(this.mCam.submitKey1))
			{
				bool flag2 = this.onReturnKey == UIInput.OnReturnKey.NewLine || (this.onReturnKey == UIInput.OnReturnKey.Default && this.label.multiLine && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl) && this.label.overflowMethod != UILabel.Overflow.ClampContent && this.validation == UIInput.Validation.None);
				if (flag2)
				{
					this.Insert("\n");
				}
				else
				{
					if (UICamera.controller.current != null)
					{
						UICamera.controller.clickNotification = UICamera.ClickNotification.None;
					}
					UICamera.currentKey = this.mCam.submitKey1;
					this.Submit();
				}
			}
			if (!this.mCam.useKeyboard && UICamera.GetKeyUp(KeyCode.Tab))
			{
				this.OnKey(KeyCode.Tab);
			}
		}
	}

	private void OnKey(KeyCode key)
	{
		int frameCount = Time.frameCount;
		if (UIInput.mIgnoreKey == frameCount)
		{
			return;
		}
		if (this.mCam != null && (key == this.mCam.cancelKey0 || key == this.mCam.cancelKey1))
		{
			UIInput.mIgnoreKey = frameCount;
			this.isSelected = false;
			return;
		}
		if (key == KeyCode.Tab)
		{
			UIInput.mIgnoreKey = frameCount;
			this.isSelected = false;
			UIKeyNavigation component = base.GetComponent<UIKeyNavigation>();
			if (component != null)
			{
				component.OnKey(KeyCode.Tab);
			}
		}
	}

	protected void DoBackspace()
	{
		if (!string.IsNullOrEmpty(this.mValue))
		{
			if (this.mSelectionStart == this.mSelectionEnd)
			{
				if (this.mSelectionStart < 1)
				{
					return;
				}
				this.mSelectionEnd--;
			}
			this.Insert("");
		}
	}

	protected virtual void Insert(string text)
	{
		string leftText = this.GetLeftText();
		string rightText = this.GetRightText();
		int length = rightText.get_Length();
		StringBuilder stringBuilder = new StringBuilder(leftText.get_Length() + rightText.get_Length() + text.get_Length());
		stringBuilder.Append(leftText);
		int i = 0;
		int length2 = text.get_Length();
		while (i < length2)
		{
			char c = text.get_Chars(i);
			if (c == '\b')
			{
				this.DoBackspace();
			}
			else
			{
				if (this.characterLimit > 0 && stringBuilder.get_Length() + length >= this.characterLimit)
				{
					break;
				}
				if (this.onValidate != null)
				{
					c = this.onValidate(stringBuilder.ToString(), stringBuilder.get_Length(), c);
				}
				else if (this.validation != UIInput.Validation.None)
				{
					c = this.Validate(stringBuilder.ToString(), stringBuilder.get_Length(), c);
				}
				if (c != '\0')
				{
					stringBuilder.Append(c);
				}
			}
			i++;
		}
		this.mSelectionStart = stringBuilder.get_Length();
		this.mSelectionEnd = this.mSelectionStart;
		int j = 0;
		int length3 = rightText.get_Length();
		while (j < length3)
		{
			char c2 = rightText.get_Chars(j);
			if (this.onValidate != null)
			{
				c2 = this.onValidate(stringBuilder.ToString(), stringBuilder.get_Length(), c2);
			}
			else if (this.validation != UIInput.Validation.None)
			{
				c2 = this.Validate(stringBuilder.ToString(), stringBuilder.get_Length(), c2);
			}
			if (c2 != '\0')
			{
				stringBuilder.Append(c2);
			}
			j++;
		}
		this.mValue = stringBuilder.ToString();
		this.UpdateLabel();
		this.ExecuteOnChange();
	}

	protected string GetLeftText()
	{
		int num = Mathf.Min(this.mSelectionStart, this.mSelectionEnd);
		if (!string.IsNullOrEmpty(this.mValue) && num >= 0)
		{
			return this.mValue.Substring(0, num);
		}
		return "";
	}

	protected string GetRightText()
	{
		int num = Mathf.Max(this.mSelectionStart, this.mSelectionEnd);
		if (!string.IsNullOrEmpty(this.mValue) && num < this.mValue.get_Length())
		{
			return this.mValue.Substring(num);
		}
		return "";
	}

	protected string GetSelection()
	{
		if (string.IsNullOrEmpty(this.mValue) || this.mSelectionStart == this.mSelectionEnd)
		{
			return "";
		}
		int num = Mathf.Min(this.mSelectionStart, this.mSelectionEnd);
		int num2 = Mathf.Max(this.mSelectionStart, this.mSelectionEnd);
		return this.mValue.Substring(num, num2 - num);
	}

	protected int GetCharUnderMouse()
	{
		Vector3[] worldCorners = this.label.worldCorners;
		Ray currentRay = UICamera.currentRay;
		Plane plane = new Plane(worldCorners[0], worldCorners[1], worldCorners[2]);
		float distance;
		if (!plane.Raycast(currentRay, out distance))
		{
			return 0;
		}
		return UIInput.mDrawStart + this.label.GetCharacterIndexAtPosition(currentRay.GetPoint(distance), false);
	}

	protected virtual void OnPress(bool isPressed)
	{
		if (isPressed && this.isSelected && this.label != null && (UICamera.currentScheme == UICamera.ControlScheme.Mouse || UICamera.currentScheme == UICamera.ControlScheme.Touch))
		{
			this.selectionEnd = this.GetCharUnderMouse();
			if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				this.selectionStart = this.mSelectionEnd;
			}
		}
	}

	protected virtual void OnDrag(Vector2 delta)
	{
		if (this.label != null && (UICamera.currentScheme == UICamera.ControlScheme.Mouse || UICamera.currentScheme == UICamera.ControlScheme.Touch))
		{
			this.selectionEnd = this.GetCharUnderMouse();
		}
	}

	private void OnDisable()
	{
		this.Cleanup();
	}

	protected virtual void Cleanup()
	{
		if (this.mHighlight)
		{
			this.mHighlight.enabled = false;
		}
		if (this.mCaret)
		{
			this.mCaret.enabled = false;
		}
		if (this.mBlankTex)
		{
			NGUITools.Destroy(this.mBlankTex);
			this.mBlankTex = null;
		}
	}

	public void Submit()
	{
		if (NGUITools.GetActive(this))
		{
			this.mValue = this.value;
			if (UIInput.current == null)
			{
				UIInput.current = this;
				EventDelegate.Execute(this.onSubmit);
				UIInput.current = null;
			}
			this.SaveToPlayerPrefs(this.mValue);
		}
	}

	public void UpdateLabel()
	{
		if (this.label != null)
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			bool isSelected = this.isSelected;
			string value = this.value;
			bool flag = string.IsNullOrEmpty(value) && string.IsNullOrEmpty(Input.compositionString);
			this.label.color = ((flag && !isSelected) ? this.mDefaultColor : this.activeTextColor);
			string text;
			if (flag)
			{
				text = (isSelected ? "" : this.mDefaultText);
				this.RestoreLabelPivot();
			}
			else
			{
				if (this.inputType == UIInput.InputType.Password)
				{
					text = "";
					string text2 = "*";
					if (this.label.bitmapFont != null && this.label.bitmapFont.bmFont != null && this.label.bitmapFont.bmFont.GetGlyph(42) == null)
					{
						text2 = "x";
					}
					int i = 0;
					int length = value.get_Length();
					while (i < length)
					{
						text += text2;
						i++;
					}
				}
				else
				{
					text = value;
				}
				int num = isSelected ? Mathf.Min(text.get_Length(), this.cursorPosition) : 0;
				string text3 = text.Substring(0, num);
				if (isSelected)
				{
					text3 += Input.compositionString;
				}
				text = text3 + text.Substring(num, text.get_Length() - num);
				if (isSelected && this.label.overflowMethod == UILabel.Overflow.ClampContent && this.label.maxLineCount == 1)
				{
					int num2 = this.label.CalculateOffsetToFit(text);
					if (num2 == 0)
					{
						UIInput.mDrawStart = 0;
						this.RestoreLabelPivot();
					}
					else if (num < UIInput.mDrawStart)
					{
						UIInput.mDrawStart = num;
						this.SetPivotToLeft();
					}
					else if (num2 < UIInput.mDrawStart)
					{
						UIInput.mDrawStart = num2;
						this.SetPivotToLeft();
					}
					else
					{
						num2 = this.label.CalculateOffsetToFit(text.Substring(0, num));
						if (num2 > UIInput.mDrawStart)
						{
							UIInput.mDrawStart = num2;
							this.SetPivotToRight();
						}
					}
					if (UIInput.mDrawStart != 0)
					{
						text = text.Substring(UIInput.mDrawStart, text.get_Length() - UIInput.mDrawStart);
					}
				}
				else
				{
					UIInput.mDrawStart = 0;
					this.RestoreLabelPivot();
				}
			}
			this.label.text = text;
			if (isSelected && (UIInput.mKeyboard == null || this.inputShouldBeHidden))
			{
				int num3 = this.mSelectionStart - UIInput.mDrawStart;
				int num4 = this.mSelectionEnd - UIInput.mDrawStart;
				if (this.mBlankTex == null)
				{
					this.mBlankTex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
					for (int j = 0; j < 2; j++)
					{
						for (int k = 0; k < 2; k++)
						{
							this.mBlankTex.SetPixel(k, j, Color.white);
						}
					}
					this.mBlankTex.Apply();
				}
				if (num3 != num4)
				{
					if (this.mHighlight == null)
					{
						this.mHighlight = NGUITools.AddWidget<UITexture>(this.label.cachedGameObject, 2147483647);
						this.mHighlight.name = "Input Highlight";
						this.mHighlight.mainTexture = this.mBlankTex;
						this.mHighlight.fillGeometry = false;
						this.mHighlight.pivot = this.label.pivot;
						this.mHighlight.SetAnchor(this.label.cachedTransform);
					}
					else
					{
						this.mHighlight.pivot = this.label.pivot;
						this.mHighlight.mainTexture = this.mBlankTex;
						this.mHighlight.MarkAsChanged();
						this.mHighlight.enabled = true;
					}
				}
				if (this.mCaret == null)
				{
					this.mCaret = NGUITools.AddWidget<UITexture>(this.label.cachedGameObject, 2147483647);
					this.mCaret.name = "Input Caret";
					this.mCaret.mainTexture = this.mBlankTex;
					this.mCaret.fillGeometry = false;
					this.mCaret.pivot = this.label.pivot;
					this.mCaret.SetAnchor(this.label.cachedTransform);
				}
				else
				{
					this.mCaret.pivot = this.label.pivot;
					this.mCaret.mainTexture = this.mBlankTex;
					this.mCaret.MarkAsChanged();
					this.mCaret.enabled = true;
				}
				if (num3 != num4)
				{
					this.label.PrintOverlay(num3, num4, this.mCaret.geometry, this.mHighlight.geometry, this.caretColor, this.selectionColor);
					this.mHighlight.enabled = this.mHighlight.geometry.hasVertices;
				}
				else
				{
					this.label.PrintOverlay(num3, num4, this.mCaret.geometry, null, this.caretColor, this.selectionColor);
					if (this.mHighlight != null)
					{
						this.mHighlight.enabled = false;
					}
				}
				this.mNextBlink = RealTime.time + 0.5f;
				this.mLastAlpha = this.label.finalAlpha;
				return;
			}
			this.Cleanup();
		}
	}

	protected void SetPivotToLeft()
	{
		Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.mPivot);
		pivotOffset.x = 0f;
		this.label.pivot = NGUIMath.GetPivot(pivotOffset);
	}

	protected void SetPivotToRight()
	{
		Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.mPivot);
		pivotOffset.x = 1f;
		this.label.pivot = NGUIMath.GetPivot(pivotOffset);
	}

	protected void RestoreLabelPivot()
	{
		if (this.label != null && this.label.pivot != this.mPivot)
		{
			this.label.pivot = this.mPivot;
		}
	}

	protected char Validate(string text, int pos, char ch)
	{
		if (this.validation == UIInput.Validation.None || !base.enabled)
		{
			return ch;
		}
		if (this.validation == UIInput.Validation.Integer)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (ch == '-' && pos == 0 && !text.Contains("-"))
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Float)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (ch == '-' && pos == 0 && !text.Contains("-"))
			{
				return ch;
			}
			if (ch == '.' && !text.Contains("."))
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Alphanumeric)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return ch;
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return ch;
			}
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Username)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return ch - 'A' + 'a';
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return ch;
			}
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Filename)
		{
			if (ch == ':')
			{
				return '\0';
			}
			if (ch == '/')
			{
				return '\0';
			}
			if (ch == '\\')
			{
				return '\0';
			}
			if (ch == '<')
			{
				return '\0';
			}
			if (ch == '>')
			{
				return '\0';
			}
			if (ch == '|')
			{
				return '\0';
			}
			if (ch == '^')
			{
				return '\0';
			}
			if (ch == '*')
			{
				return '\0';
			}
			if (ch == ';')
			{
				return '\0';
			}
			if (ch == '"')
			{
				return '\0';
			}
			if (ch == '`')
			{
				return '\0';
			}
			if (ch == '\t')
			{
				return '\0';
			}
			if (ch == '\n')
			{
				return '\0';
			}
			return ch;
		}
		else if (this.validation == UIInput.Validation.Name)
		{
			char c = (text.get_Length() > 0) ? text.get_Chars(Mathf.Clamp(pos, 0, text.get_Length() - 1)) : ' ';
			char c2 = (text.get_Length() > 0) ? text.get_Chars(Mathf.Clamp(pos + 1, 0, text.get_Length() - 1)) : '\n';
			if (ch >= 'a' && ch <= 'z')
			{
				if (c == ' ')
				{
					return ch - 'a' + 'A';
				}
				return ch;
			}
			else if (ch >= 'A' && ch <= 'Z')
			{
				if (c != ' ' && c != '\'')
				{
					return ch - 'A' + 'a';
				}
				return ch;
			}
			else if (ch == '\'')
			{
				if (c != ' ' && c != '\'' && c2 != '\'' && !text.Contains("'"))
				{
					return ch;
				}
			}
			else if (ch == ' ' && c != ' ' && c != '\'' && c2 != ' ' && c2 != '\'')
			{
				return ch;
			}
		}
		return '\0';
	}

	protected void ExecuteOnChange()
	{
		if (UIInput.current == null && EventDelegate.IsValid(this.onChange))
		{
			UIInput.current = this;
			EventDelegate.Execute(this.onChange);
			UIInput.current = null;
		}
	}

	public void RemoveFocus()
	{
		this.isSelected = false;
	}

	public void SaveValue()
	{
		this.SaveToPlayerPrefs(this.mValue);
	}

	public void LoadValue()
	{
		if (!string.IsNullOrEmpty(this.savedAs))
		{
			string text = this.mValue.Replace("\\n", "\n");
			this.mValue = "";
			this.value = (PlayerPrefs.HasKey(this.savedAs) ? PlayerPrefs.GetString(this.savedAs) : text);
		}
	}

	public UIInput()
	{
		this.selectAllTextOnFocus = true;
		this.activeTextColor = Color.white;
		this.caretColor = new Color(1f, 1f, 1f, 0.8f);
		this.selectionColor = new Color(1f, 0.8745098f, 0.5529412f, 0.5f);
		this.onSubmit = new List<EventDelegate>();
		this.onChange = new List<EventDelegate>();
		this.mDefaultText = "";
		this.mDefaultColor = Color.white;
		this.mDoInit = true;
		this.mLoadSavedValue = true;
		this.mCached = "";
		this.mSelectMe = -1;
		this.mSelectTime = -1;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.label);
		}
		SerializedStateWriter.Instance.WriteInt32((int)this.inputType);
		SerializedStateWriter.Instance.WriteInt32((int)this.onReturnKey);
		SerializedStateWriter.Instance.WriteInt32((int)this.keyboardType);
		SerializedStateWriter.Instance.WriteBoolean(this.hideInput);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32((int)this.validation);
		SerializedStateWriter.Instance.WriteInt32(this.characterLimit);
		SerializedStateWriter.Instance.WriteString(this.savedAs);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.selectOnTab);
		}
		if (depth <= 7)
		{
			this.activeTextColor.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.caretColor.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.selectionColor.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			if (this.onSubmit == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onSubmit.Count);
				for (int i = 0; i < this.onSubmit.Count; i++)
				{
					((this.onSubmit[i] != null) ? this.onSubmit[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onChange == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onChange.Count);
				for (int i = 0; i < this.onChange.Count; i++)
				{
					((this.onChange[i] != null) ? this.onChange[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
		SerializedStateWriter.Instance.WriteString(this.mValue);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.label = (SerializedStateReader.Instance.ReadUnityEngineObject() as UILabel);
		}
		this.inputType = (UIInput.InputType)SerializedStateReader.Instance.ReadInt32();
		this.onReturnKey = (UIInput.OnReturnKey)SerializedStateReader.Instance.ReadInt32();
		this.keyboardType = (UIInput.KeyboardType)SerializedStateReader.Instance.ReadInt32();
		this.hideInput = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.validation = (UIInput.Validation)SerializedStateReader.Instance.ReadInt32();
		this.characterLimit = SerializedStateReader.Instance.ReadInt32();
		this.savedAs = (SerializedStateReader.Instance.ReadString() as string);
		if (depth <= 7)
		{
			this.selectOnTab = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.activeTextColor.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.caretColor.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.selectionColor.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onSubmit = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				eventDelegate.Unity_Deserialize(depth + 1);
				this.onSubmit.Add(eventDelegate);
			}
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onChange = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate2 = new EventDelegate();
				eventDelegate2.Unity_Deserialize(depth + 1);
				this.onChange.Add(eventDelegate2);
			}
		}
		this.mValue = (SerializedStateReader.Instance.ReadString() as string);
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.label != null)
		{
			this.label = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.label) as UILabel);
		}
		if (this.selectOnTab != null)
		{
			this.selectOnTab = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.selectOnTab) as GameObject);
		}
		if (depth <= 7)
		{
			if (this.onSubmit != null)
			{
				for (int i = 0; i < this.onSubmit.Count; i++)
				{
					EventDelegate eventDelegate = this.onSubmit[i];
					if (eventDelegate != null)
					{
						eventDelegate.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (depth <= 7)
		{
			if (this.onChange != null)
			{
				for (int i = 0; i < this.onChange.Count; i++)
				{
					EventDelegate eventDelegate2 = this.onChange[i];
					if (eventDelegate2 != null)
					{
						eventDelegate2.Unity_RemapPPtrs(depth + 1);
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
			UnityEngine.Object arg_23_1 = this.label;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 3595);
		}
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.inputType, &var_0_cp_0[var_0_cp_1] + 3601);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.onReturnKey, &var_0_cp_0[var_0_cp_1] + 3611);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.keyboardType, &var_0_cp_0[var_0_cp_1] + 3623);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.hideInput, &var_0_cp_0[var_0_cp_1] + 3636);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.validation, &var_0_cp_0[var_0_cp_1] + 3646);
		SerializedNamedStateWriter.Instance.WriteInt32(this.characterLimit, &var_0_cp_0[var_0_cp_1] + 3657);
		SerializedNamedStateWriter.Instance.WriteString(this.savedAs, &var_0_cp_0[var_0_cp_1] + 3672);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.selectOnTab, &var_0_cp_0[var_0_cp_1] + 3680);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3692);
			this.activeTextColor.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3708);
			this.caretColor.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3719);
			this.selectionColor.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			if (this.onSubmit == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 910, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 910, this.onSubmit.Count);
				for (int i = 0; i < this.onSubmit.Count; i++)
				{
					EventDelegate arg_229_0 = (this.onSubmit[i] != null) ? this.onSubmit[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_229_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			if (this.onChange == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1446, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1446, this.onChange.Count);
				for (int i = 0; i < this.onChange.Count; i++)
				{
					EventDelegate arg_2E0_0 = (this.onChange[i] != null) ? this.onChange[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_2E0_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		SerializedNamedStateWriter.Instance.WriteString(this.mValue, &var_0_cp_0[var_0_cp_1] + 1584);
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
			this.label = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3595) as UILabel);
		}
		this.inputType = (UIInput.InputType)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3601);
		this.onReturnKey = (UIInput.OnReturnKey)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3611);
		this.keyboardType = (UIInput.KeyboardType)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3623);
		this.hideInput = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 3636);
		SerializedNamedStateReader.Instance.Align();
		this.validation = (UIInput.Validation)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3646);
		this.characterLimit = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 3657);
		this.savedAs = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 3672) as string);
		if (depth <= 7)
		{
			this.selectOnTab = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 3680) as GameObject);
		}
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3692);
			this.activeTextColor.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3708);
			this.caretColor.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 3719);
			this.selectionColor.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 910);
			this.onSubmit = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_1F0_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_1F0_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onSubmit.Add(eventDelegate);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1446);
			this.onChange = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate2 = new EventDelegate();
				EventDelegate arg_262_0 = eventDelegate2;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_262_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onChange.Add(eventDelegate2);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		this.mValue = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 1584) as string);
	}

	protected internal UIInput(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)instance).label);
	}

	public static void $Set0(object instance, long value)
	{
		((UIInput)instance).label = (UILabel)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get1(object instance)
	{
		return ((UIInput)instance).hideInput;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIInput)instance).hideInput = value;
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)instance).selectOnTab);
	}

	public static void $Set2(object instance, long value)
	{
		((UIInput)instance).selectOnTab = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get3(object instance, int index)
	{
		UIInput expr_06_cp_0 = (UIInput)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.activeTextColor.r;
		case 1:
			return expr_06_cp_0.activeTextColor.g;
		case 2:
			return expr_06_cp_0.activeTextColor.b;
		case 3:
			return expr_06_cp_0.activeTextColor.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set3(object instance, float value, int index)
	{
		UIInput expr_06_cp_0 = (UIInput)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.activeTextColor.r = value;
			return;
		case 1:
			expr_06_cp_0.activeTextColor.g = value;
			return;
		case 2:
			expr_06_cp_0.activeTextColor.b = value;
			return;
		case 3:
			expr_06_cp_0.activeTextColor.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get4(object instance, int index)
	{
		UIInput expr_06_cp_0 = (UIInput)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.caretColor.r;
		case 1:
			return expr_06_cp_0.caretColor.g;
		case 2:
			return expr_06_cp_0.caretColor.b;
		case 3:
			return expr_06_cp_0.caretColor.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set4(object instance, float value, int index)
	{
		UIInput expr_06_cp_0 = (UIInput)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.caretColor.r = value;
			return;
		case 1:
			expr_06_cp_0.caretColor.g = value;
			return;
		case 2:
			expr_06_cp_0.caretColor.b = value;
			return;
		case 3:
			expr_06_cp_0.caretColor.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get5(object instance, int index)
	{
		UIInput expr_06_cp_0 = (UIInput)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.selectionColor.r;
		case 1:
			return expr_06_cp_0.selectionColor.g;
		case 2:
			return expr_06_cp_0.selectionColor.b;
		case 3:
			return expr_06_cp_0.selectionColor.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set5(object instance, float value, int index)
	{
		UIInput expr_06_cp_0 = (UIInput)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.selectionColor.r = value;
			return;
		case 1:
			expr_06_cp_0.selectionColor.g = value;
			return;
		case 2:
			expr_06_cp_0.selectionColor.b = value;
			return;
		case 3:
			expr_06_cp_0.selectionColor.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).Cleanup();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).DoBackspace();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).ExecuteOnChange();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).caret);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).cursorPosition);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).defaultText);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).inputShouldBeHidden);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).isSelected);
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).selected);
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).selectionEnd);
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).selectionStart);
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).text);
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).value);
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).GetCharUnderMouse());
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).GetLeftText());
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).GetRightText());
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).GetSelection());
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).Init();
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).Insert(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).LoadValue();
		return -1L;
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).OnDeselectEvent();
		return -1L;
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).OnKey((KeyCode)(*(int*)args));
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).OnSelect(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).OnSelectEvent();
		return -1L;
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).RemoveFocus();
		return -1L;
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).RestoreLabelPivot();
		return -1L;
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).SaveToPlayerPrefs(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).SaveValue();
		return -1L;
	}

	public unsafe static long $Invoke31(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).cursorPosition = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke32(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).defaultText = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke33(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).isSelected = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke34(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).selected = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke35(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).selectionEnd = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke36(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).selectionStart = *(int*)args;
		return -1L;
	}

	public unsafe static long $Invoke37(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).text = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke38(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).value = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke39(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).SetPivotToLeft();
		return -1L;
	}

	public unsafe static long $Invoke40(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).SetPivotToRight();
		return -1L;
	}

	public unsafe static long $Invoke41(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke42(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).Submit();
		return -1L;
	}

	public unsafe static long $Invoke43(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke44(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke45(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke46(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke47(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke48(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}

	public unsafe static long $Invoke49(long instance, long* args)
	{
		((UIInput)GCHandledObjects.GCHandleToObject(instance)).UpdateLabel();
		return -1L;
	}

	public unsafe static long $Invoke50(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIInput)GCHandledObjects.GCHandleToObject(instance)).Validate(Marshal.PtrToStringUni(*(IntPtr*)args)));
	}
}
