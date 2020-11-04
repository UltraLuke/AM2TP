using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    [SerializeField] private float size = 1f;

    private Dictionary<Vector3, GameObject> _objects = new Dictionary<Vector3, GameObject>();

    public float Size { get => size; }
    public Dictionary<Vector3, GameObject> ObjectList => _objects;

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

        if (_objects.ContainsKey(gridPos))
            DestroyImmediate(_objects[gridPos]);

        _objects[gridPos] = obj;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (float x = 0; x < 40; x += size)
        {
            for (float z = 0; z < 40; z += size)
            {
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}
