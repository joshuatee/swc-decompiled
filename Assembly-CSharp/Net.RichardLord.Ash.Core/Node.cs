using StaRTS.Main.Models.Entities.Nodes;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace Net.RichardLord.Ash.Core
{
	public abstract class Node<TSibling>
	{
		public Entity Entity
		{
			get;
			set;
		}

		public TSibling Previous
		{
			get;
			set;
		}

		public TSibling Next
		{
			get;
			set;
		}

		public Node()
		{
		}

		public object GetProperty(string propertyName)
		{
			return base.GetType().GetProperty(propertyName).GetValue(this, null);
		}

		public virtual bool IsValid()
		{
			return this.Entity != null;
		}

		public void SetProperty(string propertyName, object value)
		{
			base.GetType().GetProperty(propertyName).SetValue(this, value, null);
		}

		protected internal Node(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<AreaTriggerBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<AreaTriggerBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<AreaTriggerBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((Node<AreaTriggerBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((Node<AreaTriggerBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ArmoryNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ArmoryNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ArmoryNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((Node<ArmoryNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((Node<ArmoryNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<BarracksNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<BarracksNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<BarracksNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((Node<BarracksNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			((Node<BarracksNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<BuffNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<BuffNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<BuffNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((Node<BuffNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((Node<BuffNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<BuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<BuildingNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<BuildingNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((Node<BuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((Node<BuildingNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<BuildingRenderNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<BuildingRenderNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<BuildingRenderNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			((Node<BuildingRenderNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((Node<BuildingRenderNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<CantinaNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<CantinaNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<CantinaNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((Node<CantinaNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((Node<CantinaNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ChampionNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ChampionNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ChampionNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			((Node<ChampionNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((Node<ChampionNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ChampionPlatformNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ChampionPlatformNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ChampionPlatformNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((Node<ChampionPlatformNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			((Node<ChampionPlatformNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ClearableNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ClearableNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ClearableNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((Node<ClearableNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((Node<ClearableNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DefenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DefenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DefenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((Node<DefenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			((Node<DefenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DefensiveHealerNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DefensiveHealerNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DefensiveHealerNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			((Node<DefensiveHealerNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((Node<DefensiveHealerNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DefensiveTroopNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DefensiveTroopNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DefensiveTroopNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((Node<DefensiveTroopNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			((Node<DefensiveTroopNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DroidHutNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DroidHutNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DroidHutNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			((Node<DroidHutNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((Node<DroidHutNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DroidNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DroidNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<DroidNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((Node<DroidNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((Node<DroidNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<EntityRenderNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<EntityRenderNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<EntityRenderNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			((Node<EntityRenderNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((Node<EntityRenderNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<FactoryNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<FactoryNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<FactoryNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((Node<FactoryNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			((Node<FactoryNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<FleetCommandNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<FleetCommandNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<FleetCommandNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((Node<FleetCommandNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((Node<FleetCommandNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<GeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<GeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<GeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			((Node<GeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			((Node<GeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<GeneratorViewNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<GeneratorViewNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<GeneratorViewNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			((Node<GeneratorViewNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			((Node<GeneratorViewNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<HQNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<HQNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<HQNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			((Node<HQNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			((Node<HQNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<HealthViewNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<HealthViewNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<HealthViewNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			((Node<HealthViewNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			((Node<HealthViewNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<HousingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<HousingNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<HousingNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			((Node<HousingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			((Node<HousingNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<LootNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<LootNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<LootNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			((Node<LootNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			((Node<LootNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<MovementNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<MovementNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<MovementNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			((Node<MovementNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			((Node<MovementNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<NavigationCenterNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<NavigationCenterNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<NavigationCenterNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			((Node<NavigationCenterNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			((Node<NavigationCenterNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<OffenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<OffenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<OffenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			((Node<OffenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			((Node<OffenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<OffensiveHealerNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<OffensiveHealerNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<OffensiveHealerNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			((Node<OffensiveHealerNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			((Node<OffensiveHealerNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<OffensiveTroopNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke141(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<OffensiveTroopNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke142(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<OffensiveTroopNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke143(long instance, long* args)
		{
			((Node<OffensiveTroopNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke144(long instance, long* args)
		{
			((Node<OffensiveTroopNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke145(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ScoutTowerNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke146(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ScoutTowerNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke147(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ScoutTowerNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke148(long instance, long* args)
		{
			((Node<ScoutTowerNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke149(long instance, long* args)
		{
			((Node<ScoutTowerNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke150(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ShieldGeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke151(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ShieldGeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke152(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<ShieldGeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke153(long instance, long* args)
		{
			((Node<ShieldGeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke154(long instance, long* args)
		{
			((Node<ShieldGeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke155(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<SquadBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke156(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<SquadBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke157(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<SquadBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke158(long instance, long* args)
		{
			((Node<SquadBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke159(long instance, long* args)
		{
			((Node<SquadBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke160(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<StarportNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke161(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<StarportNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke162(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<StarportNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke163(long instance, long* args)
		{
			((Node<StarportNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke164(long instance, long* args)
		{
			((Node<StarportNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke165(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<StorageNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke166(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<StorageNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke167(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<StorageNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke168(long instance, long* args)
		{
			((Node<StorageNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke169(long instance, long* args)
		{
			((Node<StorageNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke170(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<SupportNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke171(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<SupportNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke172(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<SupportNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke173(long instance, long* args)
		{
			((Node<SupportNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke174(long instance, long* args)
		{
			((Node<SupportNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke175(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<SupportViewNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke176(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<SupportViewNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke177(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<SupportViewNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke178(long instance, long* args)
		{
			((Node<SupportViewNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke179(long instance, long* args)
		{
			((Node<SupportViewNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke180(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TacticalCommandNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke181(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TacticalCommandNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke182(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TacticalCommandNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke183(long instance, long* args)
		{
			((Node<TacticalCommandNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke184(long instance, long* args)
		{
			((Node<TacticalCommandNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke185(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TrackingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke186(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TrackingNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke187(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TrackingNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke188(long instance, long* args)
		{
			((Node<TrackingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke189(long instance, long* args)
		{
			((Node<TrackingNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke190(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TrackingRenderNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke191(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TrackingRenderNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke192(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TrackingRenderNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke193(long instance, long* args)
		{
			((Node<TrackingRenderNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke194(long instance, long* args)
		{
			((Node<TrackingRenderNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke195(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TransportNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke196(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TransportNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke197(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TransportNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke198(long instance, long* args)
		{
			((Node<TransportNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke199(long instance, long* args)
		{
			((Node<TransportNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke200(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TrapBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke201(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TrapBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke202(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TrapBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke203(long instance, long* args)
		{
			((Node<TrapBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke204(long instance, long* args)
		{
			((Node<TrapBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke205(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TrapNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke206(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TrapNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke207(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TrapNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke208(long instance, long* args)
		{
			((Node<TrapNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke209(long instance, long* args)
		{
			((Node<TrapNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke210(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TroopNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke211(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TroopNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke212(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TroopNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke213(long instance, long* args)
		{
			((Node<TroopNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke214(long instance, long* args)
		{
			((Node<TroopNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke215(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TurretBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke216(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TurretBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke217(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TurretBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke218(long instance, long* args)
		{
			((Node<TurretBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke219(long instance, long* args)
		{
			((Node<TurretBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke220(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TurretNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke221(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TurretNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke222(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<TurretNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke223(long instance, long* args)
		{
			((Node<TurretNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke224(long instance, long* args)
		{
			((Node<TurretNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke225(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<WallNode>)GCHandledObjects.GCHandleToObject(instance)).Entity);
		}

		public unsafe static long $Invoke226(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<WallNode>)GCHandledObjects.GCHandleToObject(instance)).GetProperty(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke227(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((Node<WallNode>)GCHandledObjects.GCHandleToObject(instance)).IsValid());
		}

		public unsafe static long $Invoke228(long instance, long* args)
		{
			((Node<WallNode>)GCHandledObjects.GCHandleToObject(instance)).Entity = (Entity)GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke229(long instance, long* args)
		{
			((Node<WallNode>)GCHandledObjects.GCHandleToObject(instance)).SetProperty(Marshal.PtrToStringUni(*(IntPtr*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
