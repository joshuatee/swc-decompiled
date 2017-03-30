using StaRTS.Assets;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Main.Controllers;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace StaRTS.Externals.Manimal
{
	public class Dispatcher
	{
		public const uint SUCCESS_WITH_DESYNC = 5001u;

		private const string BATCH = "batch";

		private const string DEVICE_MODEL = "DModel";

		private const string DEVICE_TYPE = "DType";

		private const string DEVICE_OS = "DOS";

		private const string DEVICE_SYSTEM_MEMORY = "DMemory";

		private const string DEVICE_NUMBER_PROCESSORS = "DProcessors";

		private const string DEVICE_PROCESSOR_TYPE = "DProcessorType";

		private const string DEVICE_GRAPHICS_NAME = "DGName";

		private const string DEVICE_GRAPHICS_VENDOR = "DGVendor";

		private const string DEVICE_GRAPHICS_VERSION = "DGVersion";

		private const string DEVICE_GRAPHICS_MEMORY = "DGMemory";

		private const string DEVICE_GRAPHICS_SHADER_LEVEL = "DGShaderLevel";

		private const string CLIENT_APP_VERSION = "CAppVersion";

		private const string CELLOPHANE_AUTH = "Authorization";

		private const string CELLOPHANE_HEADER = "FD :";

		private const uint MAX_RETRIES = 3u;

		private string url;

		private MonoBehaviour engine;

		private bool pickupMessages;

		private bool syncDispatchLock;

		private uint lastSuccessfulSyncReqestId;

		private IResponseHandler responseHandler;

		private QuietCorrectionController qcc;

		private uint qccCount;

		public HashSet<uint> SuccessStatuses;

		private Logger logger;

		private Dictionary<string, string> headers;

		public string AuthKey
		{
			get;
			set;
		}

		public uint LastLoginTime
		{
			get;
			set;
		}

		public string Url
		{
			get
			{
				return this.url;
			}
		}

		public Dispatcher(string url, MonoBehaviour engine, bool pickupMessages, IResponseHandler responseHandler)
		{
			this.url = url;
			this.engine = engine;
			this.pickupMessages = pickupMessages;
			this.SuccessStatuses = new HashSet<uint>();
			this.SuccessStatuses.Add(0u);
			this.SuccessStatuses.Add(5001u);
			this.SuccessStatuses.Add(6300u);
			this.AuthKey = string.Empty;
			this.LastLoginTime = 0u;
			this.responseHandler = responseHandler;
			this.syncDispatchLock = false;
			this.logger = Service.Get<Logger>();
			this.qcc = Service.Get<QuietCorrectionController>();
			this.headers = this.GetDeviceInfoHeaders();
		}

		public void Exec(Batch batch)
		{
			if (batch.Sync)
			{
				if (this.lastSuccessfulSyncReqestId >= this.FindMinMaxCommandId(batch, true))
				{
					int count = batch.Commands.Count;
					ICommand command = batch.Commands[0];
					ICommand command2 = batch.Commands[count - 1];
					if (!(command is KeepAliveCommand) || !(command2 is KeepAliveCommand))
					{
						Service.Get<Logger>().WarnFormat("Successful request is being resent. Tries: {0}, Command Count: {1}, First Command Time: {2}, Last Command Time: {3} First Command ID: {4}, Last Command ID: {5} First Command: {6}, Last Command: {7}", new object[]
						{
							batch.Tries,
							count,
							command.Time,
							command2.Time,
							command.Id,
							command2.Id,
							command.Description,
							command2.Description
						});
					}
				}
				this.syncDispatchLock = true;
			}
			batch.Prepare(this.AuthKey, this.LastLoginTime, this.pickupMessages);
			string text = batch.ToJson();
			string name = "Sending batch request";
			Service.Get<AssetManager>().Profiler.RecordFetchEvent(name, text.Length, true, false);
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("batch", text);
			this.engine.StartCoroutine(this.Call(wWWForm, batch));
		}

		public void ReCall(Batch batch)
		{
			this.qccCount += 1u;
			if (this.qccCount > 3u)
			{
				this.responseHandler.Desync(DesyncType.CriticalCommandFail, 5000u);
				return;
			}
			string value = batch.ToJson();
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("batch", value);
			this.syncDispatchLock = false;
			this.engine.StartCoroutine(this.Call(wWWForm, batch));
		}

		private string CreateCommandErrorString(ICommand command, uint serverTime)
		{
			return string.Format("ID: {0}, Try: {1}, Time: {2}, Description: {3}", new object[]
			{
				command.Id,
				command.Tries,
				serverTime,
				command.Description
			});
		}

		private string CreateBatchErrorString(Batch batch, WWW www, uint serverTime)
		{
			int count = batch.Commands.Count;
			uint id = batch.Commands[0].Id;
			string description = batch.Commands[0].Description;
			uint id2 = batch.Commands[count - 1].Id;
			string description2 = batch.Commands[count - 1].Description;
			return string.Format("WWW Error: {0} received. Sync: {1}, Try: {2}, Command Count: {3}, Time: {4} First Command ID: {5}, Last Command ID: {6} First Command: {7}, Last Command: {8}", new object[]
			{
				www.error,
				batch.Sync,
				batch.Tries,
				count,
				serverTime,
				id,
				id2,
				description,
				description2
			});
		}

		private uint FindMinMaxCommandId(Batch batch, bool isMin)
		{
			uint num = batch.Commands[0].Id;
			int i = 1;
			int count = batch.Commands.Count;
			while (i < count)
			{
				uint id = batch.Commands[i].Id;
				if (isMin)
				{
					num = Math.Min(num, id);
				}
				else
				{
					num = Math.Max(num, id);
				}
				i++;
			}
			return num;
		}

		private Dictionary<string, string> GetDeviceInfoHeaders()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["DModel"] = SystemInfo.deviceModel;
			dictionary["DType"] = SystemInfo.deviceType.ToString();
			dictionary["DOS"] = SystemInfo.operatingSystem;
			dictionary["DMemory"] = string.Format("{0} MB", SystemInfo.systemMemorySize);
			dictionary["DProcessors"] = SystemInfo.processorCount.ToString();
			dictionary["DProcessorType"] = SystemInfo.processorType;
			dictionary["DGName"] = SystemInfo.graphicsDeviceName;
			dictionary["DGVendor"] = SystemInfo.graphicsDeviceVendor;
			dictionary["DGVersion"] = SystemInfo.graphicsDeviceVersion;
			dictionary["DGMemory"] = string.Format("{0} MB", SystemInfo.graphicsMemorySize);
			dictionary["DGShaderLevel"] = SystemInfo.graphicsShaderLevel.ToString();
			dictionary["Authorization"] = "FD :";
			dictionary["CAppVersion"] = "4.8.0.9512";
			return dictionary;
		}

		[DebuggerHidden]
		private IEnumerator Call(WWWForm form, Batch batch)
		{
			Dispatcher.<Call>c__Iterator13 <Call>c__Iterator = new Dispatcher.<Call>c__Iterator13();
			<Call>c__Iterator.form = form;
			<Call>c__Iterator.batch = batch;
			<Call>c__Iterator.<$>form = form;
			<Call>c__Iterator.<$>batch = batch;
			<Call>c__Iterator.<>f__this = this;
			return <Call>c__Iterator;
		}

		public bool IsFree()
		{
			return !this.syncDispatchLock;
		}
	}
}
