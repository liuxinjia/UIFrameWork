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

        private int _currentId;
        private readonly Dictionary<int, Coroutine> _runningCoroutines =
            new Dictionary<int, Coroutine>();
        private readonly IObjectPool<AsyncProcessHandle> _pool =
            new ObjectPool<AsyncProcessHandle>(() => new AsyncProcessHandle());// Since AsyncProcessHandle only appear here
        public bool ThrowException { get; set; } = true;

        void SendEditorCommand(string cmd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorWindow w = UnityEditor.EditorWindow.GetWindow<UnityEditor.EditorWindow>("CoTrackerWindow");
            if (w.GetType().Name == "CoTrackerWindow")
            {
                w.SendEvent(UnityEditor.EditorGUIUtility.CommandEvent(cmd));
            }
#endif
        }

        protected override void SingletonStarted()
        {
            CoroutineRuntimeTrackingConfig.EnableTracking = true;
            StartCoroutine(RuntimeCoroutineStats.Instance.BroadcastCoroutine());

            SendEditorCommand("AppStarted");
        }
        protected override void SingletonDestroyed()
        {
            SendEditorCommand("AppDestroyed");
        }


        public AsyncProcessHandle Run(IEnumerator routine, Action<AsyncProcessHandle> onTerminate = null)
        {
            Assert.IsNotNull<IEnumerator>(routine);

            var id = _currentId++;
            var handle = _pool.Get();
            var handleSetter = (IAsyncProcessHandleSetter)handle;
            handle.Init();
            handle.Id = id;
            if (onTerminate != null)
            {
                handle.OnTerminate += () => onTerminate(handle);
            }

            var coroutine = RuntimeCoroutineTracker.InvokeStart(this,ProcessRoutines(routine, id, handleSetter));
            _runningCoroutines.Add(id, coroutine);
            return handle;
        }
        public void Stop(AsyncProcessHandle handle)
        {
            var coroutine = _runningCoroutines[handle.Id];
            StopCoroutine(coroutine);
            OnTerminate(handle);
        }


        private IEnumerator ProcessRoutines(IEnumerator routine, int id, IAsyncProcessHandleSetter handleSetter, bool throwException = true)
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
                    OnTerminate(handleSetter);
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
            OnTerminate(handleSetter);
        }


        private void OnComplete(object result, IAsyncProcessHandleSetter handleSetter)
        {
            handleSetter.Complete(result);
        }

        private void OnError(Exception ex, IAsyncProcessHandleSetter handleSetter)
        {
            handleSetter.Error(ex);
        }

        private void OnTerminate(IAsyncProcessHandleSetter handleSetter)
        {
            if (handleSetter is AsyncProcessHandle handle)
            {
                _runningCoroutines.Remove(handle.Id);
                handle.Releasae();
                _pool.Release(handle);
            }
        }


    }
}
