using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;

namespace StaRTS.Main.Models.Commands.Player
{
	public class GetContentResponse : AbstractResponse
	{
		public List<string> CdnRoots
		{
			get;
			private set;
		}

		public string AppCode
		{
			get;
			private set;
		}

		public string Environment
		{
			get;
			private set;
		}

		public string ManifestVersion
		{
			get;
			private set;
		}

		public List<string> Patches
		{
			get;
			private set;
		}

		public override ISerializable FromObject(object obj)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			List<object> list = dictionary["secureCdnRoots"] as List<object>;
			this.CdnRoots = new List<string>();
			for (int i = 0; i < list.Count; i++)
			{
				this.CdnRoots.Add((string)list[i]);
			}
			this.AppCode = (string)dictionary["appCode"];
			this.Environment = (string)dictionary["environment"];
			this.ManifestVersion = (string)dictionary["manifestVersion"];
			list = (dictionary["patches"] as List<object>);
			this.Patches = new List<string>();
			for (int j = 0; j < list.Count; j++)
			{
				this.Patches.Add((string)list[j]);
			}
			return this;
		}
	}
}
