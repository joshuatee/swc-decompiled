using StaRTS.Externals.IAP;
using StaRTS.Externals.Manimal.TransferObjects.Request;
using StaRTS.Externals.Manimal.TransferObjects.Response;
using StaRTS.Main.Controllers;
using StaRTS.Utils.Core;
using System;
using System.Runtime.InteropServices;
using WinRTBridge;

namespace StaRTS.Main.Models.Commands.Player
{
	public class MoneyReceiptVerifyCommand : GameCommand<MoneyReceiptVerifyRequest, MoneyReceiptVerifyResponse>
	{
		public const string ACTION = "player.money.receipt.verify";

		private int[] consumePurchaseErrors;

		public string ProductId
		{
			get;
			set;
		}

		public string TransactionId
		{
			get;
			private set;
		}

		public MoneyReceiptVerifyCommand(MoneyReceiptVerifyRequest request)
		{
			this.consumePurchaseErrors = new int[]
			{
				918,
				919,
				920,
				921,
				922,
				923,
				924,
				925,
				926,
				927,
				928,
				929,
				930,
				931,
				932,
				933,
				934,
				948
			};
			base..ctor("player.money.receipt.verify", request, new MoneyReceiptVerifyResponse());
		}

		private bool IsPurchaseErrorConsumable(uint status)
		{
			for (int i = 0; i < this.consumePurchaseErrors.Length; i++)
			{
				if ((long)this.consumePurchaseErrors[i] == (long)((ulong)status))
				{
					return true;
				}
			}
			return false;
		}

		public void SetTransactionId(string transactionId)
		{
			this.TransactionId = transactionId;
			MoneyReceiptVerifyResponse responseResult = base.ResponseResult;
			if (responseResult != null)
			{
				responseResult.TransactionId = transactionId;
			}
		}

		public override OnCompleteAction OnFailure(uint status, object data)
		{
			if (this.IsPurchaseErrorConsumable(status))
			{
				Service.Get<InAppPurchaseController>().Consume(this.ProductId);
			}
			if (Service.IsSet<GameIdleController>())
			{
				Service.Get<GameIdleController>().Enabled = true;
			}
			return base.OnFailure(status, data);
		}

		public override OnCompleteAction OnComplete(Data data, bool success)
		{
			MoneyReceiptVerifyResponse responseResult = base.ResponseResult;
			responseResult.Status = data.Status;
			return base.OnComplete(data, success);
		}

		public override void OnSuccess()
		{
			if (Service.IsSet<GameIdleController>())
			{
				Service.Get<GameIdleController>().Enabled = true;
			}
		}

		protected internal MoneyReceiptVerifyCommand(UIntPtr dummy) : base(dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MoneyReceiptVerifyCommand)GCHandledObjects.GCHandleToObject(instance)).ProductId);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MoneyReceiptVerifyCommand)GCHandledObjects.GCHandleToObject(instance)).TransactionId);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((MoneyReceiptVerifyCommand)GCHandledObjects.GCHandleToObject(instance)).OnComplete((Data)GCHandledObjects.GCHandleToObject(*args), *(sbyte*)(args + 1) != 0));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((MoneyReceiptVerifyCommand)GCHandledObjects.GCHandleToObject(instance)).OnSuccess();
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((MoneyReceiptVerifyCommand)GCHandledObjects.GCHandleToObject(instance)).ProductId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			((MoneyReceiptVerifyCommand)GCHandledObjects.GCHandleToObject(instance)).TransactionId = Marshal.PtrToStringUni(*(IntPtr*)args);
			return -1L;
		}

		public unsafe static long $Invoke6(long instance, long* args)
		{
			((MoneyReceiptVerifyCommand)GCHandledObjects.GCHandleToObject(instance)).SetTransactionId(Marshal.PtrToStringUni(*(IntPtr*)args));
			return -1L;
		}
	}
}
