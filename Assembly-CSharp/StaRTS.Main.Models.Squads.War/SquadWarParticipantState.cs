using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Models.Squads.War
{
	public class SquadWarParticipantState : ISerializable
	{
		public string SquadMemberName;

		public string SquadMemberId;

		public int TurnsLeft;

		public int Score;

		public int VictoryPointsLeft;

		public int HQLevel;

		public int AttacksWon;

		public int DefensesWon;

		public uint DefendingAttackExpirationDate;

		public ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary.ContainsKey("id"))
			{
				this.SquadMemberId = Convert.ToString(dictionary["id"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("name"))
			{
				this.SquadMemberName = Convert.ToString(dictionary["name"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("turns"))
			{
				this.TurnsLeft = Convert.ToInt32(dictionary["turns"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("victoryPoints"))
			{
				this.VictoryPointsLeft = Convert.ToInt32(dictionary["victoryPoints"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("attacksWon"))
			{
				this.AttacksWon = Convert.ToInt32(dictionary["attacksWon"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("defensesWon"))
			{
				this.DefensesWon = Convert.ToInt32(dictionary["defensesWon"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("score"))
			{
				this.Score = Convert.ToInt32(dictionary["score"], CultureInfo.InvariantCulture);
			}
			if (dictionary.ContainsKey("currentlyDefending"))
			{
				Dictionary<string, object> dictionary2 = dictionary["currentlyDefending"] as Dictionary<string, object>;
				if (dictionary2 != null && dictionary2.ContainsKey("expiration"))
				{
					this.DefendingAttackExpirationDate = Convert.ToUInt32(dictionary2["expiration"], CultureInfo.InvariantCulture);
				}
			}
			if (dictionary.ContainsKey("level"))
			{
				this.HQLevel = Convert.ToInt32(dictionary["level"], CultureInfo.InvariantCulture);
				if (this.HQLevel <= 0)
				{
					this.HQLevel = 1;
					Service.Get<StaRTSLogger>().WarnFormat("War participant has no HQ level: {0}", new object[]
					{
						this.SquadMemberId
					});
				}
			}
			return this;
		}

		public string ToJson()
		{
			return Serializer.Start().End().ToString();
		}

		public SquadWarParticipantState()
		{
			this.HQLevel = 1;
			base..ctor();
		}

		protected internal SquadWarParticipantState(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarParticipantState)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((SquadWarParticipantState)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
