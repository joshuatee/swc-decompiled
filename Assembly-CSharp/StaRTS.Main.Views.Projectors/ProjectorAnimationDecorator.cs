using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.Projectors
{
	public class ProjectorAnimationDecorator : IProjectorRenderer
	{
		private IProjectorRenderer baseRenderer;

		public ProjectorAnimationDecorator(IProjectorRenderer baseRenderer)
		{
			this.baseRenderer = baseRenderer;
		}

		public void Render(ProjectorConfig config)
		{
			if (!config.AssetReady)
			{
				return;
			}
			Animator component = config.MainAsset.GetComponent<Animator>();
			if (component != null)
			{
				if (component.hasRootMotion)
				{
					component.SetInteger("Motivation", (int)config.AnimState);
				}
				else
				{
					Service.Get<StaRTSLogger>().ErrorFormat("{0} needs re-rigging, its Animator does not have a root motion.", new object[]
					{
						config.AssetName
					});
				}
			}
			else
			{
				Animation component2 = config.MainAsset.GetComponent<Animation>();
				if (component2 != null)
				{
					string text = config.AnimationName;
					if (string.IsNullOrEmpty(text))
					{
						text = config.AnimState.ToString();
					}
					if (component2.GetClip(text) != null)
					{
						component2.Play(text);
					}
				}
			}
			this.baseRenderer.Render(config);
		}

		public void PostRender(ProjectorConfig config)
		{
			this.baseRenderer.PostRender(config);
		}

		public void Destroy()
		{
			this.baseRenderer.Destroy();
			this.baseRenderer = null;
		}

		public bool DoesRenderTextureNeedReload()
		{
			return this.baseRenderer.DoesRenderTextureNeedReload();
		}

		public UITexture GetProjectorUITexture()
		{
			return this.baseRenderer.GetProjectorUITexture();
		}

		protected internal ProjectorAnimationDecorator(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ProjectorAnimationDecorator)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectorAnimationDecorator)GCHandledObjects.GCHandleToObject(instance)).DoesRenderTextureNeedReload());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ProjectorAnimationDecorator)GCHandledObjects.GCHandleToObject(instance)).GetProjectorUITexture());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ProjectorAnimationDecorator)GCHandledObjects.GCHandleToObject(instance)).PostRender((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ProjectorAnimationDecorator)GCHandledObjects.GCHandleToObject(instance)).Render((ProjectorConfig)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}
	}
}
