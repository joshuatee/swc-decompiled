using StaRTS.Main.Models.Chat;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace StaRTS.Main.Utils.Chat
{
	public static class ChatSessionUtils
	{
		private const string SUBSCRIPTION_URL = "subscriptionUrl";

		private const string SIGNED_CHAT_CHANNEL_INFO = "signedChatChannelInfo";

		public static string GetSessionUrlFromChannelResponse(string channelResponse)
		{
			string result = null;
			object obj = new JsonParser(channelResponse).Parse();
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary != null)
			{
				string text = (!dictionary.ContainsKey("subscriptionUrl")) ? null : (dictionary["subscriptionUrl"] as string);
				string text2 = (!dictionary.ContainsKey("signedChatChannelInfo")) ? null : (dictionary["signedChatChannelInfo"] as string);
				if (text != null && text2 != null)
				{
					result = string.Format("https://{0}/lp?session={1}", text, text2);
				}
			}
			return result;
		}

		public static string CreateSessionString(string userId, string userName, string locale, string expires, ChatType chatType, string shardId)
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("userId", userId);
			switch (chatType)
			{
			case ChatType.Shard:
				serializer.AddString("shardId", shardId);
				break;
			case ChatType.Alliance:
				serializer.AddString("allianceId", shardId);
				break;
			case ChatType.Raid:
				serializer.AddString("raidId", shardId);
				break;
			}
			serializer.AddString("locale", locale);
			serializer.AddString("userName", userName);
			serializer.AddString("expires", expires);
			serializer.AddString("chatType", chatType.ToString().ToLower());
			return serializer.End().ToString();
		}

		public static string GetSignedString(string json)
		{
			Encoding encoding = new UTF8Encoding(true, true);
			byte[] bytes = encoding.GetBytes("e168217158d6a34d73be5220d166f312");
			string text = ChatSessionUtils.EncodeReplaceStrip(json);
			HMACSHA256 hMACSHA = new HMACSHA256(bytes);
			byte[] bytes2 = encoding.GetBytes(text);
			MemoryStream memoryStream = new MemoryStream(bytes2);
			string hexString = ChatSessionUtils.HashAggregate(hMACSHA.ComputeHash(memoryStream));
			string str = ChatSessionUtils.ConvertHexStringToBase64(hexString);
			memoryStream.Close();
			memoryStream.Dispose();
			return str + "." + text;
		}

		private static string HashAggregate(byte[] bytes)
		{
			string text = string.Empty;
			int i = 0;
			int num = bytes.Length;
			while (i < num)
			{
				text += string.Format("{0:x2}", bytes[i]);
				i++;
			}
			return text;
		}

		private static string EncodeReplaceStrip(string toEncode)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(toEncode);
			string text = Convert.ToBase64String(bytes, 0, bytes.Length);
			text = text.Replace('+', '-');
			text = text.Replace('/', '_');
			char[] trimChars = new char[]
			{
				'='
			};
			return text.TrimEnd(trimChars);
		}

		private static string ConvertHexStringToBase64(string hexString)
		{
			byte[] array = new byte[hexString.Length / 2];
			int i = 0;
			int num = 0;
			while (i < hexString.Length)
			{
				array[num] = Convert.ToByte(Convert.ToInt32(hexString.Substring(i, 2), 16));
				i += 2;
				num++;
			}
			string text = Convert.ToBase64String(array);
			text = text.Replace('+', '-');
			text = text.Replace('/', '_');
			char[] trimChars = new char[]
			{
				'='
			};
			return text.TrimEnd(trimChars);
		}
	}
}
