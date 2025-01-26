using UnityEngine;

public class TargetBehavior : EntityBehavior 
{
    private int value = 1;
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
            return;
        
        manager.TargetDestroyed(value);
        Destroy(gameObject);
    }
}
