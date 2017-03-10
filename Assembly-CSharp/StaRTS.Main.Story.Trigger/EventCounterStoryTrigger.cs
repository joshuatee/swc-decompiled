using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Battle;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Models.Player.World;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Story.Trigger
{
	public class EventCounterStoryTrigger : AbstractStoryTrigger, IEventObserver
	{
		public const string REQUIRED_EVENTS_SYMBOL = "{REQ}";

		public const string COUNTED_EVENTS_SYMBOL = "{CNT}";

		public const string REMAINING_EVENTS_SYMBOL = "{REM}";

		private const string COUNTED_KEY = "cnt";

		private const int THRESHOLD_ARG = 0;

		private const int RELEVANT_EVENT_ARG = 1;

		private const int EVENT_DATA_ARG = 2;

		private const int UNINITIALIZED_COUNT = -1;

		private const string TROOP_DEPLOYED = "troopDeployed";

		private const string HERO_DEPLOYED = "heroDeployed";

		private const string CHAMPION_DEPLOYED = "championDeployed";

		private const string SPECIAL_ATTACK_DEPLOYED = "specialAttackDeployed";

		private const string BUILDING_KILLED = "buildingKilled";

		private const string TROOP_KILLED = "troopKilled";

		private const string BUTTON_CLICKED = "buttonClicked";

		private const string BUILDING_SELECTED = "buildingSelected";

		private const string UI_APPEARS = "uiAppears";

		private const string SCREEN_CLOSED = "screenClosed";

		private const string BUILDING_PLACED = "buildingPlaced";

		private const string CONTRACT_QUEUED = "contractQueued";

		private const string CONTRACT_STARTED = "contractStarted";

		private const string CONTRACT_COMPLETED = "contractCompleted";

		private const string INVENTORY_UPDATED = "inventoryUpdated";

		private const string DROID_PURCHASED = "droidPurchased";

		private const string TROOP_DONATION_TRACK_REWARDED = "donationTrackRewarded";

		private const string EQUIPMENT_UNLOCKED = "equipmentUnlocked";

		private int threshold;

		private int eventCount;

		private string[] eventData;

		public int RequiredEvents
		{
			get
			{
				return this.threshold;
			}
		}

		public int CountedEvents
		{
			get
			{
				return this.eventCount;
			}
		}

		public int RemainingEvents
		{
			get
			{
				return this.threshold - this.eventCount;
			}
		}

		public EventCounterStoryTrigger(StoryTriggerVO vo, ITriggerReactor parent) : base(vo, parent)
		{
			this.eventCount = -1;
		}

		public override void Activate()
		{
			base.Activate();
			this.threshold = Convert.ToInt32(this.prepareArgs[0], CultureInfo.InvariantCulture);
			this.eventData = this.prepareArgs[2].Split(new char[]
			{
				','
			});
			if (this.eventCount == -1)
			{
				this.eventCount = 0;
			}
			EventId relevantEvent = this.GetRelevantEvent(this.prepareArgs[1]);
			Service.Get<EventManager>().RegisterObserver(this, relevantEvent, EventPriority.Default);
			base.UpdateAction();
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.ButtonClicked)
			{
				if (id <= EventId.EntityKilled)
				{
					if (id <= EventId.BuildingSelected)
					{
						if (id != EventId.BuildingPurchaseSuccess)
						{
							if (id != EventId.BuildingSelected)
							{
								return EatResponse.NotEaten;
							}
							Entity entity = (Entity)cookie;
							BuildingComponent buildingComponent = entity.Get<BuildingComponent>();
							if (buildingComponent == null)
							{
								return EatResponse.NotEaten;
							}
							BuildingTypeVO buildingType = buildingComponent.BuildingType;
							if (buildingType.Uid.Equals(this.eventData[0], 5))
							{
								this.CountEvent();
								return EatResponse.NotEaten;
							}
							return EatResponse.NotEaten;
						}
						else
						{
							Entity entity2 = (Entity)cookie;
							BuildingComponent buildingComponent2 = entity2.Get<BuildingComponent>();
							if (buildingComponent2 == null)
							{
								return EatResponse.NotEaten;
							}
							BuildingTypeVO buildingType2 = buildingComponent2.BuildingType;
							if (buildingType2.Uid.Equals(this.eventData[0], 5))
							{
								this.CountEvent();
								return EatResponse.NotEaten;
							}
							return EatResponse.NotEaten;
						}
					}
					else
					{
						if (id == EventId.DroidPurchaseAnimationComplete)
						{
							goto IL_3D4;
						}
						if (id != EventId.EntityKilled)
						{
							return EatResponse.NotEaten;
						}
						Entity entity3 = (Entity)cookie;
						string text = "";
						string text2 = this.prepareArgs[1];
						if (text2 == "troopKilled")
						{
							TroopComponent troopComponent = entity3.Get<TroopComponent>();
							if (troopComponent == null)
							{
								return EatResponse.NotEaten;
							}
							text = troopComponent.TroopType.Uid;
						}
						else if (text2 == "buildingKilled")
						{
							BuildingComponent buildingComponent3 = entity3.Get<BuildingComponent>();
							if (buildingComponent3 == null)
							{
								return EatResponse.NotEaten;
							}
							BuildingTypeVO buildingType3 = buildingComponent3.BuildingType;
							text = buildingType3.Uid;
						}
						if (text.Equals(this.eventData[0], 5))
						{
							this.CountEvent();
							return EatResponse.NotEaten;
						}
						return EatResponse.NotEaten;
					}
				}
				else if (id <= EventId.TroopDonationTrackRewardReceived)
				{
					if (id != EventId.TroopDeployed)
					{
						if (id != EventId.TroopDonationTrackRewardReceived)
						{
							return EatResponse.NotEaten;
						}
						goto IL_3D4;
					}
				}
				else if (id != EventId.SpecialAttackDeployed)
				{
					if (id != EventId.ButtonClicked)
					{
						return EatResponse.NotEaten;
					}
					string text3 = (string)cookie;
					if (text3.Equals(this.eventData[0], 5))
					{
						this.CountEvent();
						return EatResponse.NotEaten;
					}
					return EatResponse.NotEaten;
				}
				else
				{
					SpecialAttack specialAttack = (SpecialAttack)cookie;
					if (specialAttack.VO.Uid.Equals(this.eventData[0], 5))
					{
						this.CountEvent();
						return EatResponse.NotEaten;
					}
					return EatResponse.NotEaten;
				}
			}
			else if (id <= EventId.InventoryResourceUpdated)
			{
				if (id <= EventId.ContractStarted)
				{
					if (id != EventId.ContractAdded)
					{
						if (id != EventId.ContractStarted)
						{
							return EatResponse.NotEaten;
						}
						ContractEventData contractEventData = (ContractEventData)cookie;
						if (contractEventData.Contract.ProductUid.Equals(this.eventData[0], 5))
						{
							this.CountEvent();
							return EatResponse.NotEaten;
						}
						return EatResponse.NotEaten;
					}
					else
					{
						ContractEventData contractEventData2 = (ContractEventData)cookie;
						if (contractEventData2.Contract.ProductUid.Equals(this.eventData[0], 5))
						{
							this.CountEvent();
							return EatResponse.NotEaten;
						}
						return EatResponse.NotEaten;
					}
				}
				else if (id != EventId.ContractCompletedForStoryAction)
				{
					if (id != EventId.InventoryResourceUpdated)
					{
						return EatResponse.NotEaten;
					}
					string text4 = (string)cookie;
					if (text4.Equals(this.eventData[0], 5))
					{
						this.CountEvent();
						return EatResponse.NotEaten;
					}
					return EatResponse.NotEaten;
				}
				else
				{
					ContractTO contractTO = (ContractTO)cookie;
					if (contractTO.Uid.Equals(this.eventData[0], 5))
					{
						this.CountEvent();
						return EatResponse.NotEaten;
					}
					return EatResponse.NotEaten;
				}
			}
			else if (id <= EventId.ScreenLoaded)
			{
				if (id != EventId.ScreenClosing)
				{
					if (id != EventId.ScreenLoaded)
					{
						return EatResponse.NotEaten;
					}
					UXFactory uXFactory = cookie as UXFactory;
					string name = uXFactory.Root.name;
					if (name.Equals(this.eventData[0], 5))
					{
						this.CountEvent();
						return EatResponse.NotEaten;
					}
					return EatResponse.NotEaten;
				}
				else
				{
					UXFactory uXFactory2 = cookie as UXFactory;
					string text5 = (uXFactory2 == null || uXFactory2.Root == null) ? string.Empty : uXFactory2.Root.name;
					if (text5.Equals(this.eventData[0], 5))
					{
						this.CountEvent();
						return EatResponse.NotEaten;
					}
					return EatResponse.NotEaten;
				}
			}
			else if (id != EventId.HeroDeployed && id != EventId.ChampionDeployed)
			{
				if (id != EventId.EquipmentUnlocked)
				{
					return EatResponse.NotEaten;
				}
				goto IL_3D4;
			}
			Entity entity4 = (Entity)cookie;
			TroopComponent troopComponent2 = entity4.Get<TroopComponent>();
			ITroopDeployableVO troopType = troopComponent2.TroopType;
			if (troopType.Uid.Equals(this.eventData[0], 5))
			{
				this.CountEvent();
				return EatResponse.NotEaten;
			}
			return EatResponse.NotEaten;
			IL_3D4:
			this.CountEvent();
			return EatResponse.NotEaten;
		}

		private void CountEvent()
		{
			this.eventCount++;
			if (!this.EvaluateThreshold())
			{
				base.UpdateAction();
			}
		}

		private bool EvaluateThreshold()
		{
			if (this.eventCount >= this.threshold)
			{
				this.parent.SatisfyTrigger(this);
				return true;
			}
			return false;
		}

		private EventId GetRelevantEvent(string name)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 1014291712u)
			{
				if (num <= 390872840u)
				{
					if (num <= 90849046u)
					{
						if (num != 83568195u)
						{
							if (num != 90849046u)
							{
								goto IL_2D7;
							}
							if (!(name == "droidPurchased"))
							{
								goto IL_2D7;
							}
							return EventId.DroidPurchaseAnimationComplete;
						}
						else
						{
							if (!(name == "heroDeployed"))
							{
								goto IL_2D7;
							}
							return EventId.HeroDeployed;
						}
					}
					else if (num != 280708703u)
					{
						if (num != 390872840u)
						{
							goto IL_2D7;
						}
						if (!(name == "donationTrackRewarded"))
						{
							goto IL_2D7;
						}
						return EventId.TroopDonationTrackRewardReceived;
					}
					else
					{
						if (!(name == "uiAppears"))
						{
							goto IL_2D7;
						}
						return EventId.ScreenLoaded;
					}
				}
				else if (num <= 473917134u)
				{
					if (num != 463470584u)
					{
						if (num != 473917134u)
						{
							goto IL_2D7;
						}
						if (!(name == "equipmentUnlocked"))
						{
							goto IL_2D7;
						}
						return EventId.EquipmentUnlocked;
					}
					else
					{
						if (!(name == "buildingPlaced"))
						{
							goto IL_2D7;
						}
						return EventId.BuildingPurchaseSuccess;
					}
				}
				else if (num != 676851390u)
				{
					if (num != 930184452u)
					{
						if (num != 1014291712u)
						{
							goto IL_2D7;
						}
						if (!(name == "buildingKilled"))
						{
							goto IL_2D7;
						}
					}
					else
					{
						if (!(name == "buildingSelected"))
						{
							goto IL_2D7;
						}
						return EventId.BuildingSelected;
					}
				}
				else
				{
					if (!(name == "specialAttackDeployed"))
					{
						goto IL_2D7;
					}
					return EventId.SpecialAttackDeployed;
				}
			}
			else if (num <= 2138028830u)
			{
				if (num <= 1246767052u)
				{
					if (num != 1127712780u)
					{
						if (num != 1246767052u)
						{
							goto IL_2D7;
						}
						if (!(name == "championDeployed"))
						{
							goto IL_2D7;
						}
						return EventId.ChampionDeployed;
					}
					else
					{
						if (!(name == "contractCompleted"))
						{
							goto IL_2D7;
						}
						return EventId.ContractCompletedForStoryAction;
					}
				}
				else if (num != 1530602600u)
				{
					if (num != 2138028830u)
					{
						goto IL_2D7;
					}
					if (!(name == "inventoryUpdated"))
					{
						goto IL_2D7;
					}
					return EventId.InventoryResourceUpdated;
				}
				else
				{
					if (!(name == "contractStarted"))
					{
						goto IL_2D7;
					}
					return EventId.ContractStarted;
				}
			}
			else if (num <= 2940767986u)
			{
				if (num != 2637417530u)
				{
					if (num != 2940767986u)
					{
						goto IL_2D7;
					}
					if (!(name == "troopKilled"))
					{
						goto IL_2D7;
					}
				}
				else
				{
					if (!(name == "buttonClicked"))
					{
						goto IL_2D7;
					}
					return EventId.ButtonClicked;
				}
			}
			else if (num != 3152622145u)
			{
				if (num != 3305455698u)
				{
					if (num != 3517888473u)
					{
						goto IL_2D7;
					}
					if (!(name == "troopDeployed"))
					{
						goto IL_2D7;
					}
					return EventId.TroopDeployed;
				}
				else
				{
					if (!(name == "contractQueued"))
					{
						goto IL_2D7;
					}
					return EventId.ContractAdded;
				}
			}
			else
			{
				if (!(name == "screenClosed"))
				{
					goto IL_2D7;
				}
				return EventId.ScreenClosing;
			}
			return EventId.EntityKilled;
			IL_2D7:
			Service.Get<StaRTSLogger>().ErrorFormat("No event type associated with {0}", new object[]
			{
				name
			});
			return EventId.Nop;
		}

		public override void Destroy()
		{
			EventId relevantEvent = this.GetRelevantEvent(this.prepareArgs[1]);
			Service.Get<EventManager>().UnregisterObserver(this, relevantEvent);
			base.Destroy();
		}

		protected override void AddData(ref Serializer serializer)
		{
			base.AddData(ref serializer);
			serializer.Add<int>("cnt", this.eventCount);
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (!dictionary.ContainsKey("cnt"))
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Quest Deserialization Error: Could not find {0} property in trigger {1}", new object[]
				{
					"cnt",
					this.vo.Uid
				});
				return null;
			}
			this.eventCount = Convert.ToInt32(dictionary["cnt"], CultureInfo.InvariantCulture);
			return base.FromObject(obj);
		}

		protected internal EventCounterStoryTrigger(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EventCounterStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Activate();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((EventCounterStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).CountEvent();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EventCounterStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventCounterStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).EvaluateThreshold());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventCounterStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).FromObject(GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventCounterStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).CountedEvents);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventCounterStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).RemainingEvents);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventCounterStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).RequiredEvents);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventCounterStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).GetRelevantEvent(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EventCounterStoryTrigger)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}
	}
}
