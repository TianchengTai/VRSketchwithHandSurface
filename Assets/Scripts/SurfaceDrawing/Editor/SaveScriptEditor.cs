using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRPainting;


[CustomEditor(typeof(SaveScript))]
public class SaveScriptEditor : Editor
{
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var target = (SaveScript)(serializedObject.targetObject);
        if(GUILayout.Button("保存")){
           target.Save();
        }
       if(GUILayout.Button("加载")){
           target.Load(target.load_id);
        }
    }
    
}
