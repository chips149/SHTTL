using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleMapBuilder))]
public class BattleMapEditor : Editor
{

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        BattleMapBuilder builder = (BattleMapBuilder)target;
        
        var nailParent = builder.transform.Find("NailParent");
        var userAreaParent = builder.transform.Find("UserAreaParent");
        
        if (GUILayout.Button("BUILD"))
        {
            var width = builder.width;
            var height = builder.height;

            var nailCount = (height * width) - Mathf.FloorToInt(height * 0.5f);
            Build(nailParent, nailCount, builder.nailPrefab);

            var userAreaCount = (height * width) - Mathf.CeilToInt(height * 0.5f);
            Build (userAreaParent, userAreaCount, builder.userAreaPrefab);
            
            builder.OnValidate();
        }
    }


    public void Build(Transform parent, int count, GameObject prefab)
    {

        while (parent.childCount > 0)
        {
            DestroyImmediate(parent.GetChild(0).gameObject);
        }
        
        while (parent.childCount <= count)
        {
            PrefabUtility.InstantiatePrefab(prefab, parent);
        }
 
    }
}
