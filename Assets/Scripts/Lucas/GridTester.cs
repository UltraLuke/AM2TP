using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTester : MonoBehaviour
{
    public int rows;
    public int columns;
    //public int floors;
    public float cellSize;
    public Vector3 origPos;


    Grid<GameObject> _grid;

    public Grid<GameObject> Grid { get => _grid; }

    private void Start()
    {
        GenerateNewGrid();
    }

    public void GenerateNewGrid()
    {
        _grid = new Grid<GameObject>(columns, rows, 1, cellSize, origPos, (int x, int y, int z) => null);
    }

    
}
