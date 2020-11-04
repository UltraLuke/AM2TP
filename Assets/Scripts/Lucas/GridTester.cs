using System;
using UnityEngine;

public class GridTester : MonoBehaviour
{
    public LayerMask layerMask;
    private CustomGrid grid;
    //public PrimitiveType primitive = PrimitiveType.Cube;
    public GameObject currObj;

    public CustomGrid Grid { get => grid; }

    private void Awake()
    {
        grid = FindObjectOfType<CustomGrid>();
    }
}
