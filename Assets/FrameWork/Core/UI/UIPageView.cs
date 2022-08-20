using System.Collections;
using Cr7Sund.Animation.UI;
using Cr7Sund.MyCoroutine;
using UnityEngine;

namespace Cr7Sund.UIFrameWork
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIPageView : MonoBehaviour
    {
        // public Button btn;

        // public UIPageController Bind()=>new DemoPageController();
        public abstract UIPageController Bind();

        [SerializeField] public int renderingOrder;
        [SerializeField] public UIType uiType;
        public PageTransitionAnimationContainer transitionAniamtionController;
    }
}