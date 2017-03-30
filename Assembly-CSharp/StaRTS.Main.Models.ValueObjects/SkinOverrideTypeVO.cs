using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using StaRTS.Utils.MetaData;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Models.ValueObjects
{
	public class SkinOverrideTypeVO : IValueObject
	{
		public static int COLUMN_projectileType
		{
			get;
			private set;
		}

		public static int COLUMN_shotCount
		{
			get;
			private set;
		}

		public static int COLUMN_chargeTime
		{
			get;
			private set;
		}

		public static int COLUMN_animationDelay
		{
			get;
			private set;
		}

		public static int COLUMN_shotDelay
		{
			get;
			private set;
		}

		public static int COLUMN_reload
		{
			get;
			private set;
		}

		public static int COLUMN_gunSequence
		{
			get;
			private set;
		}

		public string Uid
		{
			get;
			set;
		}

		public ProjectileTypeVO ProjectileType
		{
			get;
			set;
		}

		public uint ShotCount
		{
			get;
			set;
		}

		public uint WarmupDelay
		{
			get;
			set;
		}

		public uint AnimationDelay
		{
			get;
			set;
		}

		public uint ShotDelay
		{
			get;
			set;
		}

		public uint CooldownDelay
		{
			get;
			set;
		}

		public int[] GunSequence
		{
			get;
			set;
		}

		public Dictionary<int, int> Sequences
		{
			get;
			private set;
		}

		public void ReadRow(Row row)
		{
			this.Uid = row.Uid;
			IDataController dataController = Service.Get<IDataController>();
			string value = row.TryGetString(SkinOverrideTypeVO.COLUMN_projectileType);
			if (!string.IsNullOrEmpty(value))
			{
				this.ProjectileType = dataController.Get<ProjectileTypeVO>(row.TryGetString(SkinOverrideTypeVO.COLUMN_projectileType));
			}
			this.WarmupDelay = row.TryGetUint(SkinOverrideTypeVO.COLUMN_chargeTime);
			this.AnimationDelay = row.TryGetUint(SkinOverrideTypeVO.COLUMN_animationDelay);
			this.ShotDelay = row.TryGetUint(SkinOverrideTypeVO.COLUMN_shotDelay);
			this.CooldownDelay = row.TryGetUint(SkinOverrideTypeVO.COLUMN_reload);
			this.ShotCount = row.TryGetUint(SkinOverrideTypeVO.COLUMN_shotCount);
			ValueObjectController valueObjectController = Service.Get<ValueObjectController>();
			SequencePair gunSequences = valueObjectController.GetGunSequences(this.Uid, row.TryGetString(SkinOverrideTypeVO.COLUMN_gunSequence));
			this.GunSequence = gunSequences.GunSequence;
			this.Sequences = gunSequences.Sequences;
		}
	}
}
