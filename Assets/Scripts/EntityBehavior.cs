using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class EntityBehavior : MonoBehaviour
{
    public float moveSpeed = 5;
    public int id;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(new Vector3(rb.position.x, rb.position.y, rb.position.z - (moveSpeed * Time.deltaTime)));
    }
}

