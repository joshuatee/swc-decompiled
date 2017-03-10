using System;
using UnityEngine;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Click")]
public class UICenterOnClick : MonoBehaviour, IUnitySerializable
{
	private void OnClick()
	{
		UICenterOnChild uICenterOnChild = NGUITools.FindInParents<UICenterOnChild>(base.gameObject);
		UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		if (uICenterOnChild != null)
		{
			if (uICenterOnChild.enabled)
			{
				uICenterOnChild.CenterOn(base.transform);
				return;
			}
		}
		else if (uIPanel != null && uIPanel.clipping != UIDrawCall.Clipping.None)
		{
			UIScrollView component = uIPanel.GetComponent<UIScrollView>();
			Vector3 pos = -uIPanel.cachedTransform.InverseTransformPoint(base.transform.position);
			if (!component.canMoveHorizontally)
			{
				pos.x = uIPanel.cachedTransform.localPosition.x;
			}
			if (!component.canMoveVertically)
			{
				pos.y = uIPanel.cachedTransform.localPosition.y;
			}
			SpringPanel.Begin(uIPanel.cachedGameObject, pos, 6f);
		}
	}

	public UICenterOnClick()
	{
	}

	public override void Unity_Serialize(int depth)
	{
	}

	public override void Unity_Deserialize(int depth)
	{
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public override void Unity_NamedSerialize(int depth)
	{
	}

	public override void Unity_NamedDeserialize(int depth)
	{
	}

	protected internal UICenterOnClick(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UICenterOnClick)GCHandledObjects.GCHandleToObject(instance)).OnClick();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UICenterOnClick)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UICenterOnClick)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UICenterOnClick)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UICenterOnClick)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UICenterOnClick)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
