using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Data))]
public class DataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Data myTarget = (Data)target;
        DrawDefaultInspector();        
       
        if(GUILayout.Button("Build Object"))
        {
            myTarget.CreateData();
        }
        
    }
}
