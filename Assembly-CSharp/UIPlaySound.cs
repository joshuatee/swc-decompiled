using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Play Sound")]
public class UIPlaySound : MonoBehaviour, IUnitySerializable
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		Custom,
		OnEnable,
		OnDisable
	}

	public AudioClip audioClip;

	public UIPlaySound.Trigger trigger;

	[Range(0f, 1f)]
	public float volume;

	[Range(0f, 2f)]
	public float pitch;

	private bool mIsOver;

	private bool canPlay
	{
		get
		{
			if (!base.enabled)
			{
				return false;
			}
			UIButton component = base.GetComponent<UIButton>();
			return component == null || component.isEnabled;
		}
	}

	private void OnEnable()
	{
		if (this.trigger == UIPlaySound.Trigger.OnEnable)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	private void OnDisable()
	{
		if (this.trigger == UIPlaySound.Trigger.OnDisable)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	private void OnHover(bool isOver)
	{
		if (this.trigger == UIPlaySound.Trigger.OnMouseOver)
		{
			if (this.mIsOver == isOver)
			{
				return;
			}
			this.mIsOver = isOver;
		}
		if (this.canPlay && ((isOver && this.trigger == UIPlaySound.Trigger.OnMouseOver) || (!isOver && this.trigger == UIPlaySound.Trigger.OnMouseOut)))
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (this.trigger == UIPlaySound.Trigger.OnPress)
		{
			if (this.mIsOver == isPressed)
			{
				return;
			}
			this.mIsOver = isPressed;
		}
		if (this.canPlay && ((isPressed && this.trigger == UIPlaySound.Trigger.OnPress) || (!isPressed && this.trigger == UIPlaySound.Trigger.OnRelease)))
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	private void OnClick()
	{
		if (this.canPlay && this.trigger == UIPlaySound.Trigger.OnClick)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (this.canPlay && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	public void Play()
	{
		NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
	}

	public UIPlaySound()
	{
		this.volume = 1f;
		this.pitch = 1f;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.audioClip);
		}
		SerializedStateWriter.Instance.WriteInt32((int)this.trigger);
		SerializedStateWriter.Instance.WriteSingle(this.volume);
		SerializedStateWriter.Instance.WriteSingle(this.pitch);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.audioClip = (SerializedStateReader.Instance.ReadUnityEngineObject() as AudioClip);
		}
		this.trigger = (UIPlaySound.Trigger)SerializedStateReader.Instance.ReadInt32();
		this.volume = SerializedStateReader.Instance.ReadSingle();
		this.pitch = SerializedStateReader.Instance.ReadSingle();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.audioClip != null)
		{
			this.audioClip = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.audioClip) as AudioClip);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_23_1 = this.audioClip;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.WriteUnityEngineObject(arg_23_1, &var_0_cp_0[var_0_cp_1] + 1196);
		}
		SerializedNamedStateWriter.Instance.WriteInt32((int)this.trigger, &var_0_cp_0[var_0_cp_1] + 415);
		SerializedNamedStateWriter.Instance.WriteSingle(this.volume, &var_0_cp_0[var_0_cp_1] + 1206);
		SerializedNamedStateWriter.Instance.WriteSingle(this.pitch, &var_0_cp_0[var_0_cp_1] + 1213);
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
			this.audioClip = (arg_1E_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1196) as AudioClip);
		}
		this.trigger = (UIPlaySound.Trigger)SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 415);
		this.volume = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 1206);
		this.pitch = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 1213);
	}

	protected internal UIPlaySound(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPlaySound)instance).audioClip);
	}

	public static void $Set0(object instance, long value)
	{
		((UIPlaySound)instance).audioClip = (AudioClip)GCHandledObjects.GCHandleToObject(value);
	}

	public static float $Get1(object instance)
	{
		return ((UIPlaySound)instance).volume;
	}

	public static void $Set1(object instance, float value)
	{
		((UIPlaySound)instance).volume = value;
	}

	public static float $Get2(object instance)
	{
		return ((UIPlaySound)instance).pitch;
	}

	public static void $Set2(object instance, float value)
	{
		((UIPlaySound)instance).pitch = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).canPlay);
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).OnClick();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).OnHover(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).OnSelect(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).Play();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIPlaySound)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
