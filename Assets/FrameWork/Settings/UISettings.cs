namespace Cr7Sund.Settings
{
    using Cr7Sund.Transition.UI;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Cr7SundFrameWork/UI/UISettings")]
    public class UISettings : ScriptableObject
    {
        public const string resourcesKey = "CustomUISettings";

        [SerializeField] private TransitionObject PageEnterAnimation;
        [SerializeField] private TransitionObject PageExitAnimation;


        public ITransitionAnimation GetDefaultPageTransitionAnimation(bool push, bool enter)
        =>  (enter ? PageEnterAnimation : PageExitAnimation);

        public void InitDefaultSettings()
        {

        }
#if UNITY_EDITOR
#endif

    }
}