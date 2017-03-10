using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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
				if (current.PlayerFacing && current.Faction == faction && current.DPS > this.MaxSpecialAttackDps)
				{
					this.MaxSpecialAttackDps = current.DPS;
				}
			}
			Service.Set<StarshipUpgradeCatalog>(this);
		}

		public StarshipUpgradeCatalog()
		{
		}

		protected internal StarshipUpgradeCatalog(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StarshipUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).MaxSpecialAttackDps);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((StarshipUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).InitService();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((StarshipUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).MaxSpecialAttackDps = *(int*)args;
			return -1L;
		}
	}
}
