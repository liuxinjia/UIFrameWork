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

        [SerializeField] private Animator _animator;

        public override float Duration => _timeLength;

        private float _timeLength;
        const string OpenTransitionName = "Open";

        private int _openParameterId;

        protected override void Setup()
        {
            _openParameterId = Animator.StringToHash(OpenTransitionName);
            _timeLength = _animator.GetCurrentAnimatorStateInfo(0).length + _animator.GetAnimatorTransitionInfo(0).duration/2;
        }

        public override void Play(bool enter)
        {
            // if (_animator != null) //TRy This 
            {
                _animator.SetBool(_openParameterId, enter);

                _timeLength = _animator.GetCurrentAnimatorStateInfo(0).length + _animator.GetAnimatorTransitionInfo(0).duration/2;
                Debug.Log(_timeLength);
            }
        }

        public override void Stop()
        {
        }

        public override void Resume(float time)
        {
        }

        public override void Reset()
        {

        }


    }
}