using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 5;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    void FixedUpdate()
    {
        rb.MovePosition(new Vector3(rb.position.x, rb.position.y, rb.position.z - (moveSpeed * Time.deltaTime)));
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Player")
            Destroy(gameObject);
    }
}
