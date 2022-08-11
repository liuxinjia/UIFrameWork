using System.Collections;

namespace Cr7Sund.UIFrameWork
{
    public interface IUIPageController : System.IDisposable
    {
        IEnumerator OnShowAsync();
        void OnShow();


        void OnHide();

        IEnumerator OnRelease();

    }
}