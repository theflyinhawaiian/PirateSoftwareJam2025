using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();        
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Vector3.zero;
        var vert = Input.GetAxis("Vertical");
        var lat = Input.GetAxis("Horizontal");
        rb.MovePosition(new Vector3(
            rb.position.x + (lat * Time.deltaTime * moveSpeed),
            rb.position.y + (vert * Time.deltaTime * moveSpeed), 
            rb.position.z));
    }
}
