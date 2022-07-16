using UnityEngine;
using UnityEngine.InputSystem;
using DataPipeline;

public class InputReader : PlayerInput, IGenerator<PlayerInteractionState>
{
    [HideInInspector]
    public PlayerState playerState;

    public InputReader()
    {
        playerState = new PlayerState();
    }

    public void Start()
    {
        onActionTriggered += ReadAction;
    }

    public void StartRound()
    {

    }

    public void Write(ref PlayerInteractionState data)
    {
        data.PlayerState.SetDifference(in playerState);
    }

    public bool IsNotDoneWriting()
    {
        return false;
    }

    public void ReadAction(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Fire":
                ReadBool(context, ref playerState.Fire);
                break;
            case "Reload":
                ReadBool(context, ref playerState.Reload);
                break;
            case "Crouch":
                ReadBool(context, ref playerState.Crouch);
                break;
            case "Dodge":
                ReadBool(context, ref playerState.Dodge);
                break;
            case "Move":
                ReadVector2(context, ref playerState.Move);
                break;
            case "Aim":
                if (currentControlScheme == "Keyboard&Mouse")
                {
                    ReadScreenVector2(context, ref playerState.Aim);
                }
                else
                {
                    ReadVector2(context, ref playerState.Aim);
                }

                ReadVector2(context, ref playerState.CursorPos);
                break;
            case "Pause":
                ReadBool(context, ref playerState.Pause);
                break;
            default:
                break;
        }
    }

    private void ReadBool(InputAction.CallbackContext context, ref bool action)
    {
        if (context.performed)
        {
            action = true;
        }
        else if (context.canceled)
        {
            action = false;
        }
    }

    private void ReadVector2(InputAction.CallbackContext context, ref Vector2 action)
    {
        if (context.performed || context.canceled)
        {
            action = context.ReadValue<Vector2>();
        }
    }

    private void ReadScreenVector2(InputAction.CallbackContext context, ref Vector2 action)
    {
        if (context.performed || context.canceled)
        {
            Vector2 screenCoords = context.ReadValue<Vector2>();
            Vector2 inputDirection = new Vector2(screenCoords.x / Screen.width * 2 - 1, screenCoords.y / Screen.height * 2 - 1);
            inputDirection.Normalize();
            action = inputDirection;
        }
    }
}
