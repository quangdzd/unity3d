using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private int cost = 0;
    private int reward = 0;

    public void SetCost(int cost)
    {
        this.cost = cost;
    }
    public void SetReward(int reward)
    {
        this.reward = reward;
    }

    public int GetCost() => this.cost;
    public int GetReward() => this.reward;
    
}
