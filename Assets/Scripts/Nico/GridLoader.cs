using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using Object = UnityEngine.Object;

public class GridLoader : Editor
{
    static ItemObject[] AllObjInGrid;
    [MenuItem("CustomTools/CustomGrid/Load")]
    public static void Load()
    {
        LoadGrid();
        LoadItemsGrid();
    }
    private static void LoadItemsGrid()
    {
        //Chequeos sobre la carpeta
        AllObjInGrid = Resources.LoadAll<ItemObject>("SavedItems/");

        foreach (ItemObject item in AllObjInGrid)
        {
            string path = item.path;
            Object go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            GameObject objInScene = (GameObject)PrefabUtility.InstantiatePrefab(go);
            objInScene.transform.position = item.position;
            objInScene.transform.rotation = item.rotation;
            objInScene.transform.localScale = item.scale;
        }
    }
    private static void LoadGrid()
    {

    }
}
