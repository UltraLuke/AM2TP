using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class GridSaver : EditorWindow
{
    public GUIStyle _myStyle;
    static List<GameObject> items = new List<GameObject>();
    private string _FolderName;

    [MenuItem("CustomTools/CustomGrid/Save")]
    public static void OpenWindow()
    {        
        var saveWindow = GetWindow<GridSaver>();

        saveWindow._myStyle = new GUIStyle
        {
            fontStyle = FontStyle.BoldAndItalic,
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft,
            wordWrap = true
        };
        saveWindow.Show();
    }
    private void OnGUI()
    {        
        _FolderName = EditorGUILayout.TextField("Folder Name: ", _FolderName);
        EditorGUILayout.LabelField("Path: Assets/Resources/" + _FolderName);

        if (GUILayout.Button("Save"))
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources/" + _FolderName))
            {
                AssetDatabase.CreateFolder("Assets/Resources", _FolderName);
            }

            CopyDataFromGrid();
            SaveItemsGrid(_FolderName);
            SaveGrid(_FolderName);
        }
    }
    private static void SaveItemsGrid(string folderName)
    {
        foreach (GameObject item in items)
        {
            //Obtengo el prefab del item
            var prefab = PrefabUtility.GetCorrespondingObjectFromSource(item);
            var PrefabPath = AssetDatabase.GetAssetPath(prefab);
            
            //Creo el scriptable
            var scriptable = ScriptableObject.CreateInstance<ItemObject>();
            scriptable.Setter(item.name, PrefabPath, item.transform.position, item.transform.localScale, item.transform.rotation);

            //Guardo el scriptable
            var path = "Assets/Resources/" + folderName+"/"+ item.name + ".asset"; 
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(scriptable, path);
        }

        items.Clear();
    }
    private static void SaveGrid(string folderName)
    {
        var grid = FindObjectOfType<CustomGrid>();

        float size = grid.size;

        Vector3 origin = grid.transform.position;

        var scriptable = ScriptableObject.CreateInstance<GridObject>();
        scriptable.Setter(size, origin);

        var path = "Assets/Resources/" + folderName + "/" + grid.name + ".asset";
        //path = AssetDatabase.GenerateUniqueAssetPath(path); Creo que deberia existir una grilla por carpeta
        AssetDatabase.CreateAsset(scriptable, path);
    }
    public static void CopyDataFromGrid()
    {
        var objects = FindObjectOfType<CustomGrid>().ObjectList;

        foreach (var item in objects)
        {
            items.Add(item.Value);
            Debug.Log("Adding to item: " + item.Value.name);
            //positionInScene.Add(item.Key);
        }
    }
}
