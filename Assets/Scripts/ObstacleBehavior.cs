using UnityEngine;

public class ObstacleBehavior : EntityBehavior 
{
    public void SetManager(GameManager manager){
        this.manager = manager;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            manager.ObstacleImpacted(1);
            // Maybe do something different?
            Destroy(gameObject);
        }

    }
}
