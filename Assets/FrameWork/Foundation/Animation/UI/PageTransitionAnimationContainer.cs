namespace Cr7Sund.Animation.UI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    [System.Serializable]
    public class PageTransitionAnimationContainer 
    {
        //PLAN Supoort diffent animation with different pages
        [SerializeField] private TransitionAnimationObject PagePushEnterAnimation;
        [SerializeField] private TransitionAnimationObject PagePushExitAnimation;
        [SerializeField] private TransitionAnimationObject PagePopEnterAnimation;
        [SerializeField] private TransitionAnimationObject PagePopExitAnimation; 


        public ITransitionAnimation GetAnimation(bool push, bool enter)
        => push
            ? (enter ? PagePushEnterAnimation : PagePushExitAnimation)
            : (enter ? PagePopEnterAnimation : PagePopExitAnimation);




    }
}