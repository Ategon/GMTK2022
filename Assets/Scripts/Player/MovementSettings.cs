using UnityEngine;

[System.Serializable]
public class MovementSettings : IInteractionData
{
    [HideInInspector] public CharacterController characterController;

    [Header("Player Parameters")]
    public float walkingSpeed = 5;
    public float crouchSpeed = 3;
    public float dodgeSpeed = 20;
    public float dodgeTime = 1f;
    public float dodgeCooldown = 0.5f;
     
    public Vector3 gravity = new Vector3(0f, -9.8f, 0f);
     
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDist;
}
