using UnityEngine;
using UnityEngine.InputSystem;
using DataPipeline;

public class InputReader : PlayerInput, IGenerator<PlayerInteractionState>
{
    [HideInInspector]
    public PlayerState playerState;
    public PlayerState.Dirty dirtyFields;

    public InputReader()
    {
        playerState = new PlayerState();
        dirtyFields = new PlayerState.Dirty();
        dirtyFields.Reset();
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
        data.PlayerState.SetDirty(in playerState, in dirtyFields);
        dirtyFields.Reset();
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
                dirtyFields.Fire = true;
                break;
            case "Reload":
                ReadBool(context, ref playerState.Reload);
                dirtyFields.Reload = true;
                break;
            case "Crouch":
                ReadBool(context, ref playerState.Crouch);
                dirtyFields.Crouch = true;
                break;
            case "Dodge":
                ReadBool(context, ref playerState.Dodge);
                dirtyFields.Dodge = true;
                break;
            case "Move":
                ReadVector2(context, ref playerState.Move);
                dirtyFields.Move = true;
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
                dirtyFields.Aim = true;

                ReadVector2(context, ref playerState.CursorPos);
                dirtyFields.CursorPos = true;
                break;
            case "Pause":
                ReadBool(context, ref playerState.Pause);
                dirtyFields.Pause = true;
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
