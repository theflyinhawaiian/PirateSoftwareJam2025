using System.Linq;
using UnityEngine;

public class EntityBehavior : MonoBehaviour
{
    public float moveSpeed = 5f;
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
        var compositeBounds = 
            transform.GetComponentsInChildren<Renderer>()
                .Select(x => x.bounds)
                .Aggregate((intermediateBounds, nextBounds) => {
                    intermediateBounds.Encapsulate(nextBounds);
                    return intermediateBounds;
                });

        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(compositeBounds.center, compositeBounds.size * 1.01f);
    }
}

