namespace Cr7Sund.AssetLoader
{
    using System.Collections.Generic;
    using UnityEngine;
    using Cr7Sund.Pool;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using System;

    using Object = UnityEngine.Object;

    internal sealed class AddressableAssetLoader : IAssetLoader
    {
        private int _nextControlId;
        // Why we don't use assetky as the key 
        // 1. String key Performance and hash detect collisions 
        // 2. sine we always release handle when we had load
        private Dictionary<int, AsyncOperationHandle> _addressableHanlders
            = new Dictionary<int, AsyncOperationHandle>();


        public AssetLoadHandle<T> Load<T>(string key) where T : Object
        {
            var controlId = _nextControlId++;

            var handle = PoolManager.Instance.Get<AssetLoadHandle<T>>(); // maybe replace with struct
            var setter = (IAssetLoadHandleSetter<T>)handle;
            handle.ControlId = controlId;

            var addressableHandle = Addressables.LoadAssetAsync<T>(key);
            _addressableHanlders.Add(handle.ControlId, addressableHandle);
            
            addressableHandle.WaitForCompletion();

            setter.SetPercentCompleteFunc(() => addressableHandle.PercentComplete);
            setter.SetLoadResult<T>(key, addressableHandle.Result);

            return handle;
        }

        public AssetLoadHandle<T> LoadAsync<T>(string key) where T : Object
        {
            var controlId = _nextControlId++;

            var handle = PoolManager.Instance.Get<AssetLoadHandle<T>>();
            var setter = (IAssetLoadHandleSetter<T>)handle;
            handle.ControlId = controlId;

            var addressableHandle = Addressables.LoadAssetAsync<T>(key);
            _addressableHanlders.Add(handle.ControlId, addressableHandle);
            addressableHandle.Completed += x =>
            {
                setter.SetLoadResult<T>(key, x.Result);
            };

            setter.SetPercentCompleteFunc(() => addressableHandle.PercentComplete);
            return handle;
        }


        public void Release<T>(AssetLoadHandle<T> handle) where T : Object
        {
            PoolManager.Instance.Release<AssetLoadHandle<T>>(handle);

            if (!_addressableHanlders.TryGetValue(handle.ControlId, out var addressableHandle))
            {
                throw new InvalidOperationException($"There is no asset that has been requested for release (ControlId: {handle.ControlId}.");
            }

            _addressableHanlders.Remove(handle.ControlId);
            Addressables.Release(addressableHandle);
        }
    }

}