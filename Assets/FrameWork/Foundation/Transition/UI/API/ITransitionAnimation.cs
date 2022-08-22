namespace Cr7Sund.Transition.UI
{

    using System.Collections;
    using UnityEngine;
    using System;
    using Cr7Sund.Pool;

    [Obsolete("Please use")]
    public interface ITransitionAnimation : ITransition
    {
        void Setup(RectTransform rectTransform);
        void SetPartner(RectTransform partnerRectTransform);
    }

    public static class TransitionAnimationExtensions
    {
        public static IEnumerator CreatePlayRoutine(this ITransitionAnimation self, IProgress<float> progress = null, bool enter = true)
        {
            var player = PoolManager.Instance.Get<TransitionPlayer>();
            player.Animation = self;
            player.Reset();

            UpdateDispatcher.Instance.Register(player);
            {
                progress?.Report(.0f);
                player.Play(enter);
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