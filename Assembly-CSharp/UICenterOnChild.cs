using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Child")]
public class UICenterOnChild : MonoBehaviour, IUnitySerializable
{
	public delegate void OnCenterCallback(GameObject centeredObject);

	public float springStrength;

	public float nextPageThreshold;

	public SpringPanel.OnFinished onFinished;

	public UICenterOnChild.OnCenterCallback onCenter;

	private UIScrollView mScrollView;

	private GameObject mCenteredObject;

	public GameObject centeredObject
	{
		get
		{
			return this.mCenteredObject;
		}
	}

	private void Start()
	{
		this.Recenter();
	}

	private void OnEnable()
	{
		if (this.mScrollView)
		{
			this.mScrollView.centerOnChild = this;
			this.Recenter();
		}
	}

	private void OnDisable()
	{
		if (this.mScrollView)
		{
			this.mScrollView.centerOnChild = null;
		}
	}

	private void OnDragFinished()
	{
		if (base.enabled)
		{
			this.Recenter();
		}
	}

	private void OnValidate()
	{
		this.nextPageThreshold = Mathf.Abs(this.nextPageThreshold);
	}

	[ContextMenu("Execute")]
	public void Recenter()
	{
		if (this.mScrollView == null)
		{
			this.mScrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
			if (this.mScrollView == null)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					base.GetType(),
					" requires ",
					typeof(UIScrollView),
					" on a parent object in order to work"
				}), this);
				base.enabled = false;
				return;
			}
			if (this.mScrollView)
			{
				this.mScrollView.centerOnChild = this;
				UIScrollView expr_8E = this.mScrollView;
				expr_8E.onDragFinished = (UIScrollView.OnDragNotification)Delegate.Combine(expr_8E.onDragFinished, new UIScrollView.OnDragNotification(this.OnDragFinished));
			}
			if (this.mScrollView.horizontalScrollBar != null)
			{
				UIProgressBar expr_CD = this.mScrollView.horizontalScrollBar;
				expr_CD.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(expr_CD.onDragFinished, new UIProgressBar.OnDragFinished(this.OnDragFinished));
			}
			if (this.mScrollView.verticalScrollBar != null)
			{
				UIProgressBar expr_10C = this.mScrollView.verticalScrollBar;
				expr_10C.onDragFinished = (UIProgressBar.OnDragFinished)Delegate.Combine(expr_10C.onDragFinished, new UIProgressBar.OnDragFinished(this.OnDragFinished));
			}
		}
		if (this.mScrollView.panel == null)
		{
			return;
		}
		Transform transform = base.transform;
		if (transform.childCount == 0)
		{
			return;
		}
		Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
		Vector3 vector = (worldCorners[2] + worldCorners[0]) * 0.5f;
		Vector3 vector2 = this.mScrollView.currentMomentum * this.mScrollView.momentumAmount;
		Vector3 a = NGUIMath.SpringDampen(ref vector2, 9f, 2f);
		Vector3 b = vector - a * 0.01f;
		float num = 3.40282347E+38f;
		Transform target = null;
		int index = 0;
		int num2 = 0;
		UIGrid component = base.GetComponent<UIGrid>();
		List<Transform> list = null;
		if (component != null)
		{
			list = component.GetChildList();
			int i = 0;
			int count = list.Count;
			int num3 = 0;
			while (i < count)
			{
				Transform transform2 = list[i];
				if (transform2.gameObject.activeInHierarchy)
				{
					float num4 = Vector3.SqrMagnitude(transform2.position - b);
					if (num4 < num)
					{
						num = num4;
						target = transform2;
						index = i;
						num2 = num3;
					}
					num3++;
				}
				i++;
			}
		}
		else
		{
			int j = 0;
			int childCount = transform.childCount;
			int num5 = 0;
			while (j < childCount)
			{
				Transform child = transform.GetChild(j);
				if (child.gameObject.activeInHierarchy)
				{
					float num6 = Vector3.SqrMagnitude(child.position - b);
					if (num6 < num)
					{
						num = num6;
						target = child;
						index = j;
						num2 = num5;
					}
					num5++;
				}
				j++;
			}
		}
		if (this.nextPageThreshold > 0f && UICamera.currentTouch != null && this.mCenteredObject != null && this.mCenteredObject.transform == ((list != null) ? list[index] : transform.GetChild(index)))
		{
			Vector3 vector3 = UICamera.currentTouch.totalDelta;
			vector3 = base.transform.rotation * vector3;
			UIScrollView.Movement movement = this.mScrollView.movement;
			float num7;
			if (movement != UIScrollView.Movement.Horizontal)
			{
				if (movement != UIScrollView.Movement.Vertical)
				{
					num7 = vector3.magnitude;
				}
				else
				{
					num7 = vector3.y;
				}
			}
			else
			{
				num7 = vector3.x;
			}
			if (Mathf.Abs(num7) > this.nextPageThreshold)
			{
				if (num7 > this.nextPageThreshold)
				{
					if (list != null)
					{
						if (num2 > 0)
						{
							target = list[num2 - 1];
						}
						else
						{
							target = ((base.GetComponent<UIWrapContent>() == null) ? list[0] : list[list.Count - 1]);
						}
					}
					else if (num2 > 0)
					{
						target = transform.GetChild(num2 - 1);
					}
					else
					{
						target = ((base.GetComponent<UIWrapContent>() == null) ? transform.GetChild(0) : transform.GetChild(transform.childCount - 1));
					}
				}
				else if (num7 < -this.nextPageThreshold)
				{
					if (list != null)
					{
						if (num2 < list.Count - 1)
						{
							target = list[num2 + 1];
						}
						else
						{
							target = ((base.GetComponent<UIWrapContent>() == null) ? list[list.Count - 1] : list[0]);
						}
					}
					else if (num2 < transform.childCount - 1)
					{
						target = transform.GetChild(num2 + 1);
					}
					else
					{
						target = ((base.GetComponent<UIWrapContent>() == null) ? transform.GetChild(transform.childCount - 1) : transform.GetChild(0));
					}
				}
			}
		}
		this.CenterOn(target, vector);
	}

	private void CenterOn(Transform target, Vector3 panelCenter)
	{
		if (target != null && this.mScrollView != null && this.mScrollView.panel != null)
		{
			Transform cachedTransform = this.mScrollView.panel.cachedTransform;
			this.mCenteredObject = target.gameObject;
			Vector3 a = cachedTransform.InverseTransformPoint(target.position);
			Vector3 b = cachedTransform.InverseTransformPoint(panelCenter);
			Vector3 b2 = a - b;
			if (!this.mScrollView.canMoveHorizontally)
			{
				b2.x = 0f;
			}
			if (!this.mScrollView.canMoveVertically)
			{
				b2.y = 0f;
			}
			b2.z = 0f;
			SpringPanel.Begin(this.mScrollView.panel.cachedGameObject, cachedTransform.localPosition - b2, this.springStrength).onFinished = this.onFinished;
		}
		else
		{
			this.mCenteredObject = null;
		}
		if (this.onCenter != null)
		{
			this.onCenter(this.mCenteredObject);
		}
	}

	public void CenterOn(Transform target)
	{
		if (this.mScrollView != null && this.mScrollView.panel != null)
		{
			Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
			Vector3 panelCenter = (worldCorners[2] + worldCorners[0]) * 0.5f;
			this.CenterOn(target, panelCenter);
		}
	}

	public UICenterOnChild()
	{
		this.springStrength = 8f;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteSingle(this.springStrength);
		SerializedStateWriter.Instance.WriteSingle(this.nextPageThreshold);
	}

	public override void Unity_Deserialize(int depth)
	{
		this.springStrength = SerializedStateReader.Instance.ReadSingle();
		this.nextPageThreshold = SerializedStateReader.Instance.ReadSingle();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		ISerializedNamedStateWriter arg_1F_0 = SerializedNamedStateWriter.Instance;
		float arg_1F_1 = this.springStrength;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		arg_1F_0.WriteSingle(arg_1F_1, &var_0_cp_0[var_0_cp_1] + 439);
		SerializedNamedStateWriter.Instance.WriteSingle(this.nextPageThreshold, &var_0_cp_0[var_0_cp_1] + 454);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		ISerializedNamedStateReader arg_1A_0 = SerializedNamedStateReader.Instance;
		byte[] var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
		int var_0_cp_1 = 0;
		this.springStrength = arg_1A_0.ReadSingle(&var_0_cp_0[var_0_cp_1] + 439);
		this.nextPageThreshold = SerializedNamedStateReader.Instance.ReadSingle(&var_0_cp_0[var_0_cp_1] + 454);
	}

	protected internal UICenterOnChild(UIntPtr dummy) : base(dummy)
	{
	}

	public static float $Get0(object instance)
	{
		return ((UICenterOnChild)instance).springStrength;
	}

	public static void $Set0(object instance, float value)
	{
		((UICenterOnChild)instance).springStrength = value;
	}

	public static float $Get1(object instance)
	{
		return ((UICenterOnChild)instance).nextPageThreshold;
	}

	public static void $Set1(object instance, float value)
	{
		((UICenterOnChild)instance).nextPageThreshold = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).CenterOn((Transform)GCHandledObjects.GCHandleToObject(*args));
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).CenterOn((Transform)GCHandledObjects.GCHandleToObject(*args), *(*(IntPtr*)(args + 1)));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).centeredObject);
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).OnDragFinished();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).OnValidate();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).Recenter();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke12(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke13(long instance, long* args)
	{
		((UICenterOnChild)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
