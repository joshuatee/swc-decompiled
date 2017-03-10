using StaRTS.Assets;
using StaRTS.Main.Configs;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UserInput;
using StaRTS.Main.Views.UX;
using StaRTS.Main.Views.UX.Elements;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Scheduling;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class ScreenController : IViewFrameTimeObserver, IEventObserver
	{
		private List<ScreenInfo> screens;

		private Dictionary<string, GameObjectContainer> cache;

		private List<AssetRequest> loadQueue;

		private Queue<ScreenInfo> queuedScreens;

		private ScreenInfo scrim;

		public ScreenController()
		{
			Service.Set<ScreenController>(this);
			this.screens = new List<ScreenInfo>();
			this.cache = new Dictionary<string, GameObjectContainer>();
			this.loadQueue = new List<AssetRequest>();
			this.queuedScreens = new Queue<ScreenInfo>();
			this.scrim = null;
			Service.Get<EventManager>().RegisterObserver(this, EventId.UpdateScrim, EventPriority.Default);
		}

		public void AddScreen(UXElement screen, bool modal, QueueScreenBehavior subType)
		{
			this.AddScreen(screen, modal, true, subType);
		}

		public void AddScreen(UXElement screen, bool modal)
		{
			this.AddScreen(screen, modal, true, QueueScreenBehavior.Default);
		}

		public void AddScreen(UXElement screen, bool modal, bool visibleScrim)
		{
			this.AddScreen(screen, modal, visibleScrim, QueueScreenBehavior.Default);
		}

		public void AddScreen(ScreenBase screen, QueueScreenBehavior subType)
		{
			this.AddScreen(screen, true, subType);
			if (!UXUtils.ShouldShowHudBehindScreen(screen.AssetName))
			{
				Service.Get<UXController>().HUD.Visible = false;
			}
		}

		public ScreenInfo AddScreen(UXElement screen, bool modal, bool visibleScrim, QueueScreenBehavior subType)
		{
			ScreenBase highestLevelScreen = this.GetHighestLevelScreen<ScreenBase>();
			if (highestLevelScreen != null)
			{
				AlertScreen alertScreen = highestLevelScreen as AlertScreen;
				if (alertScreen != null && alertScreen.IsFatal)
				{
					screen.Visible = false;
					return null;
				}
			}
			ScreenBase screenBase = screen as ScreenBase;
			AlertScreen alertScreen2 = screen as AlertScreen;
			ScreenInfo screenInfo = new ScreenInfo(screen, modal, visibleScrim, subType);
			if (!this.HandleScreenQueue(screenInfo))
			{
				if (highestLevelScreen != null && highestLevelScreen.IsAlwaysOnTop && this.screens.Count > 0)
				{
					if ((screenBase != null && screenBase.IsAlwaysOnTop) || (alertScreen2 != null && alertScreen2.IsFatal))
					{
						this.screens.Add(screenInfo);
					}
					else
					{
						this.screens.Insert(this.screens.Count - 1, screenInfo);
					}
				}
				else
				{
					this.screens.Add(screenInfo);
				}
				screen.Visible = true;
				this.UpdateScrimAndDepths();
				Service.Get<UserInputManager>().ResetLastScreenPosition();
			}
			return screenInfo;
		}

		public UXElement GetLastScreen()
		{
			if (this.screens.Count > 0)
			{
				return this.screens[this.screens.Count - 1].Screen;
			}
			return null;
		}

		private bool HandleScreenQueue(ScreenInfo screen)
		{
			bool flag = false;
			if ((screen.QueueBehavior == QueueScreenBehavior.Queue || screen.QueueBehavior == QueueScreenBehavior.QueueAndDeferTillClosed) && (this.queuedScreens.Count > 1 || this.IsModalDialogActive()))
			{
				flag = true;
			}
			if (!flag)
			{
				flag = this.ShouldForceScreenToQueue();
			}
			if (flag)
			{
				this.AddScreenInfoToQueue(screen);
			}
			return flag;
		}

		private void AddScreenInfoToQueue(ScreenInfo screen)
		{
			screen.OnEnqueued();
			this.queuedScreens.Enqueue(screen);
			ScreenBase screenBase = screen.Screen as ScreenBase;
			if (screenBase != null)
			{
				screenBase.OnScreenAddedToQueue();
			}
		}

		private bool ShouldForceScreenToQueue()
		{
			for (int i = this.screens.Count - 1; i >= 0; i--)
			{
				if (this.screens[i].ShouldDefer())
				{
					return true;
				}
			}
			return false;
		}

		private bool ShouldDequeueScreen(ScreenInfo screen)
		{
			bool flag = !this.ShouldForceScreenToQueue();
			if (flag)
			{
				flag = (!screen.HasQueueBehavior() || !this.IsModalDialogActive());
			}
			return flag;
		}

		private void PopAndShowNextQueuedScreen()
		{
			if (this.queuedScreens.Count >= 1 && this.ShouldDequeueScreen(this.queuedScreens.Peek()))
			{
				ScreenInfo screenInfo = this.queuedScreens.Dequeue();
				screenInfo.OnDequeued();
				screenInfo = this.AddScreen(screenInfo.Screen, screenInfo.IsModal, screenInfo.VisibleScrim, screenInfo.QueueBehavior);
				if (screenInfo != null)
				{
					ScreenBase screenBase = screenInfo.Screen as ScreenBase;
					if (screenBase != null)
					{
						screenBase.OnScreeenPoppedFromQueue();
					}
					if (screenInfo.QueueBehavior == QueueScreenBehavior.Default && !screenInfo.IsModal)
					{
						this.PopAndShowNextQueuedScreen();
					}
				}
			}
		}

		public bool IsModalDialogActive()
		{
			for (int i = this.screens.Count - 1; i >= 0; i--)
			{
				if (this.screens[i].IsModal)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsFatalAlertActive()
		{
			for (int i = this.screens.Count - 1; i >= 0; i--)
			{
				AlertScreen alertScreen = this.screens[i].Screen as AlertScreen;
				if (alertScreen != null && alertScreen.IsFatal)
				{
					return true;
				}
			}
			return false;
		}

		public void AddScreen(ScreenBase screen)
		{
			this.AddScreen(screen, true);
			if (!UXUtils.ShouldShowHudBehindScreen(screen.AssetName))
			{
				Service.Get<UXController>().HUD.Visible = false;
			}
		}

		public void RemoveScreen(ScreenBase screen)
		{
			if (this.RemoveScreenHelper(screen))
			{
				this.UpdateScrimAndDepths();
			}
			this.PopAndShowNextQueuedScreen();
		}

		public void RecalculateHudVisibility()
		{
			HUD hUD = Service.Get<UXController>().HUD;
			if (!hUD.ReadyToToggleVisiblity)
			{
				return;
			}
			bool flag = true;
			for (int i = this.screens.Count - 1; i >= 0; i--)
			{
				ScreenBase screenBase = this.screens[i].Screen as ScreenBase;
				if (screenBase != null)
				{
					if (!screenBase.IsClosing)
					{
						flag = UXUtils.ShouldShowHudBehindScreen(screenBase.AssetName);
						if (!flag)
						{
							break;
						}
					}
					else
					{
						flag = !UXUtils.ShouldHideHudAfterClosingScreen(screenBase.AssetName);
						if (!flag)
						{
							break;
						}
					}
				}
			}
			if (hUD.Visible != flag)
			{
				hUD.Visible = flag;
			}
		}

		public void RecalculateCurrencyTrayVisibility()
		{
			UXController uXController = Service.Get<UXController>();
			if (uXController == null || !uXController.MiscElementsManager.IsLoaded())
			{
				return;
			}
			bool flag = false;
			for (int i = this.screens.Count - 1; i >= 0; i--)
			{
				ScreenBase screenBase = this.screens[i].Screen as ScreenBase;
				if (screenBase != null && screenBase.IsLoaded() && screenBase.ShowCurrencyTray)
				{
					flag = true;
					uXController.MiscElementsManager.DetachCurrencyTray();
					screenBase.UpdateCurrencyTrayAttachment();
					break;
				}
			}
			if (!flag)
			{
				uXController.MiscElementsManager.DetachCurrencyTray();
			}
		}

		public void LogAllScreens()
		{
			for (int i = this.screens.Count - 1; i >= 0; i--)
			{
				if (this.screens[i].Screen != null)
				{
					Service.Get<StaRTSLogger>().Warn("Active Screen: " + this.screens[i].Screen.Root.name);
				}
			}
		}

		public T GetHighestLevelScreen<T>() where T : UXElement
		{
			for (int i = this.screens.Count - 1; i >= 0; i--)
			{
				if (this.screens[i].Screen is T)
				{
					return this.screens[i].Screen as T;
				}
			}
			return default(T);
		}

		public T FindElement<T>(string elementName) where T : UXElement
		{
			if (this.screens.Count == 0)
			{
				return default(T);
			}
			for (int i = this.screens.Count - 1; i >= 0; i--)
			{
				if (this.screens[i].Screen is UXFactory)
				{
					UXFactory uXFactory = this.screens[i].Screen as UXFactory;
					if (uXFactory.HasElement<T>(elementName))
					{
						return uXFactory.GetElement<T>(elementName);
					}
				}
			}
			return default(T);
		}

		public void PreloadAndCacheScreens(AssetsCompleteDelegate onComplete, object cookie)
		{
			int num = AssetConstants.GUI_PRELOADED_SCREENS.Length;
			if (num == 0)
			{
				onComplete(cookie);
				return;
			}
			List<string> list = new List<string>();
			List<object> list2 = new List<object>();
			List<AssetHandle> list3 = new List<AssetHandle>();
			for (int i = 0; i < num; i++)
			{
				string item = AssetConstants.GUI_PRELOADED_SCREENS[i];
				list.Add(item);
				list2.Add(item);
				list3.Add(AssetHandle.Invalid);
			}
			Service.Get<AssetManager>().MultiLoad(list3, list, new AssetSuccessDelegate(this.PreloadSuccess), null, list2, onComplete, cookie);
		}

		private void PreloadSuccess(object asset, object cookie)
		{
			string text = (string)cookie;
			bool flag = false;
			string[] gUI_CACHED_SCREENS = AssetConstants.GUI_CACHED_SCREENS;
			int i = 0;
			int num = gUI_CACHED_SCREENS.Length;
			while (i < num)
			{
				if (gUI_CACHED_SCREENS[i] == text)
				{
					flag = true;
					break;
				}
				i++;
			}
			if (flag)
			{
				GameObject gameObject = Service.Get<AssetManager>().CloneGameObject(asset as GameObject);
				gameObject.SetActive(false);
				this.cache.Add(text, new GameObjectContainer(gameObject));
			}
		}

		public bool LoadCachedScreen(ref AssetHandle assetHandle, string assetName, AssetSuccessDelegate onSuccess, AssetFailureDelegate onFailure, object cookie)
		{
			if (this.cache.ContainsKey(assetName))
			{
				assetHandle = AssetHandle.FirstUser;
				AssetRequest item = new AssetRequest(assetHandle, assetName, onSuccess, onFailure, cookie);
				this.loadQueue.Add(item);
				Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver(this);
				return true;
			}
			return false;
		}

		public bool UnloadCachedScreen(string assetName)
		{
			if (this.cache.ContainsKey(assetName))
			{
				int i = 0;
				int count = this.loadQueue.Count;
				while (i < count)
				{
					if (this.loadQueue[i].AssetName == assetName)
					{
						this.LoadQueueRemoveAt(i);
						break;
					}
					i++;
				}
				GameObjectContainer gameObjectContainer = this.cache[assetName];
				GameObject gameObj = gameObjectContainer.GameObj;
				gameObj.name = assetName;
				gameObj.SetActive(false);
				gameObjectContainer.Flagged = false;
				return true;
			}
			return false;
		}

		public void OnViewFrameTime(float dt)
		{
			int num = this.loadQueue.Count - 1;
			AssetRequest assetRequest = this.loadQueue[num];
			this.LoadQueueRemoveAt(num);
			string assetName = assetRequest.AssetName;
			GameObjectContainer gameObjectContainer = this.cache[assetName];
			GameObject gameObj = gameObjectContainer.GameObj;
			gameObj.SetActive(true);
			if (gameObjectContainer.Flagged)
			{
				Service.Get<StaRTSLogger>().Error("Cannot use a cached screen multiple times: " + gameObj.name);
				if (assetRequest.OnFailure != null)
				{
					assetRequest.OnFailure(assetRequest.Cookie);
					return;
				}
			}
			else
			{
				gameObjectContainer.Flagged = true;
				if (assetRequest.OnSuccess != null)
				{
					assetRequest.OnSuccess(gameObj, assetRequest.Cookie);
				}
			}
		}

		private void LoadQueueRemoveAt(int i)
		{
			this.loadQueue.RemoveAt(i);
			if (this.loadQueue.Count == 0)
			{
				Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver(this);
			}
		}

		private bool RemoveScreenHelper(UXElement screen)
		{
			int i = 0;
			int count = this.screens.Count;
			while (i < count)
			{
				ScreenInfo screenInfo = this.screens[i];
				if (screenInfo.Screen == screen)
				{
					this.RestoreDepths(i);
					this.screens.RemoveAt(i);
					return true;
				}
				i++;
			}
			return false;
		}

		private void UpdateScrimAndDepths()
		{
			bool flag = false;
			if (this.scrim != null)
			{
				this.RemoveScreenHelper(this.scrim.Screen);
				this.scrim = null;
			}
			for (int i = this.screens.Count - 1; i >= 0; i--)
			{
				if (this.screens[i].IsModal || this.screens[i].IsPersistentAndOpen())
				{
					UXElement screen = Service.Get<UXController>().MiscElementsManager.ShowScrim(true, this.screens[i].VisibleScrim);
					this.scrim = new ScreenInfo(screen, false);
					this.screens.Insert(i, this.scrim);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Service.Get<UXController>().MiscElementsManager.ShowScrim(false);
			}
			this.AdjustDepths();
		}

		public void AdjustDepths()
		{
			int num = 0;
			int i = 0;
			int count = this.screens.Count;
			while (i < count)
			{
				int num2 = 0;
				ScreenInfo screenInfo = this.screens[i];
				UXElement screen = screenInfo.Screen;
				if (screen.Root != null)
				{
					int depth = screenInfo.Depth;
					if (depth != num)
					{
						this.AddDepthRecursively(screen.Root, num, depth, ref num2);
						screenInfo.Depth = num;
						screenInfo.ScreenPanelThickness = num2;
					}
					else
					{
						num2 += screenInfo.ScreenPanelThickness;
					}
				}
				num += num2 + 1;
				i++;
			}
		}

		private void RestoreDepths(int i)
		{
			int num = 0;
			int num2 = 0;
			ScreenInfo screenInfo = this.screens[i];
			UXElement screen = screenInfo.Screen;
			if (screen.Root != null)
			{
				int depth = screenInfo.Depth;
				this.AddDepthRecursively(screen.Root, num, depth, ref num2);
				screenInfo.Depth = num;
			}
		}

		public void HideAll()
		{
			int i = 0;
			int count = this.screens.Count;
			while (i < count)
			{
				ScreenInfo screenInfo = this.screens[i];
				screenInfo.WasVisible = screenInfo.Screen.Visible;
				screenInfo.Screen.Visible = false;
				i++;
			}
			if (this.scrim != null)
			{
				this.scrim.WasVisible = this.scrim.Screen.Visible;
				this.scrim.Screen.Visible = false;
			}
		}

		public void RestoreVisibilityToAll()
		{
			int i = 0;
			int count = this.screens.Count;
			while (i < count)
			{
				ScreenInfo screenInfo = this.screens[i];
				screenInfo.Screen.Visible = screenInfo.WasVisible;
				i++;
			}
			this.scrim.Screen.Visible = this.scrim.WasVisible;
		}

		private bool IsScreenAutoCloseable(ScreenInfo screenInfo)
		{
			bool result = false;
			if (screenInfo.Screen is ScreenBase && !(screenInfo.Screen is HUD) && !(screenInfo.Screen is PersistentAnimatedScreen))
			{
				result = true;
			}
			return result;
		}

		public void CloseAll()
		{
			List<ScreenBase> list = new List<ScreenBase>();
			int i = 0;
			int count = this.screens.Count;
			while (i < count)
			{
				if (this.IsScreenAutoCloseable(this.screens[i]))
				{
					list.Add(this.screens[i].Screen as ScreenBase);
				}
				i++;
			}
			int count2 = this.queuedScreens.Count;
			int num = 0;
			while (num < count2 && this.queuedScreens.Count > 0)
			{
				ScreenInfo screenInfo = this.queuedScreens.Dequeue();
				if (this.IsScreenAutoCloseable(screenInfo))
				{
					list.Add(screenInfo.Screen as ScreenBase);
				}
				num++;
			}
			for (int j = list.Count - 1; j >= 0; j--)
			{
				list[j].CloseNoTransition(null);
			}
		}

		private void AddDepthRecursively(GameObject gameObject, int newDepth, int restoreDepth, ref int maxDepth)
		{
			UIPanel component = gameObject.GetComponent<UIPanel>();
			if (component != null)
			{
				int num = component.depth - restoreDepth;
				if (num > maxDepth)
				{
					maxDepth = num;
				}
				component.depth += newDepth - restoreDepth;
			}
			Transform transform = gameObject.transform;
			int i = 0;
			int childCount = transform.childCount;
			while (i < childCount)
			{
				this.AddDepthRecursively(transform.GetChild(i).gameObject, newDepth, restoreDepth, ref maxDepth);
				i++;
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.UpdateScrim)
			{
				this.UpdateScrimAndDepths();
			}
			return EatResponse.NotEaten;
		}

		protected internal ScreenController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).AddScreen((ScreenBase)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).AddScreen((UXElement)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0);
			return -1L;
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).AddScreen((ScreenBase)GCHandledObjects.GCHandleToObject(*args), (QueueScreenBehavior)(*(int*)(args + 1)));
			return -1L;
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).AddScreen((UXElement)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, (QueueScreenBehavior)(*(int*)(args + 2)));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).AddScreen((UXElement)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenController)GCHandledObjects.GCHandleToObject(instance)).AddScreen((UXElement)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0, *(sbyte*)(args + 2) != 0, (QueueScreenBehavior)(*(int*)(args + 3))));
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).AddScreenInfoToQueue((ScreenInfo)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).AdjustDepths();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).CloseAll();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenController)GCHandledObjects.GCHandleToObject(instance)).GetLastScreen());
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenController)GCHandledObjects.GCHandleToObject(instance)).HandleScreenQueue((ScreenInfo)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).HideAll();
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenController)GCHandledObjects.GCHandleToObject(instance)).IsFatalAlertActive());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenController)GCHandledObjects.GCHandleToObject(instance)).IsModalDialogActive());
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenController)GCHandledObjects.GCHandleToObject(instance)).IsScreenAutoCloseable((ScreenInfo)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).LoadQueueRemoveAt(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).LogAllScreens();
			return -1L;
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).OnViewFrameTime(*(float*)args);
			return -1L;
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).PopAndShowNextQueuedScreen();
			return -1L;
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).PreloadAndCacheScreens((AssetsCompleteDelegate)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).PreloadSuccess(GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).RecalculateCurrencyTrayVisibility();
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).RecalculateHudVisibility();
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).RemoveScreen((ScreenBase)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenController)GCHandledObjects.GCHandleToObject(instance)).RemoveScreenHelper((UXElement)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).RestoreDepths(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).RestoreVisibilityToAll();
			return -1L;
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenController)GCHandledObjects.GCHandleToObject(instance)).ShouldDequeueScreen((ScreenInfo)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenController)GCHandledObjects.GCHandleToObject(instance)).ShouldForceScreenToQueue());
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((ScreenController)GCHandledObjects.GCHandleToObject(instance)).UnloadCachedScreen(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			((ScreenController)GCHandledObjects.GCHandleToObject(instance)).UpdateScrimAndDepths();
			return -1L;
		}
	}
}
