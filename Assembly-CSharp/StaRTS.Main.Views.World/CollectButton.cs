using Net.RichardLord.Ash.Core;
using StaRTS.Assets;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Entities.Components;
using StaRTS.Main.Utils;
using StaRTS.Utils.Core;
using System;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Views.World
{
	public class CollectButton
	{
		public const float HEIGHT_OFFSET = 9f;

		public const float CONTRABAND_HEIGHT_OFFSET = 10f;

		public const string BUTTON_NAME_PREFIX = "Collect Button";

		private string assetName;

		private AssetHandle assetHandle;

		private GameObject view3d;

		private bool visible;

		private Entity building;

		public bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				if (this.visible != value)
				{
					this.visible = value;
					if (this.visible && this.assetHandle == AssetHandle.Invalid)
					{
						Service.Get<AssetManager>().Load(ref this.assetHandle, this.assetName, new AssetSuccessDelegate(this.OnLoad), null, null);
						return;
					}
					if (this.view3d != null)
					{
						Animator component = this.view3d.GetComponent<Animator>();
						if (component != null)
						{
							component.SetTrigger(this.visible ? "Show" : "Hide");
						}
					}
				}
			}
		}

		public CollectButton(Entity building)
		{
			this.building = building;
			BuildingComponent buildingComponent = building.Get<BuildingComponent>();
			this.assetName = GameUtils.GetSingleCurrencyItemAssetName(buildingComponent.BuildingType.Currency);
			this.visible = false;
		}

		private void OnLoad(object asset, object cookie)
		{
			this.view3d = UnityEngine.Object.Instantiate<GameObject>(asset as GameObject);
			this.view3d.name = this.assetName;
			GameObjectViewComponent gameObjectViewComponent = this.building.Get<GameObjectViewComponent>();
			BuildingComponent buildingComponent = this.building.Get<BuildingComponent>();
			Vector3 vector = Vector3.up;
			if (buildingComponent.BuildingType.Currency == CurrencyType.Contraband)
			{
				vector *= 10f;
			}
			else
			{
				vector *= 9f;
			}
			gameObjectViewComponent.AttachGameObject(this.view3d.name, this.view3d, vector, false, false);
			if (this.visible)
			{
				this.visible = false;
				this.Visible = true;
			}
		}

		public void Destroy()
		{
			GameObjectViewComponent gameObjectViewComponent = this.building.Get<GameObjectViewComponent>();
			if (gameObjectViewComponent != null)
			{
				gameObjectViewComponent.DetachGameObject(this.assetName);
			}
			if (this.view3d != null)
			{
				UnityEngine.Object.Destroy(this.view3d);
				this.view3d = null;
			}
			if (this.assetHandle != AssetHandle.Invalid)
			{
				Service.Get<AssetManager>().Unload(this.assetHandle);
				this.assetHandle = AssetHandle.Invalid;
			}
		}

		protected internal CollectButton(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((CollectButton)GCHandledObjects.GCHandleToObject(instance)).Destroy();
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((CollectButton)GCHandledObjects.GCHandleToObject(instance)).Visible);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((CollectButton)GCHandledObjects.GCHandleToObject(instance)).OnLoad(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((CollectButton)GCHandledObjects.GCHandleToObject(instance)).Visible = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
