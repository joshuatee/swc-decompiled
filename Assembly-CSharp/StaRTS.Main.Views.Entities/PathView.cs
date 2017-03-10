using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Utils;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class PathView
	{
		public const int MAX_TROOPS_PER_CELL = 9;

		public const int TROOPS_PER_ROW = 3;

		private int nextTurnIndex;

		public float TimeOnPathSegment;

		public PathingComponent PathComponent
		{
			get;
			protected set;
		}

		public Vector3 StartPos
		{
			get;
			set;
		}

		public float TimeToTarget
		{
			get;
			set;
		}

		public float Speed
		{
			get;
			set;
		}

		public float ClusterOffsetX
		{
			get;
			set;
		}

		public float ClusterOffsetZ
		{
			get;
			set;
		}

		public int NextTurnIndex
		{
			get
			{
				return this.nextTurnIndex;
			}
		}

		public void Reset(PathingComponent pathing)
		{
			this.TimeToTarget = 0f;
			this.TimeOnPathSegment = 0f;
			this.Speed = 0f;
			this.StartPos = Vector3.zero;
			this.PathComponent = pathing;
			this.nextTurnIndex = -1;
			if (this.PathComponent != null)
			{
				uint num = this.PathComponent.Entity.ID % 9u;
				this.ClusterOffsetX = num % 3u / 3f;
				this.ClusterOffsetZ = num / 3u % 3u / 3f;
				return;
			}
			this.ClusterOffsetX = 0f;
			this.ClusterOffsetZ = 0f;
		}

		public PathView(PathingComponent pathing)
		{
			this.Reset(pathing);
		}

		public BoardCell<Entity> GetPrevTurn()
		{
			if (this.PathComponent.CurrentPath == null)
			{
				return null;
			}
			return this.PathComponent.CurrentPath.GetTurn(this.nextTurnIndex - 1);
		}

		public BoardCell<Entity> GetNextTurn()
		{
			if (this.PathComponent.CurrentPath == null)
			{
				return null;
			}
			return this.PathComponent.CurrentPath.GetTurn(this.nextTurnIndex);
		}

		public BoardCell<Entity> GetNextNextTurn()
		{
			if (this.PathComponent.CurrentPath == null)
			{
				return null;
			}
			return this.PathComponent.CurrentPath.GetTurn(this.nextTurnIndex + 1);
		}

		public void GetTroopClusterOffset(ref float offsetX, ref float offsetZ)
		{
			offsetX = this.ClusterOffsetX;
			offsetZ = this.ClusterOffsetZ;
		}

		public void AdvanceNextTurn()
		{
			float num = 0f;
			float num2 = 0f;
			this.GetTroopClusterOffset(ref num, ref num2);
			float num3 = Units.BoardToWorldX((float)this.PathComponent.CurrentPath.TroopWidth / 2f);
			GameObjectViewComponent gameObjectViewComponent = this.PathComponent.Entity.Get<GameObjectViewComponent>();
			if (gameObjectViewComponent == null)
			{
				TransformComponent transformComponent = this.PathComponent.Entity.Get<TransformComponent>();
				this.StartPos = new Vector3(Units.BoardToWorldX(transformComponent.CenterX()), 0f, Units.BoardToWorldZ(transformComponent.CenterZ()));
			}
			else
			{
				this.StartPos = gameObjectViewComponent.MainTransform.position;
			}
			this.StartPos -= new Vector3(num3 + num, 0f, num3 + num2);
			if (this.nextTurnIndex < this.PathComponent.CurrentPath.TurnCount - 1)
			{
				this.nextTurnIndex++;
				this.TimeOnPathSegment = 0f;
				this.TimeToTarget = (float)(this.PathComponent.CurrentPath.GetTurnDistance(this.nextTurnIndex) * this.PathComponent.TimePerBoardCellMs) * 1E-06f;
				if (this.TimeToTarget == 0f)
				{
					this.Speed = 0f;
				}
				else
				{
					BoardCell<Entity> turn = this.PathComponent.CurrentPath.GetTurn(this.nextTurnIndex - 1);
					BoardCell<Entity> turn2 = this.PathComponent.CurrentPath.GetTurn(this.nextTurnIndex);
					float num4 = Units.BoardToWorldX(turn2.X - turn.X) + num;
					float num5 = Units.BoardToWorldZ(turn2.Z - turn.Z) + num2;
					this.Speed = Mathf.Sqrt(num4 * num4 + num5 * num5) / this.TimeToTarget;
				}
				TransformComponent transformComponent2 = this.PathComponent.Entity.Get<TransformComponent>();
				if (transformComponent2 != null)
				{
					transformComponent2.RotationVelocity = 0f;
				}
			}
		}

		protected internal PathView(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((PathView)GCHandledObjects.GCHandleToObject(instance)).AdvanceNextTurn();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathView)GCHandledObjects.GCHandleToObject(instance)).ClusterOffsetX);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathView)GCHandledObjects.GCHandleToObject(instance)).ClusterOffsetZ);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathView)GCHandledObjects.GCHandleToObject(instance)).NextTurnIndex);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathView)GCHandledObjects.GCHandleToObject(instance)).PathComponent);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathView)GCHandledObjects.GCHandleToObject(instance)).Speed);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathView)GCHandledObjects.GCHandleToObject(instance)).StartPos);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathView)GCHandledObjects.GCHandleToObject(instance)).TimeToTarget);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathView)GCHandledObjects.GCHandleToObject(instance)).GetNextNextTurn());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathView)GCHandledObjects.GCHandleToObject(instance)).GetNextTurn());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((PathView)GCHandledObjects.GCHandleToObject(instance)).GetPrevTurn());
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((PathView)GCHandledObjects.GCHandleToObject(instance)).Reset((PathingComponent)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((PathView)GCHandledObjects.GCHandleToObject(instance)).ClusterOffsetX = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((PathView)GCHandledObjects.GCHandleToObject(instance)).ClusterOffsetZ = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((PathView)GCHandledObjects.GCHandleToObject(instance)).PathComponent = (PathingComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((PathView)GCHandledObjects.GCHandleToObject(instance)).Speed = *(float*)args;
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((PathView)GCHandledObjects.GCHandleToObject(instance)).StartPos = *(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((PathView)GCHandledObjects.GCHandleToObject(instance)).TimeToTarget = *(float*)args;
			return -1L;
		}
	}
}
