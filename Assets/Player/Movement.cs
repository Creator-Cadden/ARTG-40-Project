using UnityEngine;

public class Movement : MonoBehaviour
{
    private bool canMove = true;

    void Update()
    {
        if (canMove)
        {
            // Implement normal movement controls (e.g., WASD or arrow keys)
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, 0, vertical) * Time.deltaTime * 5f;
            transform.Translate(movement, Space.World);
        }
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }
}