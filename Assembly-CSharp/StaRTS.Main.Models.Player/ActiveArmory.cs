using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Player
{
	public class ActiveArmory : ISerializable
	{
		public List<string> Equipment
		{
			get;
			private set;
		}

		public int MaxCapacity
		{
			get;
			private set;
		}

		public string ToJson()
		{
			return "{}";
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return this;
			}
			if (dictionary.ContainsKey("equipment"))
			{
				List<object> list = dictionary["equipment"] as List<object>;
				if (list != null)
				{
					this.Equipment = new List<string>();
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						this.Equipment.Add(list[i] as string);
						i++;
					}
				}
			}
			if (dictionary.ContainsKey("capacity"))
			{
				this.MaxCapacity = Convert.ToInt32(dictionary["capacity"], CultureInfo.InvariantCulture);
			}
			return this;
		}

		public void SetMaxEquipmentCapacity(int capacity)
		{
			this.MaxCapacity = capacity;
		}

		public ActiveArmory()
		{
		}

		protected internal ActiveArmory(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ActiveArmory)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ActiveArmory)GCHandledObjects.GCHandleToObject(instance)).Equipment);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ActiveArmory)GCHandledObjects.GCHandleToObject(instance)).MaxCapacity);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ActiveArmory)GCHandledObjects.GCHandleToObject(instance)).Equipment = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ActiveArmory)GCHandledObjects.GCHandleToObject(instance)).MaxCapacity = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((ActiveArmory)GCHandledObjects.GCHandleToObject(instance)).SetMaxEquipmentCapacity(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ActiveArmory)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
