using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class CustomGrid : MonoBehaviour
{
    private float size = 1f;
    public float Size
    {
        get => size;
        set
        {
            if (value <= 0)
                size = 0.1f;
            else
                size = value;
        }
    }

    private Dictionary<Vector3, GameObject> _objects = new Dictionary<Vector3, GameObject>();
    //public float Size { get => size; }
    public Dictionary<Vector3, GameObject> ObjectList
    {
        get => _objects;
        set => _objects = value;
    }

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(
            xCount * size,
            yCount * size,
            zCount * size);

        result += transform.position;

        return result;
    }

    public void SetObjectOnGrid(GameObject obj, Vector3 position)
    {
        var gridPos = GetNearestPointOnGrid(position);
        obj.transform.position = gridPos;

        UnityEditor.Undo.RegisterCompleteObjectUndo(this, "References changed");

        if (ObjectList.ContainsKey(gridPos))
            UnityEditor.Undo.DestroyObjectImmediate(ObjectList[gridPos]);

        ObjectList[gridPos] = obj;
    }

    public GameObject GetObjectOnGrid(Vector3 position)
    {
        position = GetNearestPointOnGrid(position);

        if (ObjectList.ContainsKey(position))
            return ObjectList[position];

        return null;
    }

    public void MoveObject(Vector3 from, Vector3 to)
    {
        from = GetNearestPointOnGrid(from);
        to = GetNearestPointOnGrid(to);

        if (ObjectList.ContainsKey(from))
        {
            var obj = _objects[from];
            obj.transform.position = from;
            UnityEditor.Undo.RegisterCompleteObjectUndo(obj.transform, "Object moved");
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Object moved");
            obj.transform.position = to;

            if (ObjectList.ContainsKey(to))
                UnityEditor.Undo.DestroyObjectImmediate(ObjectList[to]);

            ObjectList[to] = obj;
            ObjectList.Remove(from);
        }
    }

    public void DeleteObject(Vector3 position)
    {
        position = GetNearestPointOnGrid(position);

        if (ObjectList.ContainsKey(position))
        {
            UnityEditor.Undo.DestroyObjectImmediate(ObjectList[position]);
            ObjectList[position] = null;
        }
    }

    public bool CheckIfAvailablePosition(Vector3 position)
    {
        position = GetNearestPointOnGrid(position);

        bool occuped = _objects.ContainsKey(position);
        return !occuped;
    }

    public void CleanEmptyReferences()
    {
        foreach (var item in ObjectList)
        {
            if (item.Value == null)
                ObjectList.Remove(item.Key);
        }
    }

    public void OnToolGridClosed()
    {
        GridSaver.AutoSave();
        DestroyImmediate(gameObject);
    }
}
