using System;
namespace Cr7Sund.Transition.UI
{
    using Cr7Sund.AssetLoader;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;

    [System.Obsolete("Use tween or simple animations , timeline")]
    [RequireComponent(typeof(Animator))]
    public class AnimatorTransitionBehaviour : TransitionBehaviour
    {

        [SerializeField] private Animator _animation;

        public override float Duration => _timeLength;

        private float _timeLength;
        const string OpenTransitionName = "Open";

        private int _openParameterId;

        protected override void Setup()
        {
            _openParameterId = Animator.StringToHash(OpenTransitionName);
        }

        public override void Play(bool enter)
        {
            if (_animation != null)
                _animation.SetBool(_openParameterId, enter);
        }

        public override void Stop()
        {
            throw new NotImplementedException("Aniamtion can not stop");
        }

        public override void Resume(float time)
        {
           throw new NotImplementedException("Aniamtion can not resume");
        }

        public override void Reset()
        {
          
        }


    }
}