
namespace Cr7Sund.Pool
{
    using System;
    using UnityEngine;
    using Cr7Sund.Runtime.Util;
    using System.Collections.Generic;

    public class PoolManager : MonoBehaviourSingleton<PoolManager>
    {
        private readonly Dictionary<Type, object> _pools = new Dictionary<Type, object>();

        public T Get<T>() where T : class, new()
        {
            if (!_pools.TryGetValue(typeof(T), out var cachePool))
            {
                cachePool = new ObjectPool<T>(() => new T(), null, null);
                if (typeof(System.IDisposable).IsAssignableFrom(typeof(T)))
                {
                    cachePool = new ObjectPool<T>(() => new T(), null, null, (t) =>
                    {
                        var disposableObj = t as System.IDisposable;
                        disposableObj.Dispose();
                    });
                }
                else
                {
                    cachePool = new ObjectPool<T>(() => new T(), null, null);
                }
                _pools.Add(typeof(T), cachePool);
            }
            if (cachePool is ObjectPool<T> pool) return pool.Get();
            return null;


        }

        public void Release<T>(T element) where T : class, new()
        {
            if (!_pools.TryGetValue(typeof(T), out var cachePool))
            {
                //1. For example , Get A<T>, but Release A
                throw new Exception($"No pool item  has been push, Make sure you have the same type with Get, {typeof(T)}");
            }
            if (cachePool is ObjectPool<T> pool) pool.Release(element);
        }
    }
}