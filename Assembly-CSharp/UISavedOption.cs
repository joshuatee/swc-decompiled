using System;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Serialization;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour, IUnitySerializable
{
	public string keyName;

	private UIPopupList mList;

	private UIToggle mCheck;

	private UIProgressBar mSlider;

	private string key
	{
		get
		{
			if (!string.IsNullOrEmpty(this.keyName))
			{
				return this.keyName;
			}
			return "NGUI State: " + base.name;
		}
	}

	private void Awake()
	{
		this.mList = base.GetComponent<UIPopupList>();
		this.mCheck = base.GetComponent<UIToggle>();
		this.mSlider = base.GetComponent<UIProgressBar>();
	}

	private void OnEnable()
	{
		if (this.mList != null)
		{
			EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
			string @string = PlayerPrefs.GetString(this.key);
			if (!string.IsNullOrEmpty(@string))
			{
				this.mList.value = @string;
				return;
			}
		}
		else
		{
			if (this.mCheck != null)
			{
				EventDelegate.Add(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
				this.mCheck.value = (PlayerPrefs.GetInt(this.key, this.mCheck.startsActive ? 1 : 0) != 0);
				return;
			}
			if (this.mSlider != null)
			{
				EventDelegate.Add(this.mSlider.onChange, new EventDelegate.Callback(this.SaveProgress));
				this.mSlider.value = PlayerPrefs.GetFloat(this.key, this.mSlider.value);
				return;
			}
			string string2 = PlayerPrefs.GetString(this.key);
			UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				UIToggle uIToggle = componentsInChildren[i];
				uIToggle.value = (uIToggle.name == string2);
				i++;
			}
		}
	}

	private void OnDisable()
	{
		if (this.mCheck != null)
		{
			EventDelegate.Remove(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
			return;
		}
		if (this.mList != null)
		{
			EventDelegate.Remove(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
			return;
		}
		if (this.mSlider != null)
		{
			EventDelegate.Remove(this.mSlider.onChange, new EventDelegate.Callback(this.SaveProgress));
			return;
		}
		UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
		int i = 0;
		int num = componentsInChildren.Length;
		while (i < num)
		{
			UIToggle uIToggle = componentsInChildren[i];
			if (uIToggle.value)
			{
				PlayerPrefs.SetString(this.key, uIToggle.name);
				return;
			}
			i++;
		}
	}

	public void SaveSelection()
	{
		PlayerPrefs.SetString(this.key, UIPopupList.current.value);
	}

	public void SaveState()
	{
		PlayerPrefs.SetInt(this.key, UIToggle.current.value ? 1 : 0);
	}

	public void SaveProgress()
	{
		PlayerPrefs.SetFloat(this.key, UIProgressBar.current.value);
	}

	public UISavedOption()
	{
	}

	public override void Unity_Serialize(int depth)
	{
		SerializedStateWriter.Instance.WriteString(this.keyName);
	}

	public override void Unity_Deserialize(int depth)
	{
		this.keyName = (SerializedStateReader.Instance.ReadString() as string);
	}

	public override void Unity_RemapPPtrs(int depth)
	{
	}

	public unsafe override void Unity_NamedSerialize(int depth)
	{
		SerializedNamedStateWriter.Instance.WriteString(this.keyName, &$FieldNamesStorage.$RuntimeNames[0] + 1611);
	}

	public unsafe override void Unity_NamedDeserialize(int depth)
	{
		this.keyName = (SerializedNamedStateReader.Instance.ReadString(&$FieldNamesStorage.$RuntimeNames[0] + 1611) as string);
	}

	protected internal UISavedOption(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((UISavedOption)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((UISavedOption)GCHandledObjects.GCHandleToObject(instance)).key);
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((UISavedOption)GCHandledObjects.GCHandleToObject(instance)).OnDisable();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((UISavedOption)GCHandledObjects.GCHandleToObject(instance)).OnEnable();
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((UISavedOption)GCHandledObjects.GCHandleToObject(instance)).SaveProgress();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((UISavedOption)GCHandledObjects.GCHandleToObject(instance)).SaveSelection();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((UISavedOption)GCHandledObjects.GCHandleToObject(instance)).SaveState();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((UISavedOption)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((UISavedOption)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((UISavedOption)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((UISavedOption)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((UISavedOption)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
