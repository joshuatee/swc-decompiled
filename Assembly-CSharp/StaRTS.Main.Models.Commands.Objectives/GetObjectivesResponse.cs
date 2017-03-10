using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Player.Objectives;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Objectives
{
	public class GetObjectivesResponse : AbstractResponse
	{
		public Dictionary<string, ObjectiveGroup> Groups;

		public override ISerializable FromObject(object obj)
		{
			this.Groups = new Dictionary<string, ObjectiveGroup>();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				foreach (KeyValuePair<string, object> current in dictionary)
				{
					this.Groups.Add(current.get_Key(), new ObjectiveGroup(current.get_Key()).FromObject(current.get_Value()) as ObjectiveGroup);
				}
			}
			return this;
		}

		public GetObjectivesResponse()
		{
		}

		protected internal GetObjectivesResponse(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GetObjectivesResponse)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
