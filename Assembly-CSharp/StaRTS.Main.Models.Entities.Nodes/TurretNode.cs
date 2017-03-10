using Net.RichardLord.Ash.Core;
using StaRTS.Main.Models.Entities.Components;
using System;
using WinRTBridge;

namespace StaRTS.Main.Models.Entities.Nodes
{
	public class TurretNode : Node<TurretNode>
	{
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

		public GameObjectViewComponent View
		{
			get;
			set;
		}

		public StateComponent StateComp
		{
			get;
			set;
		}

		public HealthComponent HealthComp
		{
			get;
			set;
		}

		public TurretShooterComponent TurretShooterComp
		{
			get;
			set;
		}

		public TurretNode()
		{
		}

		protected internal TurretNode(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretNode)GCHandledObjects.GCHandleToObject(instance)).HealthComp);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretNode)GCHandledObjects.GCHandleToObject(instance)).ShooterComp);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretNode)GCHandledObjects.GCHandleToObject(instance)).StateComp);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretNode)GCHandledObjects.GCHandleToObject(instance)).Transform);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretNode)GCHandledObjects.GCHandleToObject(instance)).TurretShooterComp);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((TurretNode)GCHandledObjects.GCHandleToObject(instance)).View);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((TurretNode)GCHandledObjects.GCHandleToObject(instance)).HealthComp = (HealthComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((TurretNode)GCHandledObjects.GCHandleToObject(instance)).ShooterComp = (ShooterComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((TurretNode)GCHandledObjects.GCHandleToObject(instance)).StateComp = (StateComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((TurretNode)GCHandledObjects.GCHandleToObject(instance)).Transform = (TransformComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((TurretNode)GCHandledObjects.GCHandleToObject(instance)).TurretShooterComp = (TurretShooterComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((TurretNode)GCHandledObjects.GCHandleToObject(instance)).View = (GameObjectViewComponent)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}
	}
}
