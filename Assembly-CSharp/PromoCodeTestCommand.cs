using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Commands.Cheats;
using StaRTS.Main.Models.Commands.Player;
using System;

public class PromoCodeTestCommand : GameCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>
{
	public const string ACTION = "cheats.test.promocode";

	public PromoCodeTestCommand(PromoCodeTestRequest request) : base("cheats.test.promocode", request, new MoneyReceiptVerifyResponse())
	{
	}

	protected internal PromoCodeTestCommand(UIntPtr dummy) : base(dummy)
	{
	}
}
