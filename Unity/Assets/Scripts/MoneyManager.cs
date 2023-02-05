using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private int currentPLayerMoney;

    public int starterMoney;

    public void Start()
    {
        currentPLayerMoney = starterMoney;
    }

    public int GetCurrentMoney()
    {
        return currentPLayerMoney;
    }

    public void AddMoney(int amount)
    {
        currentPLayerMoney += amount;
    }

    public void RemoveMoney(int amount)
    {
        currentPLayerMoney -= amount;
        Debug.Log("Removed " + amount + " from playe's money! The player now has " + currentPLayerMoney);
    }
}
