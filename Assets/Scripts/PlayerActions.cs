using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerReactions playerReactions;
    
    public Controls controls;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerReactions = GetComponent<PlayerReactions>();
    }

    private void Update()
    {
        playerReactions.Move(playerController.Move);
        playerReactions.Fire(playerController.Fire);
    }
}
