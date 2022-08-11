namespace Cr7Sund.Settings
{
    using Cr7Sund.Animation.UI;
    using UnityEngine;

    public class UISettings : ScriptableObject
    {
        public const string path = "";

        [SerializeField] private  TransitionAnimationObject PagePushEnterAnimation;
        [SerializeField] private  TransitionAnimationObject PagePushExitAnimation;
        [SerializeField] private  TransitionAnimationObject PagePopEnterAnimation;
        [SerializeField] private  TransitionAnimationObject PagePopExitAnimation;


        public ITransitionAnimation GetDefaultPageTransitionAnimation(bool push, bool enter)
        => push
            ? (enter ? PagePushEnterAnimation : PagePushExitAnimation)
            : (enter ? PagePopEnterAnimation : PagePopExitAnimation);

        public void InitDefaultSettings(){

        }
#if UNITY_EDITOR
#endif

    }
}