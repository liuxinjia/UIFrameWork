namespace Cr7Sund.Animation
{
    using System.Collections.Generic;
    using Cr7Sund.Runtime.Util;
    using UnityEngine;

    public delegate float CalcDeltaTime(float deltaTime);

    public class UpdateDispatcher : Singleton<UpdateDispatcher>
    {
        private readonly Dictionary<IUpdatable, CalcDeltaTime> _targets =
        new Dictionary<IUpdatable, CalcDeltaTime>();

        private void Update()
        {
            foreach (var target in _targets)
            {
                var deltaTime = target.Value?.Invoke(Time.deltaTime) ?? Time.deltaTime;
                target.Key.Update(deltaTime);
            }
        }

        public void Register(IUpdatable target, CalcDeltaTime calcDeltaTime = null)
        {
            _targets.Add(target, calcDeltaTime);
        }

        public void Unregister(IUpdatable target)
        {
            _targets.Remove(target);
        }
    }
}