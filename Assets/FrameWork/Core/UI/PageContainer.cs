using System;
using System.Collections;
using System.Collections.Generic;
using Cr7Sund.AssetLoader;
using Cr7Sund.MyCoroutine;
using UnityEngine;
using UnityEngine.Assertions;
using Cr7Sund.Settings;

namespace Cr7Sund.UIFrameWork
{
    public class PageContainer
    {
        private readonly Dictionary<string, AssetLoadHandle<GameObject>> _assetLoadHandles
                = new Dictionary<string, AssetLoadHandle<GameObject>>();





        public UINode CurPage { get; private set; }

        public bool IsInTransition { get; private set; }

        public Canvas ParentCanvas;
        private IAssetLoader AssetLoader => AssetManager.Instance;



        private IEnumerator PreloadRoutine(UINode preloadPage)
        {
            if (string.IsNullOrEmpty(preloadPage.resourceKey))
            {
                throw new ArgumentNullException(nameof(preloadPage));
            }

            var preloadPageKey = preloadPage.resourceKey;
            if (_assetLoadHandles.TryGetValue(preloadPageKey, out var loaddHanle))
            {
                throw new Exception($"Alread load {preloadPage.resourceKey} - Status: {loaddHanle.Status}");
            }

            var assetLoadHandle = AssetLoader.LoadAsync<GameObject>(preloadPage.resourceKey);
            _assetLoadHandles.Add(preloadPageKey, assetLoadHandle);

            if (!assetLoadHandle.IsDone)
            {
                yield return new WaitUntil(() => assetLoadHandle.IsDone);
            }
            if (assetLoadHandle.Status == AssetLoadStatus.Failed)
            {
                throw assetLoadHandle.OperationException;
            }
        }


        private IEnumerator PushRoutine(UINode enterPage, bool playAnimation = true, bool keepInStack = true, bool loadAsync = true)
        {
            var exitPage = CurPage;
            var exitPageCtrl = exitPage?.pageController;
            var enterPageKey = enterPage.resourceKey;

            //PLAN replace with Assert
            if (string.IsNullOrEmpty(enterPageKey))
            {
                throw new ArgumentNullException(nameof(enterPage));
            }

            if (exitPage != null && exitPage.Level == enterPage.Level)
            {
                throw new Exception($"Maybe show the current page {exitPage.resourceKey}");
            }

            //Jump back to top layers if open tree layers >=2
            if (enterPage.Level != 0 && exitPage != null)
            {
                while (exitPage != null && exitPage.Level < enterPage.Level)
                {
                    exitPage = exitPage.prevKey;
                    yield return PopRoutine(false);
                }
            }

            if (IsInTransition)
            {
                throw new InvalidOperationException(
                    "Cannot transition because the screen is already in transition.");
            }


            IsInTransition = true;



            // Setup -- PrefabLoad
            /// --------------------- ---------------------
            if (!_assetLoadHandles.TryGetValue(enterPageKey, out var assetLoadHandle))
            {
                // Why we do not cache load result?
                // 1. we destory page only when 1. we open with new page but no stack; 2.pop back will destory exit page 
                // 2. 
                // Why we do need assetLoad cache?
                // 1. When we pop back , page will be destory
                assetLoadHandle = loadAsync
                  ? AssetLoader.LoadAsync<GameObject>(enterPageKey)
                  : AssetLoader.Load<GameObject>(enterPageKey);

                _assetLoadHandles.Add(enterPageKey, assetLoadHandle);
                if (!assetLoadHandle.IsDone)
                {
                    yield return new WaitUntil(() => assetLoadHandle.IsDone);
                }
                if (assetLoadHandle.Status == AssetLoadStatus.Failed)
                {
                    throw assetLoadHandle.OperationException;
                }
            }

            var instance = GameObject.Instantiate(assetLoadHandle.Result);
            instance.transform.SetParent(ParentCanvas.transform);
            var enterPageView = instance.GetComponent<UIPageView>();
            var enterPageCtrl = enterPageView.Bind();
            enterPageCtrl.pageView = enterPageView;
            enterPage.pageController = enterPageCtrl;
            enterPage.prevKey = exitPage;
            enterPage.keepInStack = keepInStack;
            enterPage.Level = (exitPage != null ? (ushort)(exitPage.Level + 1) : (ushort)1);


            // Preprocess -- After load , before Animation
            /// --------------------- ---------------------
            enterPageCtrl.AfterLoad();

            // Play Animations
            /// --------------------- ---------------------
            if (exitPage != null) yield return exitPageCtrl.Exit(true, playAnimation, enterPage);
            yield return enterPageCtrl.Enter(true, playAnimation, exitPage);


            //End Transition
            IsInTransition = false;

            // Unload Unused Page
            if (exitPage != null && !exitPage.keepInStack)// no while, since we check neighbors
            {
                var hashKey = exitPage.resourceKey;
                var handle = _assetLoadHandles[hashKey];

                exitPage.Dispose();
                _assetLoadHandles.Remove(hashKey);
                AssetLoader.Release<GameObject>(handle);

                enterPage.prevKey = exitPage.prevKey;
            }

            CurPage = enterPage;
        }


