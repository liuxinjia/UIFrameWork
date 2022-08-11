using UnityEngine;

namespace Cr7Sund.Animation.UI
{
    public abstract class TransitionAnimationObject : ScriptableObject, ITransitionAnimation
    {
        public float Duration { get; }
        public RectTransform PartnerRectTransform{get; private set;}
        public RectTransform RectTransform {get; private set;}


        public void SetPartner(RectTransform partnerRectTransform)
            => PartnerRectTransform = partnerRectTransform;

        public void Setup(RectTransform rectTransform){
            RectTransform = rectTransform;
            Init();
            SetTime(.0f);
        }

        public abstract void Init();
        public abstract void SetTime(float time);

    }
}