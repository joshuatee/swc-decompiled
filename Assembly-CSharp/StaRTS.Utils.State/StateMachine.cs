using StaRTS.Utils.Core;
using StaRTS.Utils.Diagnostics;
using System;
using System.Collections.Generic;
using WinRTBridge;

namespace StaRTS.Utils.State
{
	public class StateMachine
	{
		private IState curState;

		private Type prevStateType;

		private bool changingState;

		private Dictionary<Type, List<Type>> legalTransitions;

		private List<Type> wildcardStates;

		public IState CurrentState
		{
			get
			{
				return this.curState;
			}
		}

		public Type PreviousStateType
		{
			get
			{
				return this.prevStateType;
			}
		}

		public StateMachine()
		{
			this.curState = null;
			this.prevStateType = null;
			this.changingState = false;
			this.legalTransitions = new Dictionary<Type, List<Type>>();
			this.wildcardStates = new List<Type>();
		}

		public void SetLegalTransition(Type fromType, Type toType)
		{
			List<Type> list;
			if (this.legalTransitions.ContainsKey(fromType))
			{
				list = this.legalTransitions[fromType];
			}
			else
			{
				list = new List<Type>();
				this.legalTransitions.Add(fromType, list);
			}
			if (list.IndexOf(toType) < 0)
			{
				list.Add(toType);
			}
		}

		public void SetLegalTransition<TFrom, TTo>()
		{
			this.SetLegalTransition(typeof(TFrom), typeof(TTo));
		}

		public void SetLegalTransition(Type wildcardType)
		{
			this.wildcardStates.Add(wildcardType);
		}

		public void SetLegalTransition<TWildcard>()
		{
			this.SetLegalTransition(typeof(TWildcard));
		}

		public virtual bool SetState(IState state)
		{
			bool result = true;
			if (!this.IsLegalTransition(state))
			{
				Service.Get<StaRTSLogger>().DebugFormat("StateMachine should not transition from state {0} to {1}", new object[]
				{
					this.curState.GetType().ToString(),
					state.GetType().ToString()
				});
				result = false;
			}
			if (this.changingState)
			{
				Service.Get<StaRTSLogger>().Error("Cannot set state while changing state");
			}
			this.changingState = true;
			try
			{
				if (this.curState != null)
				{
					this.curState.OnExit(state);
				}
				this.prevStateType = ((this.curState == null) ? null : this.curState.GetType());
				this.curState = state;
				this.curState.OnEnter();
			}
			finally
			{
				this.changingState = false;
			}
			return result;
		}

		private bool IsLegalTransition(IState state)
		{
			if (state == null)
			{
				return false;
			}
			if (this.curState == null)
			{
				return true;
			}
			Type type = this.curState.GetType();
			Type type2 = state.GetType();
			return this.wildcardStates.Contains(type) || this.wildcardStates.Contains(type2) || (this.legalTransitions.ContainsKey(type) && this.legalTransitions[type].IndexOf(type2) >= 0);
		}

		protected internal StateMachine(UIntPtr dummy)
		{
		}

		public unsafe static long $Invoke0(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StateMachine)GCHandledObjects.GCHandleToObject(instance)).CurrentState);
		}

		public unsafe static long $Invoke1(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StateMachine)GCHandledObjects.GCHandleToObject(instance)).PreviousStateType);
		}

		public unsafe static long $Invoke2(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StateMachine)GCHandledObjects.GCHandleToObject(instance)).IsLegalTransition((IState)GCHandledObjects.GCHandleToObject(*args)));
		}

		public unsafe static long $Invoke3(long instance, long* args)
		{
			((StateMachine)GCHandledObjects.GCHandleToObject(instance)).SetLegalTransition((Type)GCHandledObjects.GCHandleToObject(*args));
			return -1L;
		}

		public unsafe static long $Invoke4(long instance, long* args)
		{
			((StateMachine)GCHandledObjects.GCHandleToObject(instance)).SetLegalTransition((Type)GCHandledObjects.GCHandleToObject(*args), (Type)GCHandledObjects.GCHandleToObject(args[1]));
			return -1L;
		}

		public unsafe static long $Invoke5(long instance, long* args)
		{
			return GCHandledObjects.ObjectToGCHandle(((StateMachine)GCHandledObjects.GCHandleToObject(instance)).SetState((IState)GCHandledObjects.GCHandleToObject(*args)));
		}
	}
}
