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

    static CustomGrid _customGrid;

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
            objInScene.transform.rotation = item.rotation;
            objInScene.transform.localScale = item.scale;

            _customGrid.SetObjectOnGrid(objInScene, item.position);
            //objInScene.transform.position = item.position;
        }
    }
    private static void LoadGrid()
    {
        _customGrid = FindObjectOfType<CustomGrid>();
    }
}
