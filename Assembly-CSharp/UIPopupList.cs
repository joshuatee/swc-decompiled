using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Popup List"), ExecuteInEditMode]
public class UIPopupList : UIWidgetContainer, IUnitySerializable
{
	public enum Position
	{
		Auto,
		Above,
		Below
	}

	public enum OpenOn
	{
		ClickOrTap,
		RightClick,
		DoubleClick,
		Manual
	}

	public delegate void LegacyEvent(string val);

	public static UIPopupList current;

	private static GameObject mChild;

	private static float mFadeOutComplete;

	private const float animSpeed = 0.15f;

	public UIAtlas atlas;

	public UIFont bitmapFont;

	public Font trueTypeFont;

	public int fontSize;

	public FontStyle fontStyle;

	public string backgroundSprite;

	public string highlightSprite;

	public UIPopupList.Position position;

	public NGUIText.Alignment alignment;

	public List<string> items;

	public List<object> itemData;

	public Vector2 padding;

	public Color textColor;

	public Color backgroundColor;

	public Color highlightColor;

	public bool isAnimated;

	public bool isLocalized;

	public bool separatePanel;

	public UIPopupList.OpenOn openOn;

	public List<EventDelegate> onChange;

	[HideInInspector, SerializeField]
	protected string mSelectedItem;

	[HideInInspector, SerializeField]
	protected UIPanel mPanel;

	[HideInInspector, SerializeField]
	protected UISprite mBackground;

	[HideInInspector, SerializeField]
	protected UISprite mHighlight;

	[HideInInspector, SerializeField]
	protected UILabel mHighlightedLabel;

	[HideInInspector, SerializeField]
	protected List<UILabel> mLabelList;

	[HideInInspector, SerializeField]
	protected float mBgBorder;

	[System.NonSerialized]
	protected GameObject mSelection;

	[System.NonSerialized]
	protected int mOpenFrame;

	[HideInInspector, SerializeField]
	protected internal GameObject eventReceiver;

	[HideInInspector, SerializeField]
	protected internal string functionName;

	[HideInInspector, SerializeField]
	protected internal float textScale;

	[HideInInspector, SerializeField]
	protected internal UIFont font;

	[HideInInspector, SerializeField]
	protected internal UILabel textLabel;

	private UIPopupList.LegacyEvent mLegacyEvent;

	[System.NonSerialized]
	protected bool mExecuting;

	protected bool mUseDynamicFont;

	protected bool mTweening;

	public GameObject source;

	public UnityEngine.Object ambigiousFont
	{
		get
		{
			if (this.trueTypeFont != null)
			{
				return this.trueTypeFont;
			}
			if (this.bitmapFont != null)
			{
				return this.bitmapFont;
			}
			return this.font;
		}
		set
		{
			if (value is Font)
			{
				this.trueTypeFont = (value as Font);
				this.bitmapFont = null;
				this.font = null;
				return;
			}
			if (value is UIFont)
			{
				this.bitmapFont = (value as UIFont);
				this.trueTypeFont = null;
				this.font = null;
			}
		}
	}

	[Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
	public UIPopupList.LegacyEvent onSelectionChange
	{
		get
		{
			return this.mLegacyEvent;
		}
		set
		{
			this.mLegacyEvent = value;
		}
	}

	public static bool isOpen
	{
		get
		{
			return UIPopupList.current != null && (UIPopupList.mChild != null || UIPopupList.mFadeOutComplete > Time.unscaledTime);
		}
	}

	public virtual string value
	{
		get
		{
			return this.mSelectedItem;
		}
		set
		{
			this.mSelectedItem = value;
			if (this.mSelectedItem == null)
			{
				return;
			}
			if (this.mSelectedItem != null)
			{
				this.TriggerCallbacks();
			}
		}
	}

	public virtual object data
	{
		get
		{
			int num = this.items.IndexOf(this.mSelectedItem);
			if (num <= -1 || num >= this.itemData.Count)
			{
				return null;
			}
			return this.itemData[num];
		}
	}

	public bool isColliderEnabled
	{
		get
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 != null && component2.enabled;
		}
	}

	[Obsolete("Use 'value' instead")]
	public string selection
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

	private bool isValid
	{
		get
		{
			return this.bitmapFont != null || this.trueTypeFont != null;
		}
	}

	private int activeFontSize
	{
		get
		{
			if (!(this.trueTypeFont != null) && !(this.bitmapFont == null))
			{
				return this.bitmapFont.defaultSize;
			}
			return this.fontSize;
		}
	}

	private float activeFontScale
	{
		get
		{
			if (!(this.trueTypeFont != null) && !(this.bitmapFont == null))
			{
				return (float)this.fontSize / (float)this.bitmapFont.defaultSize;
			}
			return 1f;
		}
	}

	public virtual void Clear()
	{
		this.items.Clear();
		this.itemData.Clear();
	}

	public virtual void AddItem(string text)
	{
		this.items.Add(text);
		this.itemData.Add(null);
	}

	public virtual void AddItem(string text, object data)
	{
		this.items.Add(text);
		this.itemData.Add(data);
	}

	public virtual void RemoveItem(string text)
	{
		int num = this.items.IndexOf(text);
		if (num != -1)
		{
			this.items.RemoveAt(num);
			this.itemData.RemoveAt(num);
		}
	}

	public virtual void RemoveItemByData(object data)
	{
		int num = this.itemData.IndexOf(data);
		if (num != -1)
		{
			this.items.RemoveAt(num);
			this.itemData.RemoveAt(num);
		}
	}

