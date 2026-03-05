using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [SerializeField] private int startingMoney = 0;

    public int Money { get; private set; }

    public event Action<int> OnMoneyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Money = startingMoney;
        OnMoneyChanged?.Invoke(Money);
    }

    public bool CanAfford(int cost) => Money >= cost;

    public bool Spend(int cost)
    {
        if (Money < cost) return false;
        Money -= cost;
        OnMoneyChanged?.Invoke(Money);
        return true;
    }

    public void Add(int amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke(Money);
    }
}