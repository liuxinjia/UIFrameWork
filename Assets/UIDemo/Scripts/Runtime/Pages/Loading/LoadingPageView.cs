using System.Collections;
using System.Collections.Generic;
using Cr7Sund.UIFrameWork;
using UnityEngine;

public class LoadingPageView : UIPageView
{
    public override UIPageController Bind() => new LoadingPageController();

}


public static partial class UINodes
{
    //Replace With Pool
    public static readonly UINode LoadingPage = new UINode("LoadingPage");
}