namespace Cr7Sund.AssetLoader
{
    using System;
    using Object = UnityEngine.Object;

    public interface IAssetLoader
    {
        AssetLoadHandle<T> Load<T>(string key) where T : Object;
        AssetLoadHandle<T> LoadAsync<T>(string key) where T : Object;
        void Release<T>(AssetLoadHandle<T> handle)where T : Object;
    }



}