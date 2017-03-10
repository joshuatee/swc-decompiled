using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/UI/Localize"), ExecuteInEditMode, RequireComponent(typeof(UIWidget))]
public class UILocalize : MonoBehaviour, IUnitySerializable
{
	public string key;

	private bool mStarted;

	public string value
	{
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				UIWidget component = base.GetComponent<UIWidget>();
				UILabel uILabel = component as UILabel;
				UISprite uISprite = component as UISprite;
				if (uILabel != null)
				{
					UIInput uIInput = NGUITools.FindInParents<UIInput>(uILabel.gameObject);
					if (uIInput != null && uIInput.label == uILabel)
					{
						uIInput.defaultText = value;
						return;
					}
					uILabel.text = value;
					return;
				}
				else if (uISprite != null)
				{
					UIButton uIButton = NGUITools.FindInParents<UIButton>(uISprite.gameObject);
					if (uIButton != null && uIButton.tweenTarget == uISprite.gameObject)
					{
						uIButton.normalSprite = value;
					}
					uISprite.spriteName = value;
					uISprite.MakePixelPerfect();
				}
			}
		}
	}

	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnLocalize();
		}
	}

	private void Start()
	{
		this.mStarted = true;
		this.OnLocalize();
	}

	private void OnLocalize()
	{
		if (string.IsNullOrEmpty(this.key))
		{
			UILabel component = base.GetComponent<UILabel>();
			if (component != null)
			{
				this.key = component.text;
			}
		}
		if (!string.IsNullOrEmpty(this.key))
		{
			this.value = Localization.Get(this.key);
		}
	}

	public UILocalize()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteString(this.key);
	}

	public override void Unity_Deserialize(int depth)
	{
		this.key = (SerializedStateReader.Instance.ReadString() as string);
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		SerializedNamedStateWriter.Instance.WriteString(this.key, &$FieldNamesStorage.$RuntimeNames[0] + 4085);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		this.key = (SerializedNamedStateReader.Instance.ReadString(&$FieldNamesStorage.$RuntimeNames[0] + 4085) as string);
	}

	protected internal UILocalize(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UILocalize)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((UILocalize)GCHandledObjects.GCHandleToObject(instance)).OnLocalize();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UILocalize)GCHandledObjects.GCHandleToObject(instance)).value = Marshal.PtrToStringUni(*(IntPtr*)args);
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UILocalize)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UILocalize)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UILocalize)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UILocalize)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UILocalize)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UILocalize)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
