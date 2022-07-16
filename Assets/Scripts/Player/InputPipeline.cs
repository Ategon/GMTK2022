using UnityEngine;
using UnityEngine.InputSystem;

public class InputPipeline : IPipelineBehaviour
{
    public InputData InputData { get { return inputData; } }

    private PlayerInput playerInput;
    private InputGenerator inputGenerator;
    private InputData inputData;

    private void Awake()
    {
        inputData = new InputData();

        inputGenerator = new InputGenerator();

        data.Add("InputData", inputData);
    }

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

    public void ReadAction(InputAction.CallbackContext _context)
    {
        var parameters = new InputGenerator.InputParams()
        {
            context = _context,
            ifKeyboardAndMouse = playerInput.currentControlScheme == "Keyboard&Mouse"
        };

        IInteractionData interactionData = inputData;
        inputGenerator.GenerateData(parameters, ref interactionData);
        inputData = (InputData)interactionData;
    }
}
