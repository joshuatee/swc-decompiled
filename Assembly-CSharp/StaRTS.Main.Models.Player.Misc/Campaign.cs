using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Misc
{
	public class Campaign : AbstractTimedEvent
	{
		public bool Completed
		{
			get;
			set;
		}

		public float TimeZone
		{
			get;
			set;
		}

		public uint Points
		{
			get;
			set;
		}

		public Dictionary<string, int> Purchases
		{
			get;
			set;
		}

		public override ISerializable FromObject(object obj)
		{
			base.FromObject(obj);
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			this.Completed = (bool)dictionary["completed"];
			this.TimeZone = Convert.ToSingle(dictionary["timeZone"], CultureInfo.InvariantCulture);
			if (dictionary.ContainsKey("points"))
			{
				this.Points = Convert.ToUInt32(dictionary["points"], CultureInfo.InvariantCulture);
			}
			else
			{
				this.Points = 0u;
			}
			if (dictionary.ContainsKey("items"))
			{
				Dictionary<string, object> dictionary2 = dictionary["items"] as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					this.Purchases = new Dictionary<string, int>();
					foreach (KeyValuePair<string, object> current in dictionary2)
					{
						this.Purchases.Add(current.get_Key(), Convert.ToInt32(current.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			return this;
		}

		public Campaign()
		{
		}

		protected internal Campaign(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Campaign)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Campaign)GCHandledObjects.GCHandleToObject(instance)).Completed);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Campaign)GCHandledObjects.GCHandleToObject(instance)).Purchases);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Campaign)GCHandledObjects.GCHandleToObject(instance)).TimeZone);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Campaign)GCHandledObjects.GCHandleToObject(instance)).Completed = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((Campaign)GCHandledObjects.GCHandleToObject(instance)).Purchases = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((Campaign)GCHandledObjects.GCHandleToObject(instance)).TimeZone = *(float*)args;
			return -1L;
		}
	}
}
