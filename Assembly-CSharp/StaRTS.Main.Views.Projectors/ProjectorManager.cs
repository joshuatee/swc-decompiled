using StaRTS.Main.Views.UX.Elements;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.Projectors
{
	public class ProjectorManager
	{
		private List<GeometryProjector> renderInstances;

		private List<int> unusedIndices;

		private int highestUnusedIndex;

		public ProjectorManager()
		{
			Service.Set<ProjectorManager>(this);
			this.renderInstances = new List<GeometryProjector>();
			this.unusedIndices = new List<int>();
			this.highestUnusedIndex = 0;
		}

		public int AddProjector(GeometryProjector projector)
		{
			this.DestroyProjector(projector.Config.FrameSprite);
			this.renderInstances.Add(projector);
			int num;
			if (this.unusedIndices.Count > 0)
			{
				num = this.unusedIndices[0];
				this.unusedIndices.RemoveAt(0);
			}
			else
			{
				num = this.highestUnusedIndex;
				this.highestUnusedIndex++;
			}
			Service.Get<StaRTSLogger>().DebugFormat("* About to assign {0} to booth {1}", new object[]
			{
				projector.Config.FrameSprite,
				num
			});
			return num;
		}

		public void DestroyProjector(UXSprite sprite)
		{
			if (sprite == null)
			{
				return;
			}
			for (int i = 0; i < this.renderInstances.Count; i++)
			{
				GeometryProjector geometryProjector = this.renderInstances[i];
				if (geometryProjector.Config != null && geometryProjector.Config.FrameSprite == sprite)
				{
					geometryProjector.Config.FrameSprite = null;
					geometryProjector.Destroy();
				}
			}
		}

		public void RemoveProjector(GeometryProjector projector)
		{
			this.renderInstances.Remove(projector);
			int projectorIndex = projector.ProjectorIndex;
			if (this.unusedIndices.Count == 0)
			{
				this.unusedIndices.Add(projectorIndex);
				return;
			}
			if (!this.unusedIndices.Contains(projectorIndex))
			{
				int index = ProjectorManager.BinarySearch(ref this.unusedIndices, 0, this.unusedIndices.Count, projectorIndex);
				this.unusedIndices.Insert(index, projectorIndex);
			}
		}

		private static int BinarySearch(ref List<int> list, int leftPosition, int rightPosition, int item)
		{
			if (rightPosition - leftPosition <= 1)
			{
				return rightPosition;
			}
			int num = (rightPosition - leftPosition) / 2 + leftPosition;
			if (item == list[num])
			{
				return num;
			}
			if (item > num)
			{
				return ProjectorManager.BinarySearch(ref list, num, rightPosition, item);
			}
			return ProjectorManager.BinarySearch(ref list, leftPosition, num, item);
		}

		protected internal ProjectorManager(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectorManager)GCHandledObjects.GCHandleToObject(instance)).AddProjector((GeometryProjector)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ProjectorManager)GCHandledObjects.GCHandleToObject(instance)).DestroyProjector((UXSprite)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ProjectorManager)GCHandledObjects.GCHandleToObject(instance)).RemoveProjector((GeometryProjector)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
