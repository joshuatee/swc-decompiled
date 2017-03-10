using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Squads.War;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Responses
{
	public class SquadMemberWarDataResponse : AbstractResponse
	{
		public SquadMemberWarData MemberWarData
		{
			get;
			private set;
		}

		public Dictionary<string, int> DonatedSquadTroops
		{
			get;
			private set;
		}

		public Dictionary<string, int> Champions
		{
			get;
			private set;
		}

		public List<string> Equipment
		{
			get;
			private set;
		}

		public uint ScoutingStatus
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return this;
			}
			this.MemberWarData = new SquadMemberWarData();
			this.MemberWarData.FromObject(obj);
			if (dictionary.ContainsKey("donatedTroops"))
			{
				this.DonatedSquadTroops = new Dictionary<string, int>();
				Dictionary<string, object> dictionary2 = dictionary["donatedTroops"] as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					foreach (KeyValuePair<string, object> current in dictionary2)
					{
						string key = current.get_Key();
						int num = 0;
						Dictionary<string, object> dictionary3 = current.get_Value() as Dictionary<string, object>;
						if (dictionary3 != null)
						{
							foreach (KeyValuePair<string, object> current2 in dictionary3)
							{
								num += Convert.ToInt32(current2.get_Value(), CultureInfo.InvariantCulture);
							}
							this.DonatedSquadTroops.Add(key, num);
						}
					}
				}
			}
			if (dictionary.ContainsKey("champions"))
			{
				this.Champions = new Dictionary<string, int>();
				Dictionary<string, object> dictionary4 = dictionary["champions"] as Dictionary<string, object>;
				if (dictionary4 != null)
				{
					foreach (KeyValuePair<string, object> current3 in dictionary4)
					{
						string key2 = current3.get_Key();
						this.Champions.Add(key2, Convert.ToInt32(current3.get_Value(), CultureInfo.InvariantCulture));
					}
				}
			}
			if (dictionary.ContainsKey("scoutingStatus"))
			{
				object obj2 = dictionary["scoutingStatus"];
				if (obj2 != null)
				{
					Dictionary<string, object> dictionary5 = obj2 as Dictionary<string, object>;
					this.ScoutingStatus = Convert.ToUInt32(dictionary5["code"], CultureInfo.InvariantCulture);
				}
			}
			if (dictionary.ContainsKey("equipment"))
			{
				this.Equipment = new List<string>();
				List<object> list = dictionary["equipment"] as List<object>;
				if (list != null)
				{
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						this.Equipment.Add(list[i] as string);
						i++;
					}
				}
			}
			return this;
		}

		public SquadMemberWarDataResponse()
		{
		}

		protected internal SquadMemberWarDataResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMemberWarDataResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMemberWarDataResponse)GCHandledObjects.GCHandleToObject(instance)).Champions);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMemberWarDataResponse)GCHandledObjects.GCHandleToObject(instance)).DonatedSquadTroops);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMemberWarDataResponse)GCHandledObjects.GCHandleToObject(instance)).Equipment);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadMemberWarDataResponse)GCHandledObjects.GCHandleToObject(instance)).MemberWarData);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((SquadMemberWarDataResponse)GCHandledObjects.GCHandleToObject(instance)).Champions = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((SquadMemberWarDataResponse)GCHandledObjects.GCHandleToObject(instance)).DonatedSquadTroops = (Dictionary<string, int>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((SquadMemberWarDataResponse)GCHandledObjects.GCHandleToObject(instance)).Equipment = (List<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((SquadMemberWarDataResponse)GCHandledObjects.GCHandleToObject(instance)).MemberWarData = (SquadMemberWarData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
