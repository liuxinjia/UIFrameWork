using System;
using UnityEngine;

namespace Cr7Sund.MyCoroutine
{
    internal interface IAsyncProcessHandleSetter
    {
        void Complete(object result);
        void Error(Exception ex);
    }

    public class AsyncProcessHandle : CustomYieldInstruction, IAsyncProcessHandleSetter
    {
        // keep coroutine suspended return true, vice versa, let coroutine proceed with execution return false
        // keepwaiting is queried each frame after MonoBehaviour.Update 
        public override bool keepWaiting => !IsTerminated;

        public event Action OnTerminate;

        public int Id { get; set; }
        public object Result { get; private set; }
        public bool IsTerminated { get; private set; }
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
    }
}