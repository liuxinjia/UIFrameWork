using System.Collections;
using System.Collections.Generic;
using Cr7Sund.UIFrameWork;
using UnityEngine;

public class LoadingPageController : UIPageController
{
    public override IEnumerator OnShowAsync()
    {
        yield return new WaitForSeconds(2f);
        // UIManager.Instance.Show(UINodes.DemoPage);
    }
}
