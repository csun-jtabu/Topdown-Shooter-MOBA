using UnityEngine;

public class Tower : Entity
{
    public bool MainTower = false;
    private float SpawnTimer = 5f;
    private int SpawnCount = 3;



    public void SpawnIncrement()
    {
        this.SpawnCount++;
    }

    private void SpawnEnemies()
    {
        if (this.MainTower)
        {
            for (int i = 1; i <= this.SpawnCount; i++)
            {
                //create an enemy
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
