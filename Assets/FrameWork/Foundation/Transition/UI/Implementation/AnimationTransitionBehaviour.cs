namespace Cr7Sund.Transition.UI
{
    using Cr7Sund.AssetLoader;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;

    [RequireComponent(typeof(Animation))]
    public class AnimationTransitionBehaviour : TransitionBehaviour
    {

        [SerializeField] private Animation _animation;

        public override float Duration => _timeLength;

        private float _timeLength;

        protected override void Setup()
        {
            foreach (AnimationState state in _animation)
            {
                _timeLength += state.length;
            }
        }

        public override void Play(bool enter)
        {
            foreach (AnimationState state in _animation)
            {
                _animation.PlayQueued(state.name);
            }
        }

        public override void Stop()
        {
            _animation.Stop(); //Stop ALl
        }

        public override void Resume(float time)
        {
            Debug.LogError("Aniamtion can not resume");
        }

        public override void Reset()
        {
            Stop();
        }


    }
}