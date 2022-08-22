using System.Collections;

namespace Cr7Sund.UIFrameWork
{
    public interface IUIPageController
    {
        IEnumerator OnShowAsync();
        void OnShow();
        void OnHide();
        void OnDestory();

    }
}