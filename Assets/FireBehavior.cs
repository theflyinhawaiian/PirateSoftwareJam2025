using UnityEngine;

public class FireBehavior : MonoBehaviour
{
    public int shotForce = 1;
    Rigidbody2D rb;
    bool hasFired;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && !hasFired){
            rb.simulated = true;
            rb.AddForce(new Vector2(shotForce,shotForce));
            hasFired = true;
        }
    }
}
