using UnityEngine;

namespace Cr7Sund.Transition.UI
{
    public abstract class TransitionBehaviour : MonoBehaviour, ITransitionAnimation
    {
        public RectTransform RectTransform { get; private set; }
        public RectTransform PartnerRectTransform { get; private set; }
        public abstract float Duration { get; }

        void ITransitionAnimation.SetPartner(RectTransform partnerRectTransform)
        {
            PartnerRectTransform = partnerRectTransform;
        }

        void ITransitionAnimation.Setup(RectTransform rectTransform)
        {
            RectTransform = rectTransform;
            Setup();

        }

        protected abstract void Setup();

        public abstract void Resume(float time);

        public abstract void Play(bool enter);

        public abstract void Reset();

        public abstract void Stop();


    }
}