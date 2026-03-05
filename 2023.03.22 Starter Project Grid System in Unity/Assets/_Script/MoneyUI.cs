using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    private void Start()
    {
        if (MoneyManager.Instance == null)
        {
            Debug.LogError("MoneyManager not found in scene.");
            enabled = false;
            return;
        }

        MoneyManager.Instance.OnMoneyChanged += UpdateMoney;
        UpdateMoney(MoneyManager.Instance.Money);
    }

    private void OnDestroy()
    {
        if (MoneyManager.Instance != null)
            MoneyManager.Instance.OnMoneyChanged -= UpdateMoney;
    }

    private void UpdateMoney(int money)
    {
        if (moneyText != null)
            moneyText.text = "$" + money;
    }
}