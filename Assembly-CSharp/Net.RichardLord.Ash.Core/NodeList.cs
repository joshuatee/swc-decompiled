using Net.RichardLord.Ash.Internal;
using StaRTS.Main.Models.Entities.Nodes;
using System;
using System.Runtime.CompilerServices;
using WinRTBridge;

namespace Net.RichardLord.Ash.Core
{
	public class NodeList<TNode> : INodeList where TNode : Node<TNode>, new()
	{
		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action<TNode> NodeAdded;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action<TNode> NodeRemoved;

		public TNode Head
		{
			get;
			set;
		}

		public TNode Tail
		{
			get;
			set;
		}

		public bool Empty
		{
			get
			{
				return this.Head == null;
			}
		}

		internal void Add(TNode node)
		{
			if (this.Head == null)
			{
				this.Tail = node;
				this.Head = node;
				Node<TNode> arg_38_0 = node;
				Node<TNode> arg_32_0 = node;
				TNode tNode = default(TNode);
				arg_32_0.Previous = tNode;
				arg_38_0.Next = tNode;
			}
			else
			{
				this.Tail.Next = node;
				node.Previous = this.Tail;
				node.Next = default(TNode);
				this.Tail = node;
			}
			if (this.NodeAdded != null)
			{
				this.NodeAdded.Invoke(node);
			}
		}

		internal void Remove(TNode node)
		{
			if (this.Head == node)
			{
				this.Head = this.Head.Next;
			}
			if (this.Tail == node)
			{
				this.Tail = this.Tail.Previous;
			}
			if (node.Previous != null)
			{
				node.Previous.Next = node.Next;
			}
			if (node.Next != null)
			{
				node.Next.Previous = node.Previous;
			}
			if (this.NodeRemoved != null)
			{
				this.NodeRemoved.Invoke(node);
			}
		}

		internal void RemoveAll()
		{
			while (this.Head != null)
			{
				TNode head = this.Head;
				this.Head = head.Next;
				head.Previous = default(TNode);
				head.Next = default(TNode);
				if (this.NodeRemoved != null)
				{
					this.NodeRemoved.Invoke(head);
				}
			}
			this.Tail = default(TNode);
		}

		public void Swap(TNode node1, TNode node2)
		{
			if (node1.Previous == node2)
			{
				node1.Previous = node2.Previous;
				node2.Previous = node1;
				node2.Next = node1.Next;
				node1.Next = node2;
			}
			else if (node2.Previous == node1)
			{
				node2.Previous = node1.Previous;
				node1.Previous = node2;
				node1.Next = node2.Next;
				node2.Next = node1;
			}
			else
			{
				TNode tNode = node1.Previous;
				node1.Previous = node2.Previous;
				node2.Previous = tNode;
				tNode = node1.Next;
				node1.Next = node2.Next;
				node2.Next = tNode;
			}
			if (this.Head == node1)
			{
				this.Head = node2;
			}
			else if (this.Head == node2)
			{
				this.Head = node1;
			}
			if (this.Tail == node1)
			{
				this.Tail = node2;
			}
			else if (this.Tail == node2)
			{
				this.Tail = node1;
			}
			if (node1.Previous != null)
			{
				node1.Previous.Next = node1;
			}
			if (node2.Previous != null)
			{
				node2.Previous.Next = node2;
			}
			if (node1.Next != null)
			{
				node1.Next.Previous = node1;
			}
			if (node2.Next != null)
			{
				node2.Next.Previous = node2;
			}
		}

		public int CalculateCount()
		{
			int num = 0;
			for (TNode tNode = this.Head; tNode != null; tNode = tNode.Next)
			{
				num++;
			}
			return num;
		}

		public void CleanUp()
		{
			NodeListSingleton<TNode>.NodeList = null;
		}

		public NodeList()
		{
		}

		protected internal NodeList(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<ArmoryNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((NodeList<ArmoryNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<ArmoryNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((NodeList<ArmoryNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<BarracksNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((NodeList<BarracksNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<BarracksNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((NodeList<BarracksNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<CantinaNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((NodeList<CantinaNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<CantinaNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((NodeList<CantinaNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<ChampionPlatformNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			((NodeList<ChampionPlatformNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<ChampionPlatformNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((NodeList<ChampionPlatformNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<ClearableNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			((NodeList<ClearableNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<ClearableNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((NodeList<ClearableNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<DefenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((NodeList<DefenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<DefenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((NodeList<DefenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<DroidHutNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			((NodeList<DroidHutNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<DroidHutNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((NodeList<DroidHutNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<FactoryNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			((NodeList<FactoryNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<FactoryNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((NodeList<FactoryNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<FleetCommandNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((NodeList<FleetCommandNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<FleetCommandNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((NodeList<FleetCommandNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<GeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((NodeList<GeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<GeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			((NodeList<GeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<GeneratorViewNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			((NodeList<GeneratorViewNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<GeneratorViewNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			((NodeList<GeneratorViewNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<HQNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			((NodeList<HQNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<HQNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((NodeList<HQNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<HousingNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((NodeList<HousingNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<HousingNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			((NodeList<HousingNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<NavigationCenterNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			((NodeList<NavigationCenterNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<NavigationCenterNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			((NodeList<NavigationCenterNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<OffenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			((NodeList<OffenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<OffenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((NodeList<OffenseLabNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<ScoutTowerNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((NodeList<ScoutTowerNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<ScoutTowerNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((NodeList<ScoutTowerNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<ShieldGeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			((NodeList<ShieldGeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<ShieldGeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			((NodeList<ShieldGeneratorNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<SquadBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			((NodeList<SquadBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<SquadBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			((NodeList<SquadBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<StarportNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((NodeList<StarportNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<StarportNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((NodeList<StarportNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<StorageNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			((NodeList<StorageNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<StorageNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			((NodeList<StorageNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<TacticalCommandNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			((NodeList<TacticalCommandNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<TacticalCommandNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			((NodeList<TacticalCommandNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<TrapBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((NodeList<TrapBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<TrapBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((NodeList<TrapBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<TrapNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((NodeList<TrapNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<TrapNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			((NodeList<TrapNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<TurretBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			((NodeList<TurretBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<TurretBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			((NodeList<TurretBuildingNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<WallNode>)GCHandledObjects.GCHandleToObject(instance)).CalculateCount());
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			((NodeList<WallNode>)GCHandledObjects.GCHandleToObject(instance)).CleanUp();
			return -1L;
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((NodeList<WallNode>)GCHandledObjects.GCHandleToObject(instance)).Empty);
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			((NodeList<WallNode>)GCHandledObjects.GCHandleToObject(instance)).RemoveAll();
			return -1L;
		}
	}
}
