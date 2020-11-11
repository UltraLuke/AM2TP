using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using System.IO;
using Object = UnityEngine.Object;
using UnityEngine.UI;
using System;
using System.Runtime.Remoting.Messaging;

public class GridLoader : EditorWindow
{
    private string _FolderName;

    private GUIStyle _myStyle;

    public GameObject grid;

    private bool folderError = false;
    private bool drawItems = false;

    Vector2 scrollPos;

    public class CustomLoadObject
    {
        public string name;
        public ScriptableObject myObj;
        public bool selected;
        public CustomLoadObject(string _name, ScriptableObject so, bool _selected)
        {
            name = _name;
            myObj = so;
            selected = _selected;
        }
    }

    List<CustomLoadObject> _CustomLoad= new List<CustomLoadObject>();

    //[MenuItem("CustomTools/CustomGrid/Load")]
    //public static void OpenWindow()
    //{
    //    var loadWindow = GetWindow<GridLoader>();
    //    loadWindow.wantsMouseMove = true;
        
    //    loadWindow._myStyle = new GUIStyle
    //    {
    //        fontStyle = FontStyle.BoldAndItalic,
    //        fontSize = 12,
    //        alignment = TextAnchor.MiddleLeft,
    //        wordWrap = true
    //    };
        
    //    loadWindow.Show();
    //}

    private void OnEnable()
    {
        _myStyle = new GUIStyle
        {
            fontStyle = FontStyle.BoldAndItalic,
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft,
            wordWrap = true
        };
    }
    private void OnGUI()
    {        
        _FolderName = EditorGUILayout.TextField("Folder Name: ", _FolderName);        
        if (GUILayout.Button("Load Full"))
        {
            if (AssetDatabase.IsValidFolder("Assets/Resources/" + _FolderName))
            {
                folderError = false;
                Load(_FolderName);                
            }
            else
                folderError = true;

        }
        if (folderError)
            EditorGUILayout.HelpBox("La carpeta no existe", MessageType.Error);

        EditorGUILayout.LabelField("Path: Assets/Resources/" + _FolderName, GUILayout.Width(250));

        EditorGUILayout.Space();

        if(GUILayout.Button("Custom Load"))
        {
            if (AssetDatabase.IsValidFolder("Assets/Resources/" + _FolderName))
            {
                folderError = false;
                drawItems = true;
            }
            else
                folderError = true;
        }

        if (drawItems)
        {
            if(_CustomLoad.Count == 0)
                GenerateCustomList(_FolderName);

            ShowInsideFolder();

            if(GUILayout.Button("Load Selection"))
            {
                CustomLoader();
            }
        }
    }
    private void CustomLoader()
    {
        foreach (var item in _CustomLoad)
        {
            //Debug.Log("Item: " + item.name + " selected?: " + item.selected);
            if (item.selected)
            {
                if (item.myObj is GridObject)
                    CreateGrid((GridObject)item.myObj);
            }
        }
        foreach (var item in _CustomLoad)
        {
            if (item.selected)
            {
                if (item.myObj is ItemObject)
                    CreateItem((ItemObject)item.myObj);
            }
        }
    }
    private void GenerateCustomList(string folderName)
    {
        ScriptableObject[] content = Resources.LoadAll<ScriptableObject>(folderName + "/");
        
        foreach (var item in content)
        {
            CustomLoadObject newObj = new CustomLoadObject(item.name, item, false);
            _CustomLoad.Add(newObj);
        }
    }

    private void ShowInsideFolder()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        foreach (var item in _CustomLoad)
        {
            EditorGUILayout.BeginVertical();
            item.selected = EditorGUILayout.Toggle(item.name, item.selected);
            EditorGUILayout.EndVertical();

            //Debug.Log("Escriptable: " + item.name + " selected: " + item.selected);
        }
        EditorGUILayout.EndScrollView();
    }
    private void Load(string folderName)
    {
        GridObject[] GridData = Resources.LoadAll<GridObject>(folderName + "/");
        foreach (var item in GridData)
        {
            CreateGrid(item);
        }

        ItemObject[] ItemData = Resources.LoadAll<ItemObject>(folderName + "/");
        foreach (var item in ItemData)
        {
            CreateItem(item);
        }
        EditorGUILayout.HelpBox("Complete", MessageType.Info);
    }
    private void CreateGrid(GridObject _grid)
    {   
        grid = new GameObject(_grid.name, typeof(CustomGrid)/*, typeof(GridTester)*/);
        grid.GetComponent<CustomGrid>().Size = _grid.size;
    }
    private void CreateItem(ItemObject item)
    {
        //Debug.Log("Creating obj of path: " + item.path);

        string path = item.path;
        Object go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
        //Debug.Log("Path found: " + go);

        GameObject objInScene = (GameObject)PrefabUtility.InstantiatePrefab(go);
        //Debug.Log("Prefab instantiate: " + objInScene);
        objInScene.transform.rotation = item.rotation;
        objInScene.transform.localScale = item.scale;
        objInScene.transform.position = item.position;

        AddItemToGrid(objInScene);
    }
    private void AddItemToGrid(GameObject item)
    {
        //Debug.Log("Adding to Grid: " + item);
        grid.GetComponent<CustomGrid>().SetObjectOnGrid(item, item.transform.position);
    }
}
