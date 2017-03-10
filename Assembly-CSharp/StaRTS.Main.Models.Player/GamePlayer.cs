using StaRTS.Main.Models.Player.Misc;
using StaRTS.Main.Models.Player.Store;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.Squads;
using StaRTS.Main.Utils;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Player
{
	public class GamePlayer
	{
		public Inventory Inventory
		{
			get;
			set;
		}

		public Map Map
		{
			get;
			set;
		}

		public UnlockedLevelData UnlockedLevels
		{
			get;
			set;
		}

		public Squad Squad
		{
			get;
			set;
		}

		public bool IsContrabandUnlocked
		{
			get;
			set;
		}

		public virtual string PlayerName
		{
			get;
			set;
		}

		public virtual int AttackRating
		{
			get;
			set;
		}

		public virtual int DefenseRating
		{
			get;
			set;
		}

		public virtual int AttacksWon
		{
			get;
			set;
		}

		public virtual int DefensesWon
		{
			get;
			set;
		}

		public virtual FactionType Faction
		{
			get;
			set;
		}

		public int PlayerMedals
		{
			get
			{
				return GameUtils.CalcuateMedals(this.AttackRating, this.DefenseRating);
			}
		}

		public int CurrentXPAmount
		{
			get
			{
				return this.Inventory.GetItemAmount("xp");
			}
		}

		public int CurrentCreditsAmount
		{
			get
			{
				return this.Inventory.GetItemAmount("credits");
			}
		}

		public int MaxCreditsAmount
		{
			get
			{
				return this.Inventory.GetItemCapacity("credits");
			}
		}

		public int CurrentMaterialsAmount
		{
			get
			{
				return this.Inventory.GetItemAmount("materials");
			}
		}

		public int MaxMaterialsAmount
		{
			get
			{
				return this.Inventory.GetItemCapacity("materials");
			}
		}

		public int CurrentContrabandAmount
		{
			get
			{
				return this.Inventory.GetItemAmount("contraband");
			}
		}

		public int MaxContrabandAmount
		{
			get
			{
				return this.Inventory.GetItemCapacity("contraband");
			}
		}

		public int CurrentReputationAmount
		{
			get
			{
				return this.Inventory.GetItemAmount("reputation");
			}
		}

		public int MaxReputationAmount
		{
			get
			{
				return this.Inventory.GetItemCapacity("reputation");
			}
		}

		public GamePlayer()
		{
		}

		protected internal GamePlayer(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).AttackRating);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).AttacksWon);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentContrabandAmount);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentCreditsAmount);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentMaterialsAmount);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentReputationAmount);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).CurrentXPAmount);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).DefenseRating);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).DefensesWon);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).Faction);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).Inventory);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).IsContrabandUnlocked);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).Map);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).MaxContrabandAmount);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).MaxCreditsAmount);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).MaxMaterialsAmount);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).MaxReputationAmount);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).PlayerMedals);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).PlayerName);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).Squad);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).UnlockedLevels);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).AttackRating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).AttacksWon = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).DefenseRating = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).DefensesWon = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).Faction = (FactionType)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).Inventory = (Inventory)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).IsContrabandUnlocked = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).Map = (Map)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).PlayerName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).Squad = (Squad)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((GamePlayer)GCHandledObjects.GCHandleToObject(instance)).UnlockedLevels = (UnlockedLevelData)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
