using Cr7Sund.Pool;

namespace Cr7Sund.UIFrameWork
{

    public static partial class UINodes
    {
        // public static ObjectPool<UINode> pool = new ObjectPool<UINode>(()=>new );
        // public static readonly UINode AnnouncementPanel = new UINode("announcementpanel", null);
    }

    public enum UIType{
        Default,
        Popup,
        HUD
    }

    // UI Page Node
    public  class UINode
    {
        public readonly string resourceKey; //replace with ushort

        public UIPageController pageController;


        public ushort Level = 0;
        public UINode prevKey;
        public bool keepInStack;

        public UINode(string key)
        {
            resourceKey = key;
        }

        public void Destroy()
        {
            Level = 0;
            prevKey = null;
            UnityEngine.GameObject.Destroy(pageController.pageView.gameObject);
            pageController.Dispose();
            pageController = null;
        }


    }
}