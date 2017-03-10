using StaRTS.Main.Models;
using StaRTS.Main.Models.ValueObjects;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using WinRTBridge;

namespace StaRTS.Main.Controllers.VictoryConditions
{
	public class ConditionFactory
	{
		private const string DELIMITER = "|";

		private const string DESTROY_BUILDING_TYPE = "DestroyBuildingType";

		private const string DESTROY_BUILDING_ID = "DestroyBuildingId";

		private const string DESTROY_BUILDING_UID = "DestroyBuildingUid";

		private const string DESTROY_UNIT_TYPE = "DestroyUnitType";

		private const string DESTROY_UNIT_ID = "DestroyUnitId";

		private const string DESTROY_UNIT_UID = "DestroyUnitUid";

		private const string RETAIN_UNIT_UID = "RetainUnitUid";

		private const string RETAIN_UNIT_ID_LEVEL = "RetainUnitIdLevel";

		private const string RETAIN_UNIT_TYPE_LEVEL = "RetainUnitTypeLevel";

		private const string RETAIN_BUILDING_TYPE = "RetainBuildingType";

		private const string RETAIN_BUILDING_TYPE_LEVEL = "RetainBuildingTypeLevel";

		private const string RETAIN_BUILDING_ID_LEVEL = "RetainBuildingIdLevel";

		private const string RETAIN_BUILDING_UID = "RetainBuildingUid";

		private const string DEPLOY_UNIT_TYPE = "DeployUnitType";

		private const string DEPLOY_UNIT_ID = "DeployUnitId";

		private const string DEPLOY_UNIT_UID = "DeployUnitUid";

		private const string OWN_BUILDING_UID = "OwnBuildingUid";

		private const string OWN_BUILDING_ID_LEVEL = "OwnBuildingIdLevel";

		private const string OWN_BUILDING_TYPE_LEVEL = "OwnBuildingTypeLevel";

		private const string OWN_UNIT_UID = "OwnUnitUid";

		private const string OWN_UNIT_ID_LEVEL = "OwnUnitIdLevel";

		private const string OWN_UNIT_TYPE_LEVEL = "OwnUnitTypeLevel";

		private const string OWN_HERO_UID = "OwnHeroUid";

		private const string OWN_HERO_ID_LEVEL = "OwnHeroIdLevel";

		private const string OWN_HERO_TYPE_LEVEL = "OwnHeroTypeLevel";

		private const string OWN_RESOURCE = "OwnResource";

		private const string TRAIN_UNIT_UID = "TrainUnitUid";

		private const string TRAIN_UNIT_ID_LEVEL = "TrainUnitIdLevel";

		private const string TRAIN_UNIT_TYPE_LEVEL = "TrainUnitTypeLevel";

		private const string TRAIN_HERO_UID = "TrainHeroUid";

		private const string TRAIN_HERO_ID_LEVEL = "TrainHeroIdLevel";

		private const string TRAIN_HERO_TYPE_LEVEL = "TrainHeroTypeLevel";

		private const string COUNT_EVENTS = "CountEvents";

		private const string COLLECT_CURRENCY = "CollectCurrency";

		public const string PVP_START = "PvpStart";

		public const string PVP_WIN = "PvpWin";

		private const string LOOT_CURRENCY = "LootCurrency";

		public static AbstractCondition GenerateCondition(ConditionVO vo, IConditionParent parent)
		{
			return ConditionFactory.GenerateCondition(vo, parent, 0);
		}

