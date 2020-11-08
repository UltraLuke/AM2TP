using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : ScriptableObject
{
    /*
    public int row;
    public int column;
    */
    public float size;
    public Vector3 origin;

    public void Setter(float _size, Vector3 _origin)
    {
        size = _size;
        origin = _origin;
    }
}
