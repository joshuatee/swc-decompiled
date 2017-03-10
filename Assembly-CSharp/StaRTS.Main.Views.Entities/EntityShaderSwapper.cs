using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Main.Views.Entities
{
	public class EntityShaderSwapper
	{
		private List<ShaderSwappedEntity> shaderSwappedEntities;

		public EntityShaderSwapper()
		{
			this.shaderSwappedEntities = new List<ShaderSwappedEntity>();
		}

		public void HighlightForPerk(Entity entity)
		{
			this.ResetToOriginal(entity);
			string shaderName = "PL_2Color_Mask_SA";
			PerkHighlightedEntity item = new PerkHighlightedEntity(entity, shaderName);
			this.shaderSwappedEntities.Add(item);
		}

		public void Outline(Entity entity)
		{
			this.ResetToOriginal(entity);
			OutlinedEntity item = new OutlinedEntity(entity);
			this.shaderSwappedEntities.Add(item);
		}

		public bool ResetToOriginal(Entity entity)
		{
			int num = this.IndexOfEntity(entity);
			if (num >= 0)
			{
				this.shaderSwappedEntities[num].RemoveAppliedShader();
				this.shaderSwappedEntities.RemoveAt(num);
				return true;
			}
			return false;
		}

		private int IndexOfEntity(Entity entity)
		{
			int i = 0;
			int count = this.shaderSwappedEntities.Count;
			while (i < count)
			{
				ShaderSwappedEntity shaderSwappedEntity = this.shaderSwappedEntities[i];
				if (shaderSwappedEntity.Entity == entity)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		protected internal EntityShaderSwapper(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((EntityShaderSwapper)GCHandledObjects.GCHandleToObject(instance)).HighlightForPerk((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityShaderSwapper)GCHandledObjects.GCHandleToObject(instance)).IndexOfEntity((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((EntityShaderSwapper)GCHandledObjects.GCHandleToObject(instance)).Outline((Entity)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((EntityShaderSwapper)GCHandledObjects.GCHandleToObject(instance)).ResetToOriginal((Entity)GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
