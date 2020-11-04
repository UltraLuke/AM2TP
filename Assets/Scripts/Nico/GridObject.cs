using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : ScriptableObject
{
    public int row;
    public int column;
    public Vector3 origin;
    public void ConstructGrid(int _row, int _column, Vector3 _origin)
    {
        row = _row;
        column = _column;
        origin = _origin;
    }
    //Deberiamos guardar data sobre los espacios marcados como ocupados
}
