using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using WinRTBridge;

[AddComponentMenu("NGUI/Interaction/Language Selection"), RequireComponent(typeof(UIPopupList))]
public class LanguageSelection : MonoBehaviour, IUnitySerializable
{
	[CompilerGenerated]
	[System.Serializable]
	private sealed class <>c
	{
		public static readonly LanguageSelection.<>c <>9 = new LanguageSelection.<>c();

		public static EventDelegate.Callback <>9__2_0;

		internal void <Start>b__2_0()
		{
			Localization.language = UIPopupList.current.value;
		}
	}

	private UIPopupList mList;

	private void Awake()
	{
		this.mList = base.GetComponent<UIPopupList>();
		this.Refresh();
	}

	private void Start()
	{
		List<EventDelegate> arg_2A_0 = this.mList.onChange;
		EventDelegate.Callback arg_2A_1;
		if ((arg_2A_1 = LanguageSelection.<>c.<>9__2_0) == null)
		{
			arg_2A_1 = (LanguageSelection.<>c.<>9__2_0 = new EventDelegate.Callback(LanguageSelection.<>c.<>9.<Start>b__2_0));
		}
		EventDelegate.Add(arg_2A_0, arg_2A_1);
	}

	public void Refresh()
	{
		if (this.mList != null && Localization.knownLanguages != null)
		{
			this.mList.Clear();
			int i = 0;
			int num = Localization.knownLanguages.Length;
			while (i < num)
			{
				this.mList.items.Add(Localization.knownLanguages[i]);
				i++;
			}
			this.mList.value = Localization.language;
		}
	}

	public LanguageSelection()
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

	protected internal LanguageSelection(UIntPtr dummy) : base(dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((LanguageSelection)GCHandledObjects.GCHandleToObject(instance)).Awake();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((LanguageSelection)GCHandledObjects.GCHandleToObject(instance)).Refresh();
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		((LanguageSelection)GCHandledObjects.GCHandleToObject(instance)).Start();
		return -1L;
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		((LanguageSelection)GCHandledObjects.GCHandleToObject(instance)).Unity_Deserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((LanguageSelection)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedDeserialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((LanguageSelection)GCHandledObjects.GCHandleToObject(instance)).Unity_NamedSerialize(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((LanguageSelection)GCHandledObjects.GCHandleToObject(instance)).Unity_RemapPPtrs(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((LanguageSelection)GCHandledObjects.GCHandleToObject(instance)).Unity_Serialize(*(int*)args);
		return -1L;
	}
}
