using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputData : IInteractionData
{
    public Vector2 Move;
    public Vector2 Aim;
    public bool Dodge;
    public bool Reload;
    public bool Crouch;
    public bool Fire;
    public bool Pause;
}
