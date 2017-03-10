using StaRTS.Main.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.FX
{
	public class CurrencyEffectData
	{
		private List<ParticleSystem>[] effects;

		public CurrencyEffectData()
		{
			int num = 6;
			this.effects = new List<ParticleSystem>[num];
			for (int i = 0; i < num; i++)
			{
				this.effects[i] = null;
			}
		}

		public void Add(CurrencyType currency, ParticleSystem particleSystem)
		{
			List<ParticleSystem> list = this.effects[(int)currency];
			if (list == null)
			{
				list = new List<ParticleSystem>();
				this.effects[(int)currency] = list;
			}
			list.Add(particleSystem);
		}

		public List<ParticleSystem> Get(CurrencyType currency)
		{
			return this.effects[(int)currency];
		}

		public void Destroy()
		{
			int i = 0;
			int num = 6;
			while (i < num)
			{
				List<ParticleSystem> list = this.effects[i];
				if (list != null)
				{
					int j = 0;
					int count = list.Count;
					while (j < count)
					{
						Transform root = list[j].gameObject.transform.root;
						UnityEngine.Object.Destroy(root.gameObject);
						j++;
					}
					list.Clear();
				}
				i++;
			}
			this.effects = null;
		}

		protected internal CurrencyEffectData(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CurrencyEffectData)GCHandledObjects.GCHandleToObject(instance)).Add((CurrencyType)(*(int*)args), (ParticleSystem)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((CurrencyEffectData)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CurrencyEffectData)GCHandledObjects.GCHandleToObject(instance)).Get((CurrencyType)(*(int*)args)));
		}
	}
}
