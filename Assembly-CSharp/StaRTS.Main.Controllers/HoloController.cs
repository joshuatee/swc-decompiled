using StaRTS.Main.Controllers.GameStates;
using StaRTS.Main.Models;
using StaRTS.Main.Story.Actions;
using StaRTS.Main.Utils.Events;
using StaRTS.Main.Views.UX.Screens;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class HoloController : IEventObserver
	{
		private EventManager events;

		private HolocommScreen holocommScreen;

		private List<HoloCommand> commandBuffer;

		public HoloController()
		{
			Service.Set<HoloController>(this);
			this.holocommScreen = new HolocommScreen();
			this.holocommScreen = null;
			this.commandBuffer = new List<HoloCommand>();
			this.events = Service.Get<EventManager>();
			this.events.RegisterObserver(this, EventId.HoloCommScreenDestroyed);
			this.events.RegisterObserver(this, EventId.ShowHologram);
			this.events.RegisterObserver(this, EventId.ShowTranscript);
			this.events.RegisterObserver(this, EventId.PlayHologramAnimation);
			this.events.RegisterObserver(this, EventId.HideHologram);
			this.events.RegisterObserver(this, EventId.HideTranscript);
			this.events.RegisterObserver(this, EventId.ShowNextButton);
			this.events.RegisterObserver(this, EventId.ShowStoreNextButton);
			this.events.RegisterObserver(this, EventId.HideAllHolograms);
			this.events.RegisterObserver(this, EventId.ScreenLoaded);
			this.events.RegisterObserver(this, EventId.ShowInfoPanel);
			this.events.RegisterObserver(this, EventId.HideInfoPanel);
			this.events.RegisterObserver(this, EventId.ScreenSizeChanged);
		}

		private void SafeRunThroughCommandBuffer()
		{
			List<HoloCommand> list = new List<HoloCommand>();
			list.AddRange(this.commandBuffer);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null)
				{
					this.OnEvent(list[i].EventId, list[i].Cookie);
				}
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id <= EventId.HoloCommScreenDestroyed)
			{
				if (id != EventId.ScreenLoaded)
				{
					if (id == EventId.HoloCommScreenDestroyed)
					{
						this.holocommScreen = null;
						if (!(Service.Get<GameStateMachine>().CurrentState is GalaxyState))
						{
							Service.Get<UXController>().HUD.Visible = true;
						}
					}
				}
				else if (cookie is HolocommScreen)
				{
					HolocommScreen holocommScreen = (HolocommScreen)cookie;
					if (!holocommScreen.HiddenInQueue)
					{
						this.SafeRunThroughCommandBuffer();
						this.commandBuffer.Clear();
					}
				}
			}
			else
			{
				switch (id)
				{
				case EventId.ShowHologram:
				{
					ShowHologramStoryAction showHologramStoryAction = (ShowHologramStoryAction)cookie;
					if (this.EnsureScreenActive())
					{
						this.holocommScreen.ShowHoloCharacter(showHologramStoryAction.Character);
					}
					else
					{
						this.StoreCommandInBuffer(id, cookie);
					}
					break;
				}
				case EventId.ShowHologramComplete:
				case EventId.ShowAttackButton:
				case EventId.HideHologramComplete:
					break;
				case EventId.ShowTranscript:
				{
					ShowTranscriptStoryAction showTranscriptStoryAction = (ShowTranscriptStoryAction)cookie;
					if (this.EnsureScreenActive())
					{
						this.holocommScreen.AddDialogue(showTranscriptStoryAction.Text, showTranscriptStoryAction.Title);
					}
					else
					{
						this.StoreCommandInBuffer(id, cookie);
					}
					break;
				}
				case EventId.PlayHologramAnimation:
				{
					PlayHoloAnimationStoryAction playHoloAnimationStoryAction = (PlayHoloAnimationStoryAction)cookie;
					this.holocommScreen.PlayHoloAnimation(playHoloAnimationStoryAction.AnimName);
					break;
				}
				case EventId.HideHologram:
					if (this.holocommScreen != null && this.holocommScreen.IsLoaded())
					{
						this.holocommScreen.CloseAndDestroyHoloCharacter();
					}
					break;
				case EventId.HideTranscript:
					if (this.holocommScreen != null && this.holocommScreen.IsLoaded())
					{
						this.holocommScreen.RemoveDialogue();
					}
					break;
				case EventId.ShowNextButton:
					if (this.EnsureScreenActive())
					{
						this.holocommScreen.ShowButton("BtnNext");
					}
					else
					{
						this.StoreCommandInBuffer(id, cookie);
					}
					break;
				case EventId.ShowStoreNextButton:
					if (this.EnsureScreenActive())
					{
						this.holocommScreen.ShowButton("ButtonStore");
					}
					else
					{
						this.StoreCommandInBuffer(id, cookie);
					}
					break;
				case EventId.HideAllHolograms:
					if (this.holocommScreen != null && this.holocommScreen.IsLoaded())
					{
						this.holocommScreen.HideAllElements();
						this.holocommScreen.CloseAndDestroyHoloCharacter();
					}
					break;
				case EventId.ShowInfoPanel:
					if (this.EnsureScreenActive())
					{
						ShowHologramInfoStoryAction showHologramInfoStoryAction = (ShowHologramInfoStoryAction)cookie;
						this.holocommScreen.ShowInfoPanel(showHologramInfoStoryAction.ImageName, showHologramInfoStoryAction.DisplayText, showHologramInfoStoryAction.TitleText, showHologramInfoStoryAction.PlanetPanel);
					}
					else
					{
						this.StoreCommandInBuffer(id, cookie);
					}
					break;
				case EventId.HideInfoPanel:
					if (this.holocommScreen != null && this.holocommScreen.IsLoaded())
					{
						this.holocommScreen.HideInfoPanel();
					}
					break;
				default:
					if (id == EventId.ScreenSizeChanged)
					{
						if (this.holocommScreen != null && this.holocommScreen.IsLoaded())
						{
							this.holocommScreen.ResizeCharacter();
						}
					}
					break;
				}
			}
			return EatResponse.NotEaten;
		}

		private void StoreCommandInBuffer(EventId id, object cookie)
		{
			HoloCommand holoCommand = new HoloCommand();
			holoCommand.EventId = id;
			holoCommand.Cookie = cookie;
			this.commandBuffer.Add(holoCommand);
		}

		private bool EnsureScreenActive()
		{
			if (this.holocommScreen == null)
			{
				this.holocommScreen = new HolocommScreen();
				if (Service.Get<ScreenController>().GetHighestLevelScreen<HQCelebScreen>() != null)
				{
					Service.Get<ScreenController>().AddScreen(this.holocommScreen, true, QueueScreenBehavior.QueueAndDeferTillClosed);
				}
				else
				{
					Service.Get<ScreenController>().AddScreen(this.holocommScreen, true);
				}
				Service.Get<UXController>().HUD.Visible = false;
				return false;
			}
			return this.holocommScreen.IsLoaded() && !this.holocommScreen.HiddenInQueue;
		}

		public Camera GetActiveCamera()
		{
			if (this.holocommScreen != null && this.holocommScreen.IsLoaded())
			{
				return this.holocommScreen.GetHoloCamera();
			}
			return null;
		}

		public bool HasAnyCharacter()
		{
			return this.holocommScreen != null && this.holocommScreen.IsLoaded() && this.holocommScreen.HasCharacterShowing();
		}

		public void ResizeCurrentCharacter(int width, int height)
		{
			if (this.HasAnyCharacter())
			{
				this.holocommScreen.ResizeCurrentCharacter(width, height);
			}
		}

		public bool CharacterAlreadyShowing(string characterId)
		{
			return this.holocommScreen != null && this.holocommScreen.IsLoaded() && this.holocommScreen.CharacterAlreadyShowing(characterId);
		}

		protected internal HoloController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HoloController)GCHandledObjects.GCHandleToObject(instance)).CharacterAlreadyShowing(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HoloController)GCHandledObjects.GCHandleToObject(instance)).EnsureScreenActive());
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HoloController)GCHandledObjects.GCHandleToObject(instance)).GetActiveCamera());
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HoloController)GCHandledObjects.GCHandleToObject(instance)).HasAnyCharacter());
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((HoloController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((HoloController)GCHandledObjects.GCHandleToObject(instance)).ResizeCurrentCharacter(*(int*)args, *(int*)(args + 1));
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((HoloController)GCHandledObjects.GCHandleToObject(instance)).SafeRunThroughCommandBuffer();
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((HoloController)GCHandledObjects.GCHandleToObject(instance)).StoreCommandInBuffer((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}
	}
}
