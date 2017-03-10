using System;
using WinRTBridge;

public class TapjoyEventRequest
{
	private string eventGuid;

	public int type;

	public string identifier;

	public int quantity;

	public const int TYPE_IN_APP_PURCHASE = 1;

	public const int TYPE_VIRTUAL_GOOD = 2;

	public const int TYPE_CURRENCY = 3;

	public const int TYPE_NAVIGATION = 4;

	public TapjoyEventRequest(string eventGuid, int type, string identifier, int quantity)
	{
		this.eventGuid = eventGuid;
		this.type = type;
		this.identifier = identifier;
		this.quantity = quantity;
	}

	public void EventRequestCompleted()
	{
		TapjoyPlugin.EventRequestCompleted(this.eventGuid);
	}

	public void EventRequestCancelled()
	{
		TapjoyPlugin.EventRequestCancelled(this.eventGuid);
	}

	protected internal TapjoyEventRequest(UIntPtr dummy)
	{
	}

	public unsafe static long $Invoke0(long instance, long* args)
	{
		((TapjoyEventRequest)GCHandledObjects.GCHandleToObject(instance)).EventRequestCancelled();
		return -1L;
	}

	public unsafe static long $Invoke1(long instance, long* args)
	{
		((TapjoyEventRequest)GCHandledObjects.GCHandleToObject(instance)).EventRequestCompleted();
		return -1L;
	}
}
