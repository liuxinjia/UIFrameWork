using System.Collections;
using System.Collections.Generic;
using Cr7Sund.UIFrameWork;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuWindowPopupView : UIPageView
{
    public override UIPageController Bind()=>  new MainMenuWindowPopupController();
    public Button startGameBtn;
}

public static partial class UINodes
{
    //Replace With Pool
    public static readonly UINode MainMenuWindowPopup = new UINode("MainMenu_Window_Popup");
}