using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Models.Static
{
	public class TroopUpgradeCatalog : GenericUpgradeCatalog<TroopTypeVO>
	{
		public int MaxTroopDps
		{
			get;
			private set;
		}

		public int MaxTroopHealth
		{
			get;
			private set;
		}

		protected override void InitService()
		{
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			IDataController dataController = Service.Get<IDataController>();
			foreach (TroopTypeVO current in dataController.GetAll<TroopTypeVO>())
			{
				if (current.PlayerFacing && current.Faction == faction)
				{
					if (current.DPS > this.MaxTroopDps)
					{
						this.MaxTroopDps = current.DPS;
					}
					if (current.Health > this.MaxTroopHealth)
					{
						this.MaxTroopHealth = current.Health;
					}
				}
			}
			Service.Set<TroopUpgradeCatalog>(this);
		}
	}
}
