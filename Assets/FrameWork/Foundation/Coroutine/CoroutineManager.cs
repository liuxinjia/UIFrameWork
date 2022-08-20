using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Cr7Sund.Pool;
using Cr7Sund.Runtime.Util;

namespace Cr7Sund.MyCoroutine
{
    public class CoroutineManager : MonoBehaviourSingleton<CoroutineManager>
    {

        internal AsyncProcessHandle Run(Coroutine coroutine)
        {
            throw new NotImplementedException();
        }

        private int _currentId;
        private readonly Dictionary<int, Coroutine> _runningCoroutines =
            new Dictionary<int, Coroutine>();
        private readonly IObjectPool<AsyncProcessHandle> _pool = new ObjectPool<AsyncProcessHandle>(() => new AsyncProcessHandle());

        public bool ThrowException { get; set; } = true;


        public AsyncProcessHandle Run(IEnumerator routine)
        {
            Assert.IsNotNull<IEnumerator>(routine);

            var id = _currentId++;
            var handle = _pool.Get();
            var handleSetter = (IAsyncProcessHandleSetter)handle;

            handle.Id = id;

            var coroutine = StartCoroutine(ProcessRoutines(routine, handleSetter, id));
            _runningCoroutines.Add(id, coroutine);
            return handle;
        }


        private IEnumerator ProcessRoutines(IEnumerator routine, IAsyncProcessHandleSetter handleSetter, int id, bool throwException = true)
        {
            object current = null;
            while (true)// Avoid first routine.MoveNext Exception
            {
                Exception ex = null;
                try
                {
                    // 在这里可以：
                    //     1. 标记协程的执行
                    //     2. 记录协程本次执行的时间
                    //Ref: https://blog.uwa4d.com/archives/USparkle_Coroutine.html
                    // PLAN 协程监控 https://github.com/PerfAssist/PA_CoroutineTracker
                    if (!routine.MoveNext())
                    {
                        break;
                    }

                    current = routine.Current;
                }
                catch (Exception e)
                {
                    ex = e;
                    OnError(ex, handleSetter);
                    OnTerminate(id);
                    if (throwException)
                    {
                        
                        throw new System.Exception(ex + ", " + ex.StackTrace);
                    }
                }

                if (ex != null)
                {
                    yield return ex;
                    yield break; // Equal to return;
                }

                yield return current; // if null equal to waif for seconds (0)
            }

            OnComplete(current, handleSetter);
            OnTerminate(id);
        }


        private void OnComplete(object result, IAsyncProcessHandleSetter handleSetter)
        {
            handleSetter.Complete(result);
            _pool.Release(handleSetter as AsyncProcessHandle);
        }

        private void OnError(Exception ex, IAsyncProcessHandleSetter handleSetter)
        {
            handleSetter.Error(ex);
            _pool.Release(handleSetter as AsyncProcessHandle);
        }

        private void OnTerminate(int id)
        {
            _runningCoroutines.Remove(id);
        }


    }
}

