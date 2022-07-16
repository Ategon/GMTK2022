using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputData : IInteractionData
{
    public Vector2 Move;
    public Vector2 Aim;
    public Vector2 CursorPos;
    public bool Dodge;
    public bool Reload;
    public bool Crouch;
    public bool Fire;
    public bool Pause;
}
