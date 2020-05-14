using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MathData))]
public class MathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MathData myTarget = (MathData)target;
        DrawDefaultInspector();        
       
        if(GUILayout.Button("Build Object"))
        {
            myTarget.CreateData();
        }
        
    }
}
