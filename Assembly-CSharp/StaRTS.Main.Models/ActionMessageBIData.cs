using System;

namespace StaRTS.Main.Models
{
	public struct ActionMessageBIData
	{
		public string Action
		{
			get;
			private set;
		}

		public string Message
		{
			get;
			private set;
		}

		public ActionMessageBIData(string action, string message)
		{
			this = default(ActionMessageBIData);
			this.Action = action;
			this.Message = message;
		}
	}
}
