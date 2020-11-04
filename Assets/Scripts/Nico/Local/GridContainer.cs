using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GridContainer : ScriptableObject
{
    List<ItemObject> myItems = new List<ItemObject>();

    GridObject myGrid;

    public GridContainer SetName(string name)
    {
        this.name = name;
        return this;
    }
    public void AddObject(ItemObject sObject)
    {
        myItems.Add(sObject);
    }
    public void RemoveObject(ItemObject sObject)
    {
        myItems.Remove(sObject);
    }
    public void SetGrid(GridObject sObject)
    {
        myGrid = sObject;
    }
}