        private IEnumerator PopRoutine(bool playAnimation = true)
        {
            var exitPage = CurPage;

            //PLAN replace with Assert
            Assert.IsNotNull(exitPage, "Cannot transition because there are no pages loaded on the stack.");

            if (IsInTransition)
            {
                throw new InvalidOperationException(
                    "Cannot pop because the screen is already in transition.");
            }

            IsInTransition = true;

            var enterPage = exitPage.prevKey;
            var enterPageCtrl = enterPage?.pageController;

            var exitPageCtrl = exitPage?.pageController;


            // Play Animations
            /// --------------------- ---------------------
            yield return exitPageCtrl.Exit(true, playAnimation, exitPage);
            if (enterPage != null) yield return enterPageCtrl.Enter(true, playAnimation, exitPage);
            //End Transition
            IsInTransition = false;


            // Unload Unused Page

            var exitPagekey = exitPage.resourceKey;
            var handle = _assetLoadHandles[exitPagekey];

            _assetLoadHandles.Remove(exitPagekey);
            exitPage.Dispose();
            AssetLoader.Release<GameObject>(handle);

            CurPage = enterPage;
        }

        public IEnumerator CloseAllRoutine()
        {
            while (CurPage != null)
            {
                yield return PopRoutine(true);
            }
        }

        #region  Public Methods

        public AsyncProcessHandle Show(UINode enterPage, bool playAnimation = true, bool keepInStack = true, bool loadAsync = true)
        {
            IEnumerator routine = PushRoutine(enterPage, playAnimation, keepInStack, loadAsync);
            return CoroutineManager.Instance.Run(routine);
        }

        public AsyncProcessHandle Back(bool playAnimation = true)
        {
            return CoroutineManager.Instance.Run(PopRoutine(playAnimation));
        }

        public AsyncProcessHandle Close(UINode enterPage)
        {
            throw new NotImplementedException();
        }

        public AsyncProcessHandle Preload(UINode preloadPage)
        {
            return CoroutineManager.Instance.Run(PreloadRoutine(preloadPage));
        }

        public void ReleasePreloaded(UINode preloadPage)
        {
            if (string.IsNullOrEmpty(preloadPage.resourceKey))
            {
                throw new ArgumentNullException(nameof(preloadPage));
            }
            var preloadPageKey = preloadPage.resourceKey;

            if (!_assetLoadHandles.ContainsKey(preloadPageKey))
            {
                throw new InvalidOperationException($"The resource with key \"${preloadPage.resourceKey}\" is not preloaded.");
            }

            var handle = _assetLoadHandles[preloadPageKey];
            _assetLoadHandles.Remove(preloadPageKey);
            AssetLoader.Release<GameObject>(handle);// include handle return to pool
        }



        public AsyncProcessHandle CloseAll(Action<AsyncProcessHandle> onTerminate = null)
        {
            return CoroutineManager.Instance.Run(CloseAllRoutine(), onTerminate);
        }

        #endregion
    }

}