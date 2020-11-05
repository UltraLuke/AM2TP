using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GridTester))]
public class GridTesterEditor : Editor
{
    GridTester gridTester;
    LayerMask layerMask;
    GameObject currObj;

    Plane _plane;

    CustomGrid _grid;

    private void OnEnable()
    {
        gridTester = (GridTester)target;

        _grid = FindObjectOfType<CustomGrid>();
        layerMask = gridTester.layerMask;
        currObj = gridTester.currObj;
        _plane = new Plane(Vector3.up, Vector3.zero);
        SceneView.RepaintAll();
    }

    public override void OnInspectorGUI()
    {
        gridTester.layerMask = layerMask = EditorGUILayout.LayerField("Plano de referencia", layerMask);
        gridTester.currObj = currObj = (GameObject)EditorGUILayout.ObjectField("Objeto actual", currObj, typeof(GameObject), false);


        CheckKeys();
    }

    private void CheckKeys()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var objects = _grid.ObjectList;

            foreach (var item in objects)
            {
                Debug.Log(item.Key + " | " + item.Value);
            }
        }
    }

    private void OnSceneGUI()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (_plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                var obj = (GameObject)PrefabUtility.InstantiatePrefab(currObj);
                _grid.SetObjectOnGrid(obj, hitPoint);
            }
            
        }
        _grid.CleanEmptyReferences();

    }

    public void DrawGrid()
    {
        //if (gridTester == null) return;

        //var extColor = new Color(0, 1, 0, .5f);
        //var intColor = new Color(1, 1, 1, .25f);

        ////var grid = _gridTester.Grid;

        //for (int k = 0; k < 1; k++)
        //{
        //    //Itero por columna
        //    for (int i = 0; i < _gridTester.columns; i++)
        //    {
        //        //Itero por fila
        //        for (int j = 0; j < _gridTester.rows; j++)
        //        {
        //            if (i == 0) Handles.color = extColor;
        //            else Handles.color = intColor;

        //            Handles.DrawLine(_grid.GetWorldPosition(i, k, j), _grid.GetWorldPosition(i, k, j + 1));

        //            if (j == 0) Handles.color = extColor;
        //            else Handles.color = intColor;
        //            Handles.DrawLine(_grid.GetWorldPosition(i, k, j), _grid.GetWorldPosition(i + 1, k, j));
        //        }
        //    }

        //    Handles.color = extColor;
        //    Handles.DrawLine(_grid.GetWorldPosition(0, k, _gridTester.rows), _grid.GetWorldPosition(_gridTester.columns, k, _gridTester.rows));
        //    Handles.DrawLine(_grid.GetWorldPosition(_gridTester.columns, k, 0), _grid.GetWorldPosition(_gridTester.columns, k, _gridTester.rows));
        //}
    }
}