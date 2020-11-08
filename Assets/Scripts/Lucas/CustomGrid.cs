using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    public float size = 1f;

    public Dictionary<Vector3, GameObject> _objects = new Dictionary<Vector3, GameObject>();
    private object locker;
    //public float Size { get => size; }
    public Dictionary<Vector3, GameObject> ObjectList
    {
        get
        {
            return _objects;
        }
        set
        {
            _objects = value;
        }
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

        if (ObjectList.ContainsKey(gridPos))
            DestroyImmediate(ObjectList[gridPos]);

        ObjectList[gridPos] = obj;
    }
    public GameObject GetObjectOnGrid(Vector3 position)
    {
        position = GetNearestPointOnGrid(position);

        if (ObjectList.ContainsKey(position))
            return ObjectList[position];

        return null;
    }
    //public GameObject TakeObjectFromGrid(Vector3 position)
    //{
    //    position = GetNearestPointOnGrid(position);

    //    if (_objects.ContainsKey(position))
    //    {
    //        var obj = _objects[position];
    //        _objects.Remove(position);

    //        return obj;
    //    }

    //    return null;
    //}
    public void MoveObject(Vector3 from, Vector3 to)
    {
        from = GetNearestPointOnGrid(from);
        to = GetNearestPointOnGrid(to);

        if (ObjectList.ContainsKey(from))
        {
            var obj = _objects[from];
            obj.transform.position = to;

            if (ObjectList.ContainsKey(to))
                DestroyImmediate(ObjectList[to]);

            ObjectList[to] = obj;
            ObjectList.Remove(from);
        }
    }
    public void DeleteObject(Vector3 position)
    {
        position = GetNearestPointOnGrid(position);

        if (ObjectList.ContainsKey(position))
        {
            DestroyImmediate(ObjectList[position]);
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

    //private void OnDrawGizmos()
    //{
        
    //}
}
