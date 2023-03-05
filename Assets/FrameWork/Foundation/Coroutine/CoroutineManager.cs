using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cr7Sund.Assertions;
using Cr7Sund.Pool;
using Cr7Sund.Runtime.Util;

namespace Cr7Sund.MyCoroutine
{
    public class CoroutineManager : MonoBehaviourSingleton<CoroutineManager>, ICoroutineManager
    {

        private int _currentId;
        private readonly Dictionary<int, Coroutine> _runningCoroutines =
            new Dictionary<int, Coroutine>();
        private readonly IObjectPool<AsyncProcessHandle> _pool =
            new ObjectPool<AsyncProcessHandle>(AsyncProcessHandle.Create);// Since AsyncProcessHandle only appear here
        public bool ThrowException { get; set; } = true;



        #region LifeTime

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

        #endregion

        #region API

        public AsyncProcessHandle Run(IEnumerator routine)
        {
            Assert.IsNotNull<IEnumerator>(routine);

            var id = _currentId++;
            var handle = _pool.Get();
            var handleSetter = (IAsyncProcessHandleSetter)handle;
            handle.Init(id);


            Coroutine coroutine = StartCoroutineInternal(routine, handleSetter, ThrowException);
            _runningCoroutines.Add(id, coroutine);
            return handle;
        }

        public void Stop(AsyncProcessHandle handle)
        {
            Assert.Contains<int, Coroutine>(_runningCoroutines, handle.Id);
            if (_runningCoroutines.TryGetValue(handle.Id, out var coroutine))
            {
                StopCoroutine(coroutine);
                OnTerminate(handle);
            }
        }

        public new void StopAllCoroutines()
        {
            _runningCoroutines.Clear();
            ((MonoBehaviour)this).StopAllCoroutines();
        }



        #endregion

        private UnityEngine.Coroutine StartCoroutineInternal(IEnumerator routine, IAsyncProcessHandleSetter handleSetter, bool throwException = true)
        {
#if UNITY_EDITOR
            return RuntimeCoroutineTracker.InvokeStart(this, ProcessRoutines(routine, handleSetter, throwException));

#else
            return StartCoroutine(ProcessRoutines(routine,handleSetter, throwException));
#endif

        }

        private IEnumerator ProcessRoutines(IEnumerator routine, IAsyncProcessHandleSetter handleSetter, bool throwException = true)
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
                    yield return ex; //equal to yield break, equal to return
                }

                yield return current; // if null equal to waif for seconds (0)
            }

            OnComplete(current, handleSetter);
            OnTerminate(handleSetter);
        }

        #region callbacks

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
                handle.Release();
                _pool.Release(handle);
            }
        }

        #endregion

        private void SendEditorCommand(string cmd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorWindow w = UnityEditor.EditorWindow.GetWindow<UnityEditor.EditorWindow>("CoTrackerWindow");
            if (w == null)
            {
                w = UnityEditor.EditorWindow.CreateWindow<UnityEditor.EditorWindow>("CoTrackerWindow");
            }
            if (w.GetType().Name == "CoTrackerWindow")
            {
                w.SendEvent(UnityEditor.EditorGUIUtility.CommandEvent(cmd));
            }
#endif
        }
    }
}
