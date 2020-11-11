using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SaveTester", menuName ="LevelEditor/Tester")]
public class SaveTester : ScriptableObject
{
    public string paletteName;
    public List<GameObject> objectList;

    public void CreatePalette()
    {
        PaletteManager.CreatePalette(paletteName);
    }

    public void AddObjectListToPalette()
    {
        var palette = PaletteManager.LoadPalette(paletteName);

        foreach (var item in objectList)
        {
            palette.AddObject(item);
        }
    }
}
