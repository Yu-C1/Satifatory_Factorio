using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int objectID;

    [Header("UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;

    [Header("Refs")]
    [SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private ObjectsDatabaseSO database;

    private Button button;
    private int cost;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        ObjectData data = GetDataByID(objectID);
        if (data == null)
        {
            Debug.LogError($"ShopItemButton: objectID {objectID} not found in database.");
            button.interactable = false;
            return;
        }

        cost = data.Cost;

        if (iconImage != null) iconImage.sprite = data.Icon;
        if (nameText != null) nameText.text = data.Name;
        if (priceText != null) priceText.text = "$" + data.Cost;

        UpdateState(MoneyManager.Instance.Money);
        MoneyManager.Instance.OnMoneyChanged += UpdateState;

        button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        if (MoneyManager.Instance != null)
            MoneyManager.Instance.OnMoneyChanged -= UpdateState;

        if (button != null)
            button.onClick.RemoveListener(OnClick);
    }

    private void UpdateState(int money)
    {
        button.interactable = money >= cost;
    }

    private void OnClick()
    {
        // Only start placement; money is charged on successful placement.
        placementSystem.StartPlacement(objectID);
    }

    private ObjectData GetDataByID(int id)
    {
        if (database == null || database.objectsData == null) return null;

        for (int i = 0; i < database.objectsData.Count; i++)
        {
            if (database.objectsData[i].ID == id)
                return database.objectsData[i];
        }
        return null;
    }
}