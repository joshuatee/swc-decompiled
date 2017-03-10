using Source.StaRTS.Main.Models.Commands.Player;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Models.Commands;
using StaRTS.Main.Models.Commands.Campaign;
using StaRTS.Main.Models.Commands.Cheats;
using StaRTS.Main.Models.Commands.Crates;
using StaRTS.Main.Models.Commands.Equipment;
using StaRTS.Main.Models.Commands.Holonet;
using StaRTS.Main.Models.Commands.Missions;
using StaRTS.Main.Models.Commands.Objectives;
using StaRTS.Main.Models.Commands.Perks;
using StaRTS.Main.Models.Commands.Planets;
using StaRTS.Main.Models.Commands.Player;
using StaRTS.Main.Models.Commands.Player.Account.External;
using StaRTS.Main.Models.Commands.Player.Building.Clear;
using StaRTS.Main.Models.Commands.Player.Building.Collect;
using StaRTS.Main.Models.Commands.Player.Building.Construct;
using StaRTS.Main.Models.Commands.Player.Building.Contracts;
using StaRTS.Main.Models.Commands.Player.Building.Move;
using StaRTS.Main.Models.Commands.Player.Building.Rearm;
using StaRTS.Main.Models.Commands.Player.Building.Swap;
using StaRTS.Main.Models.Commands.Player.Deployable;
using StaRTS.Main.Models.Commands.Player.Deployable.Upgrade.Start;
using StaRTS.Main.Models.Commands.Player.Fue;
using StaRTS.Main.Models.Commands.Player.Identity;
using StaRTS.Main.Models.Commands.Player.Raids;
using StaRTS.Main.Models.Commands.Player.Store;
using StaRTS.Main.Models.Commands.Pvp;
using StaRTS.Main.Models.Commands.Squads;
using StaRTS.Main.Models.Commands.Squads.Requests;
using StaRTS.Main.Models.Commands.Squads.Responses;
using StaRTS.Main.Models.Commands.TargetedBundleOffers;
using StaRTS.Main.Models.Commands.Test.Config;
using StaRTS.Main.Models.Commands.Tournament;
using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using StaRTS.Utils.Json;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Externals.Manimal.TransferObjects.Request
{
	public abstract class AbstractCommand<TRequest, TResponse> : AbstractRequest, ICommand, ISerializable where TRequest : AbstractRequest where TResponse : AbstractResponse
	{
		public delegate void OnSuccessCallback(TResponse response, object cookie);

		public delegate void OnFailureCallback(uint status, object cookie);

		private AbstractCommand<TRequest, TResponse>.OnSuccessCallback onSuccessCallback;

		private AbstractCommand<TRequest, TResponse>.OnFailureCallback onFailureCallback;

		public uint Id
		{
			get;
			private set;
		}

		public uint Tries
		{
			get;
			set;
		}

		public string Token
		{
			get;
			private set;
		}

		public uint Time
		{
			get;
			private set;
		}

		public string Action
		{
			get;
			private set;
		}

		public string Description
		{
			get
			{
				return this.Action;
			}
		}

		public TRequest RequestArgs
		{
			get;
			protected set;
		}

		public TResponse ResponseResult
		{
			get;
			protected set;
		}

		public object Context
		{
			get;
			set;
		}

		public AbstractRequest Request
		{
			get
			{
				return this.RequestArgs;
			}
		}

		public AbstractCommand(string action, TRequest request, TResponse response)
		{
			this.Id = RequestId.Get();
			this.Token = RequestToken.Get();
			this.Tries = 1u;
			this.Action = action;
			this.RequestArgs = request;
			this.ResponseResult = response;
			this.onSuccessCallback = null;
			this.onFailureCallback = null;
			this.Time = 0u;
		}

		public abstract void OnSuccess();

		public abstract OnCompleteAction OnFailure(uint status, object data);

		public void AddSuccessCallback(AbstractCommand<TRequest, TResponse>.OnSuccessCallback onSuccessCallback)
		{
			if (this.onSuccessCallback != null)
			{
				Service.Get<StaRTSLogger>().Error("Cannot add multiple success callbacks");
			}
			this.onSuccessCallback = onSuccessCallback;
		}

		public void AddFailureCallback(AbstractCommand<TRequest, TResponse>.OnFailureCallback onFailureCallback)
		{
			if (this.onFailureCallback != null)
			{
				Service.Get<StaRTSLogger>().Error("Cannot add multiple failure callbacks");
			}
			this.onFailureCallback = onFailureCallback;
		}

		public void RemoveAllCallbacks()
		{
			this.onSuccessCallback = null;
			this.onFailureCallback = null;
		}

		public virtual OnCompleteAction OnComplete(Data data, bool success)
		{
			if (data.Result != null)
			{
				this.ResponseResult.FromObject(data.Result);
			}
			if (success)
			{
				this.OnSuccess();
				if (this.onSuccessCallback != null)
				{
					this.onSuccessCallback(this.ResponseResult, this.Context);
				}
				return OnCompleteAction.Ok;
			}
			if (this.onFailureCallback != null)
			{
				this.onFailureCallback(data.Status, this.Context);
			}
			return this.OnFailure(data.Status, data.Result);
		}

		public ICommand SetTime(uint time)
		{
			this.Time = time;
			return this;
		}

		protected virtual bool IsAddToken()
		{
			return true;
		}

		public override string ToJson()
		{
			Serializer serializer = Serializer.Start().AddString("action", this.Action).AddObject<TRequest>("args", this.RequestArgs).Add<uint>("requestId", this.Id).Add<uint>("time", this.Time);
			if (this.IsAddToken())
			{
				serializer.AddString("token", this.Token);
			}
			serializer.End();
			return serializer.ToString();
		}

		protected internal AbstractCommand(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke7(long instance, long* args)
		{
			((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke8(long instance, long* args)
		{
			((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke9(long instance, long* args)
		{
			((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke10(long instance, long* args)
		{
			((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke11(long instance, long* args)
		{
			((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke12(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GeneratePlayerRequest, GeneratePlayerResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke13(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke14(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke15(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke16(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke17(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke18(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke19(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke20(long instance, long* args)
		{
			((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke21(long instance, long* args)
		{
			((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke22(long instance, long* args)
		{
			((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke23(long instance, long* args)
		{
			((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke24(long instance, long* args)
		{
			((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke25(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetAuthTokenRequest, GetAuthTokenResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke26(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke27(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke28(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke29(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke30(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke31(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke32(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke33(long instance, long* args)
		{
			((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke34(long instance, long* args)
		{
			((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke35(long instance, long* args)
		{
			((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke36(long instance, long* args)
		{
			((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke37(long instance, long* args)
		{
			((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke38(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, GetEndpointsResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke39(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke40(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke41(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke42(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke43(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke44(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke45(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke46(long instance, long* args)
		{
			((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke47(long instance, long* args)
		{
			((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke48(long instance, long* args)
		{
			((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke49(long instance, long* args)
		{
			((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke50(long instance, long* args)
		{
			((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke51(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DefaultRequest, AuthTokenIsPlayerIdResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke52(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke53(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke54(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke55(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke56(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke57(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke58(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke59(long instance, long* args)
		{
			((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke60(long instance, long* args)
		{
			((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke61(long instance, long* args)
		{
			((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke62(long instance, long* args)
		{
			((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke63(long instance, long* args)
		{
			((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke64(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetIdRequest, ForceObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke65(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke66(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke67(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke68(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke69(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke70(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke71(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke72(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke73(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke74(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke75(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke76(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke77(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke78(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke79(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke80(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke81(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke82(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke83(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke84(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke85(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke86(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke87(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke88(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke89(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke90(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke91(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke92(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke93(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke94(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke95(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke96(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke97(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke98(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke99(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke100(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke101(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke102(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke103(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetCommandCenterEntriesResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke104(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke105(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke106(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke107(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke108(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke109(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke110(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke111(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke112(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke113(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke114(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke115(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke116(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, HolonetGetMessagesResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke117(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke118(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke119(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke120(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke121(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke122(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke123(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke124(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke125(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke126(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke127(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke128(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke129(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetObjectivesResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke130(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke131(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke132(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke133(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke134(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke135(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke136(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke137(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke138(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke139(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke140(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke141(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke142(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetExternalAccountsResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke143(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke144(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke145(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke146(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke147(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke148(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke149(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke150(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke151(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke152(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke153(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke154(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke155(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ExternalCurrencySyncResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke156(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke157(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke158(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke159(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke160(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke161(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke162(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke163(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke164(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke165(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke166(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke167(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke168(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetPlayerPvpStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke169(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke170(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke171(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke172(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke173(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke174(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke175(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke176(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke177(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke178(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke179(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke180(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke181(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSyncContentResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke182(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke183(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke184(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke185(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke186(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke187(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke188(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke189(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke190(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke191(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke192(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke193(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke194(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke195(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke196(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke197(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke198(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke199(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke200(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke201(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke202(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke203(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke204(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke205(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke206(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke207(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, TapjoyPlayerSyncResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke208(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke209(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke210(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke211(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke212(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke213(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke214(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke215(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke216(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke217(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke218(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke219(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke220(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetSquadInvitesResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke221(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke222(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke223(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke224(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke225(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke226(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke227(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke228(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke229(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke230(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke231(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke232(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke233(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke234(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke235(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke236(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke237(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke238(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke239(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke240(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke241(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke242(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke243(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke244(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke245(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke246(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke247(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke248(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke249(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke250(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke251(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke252(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke253(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke254(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke255(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke256(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke257(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke258(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke259(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke260(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke261(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke262(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke263(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke264(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke265(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke266(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke267(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke268(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke269(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke270(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke271(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke272(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke273(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke274(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke275(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke276(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke277(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke278(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke279(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke280(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke281(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke282(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke283(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke284(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke285(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, GetTargetedOffersResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke286(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke287(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke288(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke289(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke290(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke291(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke292(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke293(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke294(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke295(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke296(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke297(long instance, long* args)
		{
			((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke298(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdRequest, ConflictRanks>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke299(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke300(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke301(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke302(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke303(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke304(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke305(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke306(long instance, long* args)
		{
			((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke307(long instance, long* args)
		{
			((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke308(long instance, long* args)
		{
			((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke309(long instance, long* args)
		{
			((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke310(long instance, long* args)
		{
			((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke311(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpGetNextTargetRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke312(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke313(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke314(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke315(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke316(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke317(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke318(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke319(long instance, long* args)
		{
			((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke320(long instance, long* args)
		{
			((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke321(long instance, long* args)
		{
			((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke322(long instance, long* args)
		{
			((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke323(long instance, long* args)
		{
			((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke324(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke325(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke326(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke327(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke328(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke329(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke330(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke331(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke332(long instance, long* args)
		{
			((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke333(long instance, long* args)
		{
			((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke334(long instance, long* args)
		{
			((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke335(long instance, long* args)
		{
			((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke336(long instance, long* args)
		{
			((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke337(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BattleEndRequest, PvpBattleEndResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke338(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke339(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke340(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke341(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke342(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke343(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke344(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke345(long instance, long* args)
		{
			((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke346(long instance, long* args)
		{
			((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke347(long instance, long* args)
		{
			((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke348(long instance, long* args)
		{
			((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke349(long instance, long* args)
		{
			((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke350(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke351(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke352(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke353(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke354(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke355(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke356(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke357(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke358(long instance, long* args)
		{
			((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke359(long instance, long* args)
		{
			((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke360(long instance, long* args)
		{
			((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke361(long instance, long* args)
		{
			((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke362(long instance, long* args)
		{
			((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke363(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke364(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke365(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke366(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke367(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke368(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke369(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke370(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke371(long instance, long* args)
		{
			((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke372(long instance, long* args)
		{
			((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke373(long instance, long* args)
		{
			((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke374(long instance, long* args)
		{
			((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke375(long instance, long* args)
		{
			((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke376(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CampaignStoreBuyRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke377(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke378(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke379(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke380(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke381(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke382(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke383(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke384(long instance, long* args)
		{
			((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke385(long instance, long* args)
		{
			((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke386(long instance, long* args)
		{
			((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke387(long instance, long* args)
		{
			((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke388(long instance, long* args)
		{
			((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke389(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ClaimCampaignRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke390(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke391(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke392(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke393(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke394(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke395(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke396(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke397(long instance, long* args)
		{
			((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke398(long instance, long* args)
		{
			((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke399(long instance, long* args)
		{
			((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke400(long instance, long* args)
		{
			((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke401(long instance, long* args)
		{
			((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke402(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployableUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke403(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke404(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke405(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke406(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke407(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke408(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke409(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke410(long instance, long* args)
		{
			((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke411(long instance, long* args)
		{
			((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke412(long instance, long* args)
		{
			((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke413(long instance, long* args)
		{
			((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke414(long instance, long* args)
		{
			((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke415(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatDeployablesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke416(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke417(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke418(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke419(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke420(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke421(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke422(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke423(long instance, long* args)
		{
			((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke424(long instance, long* args)
		{
			((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke425(long instance, long* args)
		{
			((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke426(long instance, long* args)
		{
			((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke427(long instance, long* args)
		{
			((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke428(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatEarnEquipmentShardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke429(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke430(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke431(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke432(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke433(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke434(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke435(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke436(long instance, long* args)
		{
			((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke437(long instance, long* args)
		{
			((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke438(long instance, long* args)
		{
			((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke439(long instance, long* args)
		{
			((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke440(long instance, long* args)
		{
			((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke441(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatFastForwardContractsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke442(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke443(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke444(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke445(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke446(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke447(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke448(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke449(long instance, long* args)
		{
			((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke450(long instance, long* args)
		{
			((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke451(long instance, long* args)
		{
			((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke452(long instance, long* args)
		{
			((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke453(long instance, long* args)
		{
			((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke454(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatGetBattleRecordRequest, CheatGetBattleRecordResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke455(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke456(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke457(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke458(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke459(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke460(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke461(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke462(long instance, long* args)
		{
			((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke463(long instance, long* args)
		{
			((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke464(long instance, long* args)
		{
			((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke465(long instance, long* args)
		{
			((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke466(long instance, long* args)
		{
			((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke467(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatPointsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke468(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke469(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke470(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke471(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke472(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke473(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke474(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke475(long instance, long* args)
		{
			((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke476(long instance, long* args)
		{
			((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke477(long instance, long* args)
		{
			((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke478(long instance, long* args)
		{
			((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke479(long instance, long* args)
		{
			((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke480(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatResetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke481(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke482(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke483(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke484(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke485(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke486(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke487(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke488(long instance, long* args)
		{
			((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke489(long instance, long* args)
		{
			((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke490(long instance, long* args)
		{
			((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke491(long instance, long* args)
		{
			((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke492(long instance, long* args)
		{
			((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke493(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSaveBattleRecordRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke494(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke495(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke496(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke497(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke498(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke499(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke500(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke501(long instance, long* args)
		{
			((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke502(long instance, long* args)
		{
			((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke503(long instance, long* args)
		{
			((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke504(long instance, long* args)
		{
			((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke505(long instance, long* args)
		{
			((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke506(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatScheduleDailyCrateRequest, CheatScheduleDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke507(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke508(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke509(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke510(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke511(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke512(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke513(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke514(long instance, long* args)
		{
			((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke515(long instance, long* args)
		{
			((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke516(long instance, long* args)
		{
			((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke517(long instance, long* args)
		{
			((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke518(long instance, long* args)
		{
			((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke519(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetBattleStatsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke520(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke521(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke522(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke523(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke524(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke525(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke526(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke527(long instance, long* args)
		{
			((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke528(long instance, long* args)
		{
			((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke529(long instance, long* args)
		{
			((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke530(long instance, long* args)
		{
			((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke531(long instance, long* args)
		{
			((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke532(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetEquipmentRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke533(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke534(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke535(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke536(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke537(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke538(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke539(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke540(long instance, long* args)
		{
			((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke541(long instance, long* args)
		{
			((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke542(long instance, long* args)
		{
			((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke543(long instance, long* args)
		{
			((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke544(long instance, long* args)
		{
			((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke545(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesProgressRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke546(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke547(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke548(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke549(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke550(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke551(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke552(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke553(long instance, long* args)
		{
			((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke554(long instance, long* args)
		{
			((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke555(long instance, long* args)
		{
			((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke556(long instance, long* args)
		{
			((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke557(long instance, long* args)
		{
			((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke558(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetObjectivesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke559(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke560(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke561(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke562(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke563(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke564(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke565(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke566(long instance, long* args)
		{
			((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke567(long instance, long* args)
		{
			((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke568(long instance, long* args)
		{
			((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke569(long instance, long* args)
		{
			((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke570(long instance, long* args)
		{
			((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke571(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetPerkActivationStateRequest, CheatSetPerkActivationStateResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke572(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke573(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke574(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke575(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke576(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke577(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke578(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke579(long instance, long* args)
		{
			((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke580(long instance, long* args)
		{
			((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke581(long instance, long* args)
		{
			((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke582(long instance, long* args)
		{
			((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke583(long instance, long* args)
		{
			((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke584(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetResourcesRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke585(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke586(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke587(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke588(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke589(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke590(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke591(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke592(long instance, long* args)
		{
			((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke593(long instance, long* args)
		{
			((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke594(long instance, long* args)
		{
			((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke595(long instance, long* args)
		{
			((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke596(long instance, long* args)
		{
			((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke597(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetSquadLevelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke598(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke599(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke600(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke601(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke602(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke603(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke604(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke605(long instance, long* args)
		{
			((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke606(long instance, long* args)
		{
			((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke607(long instance, long* args)
		{
			((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke608(long instance, long* args)
		{
			((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke609(long instance, long* args)
		{
			((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke610(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSetTroopDonateRepRequest, CheatSetTroopDonateRepResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke611(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke612(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke613(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke614(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke615(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke616(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke617(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke618(long instance, long* args)
		{
			((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke619(long instance, long* args)
		{
			((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke620(long instance, long* args)
		{
			((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke621(long instance, long* args)
		{
			((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke622(long instance, long* args)
		{
			((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke623(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTimeTravelRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke624(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke625(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke626(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke627(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke628(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke629(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke630(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke631(long instance, long* args)
		{
			((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke632(long instance, long* args)
		{
			((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke633(long instance, long* args)
		{
			((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke634(long instance, long* args)
		{
			((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke635(long instance, long* args)
		{
			((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke636(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatSquadWarTurnsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke637(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke638(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke639(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke640(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke641(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke642(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke643(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke644(long instance, long* args)
		{
			((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke645(long instance, long* args)
		{
			((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke646(long instance, long* args)
		{
			((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke647(long instance, long* args)
		{
			((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke648(long instance, long* args)
		{
			((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke649(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatStartWarRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke650(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke651(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke652(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke653(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke654(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke655(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke656(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke657(long instance, long* args)
		{
			((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke658(long instance, long* args)
		{
			((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke659(long instance, long* args)
		{
			((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke660(long instance, long* args)
		{
			((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke661(long instance, long* args)
		{
			((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke662(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatUpgradeBuildingsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke663(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke664(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke665(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke666(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke667(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke668(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke669(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke670(long instance, long* args)
		{
			((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke671(long instance, long* args)
		{
			((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke672(long instance, long* args)
		{
			((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke673(long instance, long* args)
		{
			((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke674(long instance, long* args)
		{
			((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke675(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PromoCodeTestRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke676(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke677(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke678(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke679(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke680(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke681(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke682(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke683(long instance, long* args)
		{
			((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke684(long instance, long* args)
		{
			((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke685(long instance, long* args)
		{
			((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke686(long instance, long* args)
		{
			((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke687(long instance, long* args)
		{
			((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke688(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SimulateWarMatchMakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke689(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke690(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke691(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke692(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke693(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke694(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke695(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke696(long instance, long* args)
		{
			((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke697(long instance, long* args)
		{
			((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke698(long instance, long* args)
		{
			((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke699(long instance, long* args)
		{
			((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke700(long instance, long* args)
		{
			((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke701(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<AwardCrateSuppliesRequest, AwardCrateSuppliesResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke702(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke703(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke704(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke705(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke706(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke707(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke708(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke709(long instance, long* args)
		{
			((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke710(long instance, long* args)
		{
			((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke711(long instance, long* args)
		{
			((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke712(long instance, long* args)
		{
			((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke713(long instance, long* args)
		{
			((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke714(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyCrateRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke715(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke716(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke717(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke718(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke719(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke720(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke721(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke722(long instance, long* args)
		{
			((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke723(long instance, long* args)
		{
			((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke724(long instance, long* args)
		{
			((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke725(long instance, long* args)
		{
			((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke726(long instance, long* args)
		{
			((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke727(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyLimitedEditionItemRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke728(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke729(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke730(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke731(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke732(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke733(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke734(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke735(long instance, long* args)
		{
			((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke736(long instance, long* args)
		{
			((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke737(long instance, long* args)
		{
			((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke738(long instance, long* args)
		{
			((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke739(long instance, long* args)
		{
			((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke740(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheatAddCrateRequest, CheatAddCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke741(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke742(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke743(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke744(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke745(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke746(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke747(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke748(long instance, long* args)
		{
			((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke749(long instance, long* args)
		{
			((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke750(long instance, long* args)
		{
			((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke751(long instance, long* args)
		{
			((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke752(long instance, long* args)
		{
			((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke753(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CheckDailyCrateRequest, CheckDailyCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke754(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke755(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke756(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke757(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke758(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke759(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke760(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke761(long instance, long* args)
		{
			((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke762(long instance, long* args)
		{
			((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke763(long instance, long* args)
		{
			((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke764(long instance, long* args)
		{
			((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke765(long instance, long* args)
		{
			((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke766(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<OpenCrateRequest, OpenCrateResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke767(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke768(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke769(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke770(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke771(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke772(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke773(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke774(long instance, long* args)
		{
			((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke775(long instance, long* args)
		{
			((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke776(long instance, long* args)
		{
			((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke777(long instance, long* args)
		{
			((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke778(long instance, long* args)
		{
			((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke779(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke780(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke781(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke782(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke783(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke784(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke785(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke786(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke787(long instance, long* args)
		{
			((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke788(long instance, long* args)
		{
			((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke789(long instance, long* args)
		{
			((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke790(long instance, long* args)
		{
			((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke791(long instance, long* args)
		{
			((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke792(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EquipmentUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke793(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke794(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke795(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke796(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke797(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke798(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke799(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke800(long instance, long* args)
		{
			((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke801(long instance, long* args)
		{
			((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke802(long instance, long* args)
		{
			((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke803(long instance, long* args)
		{
			((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke804(long instance, long* args)
		{
			((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke805(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetReplayRequest, GetReplayResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke806(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke807(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke808(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke809(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke810(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke811(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke812(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke813(long instance, long* args)
		{
			((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke814(long instance, long* args)
		{
			((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke815(long instance, long* args)
		{
			((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke816(long instance, long* args)
		{
			((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke817(long instance, long* args)
		{
			((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke818(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<HolonetClaimRewardRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke819(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke820(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke821(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke822(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke823(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke824(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke825(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke826(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke827(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke828(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke829(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke830(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke831(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke832(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke833(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke834(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke835(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke836(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke837(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke838(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke839(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke840(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke841(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke842(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke843(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke844(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke845(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke846(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke847(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke848(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke849(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke850(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke851(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke852(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke853(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke854(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke855(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke856(long instance, long* args)
		{
			((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke857(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MissionIdRequest, GetMissionMapResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke858(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke859(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke860(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke861(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke862(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke863(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke864(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke865(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke866(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke867(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke868(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke869(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke870(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkActivateRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke871(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke872(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke873(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke874(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke875(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke876(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke877(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke878(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke879(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke880(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke881(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke882(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke883(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkCancelRequest, PlayerPerksDataResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke884(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke885(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke886(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke887(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke888(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke889(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke890(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke891(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke892(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke893(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke894(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke895(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke896(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkInvestRequest, PlayerPerkInvestResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke897(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke898(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke899(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke900(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke901(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke902(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke903(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke904(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke905(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke906(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke907(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke908(long instance, long* args)
		{
			((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke909(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerPerkSkipCooldownRequest, PlayerPerkSkipCooldownResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke910(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke911(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke912(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke913(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke914(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke915(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke916(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke917(long instance, long* args)
		{
			((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke918(long instance, long* args)
		{
			((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke919(long instance, long* args)
		{
			((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke920(long instance, long* args)
		{
			((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke921(long instance, long* args)
		{
			((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke922(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlanetStatsRequest, PlanetStatsResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke923(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke924(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke925(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke926(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke927(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke928(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke929(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke930(long instance, long* args)
		{
			((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke931(long instance, long* args)
		{
			((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke932(long instance, long* args)
		{
			((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke933(long instance, long* args)
		{
			((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke934(long instance, long* args)
		{
			((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke935(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RelocatePlanetRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke936(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke937(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke938(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke939(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke940(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke941(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke942(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke943(long instance, long* args)
		{
			((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke944(long instance, long* args)
		{
			((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke945(long instance, long* args)
		{
			((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke946(long instance, long* args)
		{
			((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke947(long instance, long* args)
		{
			((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke948(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterExternalAccountRequest, RegisterExternalAccountResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke949(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke950(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke951(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke952(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke953(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke954(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke955(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke956(long instance, long* args)
		{
			((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke957(long instance, long* args)
		{
			((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke958(long instance, long* args)
		{
			((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke959(long instance, long* args)
		{
			((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke960(long instance, long* args)
		{
			((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke961(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<UnregisterExternalAccountRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke962(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke963(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke964(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke965(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke966(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke967(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke968(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke969(long instance, long* args)
		{
			((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke970(long instance, long* args)
		{
			((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke971(long instance, long* args)
		{
			((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke972(long instance, long* args)
		{
			((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke973(long instance, long* args)
		{
			((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke974(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingClearRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke975(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke976(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke977(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke978(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke979(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke980(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke981(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke982(long instance, long* args)
		{
			((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke983(long instance, long* args)
		{
			((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke984(long instance, long* args)
		{
			((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke985(long instance, long* args)
		{
			((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke986(long instance, long* args)
		{
			((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke987(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingCollectRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke988(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke989(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke990(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke991(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke992(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke993(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke994(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke995(long instance, long* args)
		{
			((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke996(long instance, long* args)
		{
			((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke997(long instance, long* args)
		{
			((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke998(long instance, long* args)
		{
			((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke999(long instance, long* args)
		{
			((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1000(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingConstructRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1001(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1002(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1003(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1004(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1005(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1006(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1007(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1008(long instance, long* args)
		{
			((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1009(long instance, long* args)
		{
			((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1010(long instance, long* args)
		{
			((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1011(long instance, long* args)
		{
			((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1012(long instance, long* args)
		{
			((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1013(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1014(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1015(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1016(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1017(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1018(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1019(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1020(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1021(long instance, long* args)
		{
			((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1022(long instance, long* args)
		{
			((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1023(long instance, long* args)
		{
			((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1024(long instance, long* args)
		{
			((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1025(long instance, long* args)
		{
			((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1026(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingInstantUpgradeRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1027(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1028(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1029(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1030(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1031(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1032(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1033(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1034(long instance, long* args)
		{
			((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1035(long instance, long* args)
		{
			((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1036(long instance, long* args)
		{
			((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1037(long instance, long* args)
		{
			((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1038(long instance, long* args)
		{
			((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1039(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingUpgradeAllWallsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1040(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1041(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1042(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1043(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1044(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1045(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1046(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1047(long instance, long* args)
		{
			((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1048(long instance, long* args)
		{
			((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1049(long instance, long* args)
		{
			((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1050(long instance, long* args)
		{
			((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1051(long instance, long* args)
		{
			((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1052(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1053(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1054(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1055(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1056(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1057(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1058(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1059(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1060(long instance, long* args)
		{
			((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1061(long instance, long* args)
		{
			((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1062(long instance, long* args)
		{
			((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1063(long instance, long* args)
		{
			((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1064(long instance, long* args)
		{
			((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1065(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingMultiMoveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1066(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1067(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1068(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1069(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1070(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1071(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1072(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1073(long instance, long* args)
		{
			((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1074(long instance, long* args)
		{
			((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1075(long instance, long* args)
		{
			((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1076(long instance, long* args)
		{
			((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1077(long instance, long* args)
		{
			((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1078(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<WarBaseSaveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1079(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1080(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1081(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1082(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1083(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1084(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1085(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1086(long instance, long* args)
		{
			((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1087(long instance, long* args)
		{
			((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1088(long instance, long* args)
		{
			((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1089(long instance, long* args)
		{
			((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1090(long instance, long* args)
		{
			((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1091(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RearmTrapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1092(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1093(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1094(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1095(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1096(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1097(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1098(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1099(long instance, long* args)
		{
			((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1100(long instance, long* args)
		{
			((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1101(long instance, long* args)
		{
			((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1102(long instance, long* args)
		{
			((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1103(long instance, long* args)
		{
			((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1104(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuildingSwapRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1105(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1106(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1107(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1108(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1109(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1110(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1111(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1112(long instance, long* args)
		{
			((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1113(long instance, long* args)
		{
			((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1114(long instance, long* args)
		{
			((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1115(long instance, long* args)
		{
			((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1116(long instance, long* args)
		{
			((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1117(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableContractRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1118(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1119(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1120(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1121(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1122(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1123(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1124(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1125(long instance, long* args)
		{
			((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1126(long instance, long* args)
		{
			((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1127(long instance, long* args)
		{
			((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1128(long instance, long* args)
		{
			((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1129(long instance, long* args)
		{
			((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1130(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableSpendRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1131(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1132(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1133(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1134(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1135(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1136(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1137(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1138(long instance, long* args)
		{
			((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1139(long instance, long* args)
		{
			((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1140(long instance, long* args)
		{
			((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1141(long instance, long* args)
		{
			((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1142(long instance, long* args)
		{
			((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1143(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeployableUpgradeStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1144(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1145(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1146(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1147(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1148(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1149(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1150(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1151(long instance, long* args)
		{
			((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1152(long instance, long* args)
		{
			((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1153(long instance, long* args)
		{
			((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1154(long instance, long* args)
		{
			((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1155(long instance, long* args)
		{
			((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1156(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<DeregisterDeviceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1157(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1158(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1159(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1160(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1161(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1162(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1163(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1164(long instance, long* args)
		{
			((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1165(long instance, long* args)
		{
			((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1166(long instance, long* args)
		{
			((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1167(long instance, long* args)
		{
			((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1168(long instance, long* args)
		{
			((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1169(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FueUpdateStateRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1170(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1171(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1172(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1173(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1174(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1175(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1176(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1177(long instance, long* args)
		{
			((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1178(long instance, long* args)
		{
			((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1179(long instance, long* args)
		{
			((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1180(long instance, long* args)
		{
			((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1181(long instance, long* args)
		{
			((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1182(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetContentRequest, GetContentResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1183(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1184(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1185(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1186(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1187(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1188(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1189(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1190(long instance, long* args)
		{
			((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1191(long instance, long* args)
		{
			((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1192(long instance, long* args)
		{
			((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1193(long instance, long* args)
		{
			((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1194(long instance, long* args)
		{
			((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1195(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentityGetResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1196(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1197(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1198(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1199(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1200(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1201(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1202(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1203(long instance, long* args)
		{
			((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1204(long instance, long* args)
		{
			((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1205(long instance, long* args)
		{
			((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1206(long instance, long* args)
		{
			((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1207(long instance, long* args)
		{
			((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1208(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdentityRequest, PlayerIdentitySwitchResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1209(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1210(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1211(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1212(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1213(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1214(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1215(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1216(long instance, long* args)
		{
			((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1217(long instance, long* args)
		{
			((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1218(long instance, long* args)
		{
			((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1219(long instance, long* args)
		{
			((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1220(long instance, long* args)
		{
			((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1221(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<KeepAliveRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1222(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1223(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1224(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1225(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1226(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1227(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1228(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1229(long instance, long* args)
		{
			((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1230(long instance, long* args)
		{
			((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1231(long instance, long* args)
		{
			((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1232(long instance, long* args)
		{
			((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1233(long instance, long* args)
		{
			((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1234(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<LoginRequest, LoginResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1235(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1236(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1237(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1238(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1239(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1240(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1241(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1242(long instance, long* args)
		{
			((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1243(long instance, long* args)
		{
			((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1244(long instance, long* args)
		{
			((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1245(long instance, long* args)
		{
			((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1246(long instance, long* args)
		{
			((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1247(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1248(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1249(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1250(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1251(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1252(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1253(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1254(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1255(long instance, long* args)
		{
			((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1256(long instance, long* args)
		{
			((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1257(long instance, long* args)
		{
			((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1258(long instance, long* args)
		{
			((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1259(long instance, long* args)
		{
			((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1260(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerErrorRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1261(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1262(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1263(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1264(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1265(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1266(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1267(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1268(long instance, long* args)
		{
			((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1269(long instance, long* args)
		{
			((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1270(long instance, long* args)
		{
			((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1271(long instance, long* args)
		{
			((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1272(long instance, long* args)
		{
			((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1273(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseCompleteRequest, RaidDefenseCompleteResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1274(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1275(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1276(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1277(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1278(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1279(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1280(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1281(long instance, long* args)
		{
			((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1282(long instance, long* args)
		{
			((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1283(long instance, long* args)
		{
			((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1284(long instance, long* args)
		{
			((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1285(long instance, long* args)
		{
			((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1286(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidDefenseStartRequest, RaidDefenseStartResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1287(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1288(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1289(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1290(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1291(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1292(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1293(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1294(long instance, long* args)
		{
			((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1295(long instance, long* args)
		{
			((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1296(long instance, long* args)
		{
			((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1297(long instance, long* args)
		{
			((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1298(long instance, long* args)
		{
			((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1299(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RaidUpdateRequest, RaidUpdateResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1300(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1301(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1302(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1303(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1304(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1305(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1306(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1307(long instance, long* args)
		{
			((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1308(long instance, long* args)
		{
			((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1309(long instance, long* args)
		{
			((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1310(long instance, long* args)
		{
			((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1311(long instance, long* args)
		{
			((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1312(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<RegisterDeviceRequest, RegisterDeviceResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1313(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1314(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1315(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1316(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1317(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1318(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1319(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1320(long instance, long* args)
		{
			((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1321(long instance, long* args)
		{
			((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1322(long instance, long* args)
		{
			((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1323(long instance, long* args)
		{
			((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1324(long instance, long* args)
		{
			((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1325(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetFactionRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1326(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1327(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1328(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1329(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1330(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1331(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1332(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1333(long instance, long* args)
		{
			((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1334(long instance, long* args)
		{
			((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1335(long instance, long* args)
		{
			((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1336(long instance, long* args)
		{
			((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1337(long instance, long* args)
		{
			((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1338(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPlayerNameRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1339(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1340(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1341(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1342(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1343(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1344(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1345(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1346(long instance, long* args)
		{
			((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1347(long instance, long* args)
		{
			((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1348(long instance, long* args)
		{
			((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1349(long instance, long* args)
		{
			((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1350(long instance, long* args)
		{
			((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1351(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SetPrefsRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1352(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1353(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1354(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1355(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1356(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1357(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1358(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1359(long instance, long* args)
		{
			((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1360(long instance, long* args)
		{
			((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1361(long instance, long* args)
		{
			((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1362(long instance, long* args)
		{
			((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1363(long instance, long* args)
		{
			((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1364(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarClaimRewardRequest, CrateDataResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1365(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1366(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1367(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1368(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1369(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1370(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1371(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1372(long instance, long* args)
		{
			((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1373(long instance, long* args)
		{
			((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1374(long instance, long* args)
		{
			((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1375(long instance, long* args)
		{
			((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1376(long instance, long* args)
		{
			((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1377(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyMultiResourceRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1378(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1379(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1380(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1381(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1382(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1383(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1384(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1385(long instance, long* args)
		{
			((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1386(long instance, long* args)
		{
			((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1387(long instance, long* args)
		{
			((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1388(long instance, long* args)
		{
			((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1389(long instance, long* args)
		{
			((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1390(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<VisitNeighborRequest, VisitNeighborResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1391(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1392(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1393(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1394(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1395(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1396(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1397(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1398(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1399(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1400(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1401(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1402(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1403(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1404(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1405(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1406(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1407(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1408(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1409(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1410(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1411(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1412(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1413(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1414(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1415(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1416(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1417(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1418(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1419(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1420(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1421(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1422(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1423(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1424(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1425(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1426(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1427(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1428(long instance, long* args)
		{
			((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1429(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerIdChecksumRequest, TournamentResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1430(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1431(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1432(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1433(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1434(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1435(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1436(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1437(long instance, long* args)
		{
			((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1438(long instance, long* args)
		{
			((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1439(long instance, long* args)
		{
			((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1440(long instance, long* args)
		{
			((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1441(long instance, long* args)
		{
			((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1442(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpBattleStartRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1443(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1444(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1445(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1446(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1447(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1448(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1449(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1450(long instance, long* args)
		{
			((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1451(long instance, long* args)
		{
			((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1452(long instance, long* args)
		{
			((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1453(long instance, long* args)
		{
			((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1454(long instance, long* args)
		{
			((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1455(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PvpRevengeRequest, PvpTarget>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1456(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1457(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1458(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1459(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1460(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1461(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1462(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1463(long instance, long* args)
		{
			((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1464(long instance, long* args)
		{
			((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1465(long instance, long* args)
		{
			((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1466(long instance, long* args)
		{
			((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1467(long instance, long* args)
		{
			((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1468(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SharedPrefRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1469(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1470(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1471(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1472(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1473(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1474(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1475(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1476(long instance, long* args)
		{
			((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1477(long instance, long* args)
		{
			((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1478(long instance, long* args)
		{
			((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1479(long instance, long* args)
		{
			((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1480(long instance, long* args)
		{
			((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1481(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ApplyToSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1482(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1483(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1484(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1485(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1486(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1487(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1488(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1489(long instance, long* args)
		{
			((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1490(long instance, long* args)
		{
			((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1491(long instance, long* args)
		{
			((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1492(long instance, long* args)
		{
			((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1493(long instance, long* args)
		{
			((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1494(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<CreateSquadRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1495(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1496(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1497(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1498(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1499(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1500(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1501(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1502(long instance, long* args)
		{
			((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1503(long instance, long* args)
		{
			((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1504(long instance, long* args)
		{
			((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1505(long instance, long* args)
		{
			((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1506(long instance, long* args)
		{
			((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1507(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<EditSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1508(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1509(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1510(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1511(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1512(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1513(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1514(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1515(long instance, long* args)
		{
			((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1516(long instance, long* args)
		{
			((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1517(long instance, long* args)
		{
			((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1518(long instance, long* args)
		{
			((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1519(long instance, long* args)
		{
			((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1520(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<FriendLBIDRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1521(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1522(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1523(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1524(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1525(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1526(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1527(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1528(long instance, long* args)
		{
			((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1529(long instance, long* args)
		{
			((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1530(long instance, long* args)
		{
			((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1531(long instance, long* args)
		{
			((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1532(long instance, long* args)
		{
			((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1533(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadInvitesSentRequest, GetSquadInvitesSentResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1534(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1535(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1536(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1537(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1538(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1539(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1540(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1541(long instance, long* args)
		{
			((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1542(long instance, long* args)
		{
			((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1543(long instance, long* args)
		{
			((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1544(long instance, long* args)
		{
			((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1545(long instance, long* args)
		{
			((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1546(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadNotifsRequest, SquadNotifsResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1547(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1548(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1549(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1550(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1551(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1552(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1553(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1554(long instance, long* args)
		{
			((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1555(long instance, long* args)
		{
			((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1556(long instance, long* args)
		{
			((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1557(long instance, long* args)
		{
			((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1558(long instance, long* args)
		{
			((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1559(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<GetSquadWarStatusRequest, GetSquadWarStatusResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1560(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1561(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1562(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1563(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1564(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1565(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1566(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1567(long instance, long* args)
		{
			((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1568(long instance, long* args)
		{
			((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1569(long instance, long* args)
		{
			((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1570(long instance, long* args)
		{
			((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1571(long instance, long* args)
		{
			((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1572(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1573(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1574(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1575(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1576(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1577(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1578(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1579(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1580(long instance, long* args)
		{
			((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1581(long instance, long* args)
		{
			((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1582(long instance, long* args)
		{
			((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1583(long instance, long* args)
		{
			((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1584(long instance, long* args)
		{
			((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1585(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<MemberIdRequest, SquadMemberResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1586(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1587(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1588(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1589(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1590(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1591(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1592(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1593(long instance, long* args)
		{
			((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1594(long instance, long* args)
		{
			((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1595(long instance, long* args)
		{
			((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1596(long instance, long* args)
		{
			((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1597(long instance, long* args)
		{
			((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1598(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<PlayerLeaderboardRequest, LeaderboardResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1599(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1600(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1601(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1602(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1603(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1604(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1605(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1606(long instance, long* args)
		{
			((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1607(long instance, long* args)
		{
			((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1608(long instance, long* args)
		{
			((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1609(long instance, long* args)
		{
			((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1610(long instance, long* args)
		{
			((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1611(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SearchSquadsByNameRequest, FeaturedSquadsResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1612(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1613(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1614(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1615(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1616(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1617(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1618(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1619(long instance, long* args)
		{
			((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1620(long instance, long* args)
		{
			((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1621(long instance, long* args)
		{
			((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1622(long instance, long* args)
		{
			((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1623(long instance, long* args)
		{
			((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1624(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SendSquadInviteRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1625(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1626(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1627(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1628(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1629(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1630(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1631(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1632(long instance, long* args)
		{
			((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1633(long instance, long* args)
		{
			((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1634(long instance, long* args)
		{
			((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1635(long instance, long* args)
		{
			((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1636(long instance, long* args)
		{
			((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1637(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareReplayRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1638(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1639(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1640(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1641(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1642(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1643(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1644(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1645(long instance, long* args)
		{
			((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1646(long instance, long* args)
		{
			((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1647(long instance, long* args)
		{
			((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1648(long instance, long* args)
		{
			((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1649(long instance, long* args)
		{
			((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1650(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ShareVideoRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1651(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1652(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1653(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1654(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1655(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1656(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1657(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1658(long instance, long* args)
		{
			((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1659(long instance, long* args)
		{
			((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1660(long instance, long* args)
		{
			((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1661(long instance, long* args)
		{
			((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1662(long instance, long* args)
		{
			((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1663(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1664(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1665(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1666(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1667(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1668(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1669(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1670(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1671(long instance, long* args)
		{
			((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1672(long instance, long* args)
		{
			((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1673(long instance, long* args)
		{
			((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1674(long instance, long* args)
		{
			((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1675(long instance, long* args)
		{
			((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1676(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadIDRequest, SquadResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1677(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1678(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1679(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1680(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1681(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1682(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1683(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1684(long instance, long* args)
		{
			((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1685(long instance, long* args)
		{
			((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1686(long instance, long* args)
		{
			((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1687(long instance, long* args)
		{
			((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1688(long instance, long* args)
		{
			((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1689(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackBuffBaseRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1690(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1691(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1692(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1693(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1694(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1695(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1696(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1697(long instance, long* args)
		{
			((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1698(long instance, long* args)
		{
			((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1699(long instance, long* args)
		{
			((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1700(long instance, long* args)
		{
			((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1701(long instance, long* args)
		{
			((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1702(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarAttackPlayerStartRequest, BattleIdResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1703(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1704(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1705(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1706(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1707(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1708(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1709(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1710(long instance, long* args)
		{
			((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1711(long instance, long* args)
		{
			((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1712(long instance, long* args)
		{
			((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1713(long instance, long* args)
		{
			((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1714(long instance, long* args)
		{
			((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1715(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarGetBuffBaseStatusRequest, SquadWarBuffBaseResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1716(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1717(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1718(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1719(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1720(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1721(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1722(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1723(long instance, long* args)
		{
			((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1724(long instance, long* args)
		{
			((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1725(long instance, long* args)
		{
			((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1726(long instance, long* args)
		{
			((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1727(long instance, long* args)
		{
			((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1728(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarParticipantIdRequest, SquadMemberWarDataResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1729(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1730(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1731(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1732(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1733(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1734(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1735(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1736(long instance, long* args)
		{
			((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1737(long instance, long* args)
		{
			((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1738(long instance, long* args)
		{
			((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1739(long instance, long* args)
		{
			((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1740(long instance, long* args)
		{
			((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1741(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<SquadWarStartMatchmakingRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1742(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1743(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1744(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1745(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1746(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1747(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1748(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1749(long instance, long* args)
		{
			((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1750(long instance, long* args)
		{
			((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1751(long instance, long* args)
		{
			((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1752(long instance, long* args)
		{
			((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1753(long instance, long* args)
		{
			((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1754(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopDonateRequest, TroopDonateResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1755(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1756(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1757(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1758(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1759(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1760(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1761(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1762(long instance, long* args)
		{
			((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1763(long instance, long* args)
		{
			((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1764(long instance, long* args)
		{
			((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1765(long instance, long* args)
		{
			((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1766(long instance, long* args)
		{
			((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1767(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TroopSquadRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1768(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1769(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1770(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1771(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1772(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1773(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1774(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1775(long instance, long* args)
		{
			((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1776(long instance, long* args)
		{
			((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1777(long instance, long* args)
		{
			((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1778(long instance, long* args)
		{
			((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1779(long instance, long* args)
		{
			((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1780(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<BuyTargetedOfferRequest, BuyTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1781(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1782(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1783(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1784(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1785(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1786(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1787(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1788(long instance, long* args)
		{
			((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1789(long instance, long* args)
		{
			((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1790(long instance, long* args)
		{
			((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1791(long instance, long* args)
		{
			((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1792(long instance, long* args)
		{
			((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1793(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<ReserveTargetedOfferIDRequest, DefaultResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1794(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1795(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1796(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1797(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1798(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1799(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1800(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1801(long instance, long* args)
		{
			((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1802(long instance, long* args)
		{
			((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1803(long instance, long* args)
		{
			((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1804(long instance, long* args)
		{
			((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1805(long instance, long* args)
		{
			((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1806(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TargetedOfferIDRequest, TriggerTargetedOfferResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}

		public unsafe static long $Invoke1807(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).Action);
		}

		public unsafe static long $Invoke1808(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).Context);
		}

		public unsafe static long $Invoke1809(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).Description);
		}

		public unsafe static long $Invoke1810(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).Request);
		}

		public unsafe static long $Invoke1811(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).Token);
		}

		public unsafe static long $Invoke1812(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).IsAddToken());
		}

		public unsafe static long $Invoke1813(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke1814(long instance, long* args)
		{
			((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke1815(long instance, long* args)
		{
			((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).RemoveAllCallbacks();
			return -1L;
		}

		public unsafe static long $Invoke1816(long instance, long* args)
		{
			((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).Action = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1817(long instance, long* args)
		{
			((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).Context = GCHandledObjects.GCHandleToObject(*args);
			return -1L;
		}

		public unsafe static long $Invoke1818(long instance, long* args)
		{
			((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).Token = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke1819(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((AbstractCommand<TournamentRankRequest, TournamentRankResponse>)GCHandledObjects.GCHandleToObject(instance)).ToJson());
		}
	}
}
