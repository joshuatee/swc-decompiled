using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Objectives
{
	public class ObjectiveGroup : ISerializable
	{
		private string planetId;

		public string GroupId
		{
			get;
			set;
		}

		public string GroupSeriesId
		{
			get;
			private set;
		}

		public int StartTimestamp
		{
			get;
			set;
		}

		public int GraceTimestamp
		{
			get;
			set;
		}

		public int EndTimestamp
		{
			get;
			set;
		}

		public List<ObjectiveProgress> ProgressObjects
		{
			get;
			set;
		}

		public ObjectiveGroup(ObjectiveGroup cloneFrom)
		{
			this.GroupId = cloneFrom.GroupId;
			this.GroupSeriesId = this.GroupId.Substring(0, this.GroupId.LastIndexOf('_'));
			this.StartTimestamp = cloneFrom.StartTimestamp;
			this.GraceTimestamp = cloneFrom.GraceTimestamp;
			this.EndTimestamp = cloneFrom.EndTimestamp;
			this.ProgressObjects = new List<ObjectiveProgress>();
			foreach (ObjectiveProgress current in cloneFrom.ProgressObjects)
			{
				this.ProgressObjects.Add(new ObjectiveProgress(current));
			}
			this.planetId = cloneFrom.planetId;
		}

		public ObjectiveGroup(string planetId)
		{
			this.planetId = planetId;
			this.ProgressObjects = new List<ObjectiveProgress>();
		}

		public string ToJson()
		{
			Service.Get<StaRTSLogger>().Warn("Attempting to inappropriately serialize an ObjectiveGroup");
			return string.Empty;
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("groupId"))
			{
				this.GroupId = Convert.ToString(dictionary["groupId"], CultureInfo.InvariantCulture);
				this.GroupSeriesId = this.GroupId.Substring(0, this.GroupId.LastIndexOf('_'));
			}
			if (dictionary.ContainsKey("startTime"))
			{
				this.StartTimestamp = Convert.ToInt32(dictionary["startTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("graceTime"))
			{
				this.GraceTimestamp = Convert.ToInt32(dictionary["graceTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("endTime"))
			{
				this.EndTimestamp = Convert.ToInt32(dictionary["endTime"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("progress"))
			{
				List<object> list = dictionary["progress"] as List<object>;
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					this.ProgressObjects.Add(new ObjectiveProgress(this.planetId).FromObject(list[i]) as ObjectiveProgress);
					i++;
				}
			}
			return this;
		}

		protected internal ObjectiveGroup(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).EndTimestamp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).GraceTimestamp);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).GroupId);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).GroupSeriesId);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).ProgressObjects);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).StartTimestamp);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).EndTimestamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).GraceTimestamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).GroupId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).GroupSeriesId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).ProgressObjects = (List<ObjectiveProgress>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).StartTimestamp = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveGroup)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
