using UnityEngine;

public class ClosePanelButton : MonoBehaviour
{
    [SerializeField] private GameObject shopRoot;

    public void Close()
    {
        if (shopRoot != null)
        {
            shopRoot.SetActive(false);
        }
    }
}