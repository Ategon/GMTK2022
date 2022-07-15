using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public Vector2 Move { get { return move; } private set {; } }
    public Vector2 Aim { get { return aim; } private set {; } }
    public bool Dodge { get { return dodge; } private set {; } }
    public bool Reload { get { return reload; } private set {; } }
    public bool Crouch { get { return crouch; } private set {; } }
    public bool Fire { get { return fire; } private set {; } }
    public bool Pause { get { return pause; } private set {; } }

    [SerializeField] private Vector2 move;
    [SerializeField] private Vector2 aim;
    [SerializeField] private bool dodge;
    [SerializeField] private bool reload;
    [SerializeField] private bool crouch;
    [SerializeField] private bool fire;
    [SerializeField] private bool pause;

    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += ReadAction;
    }

    private void OnDisable()
    {
        playerInput.onActionTriggered -= ReadAction;
    }

    private void OnDestroy()
    {
        playerInput.onActionTriggered -= ReadAction;
    }

    public void ReadAction(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Fire":
                ReadBool(context, ref fire);
                break;
            case "Reload":
                ReadBool(context, ref reload);
                break;
            case "Crouch":
                ReadBool(context, ref crouch);
                break;
            case "Dodge":
                ReadBool(context, ref dodge);
                break;
            case "Move":
                ReadVector2(context, ref move);
                break;
            case "Aim":
                if (playerInput.currentControlScheme == "Keyboard&Mouse")
                {
                    ReadScreenVector2(context, ref aim);
                }
                else
                {
                    ReadVector2(context, ref aim);
                }
                break;
            case "Pause":
                ReadBool(context, ref pause);
                break;
            default:
                break;
        }
    }

    public void ReadBool(InputAction.CallbackContext context, ref bool action)
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

    public void ReadVector2(InputAction.CallbackContext context, ref Vector2 action)
    {
        if (context.performed || context.canceled)
        {
            action = context.ReadValue<Vector2>();
        }
    }

    public void ReadScreenVector2(InputAction.CallbackContext context, ref Vector2 action)
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
