using UnityEngine;


public class ObstacleBehavior : EntityBehavior 
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //placeholder
            print("collidedWithPlayer");
        }

            //"Lower durability
    }
}
