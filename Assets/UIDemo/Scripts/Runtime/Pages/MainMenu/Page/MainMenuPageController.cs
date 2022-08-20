using System;
using System.Collections;
using System.Collections.Generic;
using Cr7Sund.UIFrameWork;
using UnityEngine;

public class MainMenuPageController : UIPageController
{
    public override void OnShow()
    {
        UIManager.Instance.ShowPopup(UINodes.MainMenuWindowPopup,false);
    }



}
