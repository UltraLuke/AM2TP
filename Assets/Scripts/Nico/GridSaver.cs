using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class GridSaver : Editor
{
    static List<GameObject> items = new List<GameObject>();

    [MenuItem("CustomTools/CustomGrid/Save")]
    public static void Save()
    {
        SaveGrid();
        SaveItemsGrid();
    }

    private static void SaveItemsGrid()
    {
        CreateTestList();

        foreach (GameObject item in items)
        {
            //Obtengo el prefab del item
            var prefab = PrefabUtility.GetCorrespondingObjectFromSource(item);
            var PrefabPath = AssetDatabase.GetAssetPath(prefab);
            
            //Creo el scriptable
            var scriptable = ScriptableObject.CreateInstance<ItemObject>();
            scriptable.Setter(item.name, PrefabPath, item.transform.position, item.transform.localScale, item.transform.rotation);

            //Guardo el scriptable
            //Chequeos sobre la carpeta, crearla si no existe o que hacer si ya existe
            var path = "Assets/Resources/SavedItems/" + item.name + ".asset"; 
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(scriptable, path);
        }

        items.Clear();
    }
    private static void SaveGrid()
    {
        //grid.GetGridData() Como me lo pasa? como una lista? row, column, cellsize, origpos
    }
    public static void CreateTestList()
    {

        var objects = FindObjectOfType<CustomGrid>().ObjectList;
        var objectsInScene = new List<GameObject>();

        foreach (var item in objects)
        {
            objectsInScene.Add(item.Value);
        }

        //GameObject[] objectsInScene = FindObjectsOfType<GameObject>();
        for (int i = 0; i < objectsInScene.Count; i++)
        {
            items.Add(objectsInScene[i]);
            Debug.Log("Added: " + objectsInScene[i].name);
        }

        /* var grid = FindObjectOfType<Grid>();
         * if(grid)
         * {
         *  items = grid.GetObjects();
         * }
         *
         */
    }
}
