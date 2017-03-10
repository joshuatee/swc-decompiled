using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : MonoBehaviour, IUnitySerializable
{
	public delegate void OnFinished();

	public static SpringPosition current;

	public Vector3 target;

	public float strength;

	public bool worldSpace;

	public bool ignoreTimeScale;

	public bool updateScrollView;

	public SpringPosition.OnFinished onFinished;

	[HideInInspector, SerializeField]
	protected internal GameObject eventReceiver;

	[HideInInspector, SerializeField]
	public string callWhenFinished;

	private Transform mTrans;

	private float mThreshold;

	private UIScrollView mSv;

	private void Start()
	{
		this.mTrans = base.transform;
		if (this.updateScrollView)
		{
			this.mSv = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		}
	}

	private void Update()
	{
		float deltaTime = this.ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime;
		if (this.worldSpace)
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.position).sqrMagnitude * 0.001f;
			}
			this.mTrans.position = NGUIMath.SpringLerp(this.mTrans.position, this.target, this.strength, deltaTime);
			if (this.mThreshold >= (this.target - this.mTrans.position).sqrMagnitude)
			{
				this.mTrans.position = this.target;
				this.NotifyListeners();
				base.enabled = false;
			}
		}
		else
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.localPosition).sqrMagnitude * 1E-05f;
			}
			this.mTrans.localPosition = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
			if (this.mThreshold >= (this.target - this.mTrans.localPosition).sqrMagnitude)
			{
				this.mTrans.localPosition = this.target;
				this.NotifyListeners();
				base.enabled = false;
			}
		}
		if (this.mSv != null)
		{
			this.mSv.UpdateScrollbars(true);
		}
	}

	private void NotifyListeners()
	{
		SpringPosition.current = this;
		if (this.onFinished != null)
		{
			this.onFinished();
		}
		if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
		{
			this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
		}
		SpringPosition.current = null;
	}

	public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPosition springPosition = go.GetComponent<SpringPosition>();
		if (springPosition == null)
		{
			springPosition = go.AddComponent<SpringPosition>();
		}
		springPosition.target = pos;
		springPosition.strength = strength;
		springPosition.onFinished = null;
		if (!springPosition.enabled)
		{
			springPosition.mThreshold = 0f;
			springPosition.enabled = true;
		}
		return springPosition;
	}

	public SpringPosition()
	{
		this.target = Vector3.zero;
		this.strength = 10f;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			this.target.Unity_Serialize(depth + 1);
		}
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteSingle(this.strength);
		SerializedStateWriter.Instance.WriteBoolean(this.worldSpace);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.ignoreTimeScale);
		SerializedStateWriter.Instance.Align();
		SerializedStateWriter.Instance.WriteBoolean(this.updateScrollView);
		SerializedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.eventReceiver);
		}
		SerializedStateWriter.Instance.WriteString(this.callWhenFinished);
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.target.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.strength = SerializedStateReader.Instance.ReadSingle();
		this.worldSpace = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.ignoreTimeScale = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		this.updateScrollView = SerializedStateReader.Instance.ReadBoolean();
		SerializedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.eventReceiver = (SerializedStateReader.Instance.ReadUnityEngineObject() as GameObject);
		}
		this.callWhenFinished = (SerializedStateReader.Instance.ReadString() as string);
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.eventReceiver != null)
		{
			this.eventReceiver = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.eventReceiver) as GameObject);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_23_0 = SerializedNamedStateWriter.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 265);
			this.target.Unity_NamedSerialize(depth + 1);
			SerializedNamedStateWriter.Instance.EndMetaGroup();
		}
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteSingle(this.strength, &var_0_cp_0[var_0_cp_1] + 2287);
		SerializedNamedStateWriter.Instance.WriteBoolean(this.worldSpace, &var_0_cp_0[var_0_cp_1] + 2642);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.ignoreTimeScale, &var_0_cp_0[var_0_cp_1] + 2653);
		SerializedNamedStateWriter.Instance.Align();
		SerializedNamedStateWriter.Instance.WriteBoolean(this.updateScrollView, &var_0_cp_0[var_0_cp_1] + 2669);
		SerializedNamedStateWriter.Instance.Align();
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.eventReceiver, &var_0_cp_0[var_0_cp_1] + 1165);
		}
		SerializedNamedStateWriter.Instance.WriteString(this.callWhenFinished, &var_0_cp_0[var_0_cp_1] + 1179);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateReader arg_23_0 = SerializedNamedStateReader.Instance;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_23_0.BeginMetaGroup(&var_0_cp_0[var_0_cp_1] + 265);
			this.target.Unity_NamedDeserialize(depth + 1);
			SerializedNamedStateReader.Instance.EndMetaGroup();
		}
		SerializedNamedStateReader.Instance.Align();
		this.strength = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 2287);
		this.worldSpace = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2642);
		SerializedNamedStateReader.Instance.Align();
		this.ignoreTimeScale = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2653);
		SerializedNamedStateReader.Instance.Align();
		this.updateScrollView = SerializedNamedStateReader.Instance.ReadBoolean(&var_0_cp_0[var_0_cp_1] + 2669);
		SerializedNamedStateReader.Instance.Align();
		if (depth <= 7)
		{
			this.eventReceiver = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 1165) as GameObject);
		}
		this.callWhenFinished = (SerializedNamedStateReader.Instance.ReadString(&var_0_cp_0[var_0_cp_1] + 1179) as string);
	}

	protected internal SpringPosition(UIntPtr dummy) : base(dummy)
	{
	}

	public static float $Get0(object instance, int index)
	{
		SpringPosition expr_06_cp_0 = (SpringPosition)instance;
		switch (index)
		{
		case 0:
			return expr_06_cp_0.target.x;
		case 1:
			return expr_06_cp_0.target.y;
		case 2:
			return expr_06_cp_0.target.z;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static void $Set0(object instance, float value, int index)
	{
		SpringPosition expr_06_cp_0 = (SpringPosition)instance;
		switch (index)
		{
		case 0:
			expr_06_cp_0.target.x = value;
			return;
		case 1:
			expr_06_cp_0.target.y = value;
			return;
		case 2:
			expr_06_cp_0.target.z = value;
			return;
		default:
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public static float $Get1(object instance)
	{
		return ((SpringPosition)instance).strength;
	}

	public static void $Set1(object instance, float value)
	{
		((SpringPosition)instance).strength = value;
	}

	public static bool $Get2(object instance)
	{
		return ((SpringPosition)instance).worldSpace;
	}

	public static void $Set2(object instance, bool value)
	{
		((SpringPosition)instance).worldSpace = value;
	}

	public static bool $Get3(object instance)
	{
		return ((SpringPosition)instance).ignoreTimeScale;
	}

	public static void $Set3(object instance, bool value)
	{
		((SpringPosition)instance).ignoreTimeScale = value;
	}

	public static bool $Get4(object instance)
	{
		return ((SpringPosition)instance).updateScrollView;
	}

	public static void $Set4(object instance, bool value)
	{
		((SpringPosition)instance).updateScrollView = value;
	}

	public static long $Get5(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((SpringPosition)instance).eventReceiver);
	}

	public static void $Set5(object instance, long value)
	{
		((SpringPosition)instance).eventReceiver = (GameObject)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(SpringPosition.Begin((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(float*)(args + 2)));
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((SpringPosition)GCHandledObjects.GCHandleToObject(instance)).NotifyListeners();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((SpringPosition)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((SpringPosition)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((SpringPosition)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((SpringPosition)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((SpringPosition)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((SpringPosition)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((SpringPosition)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
