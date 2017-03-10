using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Commands.Squads.Requests;
using System;

namespace StaRTS.Main.Models.Commands.Squads
{
	public class ShareVideoCommand : SquadGameCommand<ShareVideoRequest, DefaultResponse>
	{
		public const string ACTION = "guild.link.share";

		public ShareVideoCommand(ShareVideoRequest request) : base("guild.link.share", request, new DefaultResponse())
		{
		}

		protected internal ShareVideoCommand(UIntPtr dummy) : base(dummy)
		{
		}
	}
}
