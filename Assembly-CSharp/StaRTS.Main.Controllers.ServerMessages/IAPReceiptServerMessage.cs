using StaRTS.Main.Utils.Events;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Controllers.ServerMessages
{
	public class IAPReceiptServerMessage : AbstractMessage
	{
		public override object MessageCookie
		{
			get
			{
				return this;
			}
		}

		public override EventId MessageEventId
		{
			get
			{
				return EventId.IAPReceiptServerMessage;
			}
		}

		public string TransactionId
		{
			get;
			set;
		}

		public string IapUID
		{
			get;
			private set;
		}

		public double Price
		{
			get;
			private set;
		}

		public string OfferUID
		{
			get;
			private set;
		}

		public double BonusMultiplier
		{
			get;
			private set;
		}

		public bool IsPromo
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = null;
			Dictionary<string, object> dictionary2 = null;
			this.IapUID = "";
			this.Price = 0.0;
			this.BonusMultiplier = 1.0;
			this.OfferUID = null;
			Dictionary<string, object> dictionary3 = obj as Dictionary<string, object>;
			if (dictionary3.ContainsKey("sale"))
			{
				dictionary = (dictionary3["iap"] as Dictionary<string, object>);
			}
			if (dictionary3.ContainsKey("isPromo"))
			{
				this.IsPromo = (bool)dictionary3["isPromo"];
			}
			if (dictionary3.ContainsKey("sale"))
			{
				dictionary2 = (dictionary3["sale"] as Dictionary<string, object>);
			}
			if (dictionary2 != null && dictionary2.ContainsKey("bonusMultiplier"))
			{
				this.BonusMultiplier = Convert.ToDouble(dictionary2["bonusMultiplier"]);
			}
			if (dictionary != null)
			{
				if (dictionary.ContainsKey("uid"))
				{
					this.IapUID = (dictionary["uid"] as string);
				}
				if (dictionary.ContainsKey("price"))
				{
					this.Price = Convert.ToDouble(dictionary["price"]);
				}
			}
			if (dictionary3.ContainsKey("targetedOffer"))
			{
				this.OfferUID = (dictionary3["targetedOffer"] as string);
			}
			return this;
		}

		public IAPReceiptServerMessage()
		{
		}

		protected internal IAPReceiptServerMessage(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).IapUID);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).IsPromo);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).MessageCookie);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).MessageEventId);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).OfferUID);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).TransactionId);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).BonusMultiplier = *(double*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).IapUID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).IsPromo = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).OfferUID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).Price = *(double*)args;
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((IAPReceiptServerMessage)GCHandledObjects.GCHandleToObject(instance)).TransactionId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}
	}
}
