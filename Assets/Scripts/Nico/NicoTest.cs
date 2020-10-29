using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUtils;
using System;

public class NicoTest : MonoBehaviour
{
    //---- TO DO ----//
    //Lista de Objectos
    //Me da la lista la paleta? o me da el objeto donde hace click el jugador? o la saco de otro lado?
    //Se instancia en el cursor y se va snapeadno a la grilla
    //Transparente hasta que este posicionado en la escena
    //Implementar rotacion de los objetos desde el teclado

    public List<GameObject> _myObjects = new List<GameObject>();
    private int current = 0;
    private bool canScroll = true;
    GameObject objectSelected;
    private Camera myCamera;
    public LayerMask gridMask;


    private void Awake()
    {
        myCamera = Camera.main;
    }

    private void Update()
    {
        //Itero la lista desde el scroll del mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if(scroll != 0 && canScroll)
        {
            canScroll = false;
            StartCoroutine(C_ScrollCoolDown());
            if (scroll > 0)
            {
                //List up item
                current++;
            }
            else
            {
                //List down item
                current--;
            }
            UpdateSelectedItemFromList(current);
        }

        if (objectSelected)
        {
            Vector3 mousePos = GetMousePositionOnGrid();
            objectSelected.transform.position = mousePos;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            LeftRotation();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            RightRotation();
        }
    }

    private void RightRotation()
    {
        
    }

    private void LeftRotation()
    {
        
    }

    IEnumerator C_ScrollCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        canScroll = true;
    }
    private void UpdateSelectedItemFromList(int newCurrent)
    {
        if((newCurrent < 0) || (newCurrent > _myObjects.Count - 1))
        {
            return;
        }

        GameObject o = _myObjects[newCurrent];

        objectSelected = Instantiate(o);

        ChangeAlpha();
    }

    private void ChangeAlpha()
    {

    }

    private Vector3 GetMousePositionOnGrid()
    {
        Vector3 mousePositionOnGrid = Vector3.zero;
        
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 500, gridMask))
        {
            Debug.Log("Ray pego a: " + hit.transform.name + "pos: " + hit.transform.position);
            mousePositionOnGrid = hit.transform.position;
        }
        //mousePositionOnGrid = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
        Debug.Log("MousePositionOnGrid: " + mousePositionOnGrid);
        return mousePositionOnGrid;
    }
    




}
