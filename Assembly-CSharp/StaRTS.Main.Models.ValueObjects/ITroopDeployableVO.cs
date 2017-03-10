using System;
using UnityEngine;

namespace StaRTS.Main.Models.ValueObjects
{
	public interface ITroopDeployableVO : IDeployableVO, IUpgradeableVO, IValueObject, IAssetVO, IGeometryVO, IUnlockableVO
	{
		TroopType Type
		{
			get;
		}

		string TroopID
		{
			get;
		}

		ArmorType ArmorType
		{
			get;
		}

		bool IsHealer
		{
			get;
		}

		uint SupportFollowDistance
		{
			get;
		}

		int SizeX
		{
			get;
		}

		int SizeY
		{
			get;
		}

		Vector3 BuffAssetOffset
		{
			get;
			set;
		}

		Vector3 BuffAssetBaseOffset
		{
			get;
			set;
		}

		string[] SpawnApplyBuffs
		{
			get;
		}

		bool AttackShieldBorder
		{
			get;
		}

		bool CrushesWalls
		{
			get;
		}

		uint PathSearchWidth
		{
			get;
		}

		bool IsFlying
		{
			get;
		}

		uint ShieldCooldown
		{
			get;
		}

		int RunSpeed
		{
			get;
		}

		int RunThreshold
		{
			get;
		}

		uint TargetInRangeModifier
		{
			get;
		}
	}
}
