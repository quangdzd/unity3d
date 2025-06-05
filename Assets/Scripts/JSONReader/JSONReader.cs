using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class JSONReader 
{

    string folderPath = "Assets/Resources";
    public WaveDatas WavedatasReader(string path)
    {
        String jsonPath = Path.Combine(folderPath, path);
        string jsonContent = File.ReadAllText(jsonPath);
        return JsonUtility.FromJson<WaveDatas>(jsonContent);
    }
    
    


}
