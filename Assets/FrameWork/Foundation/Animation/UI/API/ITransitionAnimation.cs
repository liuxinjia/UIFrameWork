namespace Cr7Sund.Animation.UI
{

    using System.Collections;
    using UnityEngine;
    using System;
    using Cr7Sund.Pool;

    public interface ITransitionAnimation : IAnimation
    {
        void Setup(RectTransform rectTransform);
        void SetPartner(RectTransform partnerRectTransform);
    }

    public static class TransitionAnimationExtensions
    {
        public static IEnumerator CreatePlayRoutine(this ITransitionAnimation self, IProgress<float> progress = null)
        {
            var player = PoolManager.Instance.Get<AnimationPlayer>();
            player.Animation = self;

            UpdateDispatcher.Instance.Register(player);
            {
                progress?.Report(.0f);
                player.Play();
                while (!player.IsFinished)
                {
                    yield return null;
                    progress?.Report(player.Time / self.Duration);
                }

            }
            UpdateDispatcher.Instance.Unregister(player);
            PoolManager.Instance.Release(player);
        }
    }
}