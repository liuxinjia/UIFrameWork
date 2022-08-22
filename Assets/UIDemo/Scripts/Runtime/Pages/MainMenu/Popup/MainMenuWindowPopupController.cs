using System.Collections;
using System.Collections.Generic;
using Cr7Sund.UIFrameWork;
using UnityEngine;

public class MainMenuWindowPopupController : UIPageController
{
    public MainMenuWindowPopupView VM => (MainMenuWindowPopupView)pageView;     // Start is called before the first frame update
    public override void OnShow()
    {
        VM.startGameBtn.onClick.AddListener(StartGame);
    }



    public override void OnHide()
    {
        VM.startGameBtn.onClick.RemoveAllListeners();
    }

    // public override IEnumerator OnShowAsync()
    // {
    //     throw new System.NotImplementedException();
    // }


    private void StartGame()
    {
        UIManager.Instance.Show(UINodes.LoadingPage, true, false, true, true);
    }
}

