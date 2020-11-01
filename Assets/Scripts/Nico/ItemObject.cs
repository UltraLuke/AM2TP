using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : ScriptableObject
{
    public string myName;
    public string path; //Ruta del Prefab
    public Vector3 position;
    public Vector3 scale;
    public Quaternion rotation;

    public void Setter(string _name, string PrefabPath, Vector3 _position, Vector3 _scale, Quaternion _rotation)
    {
        myName = _name;
        path = PrefabPath;
        position = _position;
        scale = _scale;
        rotation = _rotation;
    }
}
