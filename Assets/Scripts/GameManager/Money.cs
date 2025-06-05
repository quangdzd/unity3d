using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money
{
    private int _money;
    public event Action MoneyChanged;

    public Money()
    {
        this._money = 0;
        MoneyChanged?.Invoke();
        
    }

    public void Add(int amount)
    {
        _money += amount;
        // Debug.Log("" + _money);
        MoneyChanged?.Invoke();
    }

    public void Subtract(int amount)
    {
        _money -= amount;
        // Debug.Log("" + _money);
        if (_money < 0) _money = 0; 
        MoneyChanged?.Invoke();

    }

    public int GetAmount()
    {
        return _money;
    }


}
