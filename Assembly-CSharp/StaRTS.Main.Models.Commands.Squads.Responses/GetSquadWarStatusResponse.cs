using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Squads.Responses
{
	public class GetSquadWarStatusResponse : AbstractResponse
	{
		public string Id
		{
			get;
			private set;
		}

		public object Squad1Data
		{
			get;
			private set;
		}

		public object Squad2Data
		{
			get;
			private set;
		}

		public object BuffBaseData
		{
			get;
			private set;
		}

		public int PrepGraceStartTimeStamp
		{
			get;
			private set;
		}

		public int PrepEndTimeStamp
		{
			get;
			private set;
		}

		public int ActionGraceStartTimeStamp
		{
			get;
			private set;
		}

		public int ActionEndTimeStamp
		{
			get;
			private set;
		}

		public int CooldownEndTimeStamp
		{
			get;
			private set;
		}

		public int StartTimeStamp
		{
			get;
			private set;
		}

		public bool RewardsProcessed
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
			if (dictionary.ContainsKey("id"))
			{
				this.Id = Convert.ToString(dictionary["id"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("guild"))
			{
				this.Squad1Data = dictionary["guild"];
			}
			if (dictionary.ContainsKey("rival"))
			{
				this.Squad2Data = dictionary["rival"];
			}
			if (dictionary.ContainsKey("buffBases"))
			{
				this.BuffBaseData = dictionary["buffBases"];
			}
			if (dictionary.ContainsKey("prepGraceStartTime"))
			{
				this.PrepGraceStartTimeStamp = Convert.ToInt32(dictionary["prepGraceStartTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("prepEndTime"))
			{
				this.PrepEndTimeStamp = Convert.ToInt32(dictionary["prepEndTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("actionGraceStartTime"))
			{
				this.ActionGraceStartTimeStamp = Convert.ToInt32(dictionary["actionGraceStartTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("actionEndTime"))
			{
				this.ActionEndTimeStamp = Convert.ToInt32(dictionary["actionEndTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("cooldownEndTime"))
			{
				this.CooldownEndTimeStamp = Convert.ToInt32(dictionary["cooldownEndTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("startTime"))
			{
				this.StartTimeStamp = Convert.ToInt32(dictionary["startTime"], CultureInfo.InvariantCulture);
			}
			else
			{
				this.StartTimeStamp = this.PrepEndTimeStamp;
			}
			if (dictionary.ContainsKey("rewardsProcessed"))
			{
				this.RewardsProcessed = Convert.ToBoolean(dictionary["rewardsProcessed"], CultureInfo.InvariantCulture);
			}
			return this;
		}

		public GetSquadWarStatusResponse()
		{
		}

		protected internal GetSquadWarStatusResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).ActionEndTimeStamp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).ActionGraceStartTimeStamp);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).BuffBaseData);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).CooldownEndTimeStamp);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).Id);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).PrepEndTimeStamp);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).PrepGraceStartTimeStamp);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).RewardsProcessed);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).Squad1Data);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).Squad2Data);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).StartTimeStamp);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).ActionEndTimeStamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).ActionGraceStartTimeStamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).BuffBaseData = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).CooldownEndTimeStamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).Id = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).PrepEndTimeStamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).PrepGraceStartTimeStamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).RewardsProcessed = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).Squad1Data = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).Squad2Data = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((GetSquadWarStatusResponse)GCHandledObjects.GCHandleToObject(instance)).StartTimeStamp = *(int*)args;
			return -1L;
		}
	}
}
