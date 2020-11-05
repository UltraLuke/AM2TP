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

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        RaycastHit hit;
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            Debug.Log("Raycasting");
    //            //Debug.Log("Instantiate");
    //            //Vector3 hitPoint = ray.GetPoint(hitInfo.po);
    //            //var obj = Instantiate(currObj);
    //            //_grid.SetObjectOnGrid(obj, hitPoint);

    //            Vector3 hitPoint = hit.point;
    //            var obj = Instantiate(currObj);
    //            grid.SetObjectOnGrid(obj, hitPoint);
    //        }
    //    }
    //}
}
