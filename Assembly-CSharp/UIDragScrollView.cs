using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Drag Scroll View")]
public class UIDragScrollView : MonoBehaviour, IUnitySerializable
{
	public UIScrollView scrollView;

	[HideInInspector, SerializeField]
	protected internal UIScrollView draggablePanel;

	private Transform mTrans;

	private UIScrollView mScroll;

	private bool mAutoFind;

	private bool mStarted;

	private void OnEnable()
	{
		this.mTrans = base.transform;
		if (this.scrollView == null && this.draggablePanel != null)
		{
			this.scrollView = this.draggablePanel;
			this.draggablePanel = null;
		}
		if (this.mStarted && (this.mAutoFind || this.mScroll == null))
		{
			this.FindScrollView();
		}
	}

	private void Start()
	{
		this.mStarted = true;
		this.FindScrollView();
	}

	private void FindScrollView()
	{
		UIScrollView uIScrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
		if (this.scrollView == null || (this.mAutoFind && uIScrollView != this.scrollView))
		{
			this.scrollView = uIScrollView;
			this.mAutoFind = true;
		}
		else if (this.scrollView == uIScrollView)
		{
			this.mAutoFind = true;
		}
		this.mScroll = this.scrollView;
	}

	private void OnPress(bool pressed)
	{
		if (this.mAutoFind && this.mScroll != this.scrollView)
		{
			this.mScroll = this.scrollView;
			this.mAutoFind = false;
		}
		if (this.scrollView && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.scrollView.Press(pressed);
			if (!pressed && this.mAutoFind)
			{
				this.scrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
				this.mScroll = this.scrollView;
			}
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.Drag();
		}
	}

	private void OnScroll(float delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.Scroll(delta);
		}
	}

	public void OnPan(Vector2 delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.OnPan(delta);
		}
	}

	public UIDragScrollView()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.scrollView);
		}
		if (depth <= 7)
		{
			SerializedStateWriter.Instance.WriteUnityEngineObject(this.draggablePanel);
		}
	}

	public override void Unity_Deserialize(int depth)
	{
		if (depth <= 7)
		{
			this.scrollView = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIScrollView);
		}
		if (depth <= 7)
		{
			this.draggablePanel = (SerializedStateReader.Instance.ReadUnityEngineObject() as UIScrollView);
		}
	}

	public override void Unity_RemapPPtrs(int depth)
	{
		if (this.scrollView != null)
		{
			this.scrollView = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.scrollView) as UIScrollView);
		}
		if (this.draggablePanel != null)
		{
			this.draggablePanel = (PPtrRemapper.Instance.GetNewInstanceToReplaceOldInstance(this.draggablePanel) as UIScrollView);
		}
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		byte[] var_0_cp_0;
		int var_0_cp_1;
		if (depth <= 7)
		{
			ISerializedNamedStateWriter arg_20_0 = SerializedNamedStateWriter.Instance;
			UnityEngine.Object arg_20_1 = this.scrollView;
			var_0_cp_0 = $FieldNamesStorage.$RuntimeNames;
			var_0_cp_1 = 0;
			arg_20_0.WriteUnityEngineObject(arg_20_1, &var_0_cp_0[var_0_cp_1] + 55);
		}
		if (depth <= 7)
		{
			SerializedNamedStateWriter.Instance.WriteUnityEngineObject(this.draggablePanel, &var_0_cp_0[var_0_cp_1] + 755);
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
			this.scrollView = (arg_1B_0.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 55) as UIScrollView);
		}
		if (depth <= 7)
		{
			this.draggablePanel = (SerializedNamedStateReader.Instance.ReadUnityEngineObject(&var_0_cp_0[var_0_cp_1] + 755) as UIScrollView);
		}
	}

	protected internal UIDragScrollView(UIntPtr dummy) : base(dummy)
	{
	}

	public static long $Get0(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDragScrollView)instance).scrollView);
	}

	public static void $Set0(object instance, long value)
	{
		((UIDragScrollView)instance).scrollView = (UIScrollView)GCHandledObjects.GCHandleToObject(value);
	}

	public static long $Get1(object instance)
	{
		return GCHandledObjects.ObjectToGCHandle(((UIDragScrollView)instance).draggablePanel);
	}

	public static void $Set1(object instance, long value)
	{
		((UIDragScrollView)instance).draggablePanel = (UIScrollView)GCHandledObjects.GCHandleToObject(value);
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UIDragScrollView)GCHandledObjects.GCHandleToObject(instance)).FindScrollView();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UIDragScrollView)GCHandledObjects.GCHandleToObject(instance)).OnDrag(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UIDragScrollView)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UIDragScrollView)GCHandledObjects.GCHandleToObject(instance)).OnPan(*(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UIDragScrollView)GCHandledObjects.GCHandleToObject(instance)).OnPress(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UIDragScrollView)GCHandledObjects.GCHandleToObject(instance)).OnScroll(*(float*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UIDragScrollView)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UIDragScrollView)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UIDragScrollView)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UIDragScrollView)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UIDragScrollView)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UIDragScrollView)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
