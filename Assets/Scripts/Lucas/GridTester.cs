using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTester : MonoBehaviour
{
    public int rows;
    public int columns;
    public int floors;
    public float cellSize;
    public Vector3 origPos;


    Grid<GameObject> _grid;

    private void Start()
    {
        _grid = new Grid<GameObject>(columns, rows, floors, cellSize, origPos, (Grid<GameObject> g, int x, int y, int z) => null);
    }
}
