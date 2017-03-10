using StaRTS.Main.Controllers;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.ValueObjects
{
	public class BuffTypeVO : IValueObject, IAssetVO
	{
		private const string BUFF_DEFLECT_UID = "buffDeflect";

		private const string ASSET_OFFSET_TOP = "Top";

		public const int NUM_ASSET_SIZES = 7;

		public static int COLUMN_assetName
		{
			get;
			private set;
		}

		public static int COLUMN_bundleName
		{
			get;
			private set;
		}

		public static int COLUMN_lvl
		{
			get;
			private set;
		}

		public static int COLUMN_modifier
		{
			get;
			private set;
		}

		public static int COLUMN_value
		{
			get;
			private set;
		}

		public static int COLUMN_wall
		{
			get;
			private set;
		}

		public static int COLUMN_building
		{
			get;
			private set;
		}

		public static int COLUMN_storage
		{
			get;
			private set;
		}

		public static int COLUMN_resource
		{
			get;
			private set;
		}

		public static int COLUMN_turret
		{
			get;
			private set;
		}

		public static int COLUMN_HQ
		{
			get;
			private set;
		}

		public static int COLUMN_shield
		{
			get;
			private set;
		}

		public static int COLUMN_shieldGenerator
		{
			get;
			private set;
		}

		public static int COLUMN_infantry
		{
			get;
			private set;
		}

		public static int COLUMN_bruiserInfantry
		{
			get;
			private set;
		}

		public static int COLUMN_vehicle
		{
			get;
			private set;
		}

		public static int COLUMN_bruiserVehicle
		{
			get;
			private set;
		}

		public static int COLUMN_heroInfantry
		{
			get;
			private set;
		}

		public static int COLUMN_heroVehicle
		{
			get;
			private set;
		}

		public static int COLUMN_heroBruiserInfantry
		{
			get;
			private set;
		}

		public static int COLUMN_heroBruiserVehicle
		{
			get;
			private set;
		}

		public static int COLUMN_flierInfantry
		{
			get;
			private set;
		}

		public static int COLUMN_flierVehicle
		{
			get;
			private set;
		}

		public static int COLUMN_healerInfantry
		{
			get;
			private set;
		}

		public static int COLUMN_trap
		{
			get;
			private set;
		}

		public static int COLUMN_champion
		{
			get;
			private set;
		}

		public static int COLUMN_applyValueAs
		{
			get;
			private set;
		}

		public static int COLUMN_duration
		{
			get;
			private set;
		}

		public static int COLUMN_stack
		{
			get;
			private set;
		}

		public static int COLUMN_msFirstProc
		{
			get;
			private set;
		}

		public static int COLUMN_msPerProc
		{
			get;
			private set;
		}

		public static int COLUMN_isRefreshing
		{
			get;
			private set;
		}

		public static int COLUMN_target
		{
			get;
			private set;
		}

		public static int COLUMN_tags
		{
			get;
			private set;
		}

		public static int COLUMN_cancelTags
		{
			get;
			private set;
		}

		public static int COLUMN_preventTags
		{
			get;
			private set;
		}

		public static int COLUMN_audioAbilityEvent
		{
			get;
			private set;
		}

		public static int COLUMN_shaderOverride
		{
			get;
			private set;
		}

		public static int COLUMN_projectileAttachmentBundle
		{
			get;
			private set;
		}

		public static int COLUMN_muzzleAssetNameRebel
		{
			get;
			private set;
		}

		public static int COLUMN_muzzleAssetNameEmpire
		{
			get;
			private set;
		}

		public static int COLUMN_impactAssetNameRebel
		{
			get;
			private set;
		}

		public static int COLUMN_impactAssetNameEmpire
		{
			get;
			private set;
		}

		public static int COLUMN_assetOffsetType
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public string AssetName
		{
			get;
			set;
		}

		public string BundleName
		{
			get;
			set;
		}

		public string ShaderName
		{
			get;
			set;
		}

		public BuffAssetOffset OffsetType
		{
			get;
			set;
		}

		public string RebelMuzzleAssetName
		{
			get;
			set;
		}

		public string RebelImpactAssetName
		{
			get;
			set;
		}

		public string EmpireMuzzleAssetName
		{
			get;
			set;
		}

		public string EmpireImpactAssetName
		{
			get;
			set;
		}

		public string ProjectileAttachmentBundle
		{
			get;
			set;
		}

		public string BuffID
		{
			get;
			private set;
		}

		public int Lvl
		{
			get;
			private set;
		}

		public BuffModify Modify
		{
			get;
			private set;
		}

		public int[] Values
		{
			get;
			private set;
		}

		public BuffApplyAs ApplyAs
		{
			get;
			private set;
		}

		public int Duration
		{
			get;
			private set;
		}

		public uint MaxStacks
		{
			get;
			private set;
		}

		public int MillisecondsToFirstProc
		{
			get;
			private set;
		}

		public int MillisecondsPerProc
		{
			get;
			private set;
		}

		public bool IsRefreshing
		{
			get;
			private set;
		}

		public bool ApplyToSelf
		{
			get;
			private set;
		}

		public bool ApplyToAllies
		{
			get;
			private set;
		}

		public bool ApplyToEnemies
		{
			get;
			private set;
		}

		public HashSet<string> Tags
		{
			get;
			private set;
		}

		public HashSet<string> CancelTags
		{
			get;
			private set;
		}

		public HashSet<string> PreventTags
		{
			get;
			private set;
		}

		public List<StrIntPair> AudioAbilityEvent
		{
			get;
			private set;
		}

		public bool IsDeflector
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			this.AssetName = row.TryGetString(BuffTypeVO.COLUMN_assetName);
			this.BundleName = row.TryGetString(BuffTypeVO.COLUMN_bundleName);
			this.ShaderName = row.TryGetString(BuffTypeVO.COLUMN_shaderOverride, "");
			this.RebelMuzzleAssetName = row.TryGetString(BuffTypeVO.COLUMN_muzzleAssetNameRebel);
			this.RebelImpactAssetName = row.TryGetString(BuffTypeVO.COLUMN_impactAssetNameRebel);
			this.EmpireMuzzleAssetName = row.TryGetString(BuffTypeVO.COLUMN_muzzleAssetNameEmpire);
			this.EmpireImpactAssetName = row.TryGetString(BuffTypeVO.COLUMN_impactAssetNameEmpire);
			this.ProjectileAttachmentBundle = row.TryGetString(BuffTypeVO.COLUMN_projectileAttachmentBundle);
			this.BuffID = row.Uid;
			this.Lvl = row.TryGetInt(BuffTypeVO.COLUMN_lvl, 1);
			this.Modify = StringUtils.ParseEnum<BuffModify>(row.TryGetString(BuffTypeVO.COLUMN_modifier));
			int num = row.TryGetInt(BuffTypeVO.COLUMN_value);
			this.Values = new int[23];
			this.Values[0] = num;
			this.Values[1] = row.TryGetInt(BuffTypeVO.COLUMN_wall, num);
			this.Values[2] = row.TryGetInt(BuffTypeVO.COLUMN_building, num);
			this.Values[3] = row.TryGetInt(BuffTypeVO.COLUMN_storage, num);
			this.Values[4] = row.TryGetInt(BuffTypeVO.COLUMN_resource, num);
			this.Values[5] = row.TryGetInt(BuffTypeVO.COLUMN_turret, num);
			this.Values[6] = row.TryGetInt(BuffTypeVO.COLUMN_HQ, num);
			this.Values[7] = row.TryGetInt(BuffTypeVO.COLUMN_shield, num);
			this.Values[8] = row.TryGetInt(BuffTypeVO.COLUMN_shieldGenerator, num);
			this.Values[9] = row.TryGetInt(BuffTypeVO.COLUMN_infantry, num);
			this.Values[10] = row.TryGetInt(BuffTypeVO.COLUMN_bruiserInfantry, num);
			this.Values[11] = row.TryGetInt(BuffTypeVO.COLUMN_vehicle, num);
			this.Values[12] = row.TryGetInt(BuffTypeVO.COLUMN_bruiserVehicle, num);
			this.Values[13] = row.TryGetInt(BuffTypeVO.COLUMN_heroInfantry, num);
			this.Values[14] = row.TryGetInt(BuffTypeVO.COLUMN_heroVehicle, num);
			this.Values[15] = row.TryGetInt(BuffTypeVO.COLUMN_heroBruiserInfantry, num);
			this.Values[16] = row.TryGetInt(BuffTypeVO.COLUMN_heroBruiserVehicle, num);
			this.Values[17] = row.TryGetInt(BuffTypeVO.COLUMN_flierInfantry, num);
			this.Values[18] = row.TryGetInt(BuffTypeVO.COLUMN_flierVehicle, num);
			this.Values[19] = row.TryGetInt(BuffTypeVO.COLUMN_healerInfantry, num);
			this.Values[20] = row.TryGetInt(BuffTypeVO.COLUMN_trap, num);
			this.Values[21] = row.TryGetInt(BuffTypeVO.COLUMN_champion, num);
			this.Values[22] = num;
			this.ApplyAs = StringUtils.ParseEnum<BuffApplyAs>(row.TryGetString(BuffTypeVO.COLUMN_applyValueAs));
			this.Duration = row.TryGetInt(BuffTypeVO.COLUMN_duration, -1);
			if (this.Duration < -1)
			{
				this.Duration = -1;
			}
			int num2 = row.TryGetInt(BuffTypeVO.COLUMN_stack, 0);
			if (num2 < 0)
			{
				num2 = 0;
			}
			this.MaxStacks = (uint)num2;
			this.MillisecondsToFirstProc = row.TryGetInt(BuffTypeVO.COLUMN_msFirstProc);
			this.MillisecondsPerProc = row.TryGetInt(BuffTypeVO.COLUMN_msPerProc);
			if (this.MillisecondsPerProc == 0)
			{
				this.MillisecondsPerProc = -1;
			}
			this.IsRefreshing = row.TryGetBool(BuffTypeVO.COLUMN_isRefreshing);
			this.ApplyToSelf = false;
			this.ApplyToAllies = false;
			this.ApplyToEnemies = false;
			string[] commaSeparatedStrings = this.GetCommaSeparatedStrings(row, BuffTypeVO.COLUMN_target);
			if (commaSeparatedStrings != null)
			{
				int i = 0;
				int num3 = commaSeparatedStrings.Length;
				while (i < num3)
				{
					switch (StringUtils.ParseEnum<BuffApplyTo>(commaSeparatedStrings[i]))
					{
					case BuffApplyTo.Self:
						this.ApplyToSelf = true;
						break;
					case BuffApplyTo.Allies:
						this.ApplyToAllies = true;
						break;
					case BuffApplyTo.Enemies:
						this.ApplyToEnemies = true;
						break;
					}
					i++;
				}
			}
			this.Tags = this.GetCommaSeparatedHashSet(row, BuffTypeVO.COLUMN_tags);
			this.CancelTags = this.GetCommaSeparatedHashSet(row, BuffTypeVO.COLUMN_cancelTags);
			this.PreventTags = this.GetCommaSeparatedHashSet(row, BuffTypeVO.COLUMN_preventTags);
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			this.AudioAbilityEvent = valueObjectController.GetStrIntPairs(this.Uid, row.TryGetString(BuffTypeVO.COLUMN_audioAbilityEvent));
			this.OffsetType = StringUtils.ParseEnum<BuffAssetOffset>(row.TryGetString(BuffTypeVO.COLUMN_assetOffsetType, "Top"));
			this.IsDeflector = (this.Uid == "buffDeflect");
		}

		private string[] GetCommaSeparatedStrings(Row row, int columnIndex)
		{
			string text = row.TryGetString(columnIndex);
			if (!string.IsNullOrEmpty(text))
			{
				return text.Split(new char[]
				{
					','
				});
			}
			return null;
		}

		private HashSet<string> GetCommaSeparatedHashSet(Row row, int columnIndex)
		{
			string text = row.TryGetString(columnIndex);
			if (!string.IsNullOrEmpty(text))
			{
				return new HashSet<string>(text.Split(new char[]
				{
					','
				}));
			}
			return new HashSet<string>();
		}

		public string GetImpactAssetNameBasedOnFaction(FactionType faction)
		{
			if (faction == FactionType.Empire)
			{
				return this.EmpireImpactAssetName;
			}
			if (faction != FactionType.Rebel)
			{
				return null;
			}
			return this.RebelImpactAssetName;
		}

		public string GetMuzzleAssetNameBasedOnFaction(FactionType faction)
		{
			if (faction == FactionType.Empire)
			{
				return this.EmpireMuzzleAssetName;
			}
			if (faction != FactionType.Rebel)
			{
				return null;
			}
			return this.RebelMuzzleAssetName;
		}

		public bool WillAffect(ArmorType armorType)
		{
			if (this.Modify == BuffModify.Nothing)
			{
				return true;
			}
			int num = this.Values[(int)armorType];
			switch (this.ApplyAs)
			{
			case BuffApplyAs.Relative:
				return num != 0;
			case BuffApplyAs.Absolute:
				return true;
			case BuffApplyAs.RelativePercent:
			case BuffApplyAs.RelativePercentOfMax:
				return num != 0;
			case BuffApplyAs.AbsolutePercent:
			case BuffApplyAs.AbsolutePercentOfMax:
				return num != 100;
			default:
				return false;
			}
		}

		public BuffTypeVO()
		{
		}

		protected internal BuffTypeVO(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ApplyAs);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ApplyToAllies);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ApplyToEnemies);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ApplyToSelf);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioAbilityEvent);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuffID);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).CancelTags);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_applyValueAs);
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_assetName);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_assetOffsetType);
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_audioAbilityEvent);
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_bruiserInfantry);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_bruiserVehicle);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_building);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_bundleName);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_cancelTags);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_champion);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_duration);
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_flierInfantry);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_flierVehicle);
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_healerInfantry);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_heroBruiserInfantry);
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_heroBruiserVehicle);
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_heroInfantry);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_heroVehicle);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_HQ);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_impactAssetNameEmpire);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_impactAssetNameRebel);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_infantry);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_isRefreshing);
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_lvl);
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_modifier);
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_msFirstProc);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_msPerProc);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_muzzleAssetNameEmpire);
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_muzzleAssetNameRebel);
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_preventTags);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_projectileAttachmentBundle);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_resource);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_shaderOverride);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_shield);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_shieldGenerator);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_stack);
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_storage);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_tags);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_target);
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_trap);
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_turret);
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_value);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_vehicle);
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(BuffTypeVO.COLUMN_wall);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).Duration);
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).EmpireImpactAssetName);
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).EmpireMuzzleAssetName);
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsDeflector);
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsRefreshing);
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).Lvl);
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).MillisecondsPerProc);
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).MillisecondsToFirstProc);
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).Modify);
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).OffsetType);
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).PreventTags);
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileAttachmentBundle);
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).RebelImpactAssetName);
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).RebelMuzzleAssetName);
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ShaderName);
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).Tags);
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid);
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).Values);
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).GetCommaSeparatedHashSet((Row)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).GetCommaSeparatedStrings((Row)GCHandledObjects.GCHandleToObject(*args), *(int*)(args + 1)));
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).GetImpactAssetNameBasedOnFaction((FactionType)(*(int*)args)));
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).GetMuzzleAssetNameBasedOnFaction((FactionType)(*(int*)args)));
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ReadRow((Row)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ApplyAs = (BuffApplyAs)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ApplyToAllies = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ApplyToEnemies = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ApplyToSelf = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).AssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).AudioAbilityEvent = (List<StrIntPair>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).BuffID = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).BundleName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).CancelTags = (HashSet<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			BuffTypeVO.COLUMN_applyValueAs = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			BuffTypeVO.COLUMN_assetName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			BuffTypeVO.COLUMN_assetOffsetType = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			BuffTypeVO.COLUMN_audioAbilityEvent = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			BuffTypeVO.COLUMN_bruiserInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			BuffTypeVO.COLUMN_bruiserVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			BuffTypeVO.COLUMN_building = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			BuffTypeVO.COLUMN_bundleName = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			BuffTypeVO.COLUMN_cancelTags = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			BuffTypeVO.COLUMN_champion = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			BuffTypeVO.COLUMN_duration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			BuffTypeVO.COLUMN_flierInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			BuffTypeVO.COLUMN_flierVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			BuffTypeVO.COLUMN_healerInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			BuffTypeVO.COLUMN_heroBruiserInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			BuffTypeVO.COLUMN_heroBruiserVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			BuffTypeVO.COLUMN_heroInfantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			BuffTypeVO.COLUMN_heroVehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			BuffTypeVO.COLUMN_HQ = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			BuffTypeVO.COLUMN_impactAssetNameEmpire = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			BuffTypeVO.COLUMN_impactAssetNameRebel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			BuffTypeVO.COLUMN_infantry = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			BuffTypeVO.COLUMN_isRefreshing = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			BuffTypeVO.COLUMN_lvl = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			BuffTypeVO.COLUMN_modifier = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			BuffTypeVO.COLUMN_msFirstProc = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			BuffTypeVO.COLUMN_msPerProc = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			BuffTypeVO.COLUMN_muzzleAssetNameEmpire = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			BuffTypeVO.COLUMN_muzzleAssetNameRebel = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			BuffTypeVO.COLUMN_preventTags = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			BuffTypeVO.COLUMN_projectileAttachmentBundle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			BuffTypeVO.COLUMN_resource = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			BuffTypeVO.COLUMN_shaderOverride = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			BuffTypeVO.COLUMN_shield = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			BuffTypeVO.COLUMN_shieldGenerator = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			BuffTypeVO.COLUMN_stack = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			BuffTypeVO.COLUMN_storage = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			BuffTypeVO.COLUMN_tags = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			BuffTypeVO.COLUMN_target = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			BuffTypeVO.COLUMN_trap = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			BuffTypeVO.COLUMN_turret = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			BuffTypeVO.COLUMN_value = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			BuffTypeVO.COLUMN_vehicle = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			BuffTypeVO.COLUMN_wall = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).Duration = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).EmpireImpactAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).EmpireMuzzleAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsDeflector = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).IsRefreshing = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).Lvl = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).MillisecondsPerProc = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).MillisecondsToFirstProc = *(int*)args;
			return -1L;
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).Modify = (BuffModify)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).OffsetType = (BuffAssetOffset)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).PreventTags = (HashSet<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ProjectileAttachmentBundle = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke141(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).RebelImpactAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke142(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).RebelMuzzleAssetName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke143(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).ShaderName = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke144(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).Tags = (HashSet<string>)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke145(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).Uid = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke146(long instance, long* args)
		{
			((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).Values = (int[])GCHandledObjects.GCHandleToPinnedArrayObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke147(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((BuffTypeVO)GCHandledObjects.GCHandleToObject(instance)).WillAffect((ArmorType)(*(int*)args)));
		}
	}
}
