namespace Cr7Sund.AssetLoader
{
    using UnityEngine;
    using Cr7Sund.Runtime.Util;

    public class AssetManager : Singleton<AssetManager>, IAssetLoader
    {
        public IAssetLoader AssetLoader;

        public AssetManager()
        {
            //  1. There will beonly one resources loader
            //  2. If we want to support different load type, for example, different asset loader object supoort differnt load type
            // Pay attention to the object pool

#if ADDRESSABLES_1_17_4_OR_NEWER
            AssetLoader = new AddressableAssetLoader();
#else
            AssetLoader = new ResourcesAssetLoader();
#endif

        }

        public AssetLoadHandle<T> Load<T>(string key) where T : Object
           => AssetLoader.Load<T>(key);

        public AssetLoadHandle<T> LoadAsync<T>(string key) where T : Object
            => AssetLoader.LoadAsync<T>(key);


        public void Release<T>(AssetLoadHandle<T> handle) where T : Object
            => AssetLoader.Release(handle);
    }
}