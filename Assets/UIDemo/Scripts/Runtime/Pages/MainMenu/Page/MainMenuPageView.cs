using System.Collections;
using System.Collections.Generic;
using Cr7Sund.UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPageView : UIPageView
{
    public override UIPageController Bind() => new MainMenuPageController();

    
}

public static partial class UINodes                
{
    //Replace With Pool
    public static readonly UINode MainMenuPage = new UINode("MainMenuPage");
}