using UnityEngine;

public class EntityBehavior : MonoBehaviour
{
    public float moveSpeed = 5;
    public int id;

    protected Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(new Vector3(rb.position.x, rb.position.y, rb.position.z - (moveSpeed * Time.deltaTime)));
    }

    void OnDrawGizmosSelected()
    {
        var r = GetComponent<Renderer>();
        if (r == null)
            return;
        var bounds = r.bounds;
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);

        Gizmos.DrawRay(transform.position, Vector3.back * 10000);
    }
}

