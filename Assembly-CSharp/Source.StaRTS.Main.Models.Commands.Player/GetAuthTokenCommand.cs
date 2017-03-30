using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Models.Commands;
using System;

namespace Source.StaRTS.Main.Models.Commands.Player
{
	internal class GetAuthTokenCommand : GameCommand<GetAuthTokenRequest, GetAuthTokenResponse>
	{
		private const string ACTION = "auth.getAuthToken";

		public GetAuthTokenCommand(GetAuthTokenRequest request) : base("auth.getAuthToken", request, new GetAuthTokenResponse())
		{
		}

		public override OnCompleteAction OnFailure(uint status, object data)
		{
			base.OnFailure(status, data);
			if (status == 800u || status == 801u)
			{
				return OnCompleteAction.Skip;
			}
			return OnCompleteAction.Retry;
		}
	}
}
