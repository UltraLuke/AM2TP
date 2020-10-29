﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUtils;
using System;

public class Grid<TGridObject>
{
    //public event EventHandler<OnGridValueChangedEventArgs> onGridValueChanged;
    //public class OnGridValueChangedEventArgs : EventArgs
    //{
    //    public int x;
    //    public int y;
    //}

    private int _width;
    private int _height;
    private int _length;
    private float _cellSize;
    private Vector3 _originPosition;
    private TGridObject[,] _gridArray;
    private TGridObject[,,] _gridArray3D;
    private TextMesh[,] _debugTextArray;

    //Properties
    public int Width => _width;
    public int Height => _height;
    public int Length => _length;

    /// <summary>
    /// Genero una grilla en el plano XY (2D) o XZ (3D).
    /// </summary>
    /// <param name="w">Ancho de la grilla (Cantidad de columnas)</param>
    /// <param name="h">Alto de la grilla (Cantidad de filas)</param>
    /// <param name="cellSize">Tamaño de cada celda (1 equivale a 1x1, 2 equivale a 2x2, etc.)</param>
    /// <param name="originPos">Punto de origen de la grilla</param>
    /// <param name="createGridObject">Función para crear objetos. Útil para generar meshes</param>
    public Grid(int w, int h, float cellSize, Vector3 originPos, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        _width = w;
        _height = h;
        _cellSize = cellSize;
        _originPosition = originPos;

        _gridArray = new TGridObject[w, h];

        //for (int x = 0; x < _gridArray.GetLength(0); x++)
        //{
        //    for (int y = 0; y < _gridArray.GetLength(1); y++)
        //    {
        //        _gridArray[x, y] = createGridObject(this, x, y);
        //    }
        //}

        //bool showDebug = false;
        //if (showDebug)
        //{
        //    //_debugTextArray = new TextMesh[w, h];

        //    for (int i = 0; i < _gridArray.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < _gridArray.GetLength(1); j++)
        //        {
        //            //_debugTextArray[i,j] = CustomUtilities.CreateWorldText(_gridArray[i, j]?.ToString(), null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);
        //            Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
        //            Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
        //        }
        //    }
        //    Debug.DrawLine(GetWorldPosition(0, h), GetWorldPosition(w, h), Color.white, 100f);
        //    Debug.DrawLine(GetWorldPosition(w, 0), GetWorldPosition(w, h), Color.white, 100f);

        //    //onGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) => {
        //    //    _debugTextArray[eventArgs.x, eventArgs.y].text = _gridArray[eventArgs.x, eventArgs.y]?.ToString();
        //    //};
        //}
    }

    /// <summary>
    /// Genero una grilla de 3 dimensiones, donde w refiere al eje X, l refiere al eje Z, y h refiere al eje Y
    /// </summary>
    /// <param name="w">Ancho de la grilla (Cantidad de columnas)</param>
    /// <param name="l">Largo de la grilla (Cantidad de filas)</param>
    /// <param name="h">Alto de la grilla (Cantidad de pisos)</param>
    /// <param name="cellSize">Tamaño de la celda (1 equivale a 1x1x1, 2 equivale a 2x2x2, etc.)</param>
    /// <param name="originPos">Punto de origen de la grilla</param>
    /// <param name="createGridObject">Función para crear objetos. Útil para generar meshes</param>
    public Grid(int w, int l, int h, float cellSize, Vector3 originPos, Func<int, int, int, TGridObject> createGridObject)
    {
        _width = w;
        _length = l;
        _height = h;
        _cellSize = cellSize;
        _originPosition = originPos;

        _gridArray3D = new TGridObject[w, l, h];

        for (int z = 0; z < _gridArray3D.GetLength(2); z++)
        {
            for (int x = 0; x < _gridArray3D.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray3D.GetLength(1); y++)
                {
                    _gridArray3D[x, y, z] = createGridObject(x, y, z);
                }
            }
        }

        //bool showDebug = true;
        //if (showDebug)
        //{
        //    //_debugTextArray = new TextMesh[w, h];

        //    //Itero por piso
        //    for (int k = 0; k < _gridArray3D.GetLength(2); k++)
        //    {
        //        //Itero por columna
        //        for (int i = 0; i < _gridArray3D.GetLength(0); i++)
        //        {
        //            //Itero por fila
        //            for (int j = 0; j < _gridArray3D.GetLength(1); j++)
        //            {

        //                //_debugTextArray[i, j] = CustomUtilities.CreateWorldText(_gridArray3D[i, j, k]?.ToString(), null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);
        //                Debug.DrawLine(GetWorldPosition(i, k, j), GetWorldPosition(i, k, j + 1), Color.white, 100f);
        //                Debug.DrawLine(GetWorldPosition(i, k, j), GetWorldPosition(i + 1, k, j), Color.white, 100f);

        //            }
        //        }
        //        Debug.DrawLine(GetWorldPosition(0, k, l), GetWorldPosition(w, k, l), new Color(255, 255, 255, .5f), 100f);
        //        Debug.DrawLine(GetWorldPosition(w, k, 0), GetWorldPosition(w, k, l), new Color(255, 255, 255, .5f), 100f);
        //        //Debug.Log(string.Format("{0}- Line 1: Position 1 = {1} | Position 2: {2}\n{0}- Line 2: Position 1 = {3} | Position 2: {4}",
        //        //    k, GetWorldPosition(0, k, h), GetWorldPosition(w, k, h), GetWorldPosition(w, k, 0), GetWorldPosition(w, k, h)));
        //    }

        //    //onGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) => {
        //    //    _debugTextArray[eventArgs.x, eventArgs.y].text = _gridArray3D[eventArgs.x, eventArgs.y, 0]?.ToString();
        //    //};
        //}
    }



    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }
    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * _cellSize + _originPosition;
    }
    public void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPos - _originPosition).y / _cellSize);
    }
    public void GetXYZ(Vector3 worldPos, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt((worldPos - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPos - _originPosition).y / _cellSize);
        z = Mathf.FloorToInt((worldPos - _originPosition).z / _cellSize);
    }

    //public void TriggerGridObjectChanged(int x, int y)
    //{
    //    if (onGridValueChanged != null) onGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
    //}

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            _gridArray[x, y] = value;

            //if (onGridValueChanged != null) onGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
            //_debugTextArray[x, y].text = _gridArray[x, y].ToString();
        }
    }
    public void SetGridObject(Vector3 worldPos, TGridObject value)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        SetGridObject(x, y, value);
    }
    public void SetGridObject3D(int x, int y, int z, TGridObject value)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < _width && y < _length && z < _height)
        {
            _gridArray3D[x, y, z] = value;

            //if (onGridValueChanged != null) onGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
            //_debugTextArray[x, y].text = _gridArray[x, y].ToString();
        }
    }
    public void SetGridObject3D(Vector3 worldPos, TGridObject value)
    {
        int x, y, z;
        GetXYZ(worldPos, out x, out y, out z);
        SetGridObject3D(x, y, z, value);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
            return _gridArray[x, y];
        else
            return default(TGridObject);
    }
    public TGridObject GetGridObject(Vector3 worldPos)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        return GetGridObject(x, y);
    }
    public TGridObject GetGridObject3D(int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < _width && y < _length && z < _height)
            return _gridArray[x, y];
        else
            return default(TGridObject);
    }
    public TGridObject GetGridObject3D(Vector3 worldPos)
    {
        int x, y, z;
        GetXYZ(worldPos, out x, out y, out z);
        return GetGridObject3D(x, y, z);
    }
}
