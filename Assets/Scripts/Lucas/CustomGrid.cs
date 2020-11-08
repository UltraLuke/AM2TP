using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    [SerializeField] public float size = 1f; // cambie a public

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

    public void CleanEmptyReferences()
    {
        foreach (var item in ObjectList)
        {
            if (item.Value == null)
                ObjectList.Remove(item.Key);
        }
    }

    private void OnDrawGizmos()
    {
        Color yellow = Color.yellow;
        Color red = Color.red;

        Color current = yellow;

        Gizmos.color = current;

        for (float x = 0; x < 40; x += size)
        {
            for (float z = 0; z < 40; z += size)
            {
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));

                if (ObjectList.ContainsKey(point))
                    Gizmos.color = red;
                else
                    Gizmos.color = yellow;

                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}
