using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

public class GridToolWindow : EditorWindow
{
    #region Basics
    //Styles
    private GUIStyle _myStyle;

    //Windows
    private GridLoader _loadWindow;
    private GridSaver _saveWindow;

    //GameObjects
    private CustomGrid _customGrid;
    #endregion

    #region Grid Editor
    private GameObject currObj;
    float cellSize;
    GameObject _lastCurrObj;
    Plane _plane;
    GameObject _selectedObject;
    Vector3 _lastSelectedPos = Vector3.zero;

    bool _canReplaceObjects = false;

    string[] _modeTabs = { "Create", "Edit", "Delete" };
    int _tabSelection = 0;

    //Flags
    bool _movingObject = false;

    public GameObject CurrObj
    {
        get => currObj;
        set
        {
            currObj = value;
            Repaint();
        }
    }

    #endregion

    private void OnEnable()
    {
        GetGridData();

        _plane = new Plane(Vector3.up, Vector3.zero);

        SceneView.duringSceneGui += OnSceneGui;
        SceneView.RepaintAll();

        GridObject[] GridData = Resources.LoadAll<GridObject>("AutosaveGrid/");

        if (GridData != null && GridData.Length > 0)
        {
            var grid = new GameObject(GridData[0].name, typeof(CustomGrid));
            cellSize = GridData[0].size;

            _customGrid = grid.GetComponent<CustomGrid>();
            _customGrid.Size = GridData[0].size;
        }
    }

    private void OnGUI()
    {
        if (_customGrid == null)
        {
            EditorGUILayout.LabelField("Comencemos");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Crear nueva grilla"))
            {
                var obj = new GameObject("GRID");
                obj.transform.position = Vector3.zero;
                _customGrid = obj.AddComponent<CustomGrid>();

                Undo.RegisterCreatedObjectUndo(obj, "Object created");
            }
            if (GUILayout.Button("Cargar grilla"))
            {
                _loadWindow = GetWindow<GridLoader>();
                _loadWindow.Show();
            }
            EditorGUILayout.EndHorizontal();
            GetGridData();
        }
        if (_customGrid != null)
        {
            if (GUILayout.Button("Guardar grilla y objetos"))
            {
                _saveWindow = GetWindow<GridSaver>();
                _saveWindow.Show();
            }

            GridEditor();
        }
    }

    private void OnDisable()
    {
        _loadWindow?.Close();
        _saveWindow?.Close();

        if (_selectedObject != null)
            DestroyImmediate(_selectedObject);

        if (_customGrid != null)
            _customGrid.OnToolGridClosed();

        SceneView.duringSceneGui -= OnSceneGui;
    }

    private void GetGridData()
    {
        _customGrid = FindObjectOfType<CustomGrid>();
        if (_customGrid != null)
        {
            cellSize = _customGrid.Size;
        }
    }

    #region Grid Editor
    //Logica principal de la herramienta de edicion
    private void GridEditor()
    {
        if (!_movingObject)
        {
            cellSize = _customGrid.Size = EditorGUILayout.FloatField("Cell Size", cellSize);
            currObj = (GameObject)EditorGUILayout.ObjectField("Objeto actual", currObj, typeof(GameObject), false);
            _canReplaceObjects = EditorGUILayout.Toggle("Can Replace Objects", _canReplaceObjects);
            _tabSelection = GUILayout.Toolbar(_tabSelection, _modeTabs);
        }
        else
        {
            EditorGUILayout.HelpBox("No se puede acceder a los controles mientras se está moviendo un objeto", MessageType.Info);
        }

        //Si entro al modo edicion instancio el objeto de muestra del objeto actual.
        if (_tabSelection == 0)
        {
            if (currObj == null)
            {
                EditorGUILayout.HelpBox("No hay objeto seleccionado. " +
                                        "Seleccione un objeto de la paleta para crear objetos en escena",
                                        MessageType.Warning);
            }
            else
            {
                if (_lastCurrObj != currObj)
                {
                    DestroyEditingObject();
                    _lastCurrObj = currObj;
                }

                if (_selectedObject == null)
                    _selectedObject = (GameObject)PrefabUtility.InstantiatePrefab(currObj);
            }
        }
        //Si salgo del modo edicion, dejo de mostrar el objeto muestra.
        else if (_tabSelection == 1)
        {
            if (!_movingObject)
                DestroyEditingObject();
        }
        else
        {
            DestroyEditingObject();
        }

        CheckKeys();
    }

    //Destruye el objeto de muestra
    private void DestroyEditingObject()
    {
        if (_selectedObject != null)
        {
            DestroyImmediate(_selectedObject);
            _selectedObject = null;
        }
    }

    //Chequeo teclas del teclado por si realiza una combinacion
    private void CheckKeys()
    {
        //TO DO
    }

    //Metodo para hacer pintado de escenas
    private void OnSceneGui(SceneView sceneView)
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

                    if (_canReplaceObjects || _customGrid.CheckIfAvailablePosition(hitPoint))
                    {
                        var obj = (GameObject)PrefabUtility.InstantiatePrefab(currObj);
                        Undo.RegisterCreatedObjectUndo(obj, "Object created");
                        _customGrid.SetObjectOnGrid(obj, hitPoint);
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

                        var pointToPlace = _customGrid.GetNearestPointOnGrid(hitPoint);
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

                    var obj = _customGrid.GetObjectOnGrid(hitPoint);

                    //Si no estoy moviendo un objeto, obtengo dicho objeto y lo uso como objeto de muestra.
                    //Es decir, empiezo a mover un objeto
                    if (!_movingObject && obj != null)
                    {
                        _selectedObject = _customGrid.GetObjectOnGrid(hitPoint);
                        _lastSelectedPos = _customGrid.GetNearestPointOnGrid(hitPoint);
                        _movingObject = true;
                    }
                    //Si estoy moviendo un objeto, dejo el mismo en la posicion donde cliquie.
                    //Dejo de mover el objeto.
                    else if (_movingObject)
                    {
                        if (obj == null)
                        {
                            _customGrid.MoveObject(_lastSelectedPos, hitPoint);
                            _selectedObject = null;
                        }
                        else
                        {
                            _selectedObject.transform.position = _customGrid.GetNearestPointOnGrid(_lastSelectedPos);
                            _selectedObject = null;
                            //_grid.SetObjectOnGrid(obj, _lastSelectedPos);
                        }

                        _customGrid.CleanEmptyReferences();
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

                        var pointToPlace = _customGrid.GetNearestPointOnGrid(hitPoint);
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
                    _customGrid.DeleteObject(hitPoint);
                }
            }
        }

        DrawGrid();
    }

    //Dibujo la grilla en la escena
    private void DrawGrid()
    {
        if (_customGrid != null && cellSize > 0)
        {
            _customGrid.CleanEmptyReferences();

            Color yellow = Color.yellow;
            Color red = Color.red;
            int identity = 0;

            Color current = yellow;

            Handles.color = current;

            for (float x = 0; x < 40; x += cellSize)
            {
                for (float z = 0; z < 40; z += cellSize)
                {
                    var point = _customGrid.GetNearestPointOnGrid(new Vector3(x, 0f, z));

                    if (_customGrid.ObjectList.ContainsKey(point))
                        Handles.color = red;
                    else
                        Handles.color = yellow;

                    Handles.DrawSphere(identity, point, Quaternion.identity, 0.1f);
                }
            }
        }
    }
    #endregion
}