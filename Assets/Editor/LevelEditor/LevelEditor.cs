using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow
{
    int tabsSelected = 0;
    List<Texture2D> textures = new List<Texture2D>();

    List<Pallete> palletes = new List<Pallete>();
    Vector2 scrollPosition;

    private GameObject areaToCreate;


    [MenuItem("CustomTools/LevelEditor")]
    public static void OpenWindow()
    {
        var myWindow = GetWindow<LevelEditor>();
        myWindow.minSize = new UnityEngine.Vector2(600, 300);
        myWindow.wantsMouseMove = true;
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
        tabsSelected = GUILayout.Toolbar(tabsSelected, GetPalletesName(palletes));
        EditorGUILayout.EndHorizontal();

        MakeWindow(tabsSelected);
    }

    void MakeWindow(int palletIndex)
    {
        textures.Clear();

        var auxPallete = palletes[palletIndex];


        if (auxPallete.prefabsList != null)
        {
            var prefabListCopy = auxPallete.prefabsList.GetRange(0, auxPallete.prefabsList.Count);

            do
            {

                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < 4; i++)
                {
                    if (prefabListCopy.Count == 0)
                        break;

                    GUILayout.BeginVertical("Box");
                    //GUI.DrawTexture(GUILayoutUtility.GetRect(50, 50), (Texture2D)Resources.Load("unity-128"), ScaleMode.ScaleToFit);
                    GUILayout.Label(prefabListCopy[0].prefabName);
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

    private string[] GetPalletesName(List<Pallete> palletes)
    {
        var namesTabs = palletes.Select(x => x.Name).ToArray();

        return namesTabs;
    }

    private List<Pallete> GetPalletes()
    {
        var auxpalletes = new List<Pallete>();
        var pallete = new Pallete();

        pallete.Name = "Floors";
        pallete.prefabsList = GetPalletePrefabs(pallete.Name, 15);
        auxpalletes.Add(pallete);

        pallete = new Pallete();
        pallete.Name = "Buildings";
        pallete.prefabsList = GetPalletePrefabs(pallete.Name, 2);
        auxpalletes.Add(pallete);

        pallete = new Pallete();
        pallete.Name = "Nueva Paleta";
        pallete.prefabsList = null;
        auxpalletes.Add(pallete);

        return auxpalletes;
    }

    private List<PalletePrefab> GetPalletePrefabs(string namePallete, int count)
    {
        var auxPrefabPalletes = new List<PalletePrefab>();

        for (int i = 0; i < count; i++)
        {
            PalletePrefab palletePrefab = new PalletePrefab();
            palletePrefab.prefabName = namePallete + "_" + i;
            auxPrefabPalletes.Add(palletePrefab);
        }
        return auxPrefabPalletes;
    }


}

public class Pallete
{
    public string Name;
    public List<PalletePrefab> prefabsList;
}

public class PalletePrefab
{
    public string prefabName;
}