	protected void TriggerCallbacks()
	{
		if (!this.mExecuting)
		{
			this.mExecuting = true;
			UIPopupList uIPopupList = UIPopupList.current;
			UIPopupList.current = this;
			if (this.mLegacyEvent != null)
			{
				this.mLegacyEvent(this.mSelectedItem);
			}
			if (EventDelegate.IsValid(this.onChange))
			{
				EventDelegate.Execute(this.onChange);
			}
			else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
			{
				this.eventReceiver.SendMessage(this.functionName, this.mSelectedItem, SendMessageOptions.DontRequireReceiver);
			}
			UIPopupList.current = uIPopupList;
			this.mExecuting = false;
		}
	}

	protected virtual void OnEnable()
	{
		if (EventDelegate.IsValid(this.onChange))
		{
			this.eventReceiver = null;
			this.functionName = null;
		}
		if (this.font != null)
		{
			if (this.font.isDynamic)
			{
				this.trueTypeFont = this.font.dynamicFont;
				this.fontStyle = this.font.dynamicFontStyle;
				this.mUseDynamicFont = true;
			}
			else if (this.bitmapFont == null)
			{
				this.bitmapFont = this.font;
				this.mUseDynamicFont = false;
			}
			this.font = null;
		}
		if (this.textScale != 0f)
		{
			this.fontSize = ((this.bitmapFont != null) ? Mathf.RoundToInt((float)this.bitmapFont.defaultSize * this.textScale) : 16);
			this.textScale = 0f;
		}
		if (this.trueTypeFont == null && this.bitmapFont != null && this.bitmapFont.isDynamic)
		{
			this.trueTypeFont = this.bitmapFont.dynamicFont;
			this.bitmapFont = null;
		}
	}

	protected virtual void OnValidate()
	{
		Font x = this.trueTypeFont;
		UIFont uIFont = this.bitmapFont;
		this.bitmapFont = null;
		this.trueTypeFont = null;
		if (x != null && (uIFont == null || !this.mUseDynamicFont))
		{
			this.bitmapFont = null;
			this.trueTypeFont = x;
			this.mUseDynamicFont = true;
			return;
		}
		if (!(uIFont != null))
		{
			this.trueTypeFont = x;
			this.mUseDynamicFont = true;
			return;
		}
		if (uIFont.isDynamic)
		{
			this.trueTypeFont = uIFont.dynamicFont;
			this.fontStyle = uIFont.dynamicFontStyle;
			this.fontSize = uIFont.defaultSize;
			this.mUseDynamicFont = true;
			return;
		}
		this.bitmapFont = uIFont;
		this.mUseDynamicFont = false;
	}

	protected virtual void Start()
	{
		if (this.textLabel != null)
		{
			EventDelegate.Add(this.onChange, new EventDelegate.Callback(this.textLabel.SetCurrentSelection));
			this.textLabel = null;
		}
		if (Application.isPlaying)
		{
			if (string.IsNullOrEmpty(this.mSelectedItem) && this.items.Count > 0)
			{
				this.mSelectedItem = this.items[0];
			}
			if (!string.IsNullOrEmpty(this.mSelectedItem))
			{
				this.TriggerCallbacks();
			}
		}
	}

	protected virtual void OnLocalize()
	{
		if (this.isLocalized)
		{
			this.TriggerCallbacks();
		}
	}

	protected virtual void Highlight(UILabel lbl, bool instant)
	{
		if (this.mHighlight != null)
		{
			this.mHighlightedLabel = lbl;
			if (this.mHighlight.GetAtlasSprite() == null)
			{
				return;
			}
			Vector3 highlightPosition = this.GetHighlightPosition();
			if (!instant && this.isAnimated)
			{
				TweenPosition.Begin(this.mHighlight.gameObject, 0.1f, highlightPosition).method = UITweener.Method.EaseOut;
				if (!this.mTweening)
				{
					this.mTweening = true;
					base.StartCoroutine("UpdateTweenPosition");
					return;
				}
			}
			else
			{
				this.mHighlight.cachedTransform.localPosition = highlightPosition;
			}
		}
	}

