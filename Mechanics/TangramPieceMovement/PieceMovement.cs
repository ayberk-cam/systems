using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    private Transform parent;
    private bool moving;
    private float startPosX;
    private float startPosY;
    private Vector3 lastPosition;

    public bool isPlaced = false;

    void Update()
    {
        if (moving)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            parent.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, -10);
        }
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && (!isPlaced))
        {
            moving = true;

            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            startPosX = mousePos.x - parent.transform.localPosition.x;
            startPosY = mousePos.y - parent.transform.localPosition.y;
        }
    }

    public void OnMouseUp()
    {
        moving = false;

        if (Mathf.Abs(parent.transform.localPosition.x - lastPosition.x) <= 0.9 && Mathf.Abs(parent.transform.localPosition.y - lastPosition.y) <= 0.9)
        {
            if (!isPlaced)
            {
                //place
            }

        }
    }
}
