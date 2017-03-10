using StaRTS.Main.Models.Chat;
using StaRTS.Utils.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using WinRTBridge;

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
				string text = dictionary.ContainsKey("subscriptionUrl") ? (dictionary["subscriptionUrl"] as string) : null;
				string text2 = dictionary.ContainsKey("signedChatChannelInfo") ? (dictionary["signedChatChannelInfo"] as string) : null;
				if (text != null && text2 != null)
				{
					result = string.Format("https://{0}/lp?session={1}", new object[]
					{
						text,
						text2
					});
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
			string text = ChatSessionUtils.EncodeReplaceStrip(json);
			string hexString = ChatSessionUtils.HashAggregate(CryptoUtility.ComputeHash(text, "87e278e1dd0a48649af0b77dc80a5ef1"));
			string text2 = ChatSessionUtils.ConvertHexStringToBase64(hexString);
			return text2 + "." + text;
		}

		private static string HashAggregate(byte[] bytes)
		{
			string text = "";
			int i = 0;
			int num = bytes.Length;
			while (i < num)
			{
				text += string.Format("{0:x2}", new object[]
				{
					bytes[i]
				});
				i++;
			}
			return text;
		}

		private static string EncodeReplaceStrip(string toEncode)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(toEncode);
			string text = Convert.ToBase64String(bytes, 0, bytes.Length);
			text = text.Replace('+', '-');
			text = text.Replace('/', '_');
			char[] array = new char[]
			{
				'='
			};
			return text.TrimEnd(array);
		}

		private static string ConvertHexStringToBase64(string hexString)
		{
			byte[] array = new byte[hexString.get_Length() / 2];
			int i = 0;
			int num = 0;
			while (i < hexString.get_Length())
			{
				array[num] = Convert.ToByte(Convert.ToInt32(hexString.Substring(i, 2), 16), CultureInfo.InvariantCulture);
				i += 2;
				num++;
			}
			string text = Convert.ToBase64String(array);
			text = text.Replace('+', '-');
			text = text.Replace('/', '_');
			char[] array2 = new char[]
			{
				'='
			};
			return text.TrimEnd(array2);
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ChatSessionUtils.ConvertHexStringToBase64(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ChatSessionUtils.CreateSessionString(Marshal.PtrToStringUni(*(IntPtr*)args), Marshal.PtrToStringUni(*(IntPtr*)(args + 1)), Marshal.PtrToStringUni(*(IntPtr*)(args + 2)), Marshal.PtrToStringUni(*(IntPtr*)(args + 3)), (ChatType)(*(int*)(args + 4)), Marshal.PtrToStringUni(*(IntPtr*)(args + 5))));
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ChatSessionUtils.EncodeReplaceStrip(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ChatSessionUtils.GetSessionUrlFromChannelResponse(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ChatSessionUtils.GetSignedString(Marshal.PtrToStringUni(*(IntPtr*)args)));
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(ChatSessionUtils.HashAggregate((byte[])GCHandledObjects.GCHandleToPinnedArrayObject(*args)));
		}
	}
}
