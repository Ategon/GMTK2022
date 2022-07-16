using UnityEngine;
using UnityEngine.InputSystem;

public class InputGenerator : IInteractionGenerator
{
    public class InputParams : IInteractionGenerator.GenerationParams
    {
        public InputAction.CallbackContext context;
        public bool ifKeyboardAndMouse;
    }

    public IInteractionData GenerateData(IInteractionGenerator.GenerationParams parameters, ref IInteractionData data)
    {
        InputParams inputParams = (InputParams)parameters;
        ReadAction(inputParams.context, inputParams.ifKeyboardAndMouse, ref data);
        return data;
    }
    
    private void ReadAction(InputAction.CallbackContext context, bool ifKeyboardAndMouse, ref IInteractionData data)
    {
        InputData inputData = (InputData)data;
        switch (context.action.name)
        {
            case "Fire":
                ReadBool(context, ref inputData.Fire);
                break;
            case "Reload":
                ReadBool(context, ref inputData.Reload);
                break;
            case "Crouch":
                ReadBool(context, ref inputData.Crouch);
                break;
            case "Dodge":
                ReadBool(context, ref inputData.Dodge);
                break;
            case "Move":
                ReadVector2(context, ref inputData.Move);
                break;
            case "Aim":
                if (ifKeyboardAndMouse)
                {
                    ReadScreenVector2(context, ref inputData.Aim);
                }
                else
                {
                    ReadVector2(context, ref inputData.Aim);
                }
                break;
            case "Pause":
                ReadBool(context, ref inputData.Pause);
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
