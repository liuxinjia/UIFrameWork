using System;

namespace Cr7Sund.Transition.UI
{
    public class TransitionPlayer : IUpdatable
    {
        public ITransition Animation;
        public bool IsPlaying { get; private set; }
        public float Time { get; private set; }
        public bool IsFinished => Time >= Animation.Duration;

        public void Update(float deltaTime)
        {
            if (!IsPlaying) return;

            SetTime(Time + deltaTime);
        }

        public void Play(bool enter)
        {
            IsPlaying = true;
            Animation.Play(enter);
        }
        public void Resume()
        {
            Animation.Resume(Time);
        }

        public void Stop()
        {
            IsPlaying = false;
            Animation.Stop();
        }

        public void Reset()
        {
            SetTime(0f);
            Animation.Reset();
        }


        public void SetTime(float time)
        {
            time = Math.Max(0, Math.Min(Animation.Duration, time));

            if (IsPlaying && time >= Animation.Duration)
            {
                Stop();
            }

            Time = time;
        }
    }
}