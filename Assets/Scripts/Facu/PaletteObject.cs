using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewPalette", menuName ="LevelEditor/NewPalette")]
public class PaletteObject : ScriptableObject
{
    public List<GameObject> content = new List<GameObject>();
    public string paletteName;

    public PaletteObject SetName(string name)
    {
        this.paletteName = name;
        return this;
    }

    public void AddObject(GameObject gameObject)
    {
        if (content.Contains(gameObject))
            return;
        content.Add(gameObject);
    }

    public void RemoveObject(GameObject gameObject)
    {
        if (content.Contains(gameObject))
            return;
        content.Remove(gameObject);
    }
}
