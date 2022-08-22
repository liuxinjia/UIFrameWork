using System.Collections;
using Cr7Sund.UIFrameWork;
using UnityEngine;

public class DemoPageController : UIPageController
{
    public DemoPageView VM => (DemoPageView)pageView;



    public override void OnShow()
    {
        VM.button.onClick.AddListener(null);
    }

    public override void OnHide()
    {
        //MARK  GetAllCompoonent to remove 
        // Maybe there exist listeners  collection
        VM.button.onClick.RemoveAllListeners(); 
    }

    // public override IEnumerator OnShowAsync()
    // {
    //     throw new System.NotImplementedException();
    // }


}