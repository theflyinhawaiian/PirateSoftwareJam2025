using UnityEngine;

public class TargetBehavior : EntityBehavior 
{
    private int value = 1;

    public void SetManager(GameManager manager){
        this.manager = manager;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
            return;
        
        manager.TargetDestroyed(value);
        Destroy(gameObject);
    }
}
