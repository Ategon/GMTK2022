using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    private PlayerReactions playerReactions;
    
    private void Awake()
    {
        playerReactions = GetComponent<PlayerReactions>();
    }
   
    public void OnMove(InputAction.CallbackContext context)
    {
        playerReactions.Move(context.ReadValue<Vector2>());
    }



}

