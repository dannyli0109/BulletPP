using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D cursorImage;
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.SetCursor(cursorImage, new Vector2(cursorImage.width / 2.0f, cursorImage.height / 2.0f), CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
