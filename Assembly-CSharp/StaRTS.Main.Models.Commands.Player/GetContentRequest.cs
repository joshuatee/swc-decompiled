using StaRTS.Externals.Manimal.TransferObjects.Request;
using System;

namespace StaRTS.Main.Models.Commands.Player
{
	public class GetContentRequest : PlayerIdRequest
	{
		public GetContentRequest()
		{
		}

		protected internal GetContentRequest(UIntPtr dummy) : base(dummy)
		{
		}
	}
}
