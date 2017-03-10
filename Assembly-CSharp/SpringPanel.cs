using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Internal/Spring Panel"), RequireComponent(typeof(UIPanel))]
public class SpringPanel : MonoBehaviour, IUnitySerializable
{
	public delegate void OnFinished();

	public static SpringPanel current;

	public Vector3 target;

	public float strength;

	public SpringPanel.OnFinished onFinished;

	private UIPanel mPanel;

	private Transform mTrans;

	private UIScrollView mDrag;

	private void Start()
	{
		this.mPanel = base.GetComponent<UIPanel>();
		this.mDrag = base.GetComponent<UIScrollView>();
		this.mTrans = base.transform;
	}

	private void Update()
	{
		this.AdvanceTowardsPosition();
	}

	protected virtual void AdvanceTowardsPosition()
	{
		float deltaTime = RealTime.deltaTime;
		bool flag = false;
		Vector3 localPosition = this.mTrans.localPosition;
		Vector3 vector = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
		if ((vector - this.target).sqrMagnitude < 0.01f)
		{
			vector = this.target;
			base.enabled = false;
			flag = true;
		}
		this.mTrans.localPosition = vector;
		Vector3 vector2 = vector - localPosition;
		Vector2 clipOffset = this.mPanel.clipOffset;
		clipOffset.x -= vector2.x;
		clipOffset.y -= vector2.y;
		this.mPanel.clipOffset = clipOffset;
		if (this.mDrag != null)
		{
			this.mDrag.UpdateScrollbars(false);
		}
		if (flag && this.onFinished != null)
		{
			SpringPanel.current = this;
			this.onFinished();
			SpringPanel.current = null;
		}
	}

	public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPanel springPanel = go.GetComponent<SpringPanel>();
		if (springPanel == null)
		{
			springPanel = go.AddComponent<SpringPanel>();
		}
		springPanel.target = pos;
		springPanel.strength = strength;
		springPanel.onFinished = null;
		springPanel.enabled = true;
		return springPanel;
	}

	public SpringPanel()
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
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.target.Unity_Deserialize(depth + 1);
		}
		SerializedStateReader.Instance.Align();
		this.strength = SerializedStateReader.Instance.ReadSingle();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
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
	}

	protected internal SpringPanel(UIntPtr dummy) : base(dummy)
	{
	}

	public static float $Get0(object instance, int index)
	{
		SpringPanel expr_06_cp_0 = (SpringPanel)instance;
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
		SpringPanel expr_06_cp_0 = (SpringPanel)instance;
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
		return ((SpringPanel)instance).strength;
	}

	public static void $Set1(object instance, float value)
	{
		((SpringPanel)instance).strength = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((SpringPanel)GCHandledObjects.GCHandleToObject(instance)).AdvanceTowardsPosition();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(SpringPanel.Begin((GameObject)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)), *(float*)(args + 2)));
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((SpringPanel)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((SpringPanel)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((SpringPanel)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((SpringPanel)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((SpringPanel)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((SpringPanel)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((SpringPanel)GCHandledObjects.GCHandleToObject(instance)).Update();
		return -1L;
	}
}
