using StaRTS.Main.Controllers;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Player.Objectives
{
	public class ObjectiveProgress : ISerializable
	{
		public string ObjectiveUid;

		public string PlanetId;

		public int HQ;

		public int Count;

		public int Target;

		public ObjectiveState State;

		public bool ClaimAttempt;

		public ObjectiveVO VO
		{
			get
			{
				return Service.Get<IDataController>().Get<ObjectiveVO>(this.ObjectiveUid);
			}
		}

		public ObjectiveProgress(ObjectiveProgress cloneFrom)
		{
			this.HQ = 1;
			this.Target = 1;
			base..ctor();
			this.ObjectiveUid = cloneFrom.ObjectiveUid;
			this.PlanetId = cloneFrom.PlanetId;
			this.HQ = cloneFrom.HQ;
			this.Count = cloneFrom.Count;
			this.Target = cloneFrom.Target;
			this.State = cloneFrom.State;
		}

		public ObjectiveProgress(string planetId)
		{
			this.HQ = 1;
			this.Target = 1;
			base..ctor();
			this.PlanetId = planetId;
		}

		public string ToJson()
		{
			Service.Get<StaRTSLogger>().Warn("Attempting to inappropriately serialize an ObjectiveGroup");
			return string.Empty;
		}

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("uid"))
			{
				this.ObjectiveUid = (dictionary["uid"] as string);
			}
			if (dictionary.ContainsKey("hq"))
			{
				this.HQ = Convert.ToInt32(dictionary["hq"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("count"))
			{
				this.Count = Convert.ToInt32(dictionary["count"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("target"))
			{
				this.Target = Convert.ToInt32(dictionary["target"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("state"))
			{
				this.State = StringUtils.ParseEnum<ObjectiveState>(dictionary["state"] as string);
			}
			return this;
		}

		protected internal ObjectiveProgress(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveProgress)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveProgress)GCHandledObjects.GCHandleToObject(instance)).VO);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ObjectiveProgress)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
