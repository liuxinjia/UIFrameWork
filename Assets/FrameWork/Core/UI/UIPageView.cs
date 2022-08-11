using System.Collections;
using Cr7Sund.Animation.UI;
using Cr7Sund.MyCoroutine;
using UnityEngine;

namespace Cr7Sund.UIFrameWork
{
    public abstract class UIPageView : MonoBehaviour
    {
        // public Button btn;

        // public UIPageController Bind()=>new DemoPageController();
        public abstract UIPageController Bind();

        [SerializeField] public int renderingOrder;
        public PageTransitionAnimationContainer transitionAniamtionController;
    }
}