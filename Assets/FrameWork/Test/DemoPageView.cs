using Cr7Sund.UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class DemoPageView : UIPageView
{
    public override UIPageController Bind() => new DemoPageController();

    public Button button;
    public Image image;
}

public static partial class UINodes
{
    //Replace With Pool
    public static readonly UINode DemoPage = new UINode("Assets/Resoures/DemoPage");
}