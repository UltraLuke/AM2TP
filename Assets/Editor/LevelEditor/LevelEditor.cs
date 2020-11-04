using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow
{
    string[] options = { "Palletes", "New Pallete" };
    int tabsSelected = 0;
    int palleteSelected = 0;
    List<Texture2D> textures = new List<Texture2D>();

    List<PaletteObject> palletes = new List<PaletteObject>();
    Vector2 scrollPosition;

    private GameObject areaToCreate;

    private List<Object> _assetList;
    private Vector2 _scroolposition = Vector2.zero;

    bool selectPrefabBtn;

    static bool[] select;

    private List<GameObject> newPalletePrefab = new List<GameObject>();

    private string  palleteName = "";

    [MenuItem("CustomTools/LevelEditor")]
    public static void OpenWindow()
    {
        var myWindow = GetWindow<LevelEditor>();
        myWindow.minSize = new UnityEngine.Vector2(600, 300);
        myWindow.wantsMouseMove = true;

        string[] paths = AssetDatabase.FindAssets("t:prefab");
        select = new bool[paths.Length];


        myWindow.Show();
    }

    private void OnGUI()
    {
        palletes = GetPalletes();

        DrawHeader();
    }

    private void DrawHeader()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Level Editor", GUILayout.Width(75));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        tabsSelected = GUILayout.Toolbar(tabsSelected, options);
        EditorGUILayout.EndHorizontal();

        if (tabsSelected == 0)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Class");
            palleteSelected = EditorGUILayout.Popup(palleteSelected, GetPalletesName(palletes));
            GUILayout.EndVertical();
            MakeWindow(palleteSelected);
        }

        if (tabsSelected == 1)
            NewPallete();
    }

    void MakeWindow(int palletIndex)
    {
        textures.Clear();

        var auxPallete = palletes[palletIndex];



        if (auxPallete.content != null)
        {
            var prefabListCopy = auxPallete.content.GetRange(0, auxPallete.content.Count);

            do
            {

                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < 4; i++)
                {
                    if (prefabListCopy.Count == 0)
                        break;

                    GUILayout.BeginVertical("Box");
                    //GUI.DrawTexture(GUILayoutUtility.GetRect(50, 50), (Texture2D)Resources.Load("unity-128"), ScaleMode.ScaleToFit);
                    GUILayout.Label(prefabListCopy[0].name);
                    Texture2D newTexture = Resources.Load("unity-128", typeof(Texture2D)) as Texture2D;
                    GUILayout.Button(newTexture, GUILayout.Width(100), GUILayout.Height(100));
                    prefabListCopy.RemoveAt(0);
                    GUILayout.EndVertical();

                }
                EditorGUILayout.EndHorizontal();
            }
            while (prefabListCopy.Count != 0);
        }
    }

    private string[] GetPalletesName(List<PaletteObject> palletes)
    {
        var namesTabs = palletes.Select(x => x.name).ToArray();

        return namesTabs;
    }

    private List<PaletteObject> GetPalletes()
    {
        var auxpalletes = new List<PaletteObject>();
        var pallete = new PaletteObject();

        pallete.name = "Floors";
        pallete.content = GetPalletePrefabs(pallete.name, 15);
        auxpalletes.Add(pallete);

        pallete = new PaletteObject();
        pallete.name = "Buildings";
        pallete.content = GetPalletePrefabs(pallete.name, 2);
        auxpalletes.Add(pallete);

        return auxpalletes;
    }

    private List<GameObject> GetPalletePrefabs(string namePallete, int count)
    {
        var auxPrefabPalletes = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject palletePrefab = new GameObject();
            palletePrefab.name = namePallete + "_" + i;
            auxPrefabPalletes.Add(palletePrefab);
        }
        return auxPrefabPalletes;
    }



    private void NewPallete()
    {
        EditorGUILayout.LabelField("New Pallete");

        
        palleteName = EditorGUILayout.TextField("Pallete Name", palleteName);

        DrawLine();

        foreach (var auxenemy in newPalletePrefab)
        {
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();

            GUILayout.Label(auxenemy.name.ToString());

            GUILayout.EndHorizontal();
            GUILayout.Space(1);
            GUILayout.EndVertical();
        }

        EditorGUILayout.LabelField("Proyect Assets:");

        _assetList = new List<Object>();
        _assetList.Clear();
        //AssetDatabase.FindAssets me retorna todos los paths de los assets que coinciden con el parámetro, en formato GUID

        string[] paths = AssetDatabase.FindAssets("t:prefab");


        for (int i = 0; i < paths.Length; i++)
        {
            //Convierto el GUID al formato "normal"
            paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);

            //cargo el asset en memoria
            var loaded = AssetDatabase.LoadAssetAtPath(paths[i], typeof(Object));

            _assetList.Add(loaded);
        }

        if (_assetList.Count > 0)
        {
            var prefabListCopy = _assetList.GetRange(0, _assetList.Count);

            do
            {
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < 4; i++)
                {
                    if (prefabListCopy.Count == 0)
                        break;

                    GUILayout.BeginVertical("Box");
                    //GUI.DrawTexture(GUILayoutUtility.GetRect(50, 50), (Texture2D)Resources.Load("unity-128"), ScaleMode.ScaleToFit);
                    GUILayout.Label(prefabListCopy[0].name);
                    Texture2D newTexture = Resources.Load(prefabListCopy[0].name, typeof(Texture2D)) as Texture2D;
                    var texture = AssetPreview.GetAssetPreview(prefabListCopy[0]);
                    if (GUILayout.Button(texture, GUILayout.Width(100), GUILayout.Height(100)))
                    {
                        GameObject gameObject = Resources.Load(prefabListCopy[0].name, typeof(GameObject)) as GameObject;

                        if (!newPalletePrefab.Contains(gameObject))
                        {
                            newPalletePrefab.Add(gameObject);
                        }
                    }

                    prefabListCopy.RemoveAt(0);
                    GUILayout.EndVertical();

                }
                EditorGUILayout.EndHorizontal();
            }
            while (prefabListCopy.Count != 0);
        }

        if (GUILayout.Button("Save Pallete", GUILayout.Width(100), GUILayout.Height(20)))
        {
            if (palleteName == string.Empty)
                return;

            PaletteManager.CreatePalette(palleteName);

            var pallete = PaletteManager.LoadPalette(palleteName);

            foreach (GameObject aux in newPalletePrefab)
                pallete.AddObject(aux);

            palleteName = "";
            newPalletePrefab.Clear();

        }
        if (GUILayout.Button("Clear Pallete", GUILayout.Width(100), GUILayout.Height(20)))
        {
            palleteName = "";
            newPalletePrefab.Clear();
            Repaint();
        }

    }

    private static void DrawLine()
    {
        GUILayout.Space(5);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
        GUILayout.Space(5);
    }
}

