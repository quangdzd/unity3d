// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ReceiveEvent : Singleton<ReceiveEvent>
// {

//     private void OnEnable()
//     {
//         // Đăng ký vào sự kiện OnButtonClicked của EventManager
//         InputManagerUI.Instance.OnButtonClicked += HandleButtonClicked;
//     }

//     private void OnDisable()
//     {
//         // Hủy đăng ký sự kiện khi không cần dùng
//         InputManagerUI.Instance.OnButtonClicked -= HandleButtonClicked;
//     }

//     private void HandleButtonClicked(StatsForEnemy statsForEnemy)
//     {
//         Debug.Log("Received message from EventManager: " + statsForEnemy._name);
//     }
// }
