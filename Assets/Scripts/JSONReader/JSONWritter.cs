using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class JSONWritter :   MonoBehaviour
{
    public EnemyData enemyData;
    private string folderPath = "Assets/Resources";
    public string fileName = "Character/Enemy.txt";



    public void SavetoJSon()
    {
        string filePath = Path.Combine(folderPath, fileName);
        string json = JsonUtility.ToJson(enemyData);

        
            if (!Directory.Exists(folderPath))
    {
        Directory.CreateDirectory(folderPath);
    }
        try{
            File.WriteAllText(filePath,json);
            Debug.Log("Tao thanh cong");
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
        
    }

}