	protected virtual Vector3 GetHighlightPosition()
	{
		if (this.mHighlightedLabel == null || this.mHighlight == null)
		{
			return Vector3.zero;
		}
		UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return Vector3.zero;
		}
		float pixelSize = this.atlas.pixelSize;
		float num = (float)atlasSprite.borderLeft * pixelSize;
		float y = (float)atlasSprite.borderTop * pixelSize;
		return this.mHighlightedLabel.cachedTransform.localPosition + new Vector3(-num, y, 1f);
	}

	[IteratorStateMachine(typeof(UIPopupList.<UpdateTweenPosition>d__81))]
	protected virtual IEnumerator UpdateTweenPosition()
	{
		if (this.mHighlight != null && this.mHighlightedLabel != null)
		{
			TweenPosition tweenPosition = this.mHighlight.GetComponent<TweenPosition>();
			while (tweenPosition != null && tweenPosition.enabled)
			{
				tweenPosition.to = this.GetHighlightPosition();
				yield return null;
			}
			tweenPosition = null;
		}
		this.mTweening = false;
		yield break;
	}

	protected virtual void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			UILabel component = go.GetComponent<UILabel>();
			this.Highlight(component, false);
		}
	}

	protected virtual void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			this.Select(go.GetComponent<UILabel>(), true);
			UIEventListener component = go.GetComponent<UIEventListener>();
			this.value = (component.parameter as string);
			UIPlaySound[] components = base.GetComponents<UIPlaySound>();
			int i = 0;
			int num = components.Length;
			while (i < num)
			{
				UIPlaySound uIPlaySound = components[i];
				if (uIPlaySound.trigger == UIPlaySound.Trigger.OnClick)
				{
					NGUITools.PlaySound(uIPlaySound.audioClip, uIPlaySound.volume, 1f);
				}
				i++;
			}
			this.CloseSelf();
		}
	}

	private void Select(UILabel lbl, bool instant)
	{
		this.Highlight(lbl, instant);
	}

	protected virtual void OnNavigate(KeyCode key)
	{
		if (base.enabled && UIPopupList.current == this)
		{
			int num = this.mLabelList.IndexOf(this.mHighlightedLabel);
			if (num == -1)
			{
				num = 0;
			}
			if (key == KeyCode.UpArrow)
			{
				if (num > 0)
				{
					this.Select(this.mLabelList[num - 1], false);
					return;
				}
			}
			else if (key == KeyCode.DownArrow && num + 1 < this.mLabelList.Count)
			{
				this.Select(this.mLabelList[num + 1], false);
			}
		}
	}

	protected virtual void OnKey(KeyCode key)
	{
		if (base.enabled && UIPopupList.current == this && (key == UICamera.current.cancelKey0 || key == UICamera.current.cancelKey1))
		{
			this.OnSelect(false);
		}
	}

	protected virtual void OnDisable()
	{
		this.CloseSelf();
	}

	protected virtual void OnSelect(bool isSelected)
	{
		if (!isSelected)
		{
			this.CloseSelf();
		}
	}

	public static void Close()
	{
		if (UIPopupList.current != null)
		{
			UIPopupList.current.CloseSelf();
			UIPopupList.current = null;
		}
	}

	public virtual void CloseSelf()
	{
		if (UIPopupList.mChild != null && UIPopupList.current == this)
		{
			base.StopCoroutine("CloseIfUnselected");
			this.mSelection = null;
			this.mLabelList.Clear();
			if (this.isAnimated)
			{
				UIWidget[] componentsInChildren = UIPopupList.mChild.GetComponentsInChildren<UIWidget>();
				int i = 0;
				int num = componentsInChildren.Length;
				while (i < num)
				{
					UIWidget uIWidget = componentsInChildren[i];
					Color color = uIWidget.color;
					color.a = 0f;
					TweenColor.Begin(uIWidget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
					i++;
				}
				Collider[] componentsInChildren2 = UIPopupList.mChild.GetComponentsInChildren<Collider>();
				int j = 0;
				int num2 = componentsInChildren2.Length;
				while (j < num2)
				{
					componentsInChildren2[j].enabled = false;
					j++;
				}
				UnityEngine.Object.Destroy(UIPopupList.mChild, 0.15f);
				UIPopupList.mFadeOutComplete = Time.unscaledTime + Mathf.Max(0.1f, 0.15f);
			}
			else
			{
				UnityEngine.Object.Destroy(UIPopupList.mChild);
				UIPopupList.mFadeOutComplete = Time.unscaledTime + 0.1f;
			}
			this.mBackground = null;
			this.mHighlight = null;
			UIPopupList.mChild = null;
			UIPopupList.current = null;
		}
	}

	protected virtual void AnimateColor(UIWidget widget)
	{
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
	}

	protected virtual void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 localPosition = widget.cachedTransform.localPosition;
		Vector3 localPosition2 = placeAbove ? new Vector3(localPosition.x, bottom, localPosition.z) : new Vector3(localPosition.x, 0f, localPosition.z);
		widget.cachedTransform.localPosition = localPosition2;
		GameObject gameObject = widget.gameObject;
		TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
	}

	protected virtual void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject gameObject = widget.gameObject;
		Transform cachedTransform = widget.cachedTransform;
		float num = (float)this.activeFontSize * this.activeFontScale + this.mBgBorder * 2f;
		cachedTransform.localScale = new Vector3(1f, num / (float)widget.height, 1f);
		TweenScale.Begin(gameObject, 0.15f, Vector3.one).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 localPosition = cachedTransform.localPosition;
			cachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y - (float)widget.height + num, localPosition.z);
			TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
		}
	}

	private void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		this.AnimateColor(widget);
		this.AnimatePosition(widget, placeAbove, bottom);
	}

	protected virtual void OnClick()
	{
		if (this.mOpenFrame == Time.frameCount)
		{
			return;
		}
		if (!(UIPopupList.mChild == null))
		{
			if (this.mHighlightedLabel != null)
			{
				this.OnItemPress(this.mHighlightedLabel.gameObject, true);
			}
			return;
		}
		if (this.openOn == UIPopupList.OpenOn.DoubleClick || this.openOn == UIPopupList.OpenOn.Manual)
		{
			return;
		}
		if (this.openOn == UIPopupList.OpenOn.RightClick && UICamera.currentTouchID != -2)
		{
			return;
		}
		this.Show();
	}

	protected virtual void OnDoubleClick()
	{
		if (this.openOn == UIPopupList.OpenOn.DoubleClick)
		{
			this.Show();
		}
	}

	[IteratorStateMachine(typeof(UIPopupList.<CloseIfUnselected>d__97))]
	private IEnumerator CloseIfUnselected()
	{
		do
		{
			yield return null;
		}
		while (!(UICamera.selectedObject != this.mSelection));
		this.CloseSelf();
		yield break;
	}

	public virtual void Show()
	{
		if (!base.enabled || !NGUITools.GetActive(base.gameObject) || !(UIPopupList.mChild == null) || !(this.atlas != null) || !this.isValid || this.items.Count <= 0)
		{
			this.OnSelect(false);
			return;
		}
		this.mLabelList.Clear();
		base.StopCoroutine("CloseIfUnselected");
		UICamera.selectedObject = (UICamera.hoveredObject ?? base.gameObject);
		this.mSelection = UICamera.selectedObject;
		this.source = UICamera.selectedObject;
		if (this.source == null)
		{
			Debug.LogError("Popup list needs a source object...");
			return;
		}
		this.mOpenFrame = Time.frameCount;
		if (this.mPanel == null)
		{
			this.mPanel = UIPanel.Find(base.transform);
			if (this.mPanel == null)
			{
				return;
			}
		}
		UIPopupList.mChild = new GameObject("Drop-down List");
		UIPopupList.mChild.layer = base.gameObject.layer;
		if (this.separatePanel)
		{
			if (base.GetComponent<Collider>() != null)
			{
				Rigidbody rigidbody = UIPopupList.mChild.AddComponent<Rigidbody>();
				rigidbody.isKinematic = true;
			}
			else if (base.GetComponent<Collider2D>() != null)
			{
				Rigidbody2D rigidbody2D = UIPopupList.mChild.AddComponent<Rigidbody2D>();
				rigidbody2D.isKinematic = true;
			}
			UIPopupList.mChild.AddComponent<UIPanel>().depth = 1000000;
		}
		UIPopupList.current = this;
		Transform transform = UIPopupList.mChild.transform;
		transform.parent = this.mPanel.cachedTransform;
		Vector3 vector;
		Vector3 vector2;
		Vector3 vector3;
		if (this.openOn == UIPopupList.OpenOn.Manual && this.mSelection != base.gameObject)
		{
			vector = UICamera.lastEventPosition;
			vector2 = this.mPanel.cachedTransform.InverseTransformPoint(this.mPanel.anchorCamera.ScreenToWorldPoint(vector));
			vector3 = vector2;
			transform.localPosition = vector2;
			vector = transform.position;
		}
		else
		{
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(this.mPanel.cachedTransform, base.transform, false, false);
			vector2 = bounds.min;
			vector3 = bounds.max;
			transform.localPosition = vector2;
			vector = transform.position;
		}
		base.StartCoroutine("CloseIfUnselected");
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		this.mBackground = NGUITools.AddSprite(UIPopupList.mChild, this.atlas, this.backgroundSprite, this.separatePanel ? 0 : NGUITools.CalculateNextDepth(this.mPanel.gameObject));
		this.mBackground.pivot = UIWidget.Pivot.TopLeft;
		this.mBackground.color = this.backgroundColor;
		Vector4 border = this.mBackground.border;
		this.mBgBorder = border.y;
		this.mBackground.cachedTransform.localPosition = new Vector3(0f, border.y, 0f);
		this.mHighlight = NGUITools.AddSprite(UIPopupList.mChild, this.atlas, this.highlightSprite, this.mBackground.depth + 1);
		this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
		this.mHighlight.color = this.highlightColor;
		UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return;
		}
		float num = (float)atlasSprite.borderTop;
		float num2 = (float)this.activeFontSize;
		float activeFontScale = this.activeFontScale;
		float num3 = num2 * activeFontScale;
		float num4 = 0f;
		float num5 = -this.padding.y;
		List<UILabel> list = new List<UILabel>();
		if (!this.items.Contains(this.mSelectedItem))
		{
			this.mSelectedItem = null;
		}
		int i = 0;
		int count = this.items.Count;
		while (i < count)
		{
			string text = this.items[i];
			UILabel uILabel = NGUITools.AddWidget<UILabel>(UIPopupList.mChild, this.mBackground.depth + 2);
			uILabel.name = i.ToString();
			uILabel.pivot = UIWidget.Pivot.TopLeft;
			uILabel.bitmapFont = this.bitmapFont;
			uILabel.trueTypeFont = this.trueTypeFont;
			uILabel.fontSize = this.fontSize;
			uILabel.fontStyle = this.fontStyle;
			uILabel.text = (this.isLocalized ? Localization.Get(text) : text);
			uILabel.color = this.textColor;
			uILabel.cachedTransform.localPosition = new Vector3(border.x + this.padding.x - uILabel.pivotOffset.x, num5, -1f);
			uILabel.overflowMethod = UILabel.Overflow.ResizeFreely;
			uILabel.alignment = this.alignment;
			list.Add(uILabel);
			num5 -= num3;
			num5 -= this.padding.y;
			num4 = Mathf.Max(num4, uILabel.printedSize.x);
			UIEventListener uIEventListener = UIEventListener.Get(uILabel.gameObject);
			uIEventListener.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
			uIEventListener.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
			uIEventListener.parameter = text;
			if (this.mSelectedItem == text || (i == 0 && string.IsNullOrEmpty(this.mSelectedItem)))
			{
				this.Highlight(uILabel, true);
			}
			this.mLabelList.Add(uILabel);
			i++;
		}
		num4 = Mathf.Max(num4, vector3.x - vector2.x - (border.x + this.padding.x) * 2f);
		float num6 = num4;
		Vector3 vector4 = new Vector3(num6 * 0.5f, -num3 * 0.5f, 0f);
		Vector3 vector5 = new Vector3(num6, num3 + this.padding.y, 1f);
		int j = 0;
		int count2 = list.Count;
		while (j < count2)
		{
			UILabel uILabel2 = list[j];
			NGUITools.AddWidgetCollider(uILabel2.gameObject);
			uILabel2.autoResizeBoxCollider = false;
			BoxCollider component = uILabel2.GetComponent<BoxCollider>();
			if (component != null)
			{
				vector4.z = component.center.z;
				component.center = vector4;
				component.size = vector5;
			}
			else
			{
				BoxCollider2D component2 = uILabel2.GetComponent<BoxCollider2D>();
				component2.offset = vector4;
				component2.size = vector5;
			}
			j++;
		}
		int width = Mathf.RoundToInt(num4);
		num4 += (border.x + this.padding.x) * 2f;
		num5 -= border.y;
		this.mBackground.width = Mathf.RoundToInt(num4);
		this.mBackground.height = Mathf.RoundToInt(-num5 + border.y);
		int k = 0;
		int count3 = list.Count;
		while (k < count3)
		{
			UILabel uILabel3 = list[k];
			uILabel3.overflowMethod = UILabel.Overflow.ShrinkContent;
			uILabel3.width = width;
			k++;
		}
		float num7 = 2f * this.atlas.pixelSize;
		float f = num4 - (border.x + this.padding.x) * 2f + (float)atlasSprite.borderLeft * num7;
		float f2 = num3 + num * num7;
		this.mHighlight.width = Mathf.RoundToInt(f);
		this.mHighlight.height = Mathf.RoundToInt(f2);
		bool flag = this.position == UIPopupList.Position.Above;
		if (this.position == UIPopupList.Position.Auto)
		{
			UICamera uICamera = UICamera.FindCameraForLayer(this.mSelection.layer);
			if (uICamera != null)
			{
				Vector3 vector6 = uICamera.cachedCamera.WorldToViewportPoint(vector);
				flag = (vector6.y < 0.5f);
			}
		}
		if (this.isAnimated)
		{
			this.AnimateColor(this.mBackground);
			if (Time.timeScale == 0f || Time.timeScale >= 0.1f)
			{
				float bottom = num5 + num3;
				this.Animate(this.mHighlight, flag, bottom);
				int l = 0;
				int count4 = list.Count;
				while (l < count4)
				{
					this.Animate(list[l], flag, bottom);
					l++;
				}
				this.AnimateScale(this.mBackground, flag, bottom);
			}
		}
		if (flag)
		{
			vector2.y = vector3.y - border.y;
			vector3.y = vector2.y + (float)this.mBackground.height;
			vector3.x = vector2.x + (float)this.mBackground.width;
			transform.localPosition = new Vector3(vector2.x, vector3.y - border.y, vector2.z);
		}
		else
		{
			vector3.y = vector2.y + border.y;
			vector2.y = vector3.y - (float)this.mBackground.height;
			vector3.x = vector2.x + (float)this.mBackground.width;
		}
		Transform parent = this.mPanel.cachedTransform.parent;
		if (parent != null)
		{
			vector2 = this.mPanel.cachedTransform.TransformPoint(vector2);
			vector3 = this.mPanel.cachedTransform.TransformPoint(vector3);
			vector2 = parent.InverseTransformPoint(vector2);
			vector3 = parent.InverseTransformPoint(vector3);
		}
		Vector3 b = this.mPanel.hasClipping ? Vector3.zero : this.mPanel.CalculateConstrainOffset(vector2, vector3);
		vector = transform.localPosition + b;
		vector.x = Mathf.Round(vector.x);
		vector.y = Mathf.Round(vector.y);
		transform.localPosition = vector;
	}

	public UIPopupList()
	{
		this.fontSize = 16;
		this.alignment = NGUIText.Alignment.Left;
		this.items = new List<string>();
		this.itemData = new List<object>();
		this.padding = new Vector3(4f, 4f);
		this.textColor = Color.white;
		this.backgroundColor = Color.white;
		this.highlightColor = new Color(0.882352948f, 0.784313738f, 0.5882353f, 1f);
		this.isAnimated = true;
		this.separatePanel = true;
		this.onChange = new List<EventDelegate>();
		this.mLabelList = new List<UILabel>();
		this.functionName = "OnSelectionChange";
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.atlas);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.bitmapFont);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.trueTypeFont);
		}
		SerializedStateWriter.Instance.WriteInt32(this.fontSize);
		SerializedStateWriter.Instance.WriteInt32((int)this.fontStyle);
		SerializedStateWriter.Instance.WriteString(this.backgroundSprite);
		SerializedStateWriter.Instance.WriteString(this.highlightSprite);
		SerializedStateWriter.Instance.WriteInt32((int)this.position);
		SerializedStateWriter.Instance.WriteInt32((int)this.alignment);
		if (depth <= 7)
		{
			if (this.items == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.items.Count);
				for (int i = 0; i < this.items.Count; i++)
				{
					SerializedStateWriter.Instance.WriteString(this.items[i]);
				}
			}
		}
		if (depth <= 7)
		{
			this.padding.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.textColor.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.backgroundColor.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.highlightColor.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.isAnimated);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.isLocalized);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.separatePanel);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteInt32((int)this.openOn);
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
		SerializedStateWriter.Instance.WriteString(this.mSelectedItem);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mPanel);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mBackground);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mHighlight);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.mHighlightedLabel);
		}
		if (depth <= 7)
		{
			if (this.mLabelList == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.mLabelList.Count);
				for (int i = 0; i < this.mLabelList.Count; i++)
				{
					SerializedStateWriter.Instance.WriteUnityEngineObject(this.mLabelList[i]);
				}
			}
		}
		SerializedStateWriter.Instance.WriteSingle(this.mBgBorder);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.eventReceiver);
		}
		SerializedStateWriter.Instance.WriteString(this.functionName);
		SerializedStateWriter.Instance.WriteSingle(this.textScale);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.font);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.textLabel);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.source);
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.atlas = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIAtlas);
		}
		if (depth <= 7)
		{
			this.bitmapFont = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIFont);
		}
		if (depth <= 7)
		{
			this.trueTypeFont = (SerializedStateReader.Instance.ReadUnityEngineObject() as Font);
		}
		this.fontSize = SerializedStateReader.Instance.ReadInt32();
		this.fontStyle = (FontStyle)SerializedStateReader.Instance.ReadInt32();
		this.backgroundSprite = (SerializedStateReader.Instance.ReadString() as string);
		this.highlightSprite = (SerializedStateReader.Instance.ReadString() as string);
		this.position = (UIPopupList.Position)SerializedStateReader.Instance.ReadInt32();
		this.alignment = (NGUIText.Alignment)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.items = new List<string>(num);
			for (int i = 0; i < num; i++)
			{
				this.items.Add(SerializedStateReader.Instance.ReadString() as string);
			}
		}
		if (depth <= 7)
		{
			this.padding.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.textColor.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.backgroundColor.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.highlightColor.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.isAnimated = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.isLocalized = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.separatePanel = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.openOn = (UIPopupList.OpenOn)SerializedStateReader.Instance.ReadInt32();
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onChange = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				eventDelegate.Unity_Deserialize(depth + 1);
				this.onChange.Add(eventDelegate);
			}
		}
		this.mSelectedItem = (SerializedStateReader.Instance.ReadString() as string);
		if (depth <= 7)
		{
			this.mPanel = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIPanel);
		}
		if (depth <= 7)
		{
			this.mBackground = (SerializedStateReader.Instance.ReadUnityEngineObject() as UISprite);
		}
		if (depth <= 7)
		{
			this.mHighlight = (SerializedStateReader.Instance.ReadUnityEngineObject() as UISprite);
		}
		if (depth <= 7)
		{
			this.mHighlightedLabel = (SerializedStateReader.Instance.ReadUnityEngineObject() as UILabel);
		}
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.mLabelList = new List<UILabel>(num);
			for (int i = 0; i < num; i++)
			{
				this.mLabelList.Add(SerializedStateReader.Instance.ReadUnityEngineObject() as UILabel);
			}
		}
		this.mBgBorder = SerializedStateReader.Instance.ReadSingle();
		if (depth <= 7)
		{
			this.eventReceiver = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.functionName = (SerializedStateReader.Instance.ReadString() as string);
		this.textScale = SerializedStateReader.Instance.ReadSingle();
		if (depth <= 7)
		{
			this.font = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIFont);
		}
		if (depth <= 7)
		{
			this.textLabel = (SerializedStateReader.Instance.ReadUnityEngineObject() as UILabel);
		}
		if (depth <= 7)
		{
			this.source = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.atlas != null)
		{
			this.atlas = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.atlas) as UIAtlas);
		}
		if (this.bitmapFont != null)
		{
			this.bitmapFont = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.bitmapFont) as UIFont);
		}
		if (this.trueTypeFont != null)
		{
			this.trueTypeFont = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.trueTypeFont) as Font);
		}
		if (depth <= 7)
		{
			if (this.onChange != null)
			{
				for (int i = 0; i < this.onChange.Count; i++)
				{
					EventDelegate eventDelegate = this.onChange[i];
					if (eventDelegate != null)
					{
						eventDelegate.Unity_RemapPPtrs(depth + 1);
					}
				}
			}
		}
		if (this.mPanel != null)
		{
			this.mPanel = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mPanel) as UIPanel);
		}
		if (this.mBackground != null)
		{
			this.mBackground = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mBackground) as UISprite);
		}
		if (this.mHighlight != null)
		{
			this.mHighlight = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mHighlight) as UISprite);
		}
		if (this.mHighlightedLabel != null)
		{
			this.mHighlightedLabel = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mHighlightedLabel) as UILabel);
		}
		if (depth <= 7)
		{
			if (this.mLabelList != null)
			{
				for (int i = 0; i < this.mLabelList.Count; i++)
				{
					this.mLabelList[i] = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.mLabelList[i]) as UILabel);
				}
			}
		}
		if (this.eventReceiver != null)
		{
			this.eventReceiver = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.eventReceiver) as GameObject);
		}
		if (this.font != null)
		{
			this.font = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.font) as UIFont);
		}
		if (this.textLabel != null)
		{
			this.textLabel = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.textLabel) as UILabel);
		}
		if (this.source != null)
		{
			this.source = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.source) as GameObject);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.atlas;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 1246);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.bitmapFont, &var_0_cp_0[var_0_cp_1] + 1252);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.trueTypeFont, &var_0_cp_0[var_0_cp_1] + 1263);
		}
		SerializedNamedStateWriter.Instance.WriteInt32(this.fontSize, &var_0_cp_0[var_0_cp_1] + 1276);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.fontStyle, &var_0_cp_0[var_0_cp_1] + 1285);
		SerializedNamedStateWriter.Instance.WriteString(this.backgroundSprite, &var_0_cp_0[var_0_cp_1] + 1295);
		SerializedNamedStateWriter.Instance.WriteString(this.highlightSprite, &var_0_cp_0[var_0_cp_1] + 1312);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.position, &var_0_cp_0[var_0_cp_1] + 1328);
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.alignment, &var_0_cp_0[var_0_cp_1] + 1337);
		if (depth <= 7)
		{
			if (this.items == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1347, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1347, this.items.Count);
				for (int i = 0; i < this.items.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteString(this.items[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1353);
			this.padding.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1361);
			this.textColor.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1371);
			this.backgroundColor.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1387);
			this.highlightColor.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.isAnimated, &var_0_cp_0[var_0_cp_1] + 1402);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.isLocalized, &var_0_cp_0[var_0_cp_1] + 1413);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.separatePanel, &var_0_cp_0[var_0_cp_1] + 1425);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.openOn, &var_0_cp_0[var_0_cp_1] + 1439);
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
					EventDelegate arg_366_0 = (this.onChange[i] != null) ? this.onChange[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_366_0.Unity_NamedSerialize(depth + 1);
					SerializedNamedStateWriter.Instance.EndMetaGroup();
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		SerializedNamedStateWriter.Instance.WriteString(this.mSelectedItem, &var_0_cp_0[var_0_cp_1] + 1455);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mPanel, &var_0_cp_0[var_0_cp_1] + 1469);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mBackground, &var_0_cp_0[var_0_cp_1] + 1476);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mHighlight, &var_0_cp_0[var_0_cp_1] + 1488);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mHighlightedLabel, &var_0_cp_0[var_0_cp_1] + 1499);
		}
		if (depth <= 7)
		{
			if (this.mLabelList == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1517, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1517, this.mLabelList.Count);
				for (int i = 0; i < this.mLabelList.Count; i++)
				{
					SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.mLabelList[i], (IntPtr)0);
				}
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
		}
		SerializedNamedStateWriter.Instance.WriteSingle(this.mBgBorder, &var_0_cp_0[var_0_cp_1] + 1528);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.eventReceiver, &var_0_cp_0[var_0_cp_1] + 1165);
		}
		SerializedNamedStateWriter.Instance.WriteString(this.functionName, &var_0_cp_0[var_0_cp_1] + 402);
		SerializedNamedStateWriter.Instance.WriteSingle(this.textScale, &var_0_cp_0[var_0_cp_1] + 1538);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.font, &var_0_cp_0[var_0_cp_1] + 1548);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.textLabel, &var_0_cp_0[var_0_cp_1] + 1553);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.source, &var_0_cp_0[var_0_cp_1] + 1563);
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
			this.atlas = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1246) as UIAtlas);
		}
		if (depth <= 7)
		{
			this.bitmapFont = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1252) as UIFont);
		}
		if (depth <= 7)
		{
			this.trueTypeFont = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1263) as Font);
		}
		this.fontSize = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1276);
		this.fontStyle = (FontStyle)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1285);
		this.backgroundSprite = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 1295) as string);
		this.highlightSprite = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 1312) as string);
		this.position = (UIPopupList.Position)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1328);
		this.alignment = (NGUIText.Alignment)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1337);
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1347);
			this.items = new List<string>(num);
			for (int i = 0; i < num; i++)
			{
				this.items.Add(SerializedNamedStateReader.Instance.ReadString((IntPtr)0) as string);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1353);
			this.padding.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1361);
			this.textColor.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1371);
			this.backgroundColor.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 1387);
			this.highlightColor.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.isAnimated = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1402);
		SerializedNamedStateReader.Instance.Align();
		this.isLocalized = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1413);
		SerializedNamedStateReader.Instance.Align();
		this.separatePanel = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 1425);
		SerializedNamedStateReader.Instance.Align();
		this.openOn = (UIPopupList.OpenOn)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 1439);
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1446);
			this.onChange = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_307_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_307_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onChange.Add(eventDelegate);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		this.mSelectedItem = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 1455) as string);
		if (depth <= 7)
		{
			this.mPanel = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1469) as UIPanel);
		}
		if (depth <= 7)
		{
			this.mBackground = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1476) as UISprite);
		}
		if (depth <= 7)
		{
			this.mHighlight = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1488) as UISprite);
		}
		if (depth <= 7)
		{
			this.mHighlightedLabel = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1499) as UILabel);
		}
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 1517);
			this.mLabelList = new List<UILabel>(num);
			for (int i = 0; i < num; i++)
			{
				this.mLabelList.Add(SerializedNamedStateReader.Instance.ReadUnityEngineObject((IntPtr)0) as UILabel);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		this.mBgBorder = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 1528);
		if (depth <= 7)
		{
			this.eventReceiver = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1165) as GameObject);
		}
		this.functionName = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 402) as string);
		this.textScale = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 1538);
		if (depth <= 7)
		{
			this.font = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1548) as UIFont);
		}
		if (depth <= 7)
		{
			this.textLabel = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1553) as UILabel);
		}
		if (depth <= 7)
		{
			this.source = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1563) as GameObject);
		}
	}

	protected internal UIPopupList(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)instance).atlas);
	}

	public static void $Set0(object instance, long value)
	{
		((UIPopupList)instance).atlas = (UIAtlas)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)instance).bitmapFont);
	}

	public static void $Set1(object instance, long value)
	{
		((UIPopupList)instance).bitmapFont = (UIFont)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)instance).trueTypeFont);
	}

	public static void $Set2(object instance, long value)
	{
		((UIPopupList)instance).trueTypeFont = (Font)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get3(object instance, int index)
	{
		UIPopupList expr_06_cp_0 = (UIPopupList)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.padding.x;
		case 1:
			return expr_06_cp_0.padding.y;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set3(object instance, float value, int index)
	{
		UIPopupList expr_06_cp_0 = (UIPopupList)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.padding.x = value;
			return;
		case 1:
			expr_06_cp_0.padding.y = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get4(object instance, int index)
	{
		UIPopupList expr_06_cp_0 = (UIPopupList)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.textColor.r;
		case 1:
			return expr_06_cp_0.textColor.g;
		case 2:
			return expr_06_cp_0.textColor.b;
		case 3:
			return expr_06_cp_0.textColor.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set4(object instance, float value, int index)
	{
		UIPopupList expr_06_cp_0 = (UIPopupList)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.textColor.r = value;
			return;
		case 1:
			expr_06_cp_0.textColor.g = value;
			return;
		case 2:
			expr_06_cp_0.textColor.b = value;
			return;
		case 3:
			expr_06_cp_0.textColor.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get5(object instance, int index)
	{
		UIPopupList expr_06_cp_0 = (UIPopupList)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.backgroundColor.r;
		case 1:
			return expr_06_cp_0.backgroundColor.g;
		case 2:
			return expr_06_cp_0.backgroundColor.b;
		case 3:
			return expr_06_cp_0.backgroundColor.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set5(object instance, float value, int index)
	{
		UIPopupList expr_06_cp_0 = (UIPopupList)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.backgroundColor.r = value;
			return;
		case 1:
			expr_06_cp_0.backgroundColor.g = value;
			return;
		case 2:
			expr_06_cp_0.backgroundColor.b = value;
			return;
		case 3:
			expr_06_cp_0.backgroundColor.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get6(object instance, int index)
	{
		UIPopupList expr_06_cp_0 = (UIPopupList)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.highlightColor.r;
		case 1:
			return expr_06_cp_0.highlightColor.g;
		case 2:
			return expr_06_cp_0.highlightColor.b;
		case 3:
			return expr_06_cp_0.highlightColor.a;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set6(object instance, float value, int index)
	{
		UIPopupList expr_06_cp_0 = (UIPopupList)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.highlightColor.r = value;
			return;
		case 1:
			expr_06_cp_0.highlightColor.g = value;
			return;
		case 2:
			expr_06_cp_0.highlightColor.b = value;
			return;
		case 3:
			expr_06_cp_0.highlightColor.a = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static bool $Get7(object instance)
	{
		return ((UIPopupList)instance).isAnimated;
	}

	public static void $Set7(object instance, bool value)
	{
		((UIPopupList)instance).isAnimated = value;
	}

	public static bool $Get8(object instance)
	{
		return ((UIPopupList)instance).isLocalized;
	}

	public static void $Set8(object instance, bool value)
	{
		((UIPopupList)instance).isLocalized = value;
	}

	public static bool $Get9(object instance)
	{
		return ((UIPopupList)instance).separatePanel;
	}

	public static void $Set9(object instance, bool value)
	{
		((UIPopupList)instance).separatePanel = value;
	}

	public static long $Get10(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)instance).mPanel);
	}

	public static void $Set10(object instance, long value)
	{
		((UIPopupList)instance).mPanel = (UIPanel)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get11(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)instance).mBackground);
	}

	public static void $Set11(object instance, long value)
	{
		((UIPopupList)instance).mBackground = (UISprite)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get12(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)instance).mHighlight);
	}

	public static void $Set12(object instance, long value)
	{
		((UIPopupList)instance).mHighlight = (UISprite)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get13(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)instance).mHighlightedLabel);
	}

	public static void $Set13(object instance, long value)
	{
		((UIPopupList)instance).mHighlightedLabel = (UILabel)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get14(object instance)
	{
		return ((UIPopupList)instance).mBgBorder;
	}

	public static void $Set14(object instance, float value)
	{
		((UIPopupList)instance).mBgBorder = value;
	}

	public static long $Get15(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)instance).eventReceiver);
	}

	public static void $Set15(object instance, long value)
	{
		((UIPopupList)instance).eventReceiver = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get16(object instance)
	{
		return ((UIPopupList)instance).textScale;
	}

	public static void $Set16(object instance, float value)
	{
		((UIPopupList)instance).textScale = value;
	}

	public static long $Get17(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)instance).font);
	}

	public static void $Set17(object instance, long value)
	{
		((UIPopupList)instance).font = (UIFont)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get18(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)instance).textLabel);
	}

	public static void $Set18(object instance, long value)
	{
		((UIPopupList)instance).textLabel = (UILabel)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get19(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)instance).source);
	}

	public static void $Set19(object instance, long value)
	{
		((UIPopupList)instance).source = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).AddItem(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).AddItem(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).Animate((UIWidget)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(float*)(args + 2));
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).AnimateColor((UIWidget)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).AnimatePosition((UIWidget)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(float*)(args + 2));
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).AnimateScale((UIWidget)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(float*)(args + 2));
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).Clear();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		UIPopupList.Close();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).CloseIfUnselected());
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).CloseSelf();
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).activeFontScale);
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).activeFontSize);
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).ambigiousFont);
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).data);
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).isColliderEnabled);
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIPopupList.isOpen);
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).isValid);
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).onSelectionChange);
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).selection);
	}

	public unsafe static long $Invoke19(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).value);
	}

	public unsafe static long $Invoke20(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).GetHighlightPosition());
	}

	public unsafe static long $Invoke21(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).Highlight((UILabel)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke22(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).OnClick();
		return -1L;
	}

	public unsafe static long $Invoke23(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke24(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).OnDoubleClick();
		return -1L;
	}

	public unsafe static long $Invoke25(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke26(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).OnItemHover((GameObject)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke27(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).OnItemPress((GameObject)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke28(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).OnKey((KeyCode)(*(int*)args));
		return -1L;
	}

	public unsafe static long $Invoke29(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).OnLocalize();
		return -1L;
	}

	public unsafe static long $Invoke30(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).OnNavigate((KeyCode)(*(int*)args));
		return -1L;
	}

	public unsafe static long $Invoke31(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).OnSelect(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke32(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).OnValidate();
		return -1L;
	}

	public unsafe static long $Invoke33(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).RemoveItem(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke34(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).RemoveItemByData(GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke35(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).Select((UILabel)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke36(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).ambigiousFont = (UnityEngine.Object)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke37(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).onSelectionChange = (UIPopupList.LegacyEvent)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke38(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).selection = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke39(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).value = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke40(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).Show();
		return -1L;
	}

	public unsafe static long $Invoke41(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke42(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).TriggerCallbacks();
		return -1L;
	}

	public unsafe static long $Invoke43(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke44(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke45(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke46(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke47(long instance, long* args)
	{
		((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke48(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPopupList)GCHandledObjects.GCHandleToObject(instance)).UpdateTweenPosition());
	}
}
