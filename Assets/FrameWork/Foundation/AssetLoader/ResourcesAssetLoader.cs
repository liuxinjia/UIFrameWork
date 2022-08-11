namespace Cr7Sund.AssetLoader
{
    using System.Collections.Generic;
    using UnityEngine;
    using Cr7Sund.Pool;

    internal sealed class ResourcesAssetLoader : IAssetLoader
    {
        private int _nextControlId;



        public AssetLoadHandle<T> Load<T>(string key) where T : Object
        {
            var controlId = _nextControlId++;

            var handle = PoolManager.Instance.Get<AssetLoadHandle<T>>();
            var setter = (IAssetLoadHandleSetter<T>)handle;
            handle.ControlId = controlId;


            var result = Resources.Load<T>(key);
            setter.SetLoadResult<T>(key, result);
            setter.SetPercentCompleteFunc(() => 1.0f);

            return handle;
        }

        public AssetLoadHandle<T> LoadAsync<T>(string key) where T : Object
        {
            var controlId = _nextControlId++;

            var handle = PoolManager.Instance.Get<AssetLoadHandle<T>>();
            var setter = (IAssetLoadHandleSetter<T>)handle;
            handle.ControlId = controlId;

            var req = Resources.LoadAsync<T>(key);
            req.completed += _ =>
            {
                var result = req.asset as T;
                setter.SetLoadResult<T>(key, result);
            };

            setter.SetPercentCompleteFunc(() => req.progress);
            return handle;
        }


        public void Release<T>(AssetLoadHandle<T> handle) where T : Object
        {
            PoolManager.Instance.Release<AssetLoadHandle<T>>(handle);
            //Resources.UnloadUnesseAssets will free the unused asset memory
        }
    }

}