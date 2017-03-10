using StaRTS.Externals.Manimal;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Player;
using StaRTS.Main.Utils.Events;
using StaRTS.Utils;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;
using WinRTBridge;

namespace StaRTS.Main.Controllers
{
	public class QuietCorrectionController : IEventObserver
	{
		public delegate void HandleBatch(Batch output);

		private bool enabled;

		private Batch input;

		private QuietCorrectionController.HandleBatch correctedCallback;

		public HashSet<uint> StatusWhitelist
		{
			get;
			private set;
		}

		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				this.enabled = value;
				if (this.enabled)
				{
					ServerAPI serverAPI = Service.Get<ServerAPI>();
					string[] array = GameConstants.QUIET_CORRECTION_WHITELIST.Split(new char[]
					{
						','
					});
					this.StatusWhitelist = new HashSet<uint>();
					int i = 0;
					int num = array.Length;
					while (i < num)
					{
						uint item = Convert.ToUInt32(array[i], CultureInfo.InvariantCulture);
						this.StatusWhitelist.Add(item);
						serverAPI.GetDispatcher().SuccessStatuses.Add(item);
						i++;
					}
				}
			}
		}

		public QuietCorrectionController()
		{
			Service.Set<QuietCorrectionController>(this);
			Service.Get<EventManager>().RegisterObserver(this, EventId.MetaDataLoadEnd, EventPriority.Default);
		}

		public void CorrectBatch(Batch input, List<Data> responses, int start, QuietCorrectionController.HandleBatch callback)
		{
			this.input = input;
			this.correctedCallback = callback;
			ICommand command = input.Commands[start];
			Service.Get<StaRTSLogger>().WarnFormat("Quietly correct client to match server - {0}", new object[]
			{
				command.Description
			});
			int count = Math.Min(start + 1, input.Commands.Count);
			input.Commands.RemoveRange(0, count);
			responses.RemoveRange(0, count);
			GetSyncContentCommand getSyncContentCommand = new GetSyncContentCommand(new PlayerIdRequest
			{
				PlayerId = Service.Get<CurrentPlayer>().PlayerId
			});
			getSyncContentCommand.AddSuccessCallback(new AbstractCommand<PlayerIdRequest, GetSyncContentResponse>.OnSuccessCallback(this.OnResyncSuccess));
			Service.Get<ServerAPI>().Async(getSyncContentCommand);
		}

		private void OnResyncSuccess(GetSyncContentResponse response, object cookie)
		{
			int i = 0;
			int count = this.input.Commands.Count;
			while (i < count)
			{
				ICommand command = this.input.Commands[i];
				if (command is GameActionCommand<PlayerIdChecksumRequest, AbstractResponse>)
				{
					GameActionCommand<PlayerIdChecksumRequest, AbstractResponse> gameActionCommand = command as GameActionCommand<PlayerIdChecksumRequest, AbstractResponse>;
					PlayerIdChecksumRequest requestArgs = gameActionCommand.RequestArgs;
					requestArgs.RecalculateChecksum();
				}
				i++;
			}
			Service.Get<UXController>().HUD.RefreshView();
			if (this.input.Commands.Count > 0 && this.correctedCallback != null)
			{
				this.correctedCallback(this.input);
			}
		}

		public EatResponse OnEvent(EventId id, object cookie)
		{
			if (id == EventId.MetaDataLoadEnd)
			{
				Service.Get<EventManager>().UnregisterObserver(this, EventId.MetaDataLoadEnd);
				this.Enabled = GameConstants.QUIET_CORRECTION_ENABLED;
			}
			return EatResponse.NotEaten;
		}

		protected internal QuietCorrectionController(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			((QuietCorrectionController)GCHandledObjects.GCHandleToObject(instance)).CorrectBatch((Batch)GCHandledObjects.GCHandleToObject(*args), (List<Data>)GCHandledObjects.GCHandleToObject(args[1]), *(int*)(args + 2), (QuietCorrectionController.HandleBatch)GCHandledObjects.GCHandleToObject(args[3]));
			return -1L;
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QuietCorrectionController)GCHandledObjects.GCHandleToObject(instance)).Enabled);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((QuietCorrectionController)GCHandledObjects.GCHandleToObject(instance)).OnEvent((EventId)(*(int*)args), GCHandledObjects.GCHandleToObject(args[1])));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((QuietCorrectionController)GCHandledObjects.GCHandleToObject(instance)).OnResyncSuccess((GetSyncContentResponse)GCHandledObjects.GCHandleToObject(*args), GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((QuietCorrectionController)GCHandledObjects.GCHandleToObject(instance)).Enabled = (*(sbyte*)args != 0);
			return -1L;
		}
	}
}
