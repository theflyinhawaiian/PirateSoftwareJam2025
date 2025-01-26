using System.Linq;
using UnityEngine;

public class EntityBehavior : MonoBehaviour
{
    public int id;

    protected Rigidbody rb;
    protected GameManager manager;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetManager(GameManager manager){
        this.manager = manager;
    }

    void FixedUpdate()
    {
        var moveSpeed = manager == null ? 0 : manager.moveSpeed;
        rb.MovePosition(new Vector4(rb.position.x, rb.position.y, rb.position.z - (moveSpeed * Time.deltaTime)));
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

