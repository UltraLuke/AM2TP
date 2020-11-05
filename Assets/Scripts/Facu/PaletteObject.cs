using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewPalette", menuName ="LevelEditor/NewPalette")]
public class PaletteObject : ScriptableObject
{
    public Dictionary<GameObject, string> content = new Dictionary<GameObject, string>();
    public string paletteName;

    public PaletteObject SetName(string name)
    {
        this.paletteName = name;
        return this;
    }

    public void AddObject(GameObject gameObject, string description)
    {
        if (content.ContainsKey(gameObject))
            return;
        content.Add(gameObject, description);
    }

    public void RemoveObject(GameObject gameObject)
    {
        if (content.ContainsKey(gameObject))
            return;
        content.Remove(gameObject);
    }
}
