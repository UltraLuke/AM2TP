using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GridTester))]
public class GridTesterEditor : Editor
{
    GridTester gridTester;
    GameObject currObj;
    GameObject _lastCurrObj;
    CustomGrid _grid;
    LayerMask layerMask;
    Plane _plane;

    GameObject _selectedObject;

    bool _canReplaceObjects = false;

    string[] _modeTabs = { "Create", "Edit", "Delete" };
    int _tabSelection = 0;

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


        _canReplaceObjects = EditorGUILayout.Toggle("Can Replace Objects", _canReplaceObjects);
        _tabSelection = GUILayout.Toolbar(_tabSelection, _modeTabs);

        //Si entro al modo edicion instancio el objeto de muestra del objeto actual.
        if(_tabSelection == 0)
        {
            if (_lastCurrObj != currObj)
            {
                DestroyEditingObject();
                _lastCurrObj = currObj;
            }

            if (_selectedObject == null)
                _selectedObject = (GameObject)PrefabUtility.InstantiatePrefab(currObj);
        }
        //Si salgo del modo edicion, dejo de mostrar el objeto muestra.
        else
        {
            DestroyEditingObject();
        }

        CheckKeys();
        _grid.CleanEmptyReferences();
    }

    private void DestroyEditingObject()
    {
        if (_selectedObject != null)
        {
            DestroyImmediate(_selectedObject);
            _selectedObject = null;
        }
    }

    private void OnSceneGUI()
    {
        Event e = Event.current;

        if (_tabSelection == 0)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                if (_plane.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);

                    if (_canReplaceObjects || _grid.CheckIfAvailablePosition(hitPoint))
                    {
                        var obj = (GameObject)PrefabUtility.InstantiatePrefab(currObj);
                        _grid.SetObjectOnGrid(obj, hitPoint);
                    }
                }
            }
            else if (e.type == EventType.MouseMove)
            {
                if (_selectedObject != null)
                {
                    Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                    if (_plane.Raycast(ray, out float enter))
                    {
                        Vector3 hitPoint = ray.GetPoint(enter);

                        var pointToPlace = _grid.GetNearestPointOnGrid(hitPoint);
                        _selectedObject.transform.position = pointToPlace;
                    }
                }
            }
        }
        else if (_tabSelection == 2)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                if (_plane.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    _grid.DeleteObject(hitPoint);
                }
            }
        }
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