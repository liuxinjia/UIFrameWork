using System;
using System.Collections;
using System.Collections.Generic;
using Cr7Sund.AssetLoader;
using Cr7Sund.MyCoroutine;
using UnityEngine;
using UnityEngine.Assertions;
using Cr7Sund.Runtime.Util;
using Cr7Sund.Settings;

namespace Cr7Sund.UIFrameWork
{

    public class UIManager : Singleton<UIManager>, IUIManager
    {

        #region  Properties
        public UINode CurKey { get; private set; }

        public bool IsInTransition { get; private set; }

        private readonly Dictionary<int, AssetLoadHandle<GameObject>> _assetLoadHandles
                = new Dictionary<int, AssetLoadHandle<GameObject>>();
        private UISettings _uiSettings => AssetLoader.Load<UISettings>(UISettings.path).Result;
        private IAssetLoader AssetLoader => AssetManager.Instance;

        public UISettings CustomUISettings;
        #endregion



        public AsyncProcessHandle Show(UINode enterPage, bool playAnimation = true, bool keepInStack = true, bool loadAsync = true)
        {
            return CoroutineManager.Instance.Run(PushRoutine(enterPage, playAnimation, keepInStack, loadAsync));
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
            int preloadPageKey = Animator.StringToHash(preloadPage.resourceKey);

            if (!_assetLoadHandles.ContainsKey(preloadPageKey))
            {
                throw new InvalidOperationException($"The resource with key \"${preloadPage.resourceKey}\" is not preloaded.");
            }

            var handle = _assetLoadHandles[preloadPageKey];
            _assetLoadHandles.Remove(preloadPageKey);
            AssetLoader.Release<GameObject>(handle);

        }



        private IEnumerator PushRoutine(UINode enterPage, bool playAnimation = true, bool keepInStack = true, bool loadAsync = true)
        {
            var exitPage = CurKey;
            var exitPageCtrl = exitPage?.pageController;
            int exitPageKey = Animator.StringToHash(exitPage.resourceKey);

            //PLAN replace with Assert
            if (string.IsNullOrEmpty(enterPage.resourceKey))
            {
                throw new ArgumentNullException(nameof(enterPage));
            }

            if (exitPage != null && exitPage.Level == enterPage.Level)
            {
                throw new Exception($"May show the current page");
            }

            //Jump back to top layers if open tree layers >=2
            if (enterPage.Level != 0 && exitPage != null)
            {
                while (exitPage != null && exitPage.Level < enterPage.Level)
                {
                    exitPage = exitPage.prevKey;
                    yield return PopRoutine(playAnimation);
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
            int enterPageKey = Animator.StringToHash(enterPage.resourceKey);
            if (_assetLoadHandles.TryGetValue(enterPageKey, out var assetLoadHandle))
            {
                // Why we do not cache load result?
                // 1. we destory page only when 1. we open with new page but no stack; 2.pop back will destory exit page 
                // 2. 
                // Why we do need assetLoad cache?
                // 1. When we pop back , page will be destory
                assetLoadHandle = loadAsync
                  ? AssetLoader.LoadAsync<GameObject>(enterPage.resourceKey)
                  : AssetLoader.Load<GameObject>(enterPage.resourceKey);

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
            var enterPageView = instance.GetComponent<UIPageView>();
            var enterPageCtrl = enterPageView.Bind();
            enterPageCtrl.pageView = enterPageView;
            enterPage.pageController = enterPageCtrl;
            enterPage.prevKey = exitPage;
            enterPage.keepInStack = keepInStack;
            enterPage.Level = (exitPage != null ? (ushort)(exitPage.Level + 1) : (ushort)1);


            // Preprocess -- After load , before Animation
            /// --------------------- ---------------------
            yield return enterPageCtrl.AfterLoadAsync();


            // Play Animations
            /// --------------------- ---------------------
            if (exitPage == null) yield return exitPageCtrl.Exit(true, playAnimation, enterPage);
            yield return enterPageCtrl.Enter(true, playAnimation, exitPage);

            //End Transition
            IsInTransition = false;

            // Unload Unused Page
            if (exitPage != null && !exitPage.keepInStack)// no while, since we check neighbors
            {
                yield return exitPage.pageController.BeforeRelease();
                int hashKey = Animator.StringToHash(exitPage.resourceKey);
                var handle = _assetLoadHandles[hashKey];

                _assetLoadHandles.Remove(hashKey);
                AssetLoader.Release<GameObject>(handle);

                enterPage.prevKey = exitPage.prevKey;
                exitPage.Destroy();
            }

            CurKey = enterPage;
        }

        private IEnumerator PopRoutine(bool playAnimation = true)
        {
            var exitPage = CurKey;

            //PLAN replace with Assert
            Assert.IsNotNull(exitPage, "Cannot transition because there are no pages loaded on the stack.");
            Assert.IsNotNull(exitPage.prevKey, "Cannot pop because it is the only left page.");

            if (IsInTransition)
            {
                throw new InvalidOperationException(
                    "Cannot pop because the screen is already in transition.");
            }

            IsInTransition = true;

            var enterPage = exitPage.prevKey;
            var enterPageCtrl = enterPage.pageController;
            var exitPageCtrl = exitPage?.pageController;


            // Play Animations
            /// --------------------- ---------------------
            yield return exitPageCtrl.Exit(true, playAnimation, exitPage);
            yield return enterPageCtrl.Enter(true, playAnimation, exitPage);

            //End Transition
            IsInTransition = false;


            // Unload Unused Page

            yield return exitPageCtrl.BeforeRelease();
            int exitPagekey = Animator.StringToHash(exitPage.resourceKey);
            var handle = _assetLoadHandles[exitPagekey];

            _assetLoadHandles.Remove(exitPagekey);
            AssetLoader.Release<GameObject>(handle);


            exitPage.Destroy();

            CurKey = enterPage;
        }

        private IEnumerator PreloadRoutine(UINode preloadPage)
        {
            if (string.IsNullOrEmpty(preloadPage.resourceKey))
            {
                throw new ArgumentNullException(nameof(preloadPage));
            }

            int preloadPageKey = Animator.StringToHash(preloadPage.resourceKey);
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





    }

}