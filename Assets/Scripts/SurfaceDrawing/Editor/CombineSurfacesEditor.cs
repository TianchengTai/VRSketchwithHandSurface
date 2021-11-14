using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRPainting;

[CustomEditor(typeof(CombineSurfaces))]
public class CombineSurfacesEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var target = (CombineSurfaces)(serializedObject.targetObject);
        if(GUILayout.Button("组合")){
           target.myTest();
        }
       
    }
}
