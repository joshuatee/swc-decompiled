using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands.Player.Account.External;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using System;

namespace StaRTS.Main.Controllers
{
	public interface IAccountSyncController
	{
		void UpdateExternalAccountInfo(OnUpdateExternalAccountInfoResponseReceived callback);

		void LoadAccount(string playerId, string playerSecret);

		string GetAccountProviderId(AccountProvider provider);

		void OnFacebookSignIn();

		void RegisterExternalAccount(RegisterExternalAccountRequest request);

		void UnregisterFacebookAccount();

		void UnregisterGameServicesAccount();

		EatResponse OnEvent(EventId id, object cookie);
	}
}
