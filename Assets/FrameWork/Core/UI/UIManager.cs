using System;
using Cr7Sund.AssetLoader;
using Cr7Sund.MyCoroutine;
using Cr7Sund.Runtime.Util;
using Cr7Sund.Settings;
using UnityEngine;

namespace Cr7Sund.UIFrameWork
{

    public class UIManager : MonoBehaviourSingleton<UIManager>, IUIManager
    {
        public UISettings CustomUISettings
        {
            get
            {
                if (_uiSettings == null) _uiSettings = AssetManager.Instance.Load<UISettings>(UISettings.resourcesKey).Result;
                return _uiSettings;
            }

        }

        private UISettings _uiSettings;

        private PageContainer _pageContainers;
        private PageContainer _popupContainers;
        private Canvas _defaultCanvas;

        private Canvas _popupCanvas;

        public Canvas DefaultCanvas
        {
            get
            {
                if (_defaultCanvas == null)
                {
                    var go = GameObject.Find(nameof(DefaultCanvas));
                    _defaultCanvas = go.GetComponent<Canvas>();
                }
                return _defaultCanvas;
            }
        }
        public Canvas PopupCanvas
        {
            get
            {
                if (_popupCanvas == null)
                {
                    var go = GameObject.Find(nameof(PopupCanvas));
                    _popupCanvas = go.GetComponent<Canvas>();
                }
                return _popupCanvas;
            }
        }

        protected override void SingletonAwakened()
        {
            _pageContainers = new PageContainer();
            _popupContainers = new PageContainer();
            _pageContainers.RootCanvas = DefaultCanvas;
            _popupContainers.RootCanvas = PopupCanvas;
        }

        #region Popup
        public AsyncProcessHandle ShowPopup(UINode enterPage, bool playAnimation = true, bool loadAsync = true)
        {
            return _popupContainers.Show(enterPage, playAnimation, true, loadAsync);
        }

        public AsyncProcessHandle ClosePopup(bool playAnimation = true)
        {
            return _popupContainers.Back(playAnimation);
        }


        #endregion

        #region Page
        public AsyncProcessHandle Back(bool playAnimation = true)
        {
            return _pageContainers.Back(playAnimation);
        }


        public AsyncProcessHandle Preload(UINode preloadPage)
        {
            return _pageContainers.Preload(preloadPage);
        }

        public void ReleasePreloaded(UINode preloadPage)
        {
            _pageContainers.ReleasePreloaded(preloadPage);
        }



        public AsyncProcessHandle Show(UINode enterPage, bool playAnimation = true, bool keepInStack = true, bool loadAsync = true, bool closeAllPopup = false)
        {
            
            if (closeAllPopup)
            {
                return _popupContainers.CloseAll((handle) =>
                        {
                            if (!handle.HasError)
                            {
                                _pageContainers.Show(enterPage, false, keepInStack, loadAsync);
                            }
                        });
            }
            else
            {
                return _pageContainers.Show(enterPage, playAnimation, keepInStack, loadAsync);
            }
        }

        #endregion
    }

}