		public static AbstractCondition GenerateCondition(ConditionVO vo, IConditionParent parent, int startingValue)
		{
			AbstractCondition result;
			try
			{
				string conditionType = vo.ConditionType;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(conditionType);
				if (num <= 1623103722u)
				{
					if (num <= 832100428u)
					{
						if (num <= 163493600u)
						{
							if (num <= 105823299u)
							{
								if (num != 64621872u)
								{
									if (num != 105823299u)
									{
										goto IL_77A;
									}
									if (!(conditionType == "PvpStart"))
									{
										goto IL_77A;
									}
									result = new CountEventsCondition(vo, parent, startingValue, "pvp_battle_started");
									return result;
								}
								else
								{
									if (!(conditionType == "DestroyBuildingId"))
									{
										goto IL_77A;
									}
									result = new DestroyBuildingIdCondition(vo, parent);
									return result;
								}
							}
							else if (num != 123671498u)
							{
								if (num != 163493600u)
								{
									goto IL_77A;
								}
								if (!(conditionType == "DeployUnitType"))
								{
									goto IL_77A;
								}
								result = new DeployUnitTypeCondition(vo, parent);
								return result;
							}
							else if (!(conditionType == "RetainBuildingTypeLevel"))
							{
								goto IL_77A;
							}
						}
						else if (num <= 520648309u)
						{
							if (num != 501321751u)
							{
								if (num != 520648309u)
								{
									goto IL_77A;
								}
								if (!(conditionType == "DeployUnitId"))
								{
									goto IL_77A;
								}
								result = new DeployUnitIdCondition(vo, parent);
								return result;
							}
							else
							{
								if (!(conditionType == "OwnHeroUid"))
								{
									goto IL_77A;
								}
								result = new OwnUnitCondition(vo, parent, ConditionMatchType.Uid, TroopType.Hero);
								return result;
							}
						}
						else if (num != 695933359u)
						{
							if (num != 747533164u)
							{
								if (num != 832100428u)
								{
									goto IL_77A;
								}
								if (!(conditionType == "LootCurrency"))
								{
									goto IL_77A;
								}
								result = new LootCurrencyCondition(vo, parent, startingValue);
								return result;
							}
							else
							{
								if (!(conditionType == "TrainUnitIdLevel"))
								{
									goto IL_77A;
								}
								result = new TrainUnitCondition(vo, parent, startingValue, ConditionMatchType.Id);
								return result;
							}
						}
						else
						{
							if (!(conditionType == "TrainUnitUid"))
							{
								goto IL_77A;
							}
							result = new TrainUnitCondition(vo, parent, startingValue, ConditionMatchType.Uid);
							return result;
						}
					}
					else if (num <= 1050956926u)
					{
						if (num <= 989645379u)
						{
							if (num != 849188692u)
							{
								if (num != 989645379u)
								{
									goto IL_77A;
								}
								if (!(conditionType == "OwnBuildingTypeLevel"))
								{
									goto IL_77A;
								}
								result = new OwnBuildingCondition(vo, parent, ConditionMatchType.Type);
								return result;
							}
							else if (!(conditionType == "RetainBuildingType"))
							{
								goto IL_77A;
							}
						}
						else if (num != 1007252735u)
						{
							if (num != 1050956926u)
							{
								goto IL_77A;
							}
							if (!(conditionType == "OwnUnitIdLevel"))
							{
								goto IL_77A;
							}
							result = new OwnUnitCondition(vo, parent, ConditionMatchType.Id, TroopType.Infantry);
							return result;
						}
						else
						{
							if (!(conditionType == "DestroyUnitType"))
							{
								goto IL_77A;
							}
							result = new DestroyUnitTypeCondition(vo, parent);
							return result;
						}
					}
					else if (num <= 1326685565u)
					{
						if (num != 1274004404u)
						{
							if (num != 1326685565u)
							{
								goto IL_77A;
							}
							if (!(conditionType == "DestroyBuildingType"))
							{
								goto IL_77A;
							}
							result = new DestroyBuildingTypeCondition(vo, parent);
							return result;
						}
						else
						{
							if (!(conditionType == "OwnBuildingIdLevel"))
							{
								goto IL_77A;
							}
							result = new OwnBuildingCondition(vo, parent, ConditionMatchType.Id);
							return result;
						}
					}
					else if (num != 1431426922u)
					{
						if (num != 1543340005u)
						{
							if (num != 1623103722u)
							{
								goto IL_77A;
							}
							if (!(conditionType == "DestroyUnitId"))
							{
								goto IL_77A;
							}
							result = new DestroyUnitIdCondition(vo, parent);
							return result;
						}
						else
						{
							if (!(conditionType == "OwnUnitUid"))
							{
								goto IL_77A;
							}
							result = new OwnUnitCondition(vo, parent, ConditionMatchType.Uid, TroopType.Infantry);
							return result;
						}
					}
					else
					{
						if (!(conditionType == "CollectCurrency"))
						{
							goto IL_77A;
						}
						result = new CollectCurrencyCondition(vo, parent, startingValue);
						return result;
					}
					result = new RetainBuildingCondition(vo, parent, ConditionMatchType.Type);
					return result;
				}
				if (num <= 2489623796u)
				{
					if (num <= 1926476989u)
					{
						if (num <= 1707822318u)
						{
							if (num != 1638088896u)
							{
								if (num == 1707822318u)
								{
									if (conditionType == "TrainHeroIdLevel")
									{
										result = new TrainUnitCondition(vo, parent, startingValue, ConditionMatchType.Id);
										return result;
									}
								}
							}
							else if (conditionType == "RetainUnitTypeLevel")
							{
								result = new RetainUnitCondition(vo, parent, ConditionMatchType.Type);
								return result;
							}
						}
						else if (num != 1898419805u)
						{
							if (num == 1926476989u)
							{
								if (conditionType == "PvpWin")
								{
									result = new CountEventsCondition(vo, parent, startingValue, "pvp_battle_won");
									return result;
								}
							}
						}
						else if (conditionType == "DestroyBuildingUid")
						{
							result = new DestroyBuildingUidCondition(vo, parent);
							return result;
						}
					}
					else if (num <= 2364440747u)
					{
						if (num != 2067306214u)
						{
							if (num == 2364440747u)
							{
								if (conditionType == "DestroyUnitUid")
								{
									result = new DestroyUnitUidCondition(vo, parent);
									return result;
								}
							}
						}
						else if (conditionType == "RetainBuildingUid")
						{
							result = new RetainBuildingCondition(vo, parent, ConditionMatchType.Type);
							return result;
						}
					}
					else if (num != 2396552976u)
					{
						if (num != 2468451715u)
						{
							if (num == 2489623796u)
							{
								if (conditionType == "OwnHeroIdLevel")
								{
									result = new OwnUnitCondition(vo, parent, ConditionMatchType.Id, TroopType.Hero);
									return result;
								}
							}
						}
						else if (conditionType == "OwnHeroTypeLevel")
						{
							result = new OwnUnitCondition(vo, parent, ConditionMatchType.Type, TroopType.Hero);
							return result;
						}
					}
					else if (conditionType == "RetainUnitUid")
					{
						result = new RetainUnitCondition(vo, parent, ConditionMatchType.Uid);
						return result;
					}
				}
				else if (num <= 3247436989u)
				{
					if (num <= 2832454201u)
					{
						if (num != 2767093421u)
						{
							if (num == 2832454201u)
							{
								if (conditionType == "RetainBuildingIdLevel")
								{
									result = new RetainBuildingCondition(vo, parent, ConditionMatchType.Type);
									return result;
								}
							}
						}
						else if (conditionType == "OwnUnitTypeLevel")
						{
							result = new OwnUnitCondition(vo, parent, ConditionMatchType.Type, TroopType.Infantry);
							return result;
						}
					}
					else if (num != 3058909421u)
					{
						if (num != 3104392635u)
						{
							if (num == 3247436989u)
							{
								if (conditionType == "TrainHeroTypeLevel")
								{
									result = new TrainUnitCondition(vo, parent, startingValue, ConditionMatchType.Type);
									return result;
								}
							}
						}
						else if (conditionType == "OwnResource")
						{
							result = new OwnResourceCondition(vo, parent);
							return result;
						}
					}
					else if (conditionType == "CountEvents")
					{
						result = new CountEventsCondition(vo, parent, startingValue);
						return result;
					}
				}
				else if (num <= 3768327639u)
				{
					if (num != 3321256586u)
					{
						if (num == 3768327639u)
						{
							if (conditionType == "OwnBuildingUid")
							{
								result = new OwnBuildingCondition(vo, parent, ConditionMatchType.Uid);
								return result;
							}
						}
					}
					else if (conditionType == "DeployUnitUid")
					{
						result = new DeployUnitUidCondition(vo, parent);
						return result;
					}
				}
				else if (num != 3844659541u)
				{
					if (num != 3865285083u)
					{
						if (num == 3968265935u)
						{
							if (conditionType == "RetainUnitIdLevel")
							{
								result = new RetainUnitCondition(vo, parent, ConditionMatchType.Id);
								return result;
							}
						}
					}
					else if (conditionType == "TrainUnitTypeLevel")
					{
						result = new TrainUnitCondition(vo, parent, startingValue, ConditionMatchType.Type);
						return result;
					}
				}
				else if (conditionType == "TrainHeroUid")
				{
					result = new TrainUnitCondition(vo, parent, startingValue, ConditionMatchType.Uid);
					return result;
				}
				IL_77A:
				Service.Get<StaRTSLogger>().ErrorFormat("Unrecognized condition {0} in {1}", new object[]
				{
					vo.ConditionType,
					vo.Uid
				});
				result = new DegenerateCondition(vo, parent);
			}
			catch (Exception ex)
			{
				Service.Get<StaRTSLogger>().ErrorFormat("Invalid condition detected in uid {0}. {1}:{2}", new object[]
				{
					vo.Uid,
					vo.ConditionType,
					vo.PrepareString
				});
				throw ex;
			}
			return result;
		}

		public ConditionFactory()
		{
		}

		protected internal ConditionFactory(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ConditionFactory.GenerateCondition((ConditionVO)GCHandledObjects.GCHandleToObject(*args), (IConditionParent)GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ConditionFactory.GenerateCondition((ConditionVO)GCHandledObjects.GCHandleToObject(*args), (IConditionParent)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2)));
		}
	}
}
