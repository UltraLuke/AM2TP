using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    private void Start()
    {
        gameObject.AddComponent<BoxCollider>();
        gameObject.layer = 1 << 8;
    }
}
