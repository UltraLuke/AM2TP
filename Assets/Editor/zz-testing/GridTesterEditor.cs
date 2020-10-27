using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GridTester))]
public class GridTesterEditor : Editor
{
    public int rows;
    public int columns;
    public float cellSize;
    public Vector3 origPos;

    GridTester _gridTester;
    Grid<GameObject> _grid;

    private void OnEnable()
    {
        _gridTester = (GridTester)target;

        _gridTester.GenerateNewGrid();
        _grid = _gridTester.Grid;

        rows = _gridTester.rows;
        columns = _gridTester.columns;
        cellSize = _gridTester.cellSize;
        origPos = _gridTester.origPos;

        SceneView.RepaintAll();
    }

    public override void OnInspectorGUI()
    {
        rows = EditorGUILayout.IntField("Rows", rows);
        columns = EditorGUILayout.IntField("Columns", columns);
        cellSize = EditorGUILayout.FloatField("Cell Size", cellSize);
        origPos = EditorGUILayout.Vector3Field("Origin Point", origPos);

        if (GUILayout.Button("Generate new grid"))
        {
            _gridTester.rows = rows;
            _gridTester.columns = columns;
            _gridTester.cellSize = cellSize;
            _gridTester.origPos = origPos;

            _gridTester.GenerateNewGrid();
            _grid = _gridTester.Grid;
            SceneView.RepaintAll();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }

    private void OnSceneGUI()
    {
        DrawGrid();
        //Repaint();
    }

    public void DrawGrid()
    {
        if (_gridTester == null || _gridTester.Grid == null) return;

        var extColor = new Color(0, 1, 0, .5f);
        var intColor = new Color(1, 1, 1, .25f);

        //var grid = _gridTester.Grid;

        for (int k = 0; k < 1; k++)
        {
            //Itero por columna
            for (int i = 0; i < _gridTester.columns; i++)
            {
                //Itero por fila
                for (int j = 0; j < _gridTester.rows; j++)
                {
                    if (i == 0) Handles.color = extColor;
                    else Handles.color = intColor;

                    Handles.DrawLine(_grid.GetWorldPosition(i, k, j), _grid.GetWorldPosition(i, k, j + 1));

                    if (j == 0) Handles.color = extColor;
                    else Handles.color = intColor;
                    Handles.DrawLine(_grid.GetWorldPosition(i, k, j), _grid.GetWorldPosition(i + 1, k, j));
                }
            }

            Handles.color = extColor;
            Handles.DrawLine(_grid.GetWorldPosition(0, k, _gridTester.rows), _grid.GetWorldPosition(_gridTester.columns, k, _gridTester.rows));
            Handles.DrawLine(_grid.GetWorldPosition(_gridTester.columns, k, 0), _grid.GetWorldPosition(_gridTester.columns, k, _gridTester.rows));
        }
    }
}
