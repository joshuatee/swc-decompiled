using StaRTS.Externals.Manimal.TransferObjects.Request;
using System;

namespace StaRTS.Main.Models.Commands.Player.Account.External
{
	public class RegisterExternalAccountCommand : GameCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>
	{
		public const string ACTION = "player.account.external.register";

		public RegisterExternalAccountCommand(RegisterExternalAccountRequest request) : base("player.account.external.register", request, new RegisterExternalAccountResponse())
		{
		}

		public override OnCompleteAction OnFailure(uint status, object data)
		{
			base.OnFailure(status, data);
			if (status == 1318u || status == 2200u || status == 2201u)
			{
				return OnCompleteAction.Ok;
			}
			return OnCompleteAction.Desync;
		}

		protected internal RegisterExternalAccountCommand(UIntPtr dummy) : base(dummy)
		{
		}
	}
}
