using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Texture2D CursorArrow;
    public Texture2D CursorHand;

    void Start()
    {
        //Cursor.visible = false;
        Cursor.SetCursor(CursorArrow, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnMouseEnter()
    {
        Cursor.SetCursor(CursorHand, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnMouseExit()
    {
        Cursor.SetCursor(CursorArrow, Vector2.zero, CursorMode.ForceSoftware);
    }
}
