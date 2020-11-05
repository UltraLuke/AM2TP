using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class PaletteManager
{
    public static void CreatePalette(string paletteName)
    {
        string path = string.Format("Assets/Resources/Palettes/{0}.asset", paletteName);

        AssetDatabase.CreateAsset(
            ScriptableObject.CreateInstance<PaletteObject>()
            .SetName(paletteName), path);
    }

    public static void DeletePalette(string paletteName)
    {
        string path = string.Format("Assets/Resources/Palettes/{0}.asset", paletteName);
        AssetDatabase.DeleteAsset(path);
    }

    //TODO: establecer checks en caso de que no esté la carpeta
    public static PaletteObject LoadPalette(string paletteName)
    {
        return Resources.Load<PaletteObject>("Palettes/" + paletteName);
    }

    public static PaletteObject[] GetPalettes()
    {
        return Resources.LoadAll<PaletteObject>("Palettes");
    }

    public static PaletteObject LoadPalette(string paletteName, string path)
    {
        new NotImplementedException();
    }
}
