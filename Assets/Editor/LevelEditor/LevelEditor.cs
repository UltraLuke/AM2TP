using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow
{
    Vector2 scrollPos;

    string[] options = { "Ver Palletes", "Nueva Pallete" };
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

    private string palleteName = "";

    private static float withWindow = 600f;
    private static float heightWindow = 400f;

    private GridToolWindow _gridToolWindow;

    [MenuItem("CustomTools/PalleteEditor")]
    public static void OpenWindow()
    {
        var myWindow = GetWindow<LevelEditor>();

        myWindow.minSize = new UnityEngine.Vector2(withWindow, heightWindow + 50);
        myWindow.wantsMouseMove = true;

        string[] paths = AssetDatabase.FindAssets("t:prefab");
        select = new bool[paths.Length];

        myWindow.Show();

        myWindow._gridToolWindow = GetWindow<GridToolWindow>();
        myWindow._gridToolWindow.Show();
    }

    private void OnGUI()
    {
        palletes = GetPalletes();

        DrawHeader();
    }

    private void OnDisable()
    {
        if(_gridToolWindow != null)
            _gridToolWindow.Close();
    }

    private void DrawHeader()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Pallete Editor", GUILayout.Width(75));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        tabsSelected = GUILayout.Toolbar(tabsSelected, options);
        EditorGUILayout.EndHorizontal();

        if (tabsSelected == 0)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Seleccionar Pallete");
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

        //scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(withWindow), GUILayout.Height(heightWindow));

        if (auxPallete.content != null)
        {
            var prefabListCopy = auxPallete.content.ToList().GetRange(0, auxPallete.content.Count);

            do
            {

                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < 4; i++)
                {
                    if (prefabListCopy.Count == 0)
                        break;

                    GUILayout.BeginVertical("Box");
                    GUILayout.Label(prefabListCopy[0].name);
                    var newTexture = AssetPreview.GetAssetPreview(prefabListCopy[0]);
                    if (GUILayout.Button(newTexture, GUILayout.Width(100), GUILayout.Height(100)))
                    {

                        var gameObject = FindGameObjectInProject(prefabListCopy[0]);

                        if (gameObject is null)
                            Debug.Log("IS NULL");

                        if(_gridToolWindow == null)
                        {
                            _gridToolWindow = GetWindow<GridToolWindow>();
                            _gridToolWindow.Show();
                        }

                        _gridToolWindow.CurrObj = gameObject;

                    }
                    prefabListCopy.RemoveAt(0);
                    GUILayout.EndVertical();

                }
                EditorGUILayout.EndHorizontal();
            }
            while (prefabListCopy.Count != 0);
        }

        //EditorGUILayout.EndScrollView();
    }

    private string[] GetPalletesName(List<PaletteObject> palletes)
    {
        var namesTabs = palletes.Select(x => x.name).ToArray();

        return namesTabs;
    }

    private List<PaletteObject> GetPalletes()
    {
        var auxpalletes = new List<PaletteObject>();


        var getPalettes = PaletteManager.GetPalettes();

        if (getPalettes.Length == 0)
            return new List<PaletteObject>() { new PaletteObject() { name = "Debe Crear un paleta" } };

        for (int x = 0; x < getPalettes.Length; x++)
        {
            auxpalletes.Add(getPalettes[x]);
        }

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
        EditorGUILayout.LabelField("Nueva Pallete");

        var boldtext = new GUIStyle(GUI.skin.label);
        boldtext.fontStyle = FontStyle.Bold;

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

        string[] paths = AssetDatabase.FindAssets("t:prefab", new[] { "Assets/Resources" });

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(withWindow), GUILayout.Height(heightWindow));

        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
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
                    GUILayout.Label(prefabListCopy[0].name, boldtext);
                    var texture = AssetPreview.GetAssetPreview(prefabListCopy[0]);
                    var description = GetParentFolder(Path.GetDirectoryName(AssetDatabase.GetAssetPath(prefabListCopy[0])));

                    description = description is null ? "Resources" : description;

                    GUILayout.Label("Folder: " + description);

                    if (GUILayout.Button(texture, GUILayout.Width(100), GUILayout.Height(100)))
                    {

                        var gameobject = FindGameObjectInProject(prefabListCopy[0]);

                        if (!newPalletePrefab.Contains(gameobject) && gameobject != null)
                        {
                            newPalletePrefab.Add(gameobject);
                        }
                    }

                    prefabListCopy.RemoveAt(0);
                    GUILayout.EndVertical();

                }
                EditorGUILayout.EndHorizontal();
            }
            while (prefabListCopy.Count != 0);
        }

        EditorGUILayout.EndScrollView();
        DrawLine();

        if (GUILayout.Button("Guardar Paleta", GUILayout.Width(100), GUILayout.Height(20)))
        {
            if (palleteName == string.Empty)
                return;

            PaletteManager.CreatePalette(palleteName);

            var pallete = PaletteManager.LoadPalette(palleteName);

            foreach (GameObject aux in newPalletePrefab)
                pallete.AddObject(aux);

            palleteName = "";
            newPalletePrefab.Clear();
            Repaint();

        }
        if (GUILayout.Button("Limpiar Paleta", GUILayout.Width(100), GUILayout.Height(20)))
        {
            palleteName = "";
            newPalletePrefab.Clear();
            Repaint();
        }


    }

    private GameObject FindGameObjectInProject(Object prefabObject)
    {
        string path = AssetDatabase.GetAssetPath(prefabObject);
        string pathAsset = "";
        if (File.Exists(path))
        {
            path = Path.GetDirectoryName(path);

            if (GetParentFolder(path) != null)
                pathAsset = GetParentFolder(path) + "/" + prefabObject.name;
            else
                pathAsset = prefabObject.name;

            GameObject gameObject = Resources.Load(pathAsset, typeof(GameObject)) as GameObject;

            return gameObject;
        }

        return null;
    }

    private string GetParentFolder(string path)
    {
        var pathSplitter = path.Split(Path.DirectorySeparatorChar);

        if (pathSplitter[pathSplitter.Length - 1] == "Resources")
            return null;
        else
            return pathSplitter[pathSplitter.Length - 1];
    }

    private bool Validation(string name)
    {
        var validate = true;
        if (name == string.Empty)
        {
            validate = false;
            EditorGUILayout.HelpBox("The Pallete name cannot be empty", MessageType.Error);
        }


        return validate;
    }

    private static void DrawLine()
    {
        GUILayout.Space(5);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(100, 2), Color.black);
        GUILayout.Space(5);
    }
}

