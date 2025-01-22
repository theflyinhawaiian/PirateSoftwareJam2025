using UnityEngine;

public class TargetBehavior : EntityBehavior 
{

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
            Destroy(gameObject);
    }
}
