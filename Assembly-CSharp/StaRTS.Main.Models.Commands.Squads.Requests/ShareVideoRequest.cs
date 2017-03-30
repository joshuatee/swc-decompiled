using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;

namespace StaRTS.Main.Models.Commands.Squads.Requests
{
	public class ShareVideoRequest : PlayerIdRequest
	{
		public string Url
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", base.PlayerId);
			serializer.AddString("url", this.Url);
			serializer.AddString("message", this.Message);
			return serializer.End().ToString();
		}
	}
}
