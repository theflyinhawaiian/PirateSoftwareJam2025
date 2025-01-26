using UnityEngine;

public class ObstacleBehavior : EntityBehavior 
{
    private GameManager gameManager;

    public void SetManager(GameManager manager){
        gameManager = manager;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.ObstacleImpacted(1);
            // Maybe do something different?
            Destroy(gameObject);
        }

    }
}
