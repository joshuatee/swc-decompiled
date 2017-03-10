using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/Root"), ExecuteInEditMode]
public class UIRoot : MonoBehaviour, IUnitySerializable
{
	public enum Scaling
	{
		Flexible,
		Constrained,
		ConstrainedOnMobiles
	}

	public enum Constraint
	{
		Fit,
		Fill,
		FitWidth,
		FitHeight
	}

	public static List<UIRoot> list = new List<UIRoot>();

	public UIRoot.Scaling scalingStyle;

	public int manualWidth;

	public int manualHeight;

	public int minimumHeight;

	public int maximumHeight;

	public bool fitWidth;

	public bool fitHeight;

	public bool adjustByDPI;

	public bool shrinkPortraitUI;

	private Transform mTrans;

	public UIRoot.Constraint constraint
	{
		get
		{
			if (this.fitWidth)
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.Fit;
				}
				return UIRoot.Constraint.FitWidth;
			}
			else
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.FitHeight;
				}
				return UIRoot.Constraint.Fill;
			}
		}
	}

	public UIRoot.Scaling activeScaling
	{
		get
		{
			UIRoot.Scaling scaling = this.scalingStyle;
			if (scaling == UIRoot.Scaling.ConstrainedOnMobiles)
			{
				return UIRoot.Scaling.Flexible;
			}
			return scaling;
		}
	}

	public int activeHeight
	{
		get
		{
			if (this.activeScaling == UIRoot.Scaling.Flexible)
			{
				Vector2 screenSize = NGUITools.screenSize;
				float num = screenSize.x / screenSize.y;
				if (screenSize.y < (float)this.minimumHeight)
				{
					screenSize.y = (float)this.minimumHeight;
					screenSize.x = screenSize.y * num;
				}
				else if (screenSize.y > (float)this.maximumHeight)
				{
					screenSize.y = (float)this.maximumHeight;
					screenSize.x = screenSize.y * num;
				}
				int num2 = Mathf.RoundToInt((this.shrinkPortraitUI && screenSize.y > screenSize.x) ? (screenSize.y / num) : screenSize.y);
				if (!this.adjustByDPI)
				{
					return num2;
				}
				return NGUIMath.AdjustByDPI((float)num2);
			}
			else
			{
				UIRoot.Constraint constraint = this.constraint;
				if (constraint == UIRoot.Constraint.FitHeight)
				{
					return this.manualHeight;
				}
				Vector2 screenSize2 = NGUITools.screenSize;
				float num3 = screenSize2.x / screenSize2.y;
				float num4 = (float)this.manualWidth / (float)this.manualHeight;
				switch (constraint)
				{
				case UIRoot.Constraint.Fit:
					if (num4 <= num3)
					{
						return this.manualHeight;
					}
					return Mathf.RoundToInt((float)this.manualWidth / num3);
				case UIRoot.Constraint.Fill:
					if (num4 >= num3)
					{
						return this.manualHeight;
					}
					return Mathf.RoundToInt((float)this.manualWidth / num3);
				case UIRoot.Constraint.FitWidth:
					return Mathf.RoundToInt((float)this.manualWidth / num3);
				default:
					return this.manualHeight;
				}
			}
		}
	}

	public float pixelSizeAdjustment
	{
		get
		{
			int num = Mathf.RoundToInt(NGUITools.screenSize.y);
			if (num != -1)
			{
				return this.GetPixelSizeAdjustment(num);
			}
			return 1f;
		}
	}

	public static float GetPixelSizeAdjustment(GameObject go)
	{
		UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(go);
		if (!(uIRoot != null))
		{
			return 1f;
		}
		return uIRoot.pixelSizeAdjustment;
	}

	public float GetPixelSizeAdjustment(int height)
	{
		height = Mathf.Max(2, height);
		if (this.activeScaling == UIRoot.Scaling.Constrained)
		{
			return (float)this.activeHeight / (float)height;
		}
		if (height < this.minimumHeight)
		{
			return (float)this.minimumHeight / (float)height;
		}
		if (height > this.maximumHeight)
		{
			return (float)this.maximumHeight / (float)height;
		}
		return 1f;
	}

	protected virtual void Awake()
	{
		this.mTrans = base.transform;
	}

	protected virtual void OnEnable()
	{
		UIRoot.list.Add(this);
	}

	protected virtual void OnDisable()
	{
		UIRoot.list.Remove(this);
	}

	protected virtual void Start()
	{
		UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
		if (componentInChildren != null)
		{
			Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
			Camera component = componentInChildren.gameObject.GetComponent<Camera>();
			componentInChildren.enabled = false;
			if (component != null)
			{
				component.orthographicSize = 1f;
				return;
			}
		}
		else
		{
			this.UpdateScale(false);
		}
	}

	private void Update()
	{
		this.UpdateScale(true);
	}

	public void UpdateScale(bool updateAnchors = true)
	{
		if (this.mTrans != null)
		{
			float num = (float)this.activeHeight;
			if (num > 0f)
			{
				float num2 = 2f / num;
				Vector3 localScale = this.mTrans.localScale;
				if (Mathf.Abs(localScale.x - num2) > 1.401298E-45f || Mathf.Abs(localScale.y - num2) > 1.401298E-45f || Mathf.Abs(localScale.z - num2) > 1.401298E-45f)
				{
					this.mTrans.localScale = new Vector3(num2, num2, num2);
					if (updateAnchors)
					{
						base.BroadcastMessage("UpdateAnchors");
					}
				}
			}
		}
	}

	public static void Broadcast(string funcName)
	{
		int i = 0;
		int count = UIRoot.list.Count;
		while (i < count)
		{
			UIRoot uIRoot = UIRoot.list[i];
			if (uIRoot != null)
			{
				uIRoot.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
			}
			i++;
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		if (param == null)
		{
			Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
			return;
		}
		int i = 0;
		int count = UIRoot.list.Count;
		while (i < count)
		{
			UIRoot uIRoot = UIRoot.list[i];
			if (uIRoot != null)
			{
				uIRoot.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
			}
			i++;
		}
	}

	public UIRoot()
	{
		this.manualWidth = 1280;
		this.manualHeight = 720;
		this.minimumHeight = 320;
		this.maximumHeight = 1536;
		this.fitHeight = true;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteInt32((int)this.scalingStyle);
		SerializedStateWriter.Instance.WriteInt32(this.manualWidth);
		SerializedStateWriter.Instance.WriteInt32(this.manualHeight);
		SerializedStateWriter.Instance.WriteInt32(this.minimumHeight);
		SerializedStateWriter.Instance.WriteInt32(this.maximumHeight);
		SerializedStateWriter.Instance.WriteBoolean(this.fitWidth);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.fitHeight);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.adjustByDPI);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.shrinkPortraitUI);
		SerializedStateWriter.Instance.Align();
	}

	public override void Unity_Deserialize(int depth)
	{
		this.scalingStyle = (UIRoot.Scaling)SerializedStateReader.Instance.ReadInt32();
		this.manualWidth = SerializedStateReader.Instance.ReadInt32();
		this.manualHeight = SerializedStateReader.Instance.ReadInt32();
		this.minimumHeight = SerializedStateReader.Instance.ReadInt32();
		this.maximumHeight = SerializedStateReader.Instance.ReadInt32();
		this.fitWidth = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.fitHeight = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.adjustByDPI = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.shrinkPortraitUI = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		int arg_1F_1 = (int)this.scalingStyle;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteInt32(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 4315);
		SerializedNamedStateWriter.Instance.WriteInt32(this.manualWidth, &var_0_cp_0[var_0_cp_1] + 4328);
		SerializedNamedStateWriter.Instance.WriteInt32(this.manualHeight, &var_0_cp_0[var_0_cp_1] + 4340);
		SerializedNamedStateWriter.Instance.WriteInt32(this.minimumHeight, &var_0_cp_0[var_0_cp_1] + 4353);
		SerializedNamedStateWriter.Instance.WriteInt32(this.maximumHeight, &var_0_cp_0[var_0_cp_1] + 4367);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.fitWidth, &var_0_cp_0[var_0_cp_1] + 4381);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.fitHeight, &var_0_cp_0[var_0_cp_1] + 4390);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.adjustByDPI, &var_0_cp_0[var_0_cp_1] + 4400);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.shrinkPortraitUI, &var_0_cp_0[var_0_cp_1] + 4412);
		SerializedNamedStateWriter.Instance.Align();
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.scalingStyle = (UIRoot.Scaling)arg_1A_0.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4315);
		this.manualWidth = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4328);
		this.manualHeight = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4340);
		this.minimumHeight = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4353);
		this.maximumHeight = SerializedNamedStateReader.Instance.ReadInt32(&var_0_cp_0[var_0_cp_1] + 4367);
		this.fitWidth = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4381);
		SerializedNamedStateReader.Instance.Align();
		this.fitHeight = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4390);
		SerializedNamedStateReader.Instance.Align();
		this.adjustByDPI = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4400);
		SerializedNamedStateReader.Instance.Align();
		this.shrinkPortraitUI = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 4412);
		SerializedNamedStateReader.Instance.Align();
	}

	protected internal UIRoot(UIntPtr dummy) : base(dummy)
	{
	}

	public static bool $Get0(object instance)
	{
		return ((UIRoot)instance).fitWidth;
	}

	public static void $Set0(object instance, bool value)
	{
		((UIRoot)instance).fitWidth = value;
	}

	public static bool $Get1(object instance)
	{
		return ((UIRoot)instance).fitHeight;
	}

	public static void $Set1(object instance, bool value)
	{
		((UIRoot)instance).fitHeight = value;
	}

	public static bool $Get2(object instance)
	{
		return ((UIRoot)instance).adjustByDPI;
	}

	public static void $Set2(object instance, bool value)
	{
		((UIRoot)instance).adjustByDPI = value;
	}

	public static bool $Get3(object instance)
	{
		return ((UIRoot)instance).shrinkPortraitUI;
	}

	public static void $Set3(object instance, bool value)
	{
		((UIRoot)instance).shrinkPortraitUI = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIRoot)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		UIRoot.Broadcast(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		UIRoot.Broadcast(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIRoot)GCHandledObjects.GCHandleToObject(instance)).activeHeight);
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIRoot)GCHandledObjects.GCHandleToObject(instance)).activeScaling);
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIRoot)GCHandledObjects.GCHandleToObject(instance)).constraint);
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIRoot)GCHandledObjects.GCHandleToObject(instance)).pixelSizeAdjustment);
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(UIRoot.GetPixelSizeAdjustment((GameObject)GCHandledObjects.GCHandleToObject(*args)));
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIRoot)GCHandledObjects.GCHandleToObject(instance)).GetPixelSizeAdjustment(*(int*)args));
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIRoot)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIRoot)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIRoot)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UIRoot)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UIRoot)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke14(long instance, long* args)
	{
		((UIRoot)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke15(long instance, long* args)
	{
		((UIRoot)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke16(long instance, long* args)
	{
		((UIRoot)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke17(long instance, long* args)
	{
		((UIRoot)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}

	public unsafe static long $Invoke18(long instance, long* args)
	{
		((UIRoot)GCHandledObjects.GCHandleToObject(instance)).UpdateScale(*(sbyte*)args != 0);
		return -1L;
	}
}
