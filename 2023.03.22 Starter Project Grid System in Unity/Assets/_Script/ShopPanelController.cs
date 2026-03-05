using UnityEngine;

public class OpenShopPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void Open()
    {
        panel.SetActive(true);
    }
}