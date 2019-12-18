using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChange : MonoBehaviour
{
    public Texture2D cursorTextureInteract;
    public Texture2D cursorTextureDefault; 
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void OnMouseEnter()
    {
		Cursor.SetCursor(cursorTextureInteract, hotSpot, cursorMode);
    }

    void OnMouseExit()
    {
		Cursor.SetCursor(cursorTextureDefault, Vector2.zero, cursorMode);
    }
}
