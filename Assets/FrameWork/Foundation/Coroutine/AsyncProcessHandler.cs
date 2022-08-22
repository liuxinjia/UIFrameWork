using System;
using UnityEngine;

namespace Cr7Sund.MyCoroutine
{
    public interface IAsyncProcessHandleSetter
    {
        void Complete(object result);
        void Error(Exception ex);

    }

    public class AsyncProcessHandle : CustomYieldInstruction, IAsyncProcessHandleSetter, Cr7Sund.Pool.IPoolObject
    {
        // keep coroutine suspended return true, vice versa, let coroutine proceed with execution return false
        // keepwaiting is queried each frame after MonoBehaviour.Update 
        public override bool keepWaiting => !IsTerminated;

        /// <summary>
        /// Pay Attention to the immediate execute action, 
        /// Recommended registering by the costructor 
        /// </summary>
        public event Action OnTerminate;

        public int Id;
        public object Result { get; private set; }
        public bool IsTerminated { get;private set; }
        public Exception Exception { get; private set; }
        public bool HasError => Exception != null;


        public void Complete(object result)
        {
            Result = result;
            IsTerminated = true;
            OnTerminate?.Invoke();
        }

        public void Error(Exception ex)
        {
            Exception = ex;
            IsTerminated = true;
            OnTerminate?.Invoke();
        }

        public void Init()
        {
            IsTerminated = false;
        }

        public void Releasae()
        {
            OnTerminate = null;
            Result = null;
            Exception = null;
            IsTerminated = true; //equal stop coroutine
        }
    }
}