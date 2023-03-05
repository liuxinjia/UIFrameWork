using Cr7Sund.MyCoroutine;

namespace Cr7Sund.UIFrameWork
{
    internal interface IUIManager
    {
        AsyncProcessHandle Show(UINode enterPage, bool playAnimation = true, bool keepInStack = true, bool loadAsync = true);

        AsyncProcessHandle Back(bool playAnimation = true);


        AsyncProcessHandle Preload(UINode preloadPage);

        void ReleasePreloaded(UINode preloadPage);
    }
}