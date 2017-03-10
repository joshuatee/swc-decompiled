using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[ExecuteInEditMode]
public class AnimatedAlpha : MonoBehaviour, IUnitySerializable
{
	[Range(0f, 1f)]
	public float alpha;

	private UIWidget mWidget;

	private UIPanel mPanel;

	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.mPanel = base.GetComponent<UIPanel>();
		this.LateUpdate();
	}

	private void LateUpdate()
	{
		if (this.mWidget != null)
		{
			this.mWidget.alpha = this.alpha;
		}
		if (this.mPanel != null)
		{
			this.mPanel.alpha = this.alpha;
		}
	}

	public AnimatedAlpha()
	{
		this.alpha = 1f;
		base..ctor();
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteSingle(this.alpha);
	}

	public override void Unity_Deserialize(int depth)
	{
		this.alpha = SerializedStateReader.Instance.ReadSingle();
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		SerializedNamedStateWriter.Instance.WriteSingle(this.alpha, &$FieldNamesStorage.$RuntimeNames[0] + 2630);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		this.alpha = SerializedNamedStateReader.Instance.ReadSingle(&$FieldNamesStorage.$RuntimeNames[0] + 2630);
	}

	protected internal AnimatedAlpha(UIntPtr dummy) : base(dummy)
	{
	}

	public static float $Get0(object instance)
	{
		return ((AnimatedAlpha)instance).alpha;
	}

	public static void $Set0(object instance, float value)
	{
		((AnimatedAlpha)instance).alpha = value;
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((AnimatedAlpha)GCHandledObjects.GCHandleToObject(instance)).LateUpdate();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((AnimatedAlpha)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((AnimatedAlpha)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((AnimatedAlpha)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((AnimatedAlpha)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((AnimatedAlpha)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((AnimatedAlpha)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
