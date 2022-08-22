namespace Cr7Sund.Transition.UI
{
    using Cr7Sund.AssetLoader;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.Playables;
    using UnityEngine.Timeline;

    [RequireComponent(typeof(PlayableDirector))]
    public class TimelineTransitionBehaviour : TransitionBehaviour
    {
        [SerializeField] private PlayableDirector _director;

        [SerializeField] private TimelineAsset _timelineAsset;

        public override float Duration => (float)_timelineAsset.duration;

        protected override void Setup()
        {
            _director.playableAsset = _timelineAsset;
            _director.time = 0;
            _director.initialTime = 0;
            _director.playOnAwake = false;
            _director.timeUpdateMode = DirectorUpdateMode.Manual;
            _director.extrapolationMode = DirectorWrapMode.None;
        }

        public override void Play(bool enter)
        {
            _director.Play();
        }

        public override void Stop()
        {
            _director.Stop();
        }

        public override void Resume(float time)
        {
            _director.time = (double)time;
            _director.Play();
        }

        public override void Reset()
        {
            _director.Stop();
            _director.time = 0;
        }


    }
}