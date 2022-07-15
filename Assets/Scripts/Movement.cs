using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private int speed = 5;

    private PlayerController playerController;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        moveDirection = new Vector3(playerController.Move.x * speed, 0, playerController.Move.y * speed);

        characterController.Move(moveDirection * Time.deltaTime);
    }
}
