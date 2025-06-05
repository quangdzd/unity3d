using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonDestroy<InputManager>
{
    private Dictionary<Func<bool>, Action> _actions = new Dictionary<Func<bool>, Action>();

    private void Update()
    {
        // Duyệt qua tất cả các điều kiện và hành động
        foreach (var pair in _actions)
        {
            // Nếu điều kiện (Func<bool>) trả về true, thì thực thi hành động (Action)
            if (pair.Key.Invoke())
            {
                pair.Value.Invoke();
            }
        }
    }

    public void RegisterAction(Func<bool> condition, Action action)
    {
        if (!_actions.ContainsKey(condition))
        {
            _actions.Add(condition, action);
        }
    }

    // Hàm hủy đăng ký một hành động với điều kiện
    public void UnregisterAction(Func<bool> condition)
    {
        if (_actions.ContainsKey(condition))
        {
            _actions.Remove(condition);
        }
    }
}
