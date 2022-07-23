using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DataPipeline;

public class Cursor : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    void Start()
    {
        UnityEngine.Cursor.SetCursor(cursorTexture, new Vector2(10, 10), CursorMode.Auto);
    }
}