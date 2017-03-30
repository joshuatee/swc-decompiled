using System;
using System.Collections.Generic;

namespace StaRTS.Main.Models.ValueObjects
{
	public class SkinnedShooterFacade : IShooterVO
	{
		protected IShooterVO original;

		protected SkinOverrideTypeVO skinned;

		public int[] Preference
		{
			get
			{
				return this.original.Preference;
			}
		}

		public int PreferencePercentile
		{
			get
			{
				return this.original.PreferencePercentile;
			}
		}

		public int NearnessPercentile
		{
			get
			{
				return this.original.NearnessPercentile;
			}
		}

		public uint ViewRange
		{
			get
			{
				return this.original.ViewRange;
			}
		}

		public uint MinAttackRange
		{
			get
			{
				return this.original.MinAttackRange;
			}
		}

		public uint MaxAttackRange
		{
			get
			{
				return this.original.MaxAttackRange;
			}
		}

		public int Damage
		{
			get
			{
				return this.original.Damage;
			}
		}

		public int DPS
		{
			get
			{
				return this.original.DPS;
			}
		}

		public uint WarmupDelay
		{
			get
			{
				return this.skinned.WarmupDelay;
			}
		}

		public uint AnimationDelay
		{
			get
			{
				return this.skinned.AnimationDelay;
			}
		}

		public uint ShotDelay
		{
			get
			{
				return this.skinned.ShotDelay;
			}
		}

		public uint CooldownDelay
		{
			get
			{
				return this.skinned.CooldownDelay;
			}
		}

		public uint ShotCount
		{
			get
			{
				return this.skinned.ShotCount;
			}
		}

		public ProjectileTypeVO ProjectileType
		{
			get
			{
				return this.skinned.ProjectileType;
			}
		}

		public int[] GunSequence
		{
			get
			{
				return this.skinned.GunSequence;
			}
		}

		public Dictionary<int, int> Sequences
		{
			get
			{
				return this.skinned.Sequences;
			}
		}

		public bool OverWalls
		{
			get
			{
				return this.original.OverWalls;
			}
		}

		public uint RetargetingOffset
		{
			get
			{
				return this.original.RetargetingOffset;
			}
		}

		public bool ClipRetargeting
		{
			get
			{
				return this.original.ClipRetargeting;
			}
		}

		public bool StrictCooldown
		{
			get
			{
				return this.original.StrictCooldown;
			}
		}

		public SkinnedShooterFacade(IShooterVO original, SkinOverrideTypeVO skinned)
		{
			this.original = original;
			this.skinned = skinned;
		}
	}
}
