﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewPalette", menuName ="LevelEditor/NewPalette")]
public class PaletteObject : ScriptableObject
{
    public List<GameObject> content = new List<GameObject>();
    public List<string> contentString = new List<string>();
    public string paletteName;

    public PaletteObject SetName(string name)
    {
        this.paletteName = name;
        return this;
    }

    public void AddObject(GameObject gameObject)
    {
        content.Add(gameObject);
    }

    public void AddString(string description)
    {
        contentString.Add(description);
    }

    public void RemoveObject(GameObject gameObject, string description)
    {
        content.Remove(gameObject);
        contentString.Remove(description);
    }
}
