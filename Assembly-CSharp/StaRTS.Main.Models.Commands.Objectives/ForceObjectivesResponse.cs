using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player.Objectives;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Objectives
{
	public class ForceObjectivesResponse : AbstractResponse
	{
		private string planetUid;

		public ObjectiveGroup Group;

		public ForceObjectivesResponse(string planetUid)
		{
			this.planetUid = planetUid;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				this.Group = (new ObjectiveGroup(this.planetUid).FromObject(dictionary) as ObjectiveGroup);
			}
			return this;
		}

		protected internal ForceObjectivesResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ForceObjectivesResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
