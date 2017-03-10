using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor, IUnitySerializable
{
	public static UIButton current;

	public bool dragHighlight;

	public string hoverSprite;

	public string pressedSprite;

	public string disabledSprite;

	public Sprite hoverSprite2D;

	public Sprite pressedSprite2D;

	public Sprite disabledSprite2D;

	public bool pixelSnap;

	public List<EventDelegate> onClick;

	[System.NonSerialized]
	private UISprite mSprite;

	[System.NonSerialized]
	private UI2DSprite mSprite2D;

	[System.NonSerialized]
	private string mNormalSprite;

	[System.NonSerialized]
	private Sprite mNormalSprite2D;

	public override bool isEnabled
	{
		get
		{
			if (!base.enabled)
			{
				return false;
			}
			Collider component = base.gameObject.GetComponent<Collider>();
			if (component && component.enabled)
			{
				return true;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 && component2.enabled;
		}
		set
		{
			if (this.isEnabled != value)
			{
				Collider component = base.gameObject.GetComponent<Collider>();
				if (component != null)
				{
					component.enabled = value;
					UIButton[] components = base.GetComponents<UIButton>();
					UIButton[] array = components;
					for (int i = 0; i < array.Length; i++)
					{
						UIButton uIButton = array[i];
						uIButton.SetState(value ? UIButtonColor.State.Normal : UIButtonColor.State.Disabled, false);
					}
					return;
				}
				Collider2D component2 = base.GetComponent<Collider2D>();
				if (component2 != null)
				{
					component2.enabled = value;
					UIButton[] components2 = base.GetComponents<UIButton>();
					UIButton[] array2 = components2;
					for (int j = 0; j < array2.Length; j++)
					{
						UIButton uIButton2 = array2[j];
						uIButton2.SetState(value ? UIButtonColor.State.Normal : UIButtonColor.State.Disabled, false);
					}
					return;
				}
				base.enabled = value;
			}
		}
	}

	public string normalSprite
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mNormalSprite;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.mSprite != null && !string.IsNullOrEmpty(this.mNormalSprite) && this.mNormalSprite == this.mSprite.spriteName)
			{
				this.mNormalSprite = value;
				this.SetSprite(value);
				NGUITools.SetDirty(this.mSprite);
				return;
			}
			this.mNormalSprite = value;
			if (this.mState == UIButtonColor.State.Normal)
			{
				this.SetSprite(value);
			}
		}
	}

	public Sprite normalSprite2D
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mNormalSprite2D;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.mSprite2D != null && this.mNormalSprite2D == this.mSprite2D.sprite2D)
			{
				this.mNormalSprite2D = value;
				this.SetSprite(value);
				NGUITools.SetDirty(this.mSprite);
				return;
			}
			this.mNormalSprite2D = value;
			if (this.mState == UIButtonColor.State.Normal)
			{
				this.SetSprite(value);
			}
		}
	}

	protected override void OnInit()
	{
		base.OnInit();
		this.mSprite = (this.mWidget as UISprite);
		this.mSprite2D = (this.mWidget as UI2DSprite);
		if (this.mSprite != null)
		{
			this.mNormalSprite = this.mSprite.spriteName;
		}
		if (this.mSprite2D != null)
		{
			this.mNormalSprite2D = this.mSprite2D.sprite2D;
		}
	}

	protected override void OnEnable()
	{
		if (this.isEnabled)
		{
			if (this.mInitDone)
			{
				this.OnHover(UICamera.hoveredObject == base.gameObject);
				return;
			}
		}
		else
		{
			this.SetState(UIButtonColor.State.Disabled, true);
		}
	}

	protected override void OnDragOver()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.gameObject))
		{
			base.OnDragOver();
		}
	}

	protected override void OnDragOut()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.gameObject))
		{
			base.OnDragOut();
		}
	}

	protected virtual void OnClick()
	{
		if (UIButton.current == null && this.isEnabled)
		{
			UIButton.current = this;
			EventDelegate.Execute(this.onClick);
			UIButton.current = null;
		}
	}

	public override void SetState(UIButtonColor.State state, bool immediate)
	{
		base.SetState(state, immediate);
		if (!(this.mSprite != null))
		{
			if (this.mSprite2D != null)
			{
				switch (state)
				{
				case UIButtonColor.State.Normal:
					this.SetSprite(this.mNormalSprite2D);
					return;
				case UIButtonColor.State.Hover:
					this.SetSprite((this.hoverSprite2D == null) ? this.mNormalSprite2D : this.hoverSprite2D);
					return;
				case UIButtonColor.State.Pressed:
					this.SetSprite(this.pressedSprite2D);
					return;
				case UIButtonColor.State.Disabled:
					this.SetSprite(this.disabledSprite2D);
					break;
				default:
					return;
				}
			}
			return;
		}
		switch (state)
		{
		case UIButtonColor.State.Normal:
			this.SetSprite(this.mNormalSprite);
			return;
		case UIButtonColor.State.Hover:
			this.SetSprite(string.IsNullOrEmpty(this.hoverSprite) ? this.mNormalSprite : this.hoverSprite);
			return;
		case UIButtonColor.State.Pressed:
			this.SetSprite(this.pressedSprite);
			return;
		case UIButtonColor.State.Disabled:
			this.SetSprite(this.disabledSprite);
			return;
		default:
			return;
		}
	}

	protected void SetSprite(string sp)
	{
		if (this.mSprite != null && !string.IsNullOrEmpty(sp) && this.mSprite.spriteName != sp)
		{
			this.mSprite.spriteName = sp;
			if (this.pixelSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}

	protected void SetSprite(Sprite sp)
	{
		if (sp != null && this.mSprite2D != null && this.mSprite2D.sprite2D != sp)
		{
			this.mSprite2D.sprite2D = sp;
			if (this.pixelSnap)
			{
				this.mSprite2D.MakePixelPerfect();
			}
		}
	}

	public UIButton()
	{
		this.onClick = new List<EventDelegate>();
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.tweenTarget);
		}
		if (depth <= 7)
		{
			this.hover.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.pressed.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			this.disabledColor.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.duration);
		SerializedStateWriter.Instance.WriteBoolean(this.dragHighlight);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteString(this.hoverSprite);
		SerializedStateWriter.Instance.WriteString(this.pressedSprite);
		SerializedStateWriter.Instance.WriteString(this.disabledSprite);
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.hoverSprite2D);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.pressedSprite2D);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.disabledSprite2D);
		}
		SerializedStateWriter.Instance.WriteBoolean(this.pixelSnap);
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			if (this.onClick == null)
			{
				SerializedStateWriter.Instance.WriteInt32(0);
			}
			else
			{
				SerializedStateWriter.Instance.WriteInt32(this.onClick.Count);
				for (int i = 0; i < this.onClick.Count; i++)
				{
					((this.onClick[i] != null) ? this.onClick[i] : new EventDelegate()).Unity_Serialize(depth + 1);
				}
			}
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.tweenTarget = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		if (depth <= 7)
		{
			this.hover.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.pressed.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.disabledColor.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.duration = SerializedStateReader.Instance.ReadSingle();
		this.dragHighlight = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.hoverSprite = (SerializedStateReader.Instance.ReadString() as string);
		this.pressedSprite = (SerializedStateReader.Instance.ReadString() as string);
		this.disabledSprite = (SerializedStateReader.Instance.ReadString() as string);
		if (depth <= 7)
		{
			this.hoverSprite2D = (SerializedStateReader.Instance.ReadUnityEngineObject() as Sprite);
		}
		if (depth <= 7)
		{
			this.pressedSprite2D = (SerializedStateReader.Instance.ReadUnityEngineObject() as Sprite);
		}
		if (depth <= 7)
		{
			this.disabledSprite2D = (SerializedStateReader.Instance.ReadUnityEngineObject() as Sprite);
		}
		this.pixelSnap = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			int num = SerializedStateReader.Instance.ReadInt32();
			this.onClick = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				eventDelegate.Unity_Deserialize(depth + 1);
				this.onClick.Add(eventDelegate);
			}
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.tweenTarget != null)
		{
			this.tweenTarget = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.tweenTarget) as GameObject);
		}
		if (this.hoverSprite2D != null)
		{
			this.hoverSprite2D = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.hoverSprite2D) as Sprite);
		}
		if (this.pressedSprite2D != null)
		{
			this.pressedSprite2D = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.pressedSprite2D) as Sprite);
		}
		if (this.disabledSprite2D != null)
		{
			this.disabledSprite2D = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.disabledSprite2D) as Sprite);
		}
		if (depth <= 7)
		{
			if (this.onClick != null)
			{
				for (int i = 0; i < this.onClick.Count; i++)
				{
					EventDelegate eventDelegate = this.onClick[i];
					if (eventDelegate != null)
					{
						eventDelegate.Unity_RemapPPtrs(depth + 1);
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
			ISerializedNamedStateWriter arg_20_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_20_1 = this.tweenTarget;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_20_0.WriteUnityEngineObject(arg_20_1, &var_0_cp_0[var_0_cp_1] + 96);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 108);
			this.hover.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 114);
			this.pressed.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 122);
			this.disabledColor.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.duration, &var_0_cp_0[var_0_cp_1] + 136);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.dragHighlight, &var_0_cp_0[var_0_cp_1] + 145);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteString(this.hoverSprite, &var_0_cp_0[var_0_cp_1] + 159);
		SerializedNamedStateWriter.Instance.WriteString(this.pressedSprite, &var_0_cp_0[var_0_cp_1] + 171);
		SerializedNamedStateWriter.Instance.WriteString(this.disabledSprite, &var_0_cp_0[var_0_cp_1] + 185);
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.hoverSprite2D, &var_0_cp_0[var_0_cp_1] + 200);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.pressedSprite2D, &var_0_cp_0[var_0_cp_1] + 214);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.disabledSprite2D, &var_0_cp_0[var_0_cp_1] + 230);
		}
		SerializedNamedStateWriter.Instance.WriteBoolean(this.pixelSnap, &var_0_cp_0[var_0_cp_1] + 247);
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			if (this.onClick == null)
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 257, 0);
				SerializedNamedStateWriter.Instance.EndMetaGroup();
			}
			else
			{
				SerializedNamedStateWriter.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 257, this.onClick.Count);
				for (int i = 0; i < this.onClick.Count; i++)
				{
					EventDelegate arg_249_0 = (this.onClick[i] != null) ? this.onClick[i] : new EventDelegate();
					SerializedNamedStateWriter.Instance.BeginMetaGroup((IntPtr)0);
					arg_249_0.Unity_NamedSerialize(depth + 1);
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
			ISerializedNamedStateReader arg_1B_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			this.tweenTarget = (arg_1B_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 96) as GameObject);
		}
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 108);
			this.hover.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 114);
			this.pressed.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateReader.Instance.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 122);
			this.disabledColor.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.duration = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 136);
		this.dragHighlight = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 145);
		SerializedNamedStateReader.Instance.Align();
		this.hoverSprite = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 159) as string);
		this.pressedSprite = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 171) as string);
		this.disabledSprite = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 185) as string);
		if (depth <= 7)
		{
			this.hoverSprite2D = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 200) as Sprite);
		}
		if (depth <= 7)
		{
			this.pressedSprite2D = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 214) as Sprite);
		}
		if (depth <= 7)
		{
			this.disabledSprite2D = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 230) as Sprite);
		}
		this.pixelSnap = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 247);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			int num = SerializedNamedStateReader.Instance.BeginSequenceGroup(&var_0_cp_0[var_0_cp_1] + 257);
			this.onClick = new List<EventDelegate>(num);
			for (int i = 0; i < num; i++)
			{
				EventDelegate eventDelegate = new EventDelegate();
				EventDelegate arg_224_0 = eventDelegate;
				SerializedNamedStateReader.Instance.BeginMetaGroup((IntPtr)0);
				arg_224_0.Unity_NamedDeserialize(depth + 1);
				SerializedNamedStateReader.Instance.EndMetaGroup();
				this.onClick.Add(eventDelegate);
			}
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
	}

	protected internal UIButton(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((UIButton)instance).dragHighlight;
	}

	public static void $Set0(object instance, bool value)
	{
		((UIButton)instance).dragHighlight = value;
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButton)instance).hoverSprite2D);
	}

	public static void $Set1(object instance, long value)
	{
		((UIButton)instance).hoverSprite2D = (Sprite)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get2(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButton)instance).pressedSprite2D);
	}

	public static void $Set2(object instance, long value)
	{
		((UIButton)instance).pressedSprite2D = (Sprite)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get3(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButton)instance).disabledSprite2D);
	}

	public static void $Set3(object instance, long value)
	{
		((UIButton)instance).disabledSprite2D = (Sprite)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get4(object instance)
	{
		return ((UIButton)instance).pixelSnap;
	}

	public static void $Set4(object instance, bool value)
	{
		((UIButton)instance).pixelSnap = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButton)GCHandledObjects.GCHandleToObject(instance)).isEnabled);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButton)GCHandledObjects.GCHandleToObject(instance)).normalSprite);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIButton)GCHandledObjects.GCHandleToObject(instance)).normalSprite2D);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).OnClick();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).OnDragOut();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).OnDragOver();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).OnInit();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).isEnabled = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).normalSprite = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).normalSprite2D = (Sprite)GCHandledObjects.GCHandleToObject(*args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).SetSprite(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).SetSprite((Sprite)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).SetState((UIButtonColor.State)(*(int*)args), *(sbyte*)(args + 1) != 0);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIButton)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
