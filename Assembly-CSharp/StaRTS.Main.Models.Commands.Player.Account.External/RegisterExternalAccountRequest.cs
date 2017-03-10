using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player.Account.External
{
	public class RegisterExternalAccountRequest : AbstractRequest
	{
		public AccountProvider Provider
		{
			get;
			set;
		}

		public string ExternalAccountId
		{
			get;
			set;
		}

		public string ExternalAccountSecurityToken
		{
			get;
			set;
		}

		public bool OverrideExistingAccountRegistration
		{
			get;
			set;
		}

		public AccountProvider OtherLinkedProvider
		{
			get;
			set;
		}

		public string PlayerId
		{
			get;
			set;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start();
			serializer.AddString("playerId", this.PlayerId);
			serializer.AddString("providerId", this.Provider.ToString());
			serializer.AddString("externalAccountId", this.ExternalAccountId);
			serializer.AddString("externalAccountSecurityToken", this.ExternalAccountSecurityToken);
			serializer.AddBool("overrideExistingAccountRegistration", this.OverrideExistingAccountRegistration);
			serializer.AddString("otherLinkedProviderId", this.OtherLinkedProvider.ToString());
			return serializer.End().ToString();
		}

		public RegisterExternalAccountRequest()
		{
		}

		protected internal RegisterExternalAccountRequest(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).ExternalAccountId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).ExternalAccountSecurityToken);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).OtherLinkedProvider);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).OverrideExistingAccountRegistration);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).PlayerId);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).Provider);
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).ExternalAccountId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).ExternalAccountSecurityToken = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).OtherLinkedProvider = (AccountProvider)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).OverrideExistingAccountRegistration = (*(sbyte*)args != 0);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).PlayerId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).Provider = (AccountProvider)(*(int*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((RegisterExternalAccountRequest)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
