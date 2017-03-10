using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using System;
using WinRTBridge;

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

		public TroopUpgradeCatalog()
		{
		}

		protected internal TroopUpgradeCatalog(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).MaxTroopDps);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TroopUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).MaxTroopHealth);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((TroopUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).InitService();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((TroopUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).MaxTroopDps = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((TroopUpgradeCatalog)GCHandledObjects.GCHandleToObject(instance)).MaxTroopHealth = *(int*)args;
			return -1L;
		}
	}
}
