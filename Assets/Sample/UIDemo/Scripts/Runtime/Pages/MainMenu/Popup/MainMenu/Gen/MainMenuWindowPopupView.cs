using System.Collections;
using System.Collections.Generic;
using Cr7Sund.UIFrameWork;
using UnityEngine;
using UnityEngine.UI;


public static partial class UINodes
{
    //Replace With Pool
    public static readonly UINode MainMenuWindowPopup = new UINode("MainMenu_Window_Popup");
}

public class MainMenuWindowPopupView : UIPageView
{
    public override UIPageController Bind() => new MainMenuWindowPopupController();
    public Button startGameBtn;

    private void AddListeners(){
        startGameBtn.onClick += 
    }

    public void OnStartGameEvent(){

    }
}