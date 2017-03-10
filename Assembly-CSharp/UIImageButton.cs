using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour, IUnitySerializable
{
	public UISprite target;

	public string normalSprite;

	public string hoverSprite;

	public string pressedSprite;

	public string disabledSprite;

	public bool pixelSnap;

	public bool isEnabled
	{
		get
		{
			Collider component = base.gameObject.GetComponent<Collider>();
			return component && component.enabled;
		}
		set
		{
			Collider component = base.gameObject.GetComponent<Collider>();
			if (!component)
			{
				return;
			}
			if (component.enabled != value)
			{
				component.enabled = value;
				this.UpdateImage();
			}
		}
	}

	private void OnEnable()
	{
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<UISprite>();
		}
		this.UpdateImage();
	}

	private void OnValidate()
	{
		if (this.target != null)
		{
			if (string.IsNullOrEmpty(this.normalSprite))
			{
				this.normalSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.hoverSprite))
			{
				this.hoverSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.pressedSprite))
			{
				this.pressedSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.disabledSprite))
			{
				this.disabledSprite = this.target.spriteName;
			}
		}
	}

	private void UpdateImage()
	{
		if (this.target != null)
		{
			if (this.isEnabled)
			{
				this.SetSprite(UICamera.IsHighlighted(base.gameObject) ? this.hoverSprite : this.normalSprite);
				return;
			}
			this.SetSprite(this.disabledSprite);
		}
	}

	private void OnHover(bool isOver)
	{
		if (this.isEnabled && this.target != null)
		{
			this.SetSprite(isOver ? this.hoverSprite : this.normalSprite);
		}
	}

	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			this.SetSprite(this.pressedSprite);
			return;
		}
		this.UpdateImage();
	}

	private void SetSprite(string sprite)
	{
		if (this.target.atlas == null || this.target.atlas.GetSprite(sprite) == null)
		{
			return;
		}
		this.target.spriteName = sprite;
		if (this.pixelSnap)
		{
			this.target.MakePixelPerfect();
		}
	}

	public UIImageButton()
	{
		this.pixelSnap = true;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.target);
		}
		SerializedStateWriter.Instance.WriteString(this.normalSprite);
		SerializedStateWriter.Instance.WriteString(this.hoverSprite);
		SerializedStateWriter.Instance.WriteString(this.pressedSprite);
		SerializedStateWriter.Instance.WriteString(this.disabledSprite);
		SerializedStateWriter.Instance.WriteBoolean(this.pixelSnap);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.target = (SerializedStateReader.Instance.ReadUnityEngineObject() as UISprite);
		}
		this.normalSprite = (SerializedStateReader.Instance.ReadString() as string);
		this.hoverSprite = (SerializedStateReader.Instance.ReadString() as string);
		this.pressedSprite = (SerializedStateReader.Instance.ReadString() as string);
		this.disabledSprite = (SerializedStateReader.Instance.ReadString() as string);
		this.pixelSnap = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.target != null)
		{
			this.target = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.target) as UISprite);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.target;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 265);
		}
		SerializedNamedStateWriter.Instance.WriteString(this.normalSprite, &var_0_cp_0[var_0_cp_1] + 1032);
		SerializedNamedStateWriter.Instance.WriteString(this.hoverSprite, &var_0_cp_0[var_0_cp_1] + 159);
		SerializedNamedStateWriter.Instance.WriteString(this.pressedSprite, &var_0_cp_0[var_0_cp_1] + 171);
		SerializedNamedStateWriter.Instance.WriteString(this.disabledSprite, &var_0_cp_0[var_0_cp_1] + 185);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.pixelSnap, &var_0_cp_0[var_0_cp_1] + 247);
		SerializedNamedStateWriter.Instance.Align();
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
			this.target = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 265) as UISprite);
		}
		this.normalSprite = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 1032) as string);
		this.hoverSprite = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 159) as string);
		this.pressedSprite = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 171) as string);
		this.disabledSprite = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 185) as string);
		this.pixelSnap = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 247);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIImageButton(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIImageButton)instance).target);
	}

	public static void $Set0(object instance, long value)
	{
		((UIImageButton)instance).target = (UISprite)GCHandledObjects.GCHandleToObject(value);
	}

	public static bool $Get1(object instance)
	{
		return ((UIImageButton)instance).pixelSnap;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIImageButton)instance).pixelSnap = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).isEnabled);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).OnHover(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).OnValidate();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).isEnabled = (*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).SetSprite(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIImageButton)GCHandledObjects.GCHandleToObject(instance)).UpdateImage();
		return -1L;
	}
}
