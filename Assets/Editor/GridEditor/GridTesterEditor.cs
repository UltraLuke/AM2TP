using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GridTester))]
public class GridTesterEditor : Editor
{
    GridTester gridTester;
    GameObject currObj;
    LayerMask layerMask;
    float cellSize;

    GameObject _lastCurrObj;
    CustomGrid _grid;
    Plane _plane;
    GameObject _selectedObject;
    Vector3 _lastSelectedPos = Vector3.zero;

    bool _canReplaceObjects = false;

    string[] _modeTabs = { "Create", "Edit", "Delete" };
    int _tabSelection = 0;

    //Flags
    bool _movingObject = false;

    private void OnEnable()
    {
        gridTester = (GridTester)target;
        _grid = FindObjectOfType<CustomGrid>();

        layerMask = gridTester.layerMask;
        currObj = gridTester.currObj;
        cellSize = _grid.size;
        _plane = new Plane(Vector3.up, Vector3.zero);

        SceneView.RepaintAll();
    }

    private void OnDisable()
    {
        if (_selectedObject != null)
            DestroyImmediate(_selectedObject);
    }

    public override void OnInspectorGUI()
    {
        //gridTester.layerMask = layerMask = EditorGUILayout.LayerField("Plano de referencia", layerMask);
        if (!_movingObject)
        {
            cellSize = _grid.size = EditorGUILayout.FloatField("Cell Size", cellSize);
            gridTester.currObj = currObj = (GameObject)EditorGUILayout.ObjectField("Objeto actual", currObj, typeof(GameObject), false);
            _canReplaceObjects = EditorGUILayout.Toggle("Can Replace Objects", _canReplaceObjects);
            _tabSelection = GUILayout.Toolbar(_tabSelection, _modeTabs);
        }
        else
        {
            EditorGUILayout.HelpBox("No se puede acceder a los controles mientras se está moviendo un objeto", MessageType.Info);
        }



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
        else if(_tabSelection == 1)
        {
            if(!_movingObject)
                DestroyEditingObject();
        }
        else
        {
            DestroyEditingObject();
        }

        CheckKeys();
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

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        //Modo creacion
        if (_tabSelection == 0)
        {
            //Cuando detecto un click del mouse, creo un objeto en el lugar.
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
            //Sino, updateo la posicion del objeto de muestra.
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
        //Modo edicion
        else if (_tabSelection == 1)
        {
            //Si hago click, puedo realizar dos acciones mencionadas abajo
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                if (_plane.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);

                    var obj = _grid.GetObjectOnGrid(hitPoint);

                    //Si no estoy moviendo un objeto, obtengo dicho objeto y lo uso como objeto de muestra.
                    //Es decir, empiezo a mover un objeto
                    if (!_movingObject && obj != null)
                    {
                        _selectedObject = _grid.GetObjectOnGrid(hitPoint);
                        _lastSelectedPos = _grid.GetNearestPointOnGrid(hitPoint);
                        _movingObject = true;
                    }
                    //Si estoy moviendo un objeto, dejo el mismo en la posicion donde cliquie.
                    //Dejo de mover el objeto.
                    else if (_movingObject)
                    {
                        if(obj == null)
                        {
                            _grid.MoveObject(_lastSelectedPos, hitPoint);
                            _selectedObject = null;
                        }
                        else
                        {
                            _selectedObject.transform.position = _grid.GetNearestPointOnGrid(_lastSelectedPos);
                            _selectedObject = null;
                            //_grid.SetObjectOnGrid(obj, _lastSelectedPos);
                        }

                        _grid.CleanEmptyReferences();
                        SceneView.RepaintAll();

                        _movingObject = false;
                    }
                    Repaint();
                }

            }
            //Si estoy moviendo un objeto, updateo la posicion de dicho objeto, sin linkearlo a la grilla.
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
        //Modo delete
        else if (_tabSelection == 2)
        {
            //Si hago click, elimina el objeto de dicha posicion.
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

        DrawGrid();
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

        _grid.CleanEmptyReferences();

        Color yellow = Color.yellow;
        Color red = Color.red;
        int identity = 0;

        Color current = yellow;

        Handles.color = current;

        for (float x = 0; x < 40; x += cellSize)
        {
            for (float z = 0; z < 40; z += cellSize)
            {
                var point = _grid.GetNearestPointOnGrid(new Vector3(x, 0f, z));

                if (_grid.ObjectList.ContainsKey(point))
                    Handles.color = red;
                else
                    Handles.color = yellow;

                Handles.DrawSphere(identity, point, Quaternion.identity, 0.1f);
            }
        }
    }
}