using UnityEngine;

public class ConveyorMovement : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right;
    public float speed = 0.5f;

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;

        if (rb != null)
        {
            rb.linearVelocity = new Vector3(
                moveDirection.x * speed,
                rb.linearVelocity.y,
                moveDirection.z * speed
            );
        }
    }
}