using Cr7Sund.UIFrameWork;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.Show(UINodes.MainMenuPage, false, false, false);
    }
}