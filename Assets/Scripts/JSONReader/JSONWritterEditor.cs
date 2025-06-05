
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(JSONWritter))]
public class JSONWritterEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        JSONWritter jSONWritter= (JSONWritter)target;
        
        if (GUILayout.Button("DataToJSon")) {
            jSONWritter.SavetoJSon();
        }
        
    }
}
