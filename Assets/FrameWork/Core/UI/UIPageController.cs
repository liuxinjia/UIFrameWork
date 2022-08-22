using System;
using System.Collections;
using Cr7Sund.MyCoroutine;
using UnityEngine;
using Cr7Sund.Runtime.Util;

namespace Cr7Sund.UIFrameWork
{
    using Cr7Sund.Pool;
    using Cr7Sund.Transition.UI;

    public abstract class UIPageController : IUIPageController
    {

        #region Properties

        // public DemoUIPageView VM =>(DemoUIPageView)pageView;
        public UIPageView pageView;


        private RectTransform _rectTransform;
        private RectTransform _parentTransform;
        private CanvasGroup _canvasGroup;
        private GameObject _gameObject => pageView.gameObject;

        public bool IsTransitioning { get => TransitionAnimationProgress > 0f; }
        private PageTransitionContainer _animationContainer => pageView.transitionController;

        #region Animation

        [SerializeField]


        /// <summary>
        /// Progress of the transition animation
        /// </summary>
        /// <value></value>
        public float TransitionAnimationProgress { get; private set; }

        /// <summary>
        ///  Event when the transition animation progress changed
        /// </summary>
        public event Action<float> TransitionAnimationProgressChanged;

        private Progress<float> _transitionProgressReporter;
        private Progress<float> TransitionProgressReporter
        {
            get
            {
                if (_transitionProgressReporter == null)
                    _transitionProgressReporter = PoolManager.Instance.Get<Progress<float>>();
                return _transitionProgressReporter;
            }
        }
        private void SetTransitionProgress(float progress)
        {
            TransitionAnimationProgress = progress;
            TransitionAnimationProgressChanged?.Invoke(progress);
        }

        #endregion

        #endregion

        #region LifeCycles
        public AsyncProcessHandle AfterLoadAsync()
        {
            _rectTransform = (RectTransform)pageView.transform;
            _canvasGroup = _gameObject.GetOrAddComponent<CanvasGroup>();
            _parentTransform = (RectTransform)_rectTransform.parent;

            // _rectTransform.FillParent(_parentTransform);

            // Set order of rendering
            var siblingIndex = 0;
            for (var i = 0; i < _parentTransform.childCount; i++)
            {
                var child = _parentTransform.GetChild(i);
                var childPage = child.GetComponent<UIPageView>();
                siblingIndex = i;
                if (pageView.renderingOrder >= childPage.renderingOrder)
                    continue;

                break;
            }
            _rectTransform.SetSiblingIndex(siblingIndex);

            _canvasGroup.alpha = 0.0f;

            return CoroutineManager.Instance.Run(CreateCoroutine(OnShowAsync()));
        }


        public AsyncProcessHandle Exit(bool push, bool playAnimation, UINode enterPage)
        {
            return CoroutineManager.Instance.Run(CreateCoroutine(ExitRoutine(push, playAnimation, enterPage)));
        }

        internal AsyncProcessHandle Enter(bool push, bool playAnimation, UINode exitPage)
        {
            return CoroutineManager.Instance.Run(EnterRoutine(push, playAnimation, exitPage));
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <returns></returns>
        public void Dispose()
        {
            OnDestory();

            if (_transitionProgressReporter != null)
            {
                PoolManager.Instance.Release<Progress<float>>(_transitionProgressReporter);
                _transitionProgressReporter = null;
            }
            pageView = null;
            _rectTransform = null;
            _parentTransform = null;
            _canvasGroup = null;
        }


        private IEnumerator ExitRoutine(bool push, bool playAnimation, UINode enterPage)
        {
            if (playAnimation)
            {
                var anim = _animationContainer.GetAnimation(push, false);
                if (anim == null)
                    anim = UIManager.Instance.CustomUISettings.GetDefaultPageTransitionAnimation(push, false);

                if (anim != null)
                {
                    anim.SetPartner(pageView.transform as RectTransform);
                    anim.Setup(_rectTransform);

                    yield return CoroutineManager.Instance.Run(
                        anim.CreatePlayRoutine(TransitionProgressReporter, false)
                       );
                }

            }

            _canvasGroup.alpha = 0.0f;
            OnHide();
            yield break;
        }


        private IEnumerator EnterRoutine(bool push, bool playAnimation, UINode exitPage)
        {
            _canvasGroup.alpha = 1.0f;
            if (playAnimation)
            {
                var anim = _animationContainer.GetAnimation(push, true);
                if (anim == null)
                    anim = UIManager.Instance.CustomUISettings.GetDefaultPageTransitionAnimation(push, true);

                if (anim != null)
                {
                    anim.SetPartner(pageView.transform as RectTransform);
                    anim.Setup(_rectTransform);

                    yield return CoroutineManager.Instance.Run(
                         anim.CreatePlayRoutine(TransitionProgressReporter, true)
                       );
                }
            }

            _rectTransform.FillParent(_parentTransform);
            OnShow();
            yield break;
        }

        #endregion

        #region OverideMethods
        public virtual IEnumerator OnShowAsync()
        {

            yield break;
        }
        public virtual void OnShow() { }
        public virtual void OnHide() { }

        public virtual void OnDestory() { }



        #endregion

        //1. Support Task
        private IEnumerator CreateCoroutine(IEnumerator target) => target;


    }


}