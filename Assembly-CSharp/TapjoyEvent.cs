using System;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

public class TapjoyEvent
{
	private string myGuid;

	private string myName;

	private string myParameter;

	private bool isContentReady;

	private ITapjoyEvent myCallback;

	public TapjoyEvent(string eventName, ITapjoyEvent callback) : this(eventName, null, callback)
	{
	}

	public TapjoyEvent(string eventName, string eventParameter, ITapjoyEvent callback)
	{
		this.myName = eventName;
		this.myParameter = eventParameter;
		this.myCallback = callback;
		this.myGuid = TapjoyPlugin.CreateEvent(this, eventName, eventParameter);
		Debug.Log(string.Format("C#: Event {0} created with GUID:{1} with Param:{2}", new object[]
		{
			this.myName,
			this.myGuid,
			this.myParameter
		}));
	}

	public void Send()
	{
		Debug.Log(string.Format("C#: Sending event {0} ", new object[]
		{
			this.myName
		}));
		TapjoyPlugin.SendEvent(this.myGuid);
	}

	public void Show()
	{
		TapjoyPlugin.ShowEvent(this.myGuid);
	}

	public void EnableAutoPresent(bool autoPresent)
	{
		TapjoyPlugin.EnableEventAutoPresent(this.myGuid, autoPresent);
	}

	public void EnablePreload(bool preload)
	{
		TapjoyPlugin.EnableEventPreload(this.myGuid, preload);
	}

	public bool IsContentReady()
	{
		return this.isContentReady;
	}

	public string GetName()
	{
		return this.myName;
	}

	public void TriggerSendEventSucceeded(bool contentIsAvailable)
	{
		Debug.Log("C#: TriggerSendEventSucceeded");
		this.myCallback.SendEventSucceeded(this, contentIsAvailable);
	}

	public void TriggerContentIsReady(int status)
	{
		Debug.Log("C#: TriggerContentIsReady with status" + status);
		this.isContentReady = true;
		this.myCallback.ContentIsReady(this, status);
	}

	public void TriggerSendEventFailed(string errorMsg)
	{
		Debug.Log("C#: TriggerSendEventFailed");
		this.myCallback.SendEventFailed(this, errorMsg);
	}

	public void TriggerContentDidAppear()
	{
		Debug.Log("C#: TriggerContentDidAppear");
		this.myCallback.ContentDidAppear(this);
	}

	public void TriggerContentDidDisappear()
	{
		Debug.Log("C#: TriggerContentDidDisappear");
		this.myCallback.ContentDidDisappear(this);
	}

	public void TriggerDidRequestAction(int type, string identifier, int quantity)
	{
		Debug.Log("C#: TriggerDidRequestAction");
		TapjoyEventRequest tapjoyEventRequest = new TapjoyEventRequest(this.myGuid, type, identifier, quantity);
		this.myCallback.DidRequestAction(this, tapjoyEventRequest);
	}

	protected internal TapjoyEvent(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((TapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).EnableAutoPresent(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((TapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).EnablePreload(*(sbyte*)args != 0);
		return -1L;
	}

	public unsafe static long $Invoke2(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((TapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).GetName());
	}

	public unsafe static long $Invoke3(long instance, long* args)
	{
		return GCHandledObjects.ObjectToGCHandle(((TapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).IsContentReady());
	}

	public unsafe static long $Invoke4(long instance, long* args)
	{
		((TapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).Send();
		return -1L;
	}

	public unsafe static long $Invoke5(long instance, long* args)
	{
		((TapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).Show();
		return -1L;
	}

	public unsafe static long $Invoke6(long instance, long* args)
	{
		((TapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).TriggerContentDidAppear();
		return -1L;
	}

	public unsafe static long $Invoke7(long instance, long* args)
	{
		((TapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).TriggerContentDidDisappear();
		return -1L;
	}

	public unsafe static long $Invoke8(long instance, long* args)
	{
		((TapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).TriggerContentIsReady(*(int*)args);
		return -1L;
	}

	public unsafe static long $Invoke9(long instance, long* args)
	{
		((TapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).TriggerDidRequestAction(*(int*)args, Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), *(int*)(args + 2));
		return -1L;
	}

	public unsafe static long $Invoke10(long instance, long* args)
	{
		((TapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).TriggerSendEventFailed(Marshal.PtrToStringUni(*(IntPtr*)args));
		return -1L;
	}

	public unsafe static long $Invoke11(long instance, long* args)
	{
		((TapjoyEvent)GCHandledObjects.GCHandleToObject(instance)).TriggerSendEventSucceeded(*(sbyte*)args != 0);
		return -1L;
	}
}
