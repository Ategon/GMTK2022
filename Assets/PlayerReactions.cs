using UnityEngine;

public class PlayerReactions : MonoBehaviour
{
    [SerializeField]
    private int speed = 5;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Move(Vector2 vector2)
    {
        moveDirection = new Vector3(vector2.x * speed, 0, vector2.y * speed);

        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void Fire(bool isFiring)
    {
        if (isFiring)
        {
            Debug.Log("BANG ur dead!");
        }
    }
}
