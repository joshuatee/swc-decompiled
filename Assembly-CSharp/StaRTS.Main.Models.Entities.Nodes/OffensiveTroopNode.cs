using Net.RichardLord.Ash.Core;
using StaRTS.GameBoard.Components;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class OffensiveTroopNode : Node<OffensiveTroopNode>
	{
		public KillerComponent KillerComp
		{
			get;
			set;
		}

		public AttackerComponent AttackerComp
		{
			get;
			set;
		}

		public ShooterComponent ShooterComp
		{
			get;
			set;
		}

		public TransformComponent Transform
		{
			get;
			set;
		}

		public SizeComponent Size
		{
			get;
			set;
		}

		public StateComponent StateComp
		{
			get;
			set;
		}

		public SecondaryTargetsComponent SecondaryTargets
		{
			get;
			set;
		}

		public TroopComponent TroopComp
		{
			get;
			set;
		}

		public TeamComponent TeamComp
		{
			get;
			set;
		}

		public OffensiveTroopNode()
		{
		}

		protected internal OffensiveTroopNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).AttackerComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).KillerComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).SecondaryTargets);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).ShooterComp);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).Size);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).StateComp);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).TeamComp);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).Transform);
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).TroopComp);
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).AttackerComp = (AttackerComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).KillerComp = (KillerComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).SecondaryTargets = (SecondaryTargetsComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).ShooterComp = (ShooterComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).Size = (SizeComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).StateComp = (StateComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).TeamComp = (TeamComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).Transform = (TransformComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((OffensiveTroopNode)GCHandledObjects.GCHandleToObject(instance)).TroopComp = (TroopComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
