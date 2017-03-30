using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;

namespace StaRTS.Main.Models.Static
{
	public class StarshipUpgradeCatalog : GenericUpgradeCatalog<SpecialAttackTypeVO>
	{
		public int MaxSpecialAttackDps
		{
			get;
			private set;
		}

		protected override void InitService()
		{
			FactionType faction = Service.Get<CurrentPlayer>().Faction;
			IDataController dataController = Service.Get<IDataController>();
			foreach (SpecialAttackTypeVO current in dataController.GetAll<SpecialAttackTypeVO>())
			{
				if (current.PlayerFacing && current.Faction == faction)
				{
					if (current.DPS > this.MaxSpecialAttackDps)
					{
						this.MaxSpecialAttackDps = current.DPS;
					}
				}
			}
			Service.Set<StarshipUpgradeCatalog>(this);
		}
	}
}
