using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Components
{
	public class BuffComponent : ComponentBase, IEventObserver
	{
		private MutableIterator miter;

		private MutableIterator niter;

		private MutableIterator oiter;

		private BuffSleepState sleepState;

		private List<Buff> buffRetryList;

		private EventId buildingLoadedEvent;

		private EventId troopLoadedEvent;

		private VisualReadyDelegate onBuildingVisualReady;

		private VisualReadyDelegate onTroopVisualReady;

		public List<Buff> Buffs
		{
			get;
			private set;
		}

		public BuffComponent()
		{
			this.buildingLoadedEvent = EventId.BuildingViewReady;
			this.troopLoadedEvent = EventId.TroopViewReady;
			this.Buffs = new List<Buff>();
			this.buffRetryList = new List<Buff>();
			this.miter = new MutableIterator();
			this.niter = new MutableIterator();
			this.oiter = new MutableIterator();
			this.sleepState = BuffSleepState.Sleeping;
		}

		public override void OnRemove()
		{
			this.onBuildingVisualReady = null;
			this.onTroopVisualReady = null;
			Service.Get<EventManager>().UnregisterObserver(this, this.buildingLoadedEvent);
			Service.Get<EventManager>().UnregisterObserver(this, this.troopLoadedEvent);
			this.Die();
		}

		public void Die()
		{
			if (this.sleepState == BuffSleepState.Dead)
			{
				return;
			}
			this.oiter.Init(this.Buffs);
			while (this.oiter.Active())
			{
				this.OnRemovingBuff(this.Buffs[this.oiter.Index]);
				this.oiter.Next();
			}
			this.oiter.Reset();
			this.Buffs.Clear();
			this.sleepState = BuffSleepState.Dead;
			this.miter.Reset();
			this.niter.Reset();
		}

		public bool HasBuff(string buffID)
		{
			int i = 0;
			int count = this.Buffs.Count;
			while (i < count)
			{
				Buff buff = this.Buffs[i];
				if (buff.BuffType.BuffID == buffID && buff.StackSize > 0)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		public Buff AddBuffStack(BuffTypeVO buffType, ArmorType armorType, BuffVisualPriority visualPriority)
		{
			int num = this.FindBuff(buffType.BuffID);
			Buff buff;
			if (num < 0)
			{
				buff = new Buff(buffType, armorType, visualPriority);
				buff.AddStack();
				this.Buffs.Add(buff);
				if (this.sleepState == BuffSleepState.Sleeping)
				{
					this.sleepState = BuffSleepState.Awake;
				}
				this.SendBuffEvent(EventId.AddedBuff, buff);
			}
			else
			{
				buff = this.Buffs[num];
				if (buffType.Lvl > buff.BuffType.Lvl)
				{
					buff.UpgradeBuff(buffType);
				}
				buff.AddStack();
			}
			this.OnBuffStackAdded();
			return buff;
		}

		private void OnBuffStackAdded()
		{
			this.UpdateBuffs(0u);
		}

		public bool RemoveBuffStack(BuffTypeVO buffType)
		{
			int num = this.FindBuff(buffType.BuffID);
			if (num >= 0)
			{
				Buff buff = this.Buffs[num];
				if (buff.RemoveStack())
				{
					this.RemoveBuffAt(num);
				}
			}
			return this.Buffs.Count == 0;
		}

		public void ApplyActiveBuffs(BuffModify modify, ref int modifyValue, int modifyValueMax)
		{
			if (this.sleepState != BuffSleepState.Awake)
			{
				return;
			}
			int i = 0;
			int count = this.Buffs.Count;
			while (i < count)
			{
				Buff buff = this.Buffs[i];
				if (buff.ProcCount > 0 && buff.BuffType.Modify == modify)
				{
					buff.ApplyStacks(ref modifyValue, modifyValueMax);
				}
				i++;
			}
		}

		public bool IsBuffPrevented(BuffTypeVO buffType)
		{
			BuffSleepState buffSleepState = this.sleepState;
			if (buffSleepState == BuffSleepState.Sleeping)
			{
				return false;
			}
			if (buffSleepState != BuffSleepState.Dead)
			{
				int i = 0;
				int count = this.Buffs.Count;
				while (i < count)
				{
					if (this.Buffs[i].BuffType.PreventTags.Overlaps(buffType.Tags))
					{
						return true;
					}
					i++;
				}
				return false;
			}
			return true;
		}

		public void RemoveBuffsCanceledBy(BuffTypeVO buffType)
		{
			this.niter.Init(this.Buffs);
			while (this.niter.Active())
			{
				int index = this.niter.Index;
				Buff buff = this.Buffs[index];
				if (buff.BuffType.BuffID != buffType.BuffID && buff.BuffType.Tags.Overlaps(buffType.CancelTags))
				{
					this.RemoveBuffAt(index);
				}
				this.niter.Next();
			}
			this.niter.Reset();
		}

		public void UpdateBuffs(uint dt)
		{
			if (this.sleepState != BuffSleepState.Awake)
			{
				return;
			}
			this.miter.Init(this.Buffs);
			while (this.miter.Active())
			{
				int num = this.miter.Index;
				Buff buff = this.Buffs[num];
				bool flag;
				bool flag2;
				buff.UpdateBuff(dt, out flag, out flag2);
				if (flag)
				{
					this.SendBuffEvent(EventId.ProcBuff, buff);
				}
				if (flag2)
				{
					num = this.FindBuff(buff.BuffType.BuffID);
					if (num >= 0)
					{
						this.RemoveBuffAt(num);
					}
				}
				this.miter.Next();
			}
			this.miter.Reset();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (this.IsBuffReadyForVisualRetry(id, cookie))
			{
				this.SendRecheckEvents();
				Service.Get<EventManager>().UnregisterObserver(this, id);
			}
			return EatResponse.NotEaten;
		}

		public void RegisterForVisualReAddOnBuilding(Buff buff)
		{
			this.AddUniqueRetryBuffToList(buff);
			Service.Get<EventManager>().RegisterObserver(this, this.buildingLoadedEvent, EventPriority.Default);
		}

		public void RegisterForVisualReAddOnTroop(Buff buff)
		{
			this.AddUniqueRetryBuffToList(buff);
			Service.Get<EventManager>().RegisterObserver(this, this.troopLoadedEvent, EventPriority.Default);
		}

		public void SetTroopLoadedEvent(EventId loadedEvent, VisualReadyDelegate trpVisualReady)
		{
			EventManager eventManager = Service.Get<EventManager>();
			bool flag = eventManager.IsEventListenerRegistered(this, this.troopLoadedEvent);
			if (flag)
			{
				eventManager.UnregisterObserver(this, this.troopLoadedEvent);
				eventManager.RegisterObserver(this, loadedEvent);
			}
			this.troopLoadedEvent = loadedEvent;
			this.onTroopVisualReady = trpVisualReady;
		}

		public void SetBuildingLoadedEvent(EventId loadedEvent, VisualReadyDelegate bldVisualReady)
		{
			EventManager eventManager = Service.Get<EventManager>();
			bool flag = eventManager.IsEventListenerRegistered(this, this.buildingLoadedEvent);
			if (flag)
			{
				eventManager.UnregisterObserver(this, this.buildingLoadedEvent);
				eventManager.RegisterObserver(this, loadedEvent);
			}
			this.buildingLoadedEvent = loadedEvent;
			this.onBuildingVisualReady = bldVisualReady;
		}

		private bool IsBuffReadyForVisualRetry(EventId id, object cookie)
		{
			SmartEntity smartEntity = (SmartEntity)this.Entity;
			bool flag = true;
			if (this.onBuildingVisualReady != null)
			{
				flag = this.onBuildingVisualReady(id, cookie, smartEntity);
			}
			else if (this.onTroopVisualReady != null)
			{
				flag = this.onTroopVisualReady(id, cookie, smartEntity);
			}
			return flag && smartEntity.GameObjectViewComp != null && smartEntity.GameObjectViewComp.MainGameObject != null;
		}

		private void AddUniqueRetryBuffToList(Buff buff)
		{
			if (!this.buffRetryList.Contains(buff))
			{
				this.buffRetryList.Add(buff);
			}
		}

		private void SendRecheckEvents()
		{
			int count = this.buffRetryList.Count;
			for (int i = 0; i < count; i++)
			{
				this.SendBuffEvent(EventId.AddedBuff, this.buffRetryList[i]);
			}
			this.buffRetryList.Clear();
		}

		private void RemoveBuffAt(int index)
		{
			this.OnRemovingBuff(this.Buffs[index]);
			this.Buffs.RemoveAt(index);
			this.miter.OnRemove(index);
			this.niter.OnRemove(index);
			this.oiter.OnRemove(index);
			if (this.Buffs.Count == 0 && this.sleepState == BuffSleepState.Awake)
			{
				this.sleepState = BuffSleepState.Sleeping;
			}
		}

		private void OnRemovingBuff(Buff buff)
		{
			if (this.buffRetryList.Count > 0)
			{
				this.buffRetryList.Remove(buff);
			}
			this.SendBuffEvent(EventId.RemovingBuff, buff);
		}

		private void SendBuffEvent(EventId id, Buff buff)
		{
			SmartEntity target = (SmartEntity)this.Entity;
			BuffEventData cookie = new BuffEventData(buff, target);
			Service.Get<EventManager>().SendEvent(id, cookie);
		}

		private int FindBuff(string buffID)
		{
			int i = 0;
			int count = this.Buffs.Count;
			while (i < count)
			{
				if (this.Buffs[i].BuffType.BuffID == buffID)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		protected internal BuffComponent(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).AddBuffStack((BuffTypeVO)GCHandledObjects.GCHandleToObject(*args), (ArmorType)(*(int*)(args + 1)), (BuffVisualPriority)(*(int*)(args + 2))));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).AddUniqueRetryBuffToList((Buff)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).Die();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).FindBuff(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).Buffs);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).HasBuff(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).IsBuffPrevented((BuffTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).IsBuffReadyForVisualRetry((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).OnBuffStackAdded();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).OnRemove();
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).OnRemovingBuff((Buff)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).RegisterForVisualReAddOnBuilding((Buff)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).RegisterForVisualReAddOnTroop((Buff)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).RemoveBuffAt(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).RemoveBuffsCanceledBy((BuffTypeVO)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).RemoveBuffStack((BuffTypeVO)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).SendBuffEvent((EventId)(*(int*)args), (Buff)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).SendRecheckEvents();
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).Buffs = (List<Buff>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).SetBuildingLoadedEvent((EventId)(*(int*)args), (VisualReadyDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((BuffComponent)GCHandledObjects.GCHandleToObject(instance)).SetTroopLoadedEvent((EventId)(*(int*)args), (VisualReadyDelegate)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
