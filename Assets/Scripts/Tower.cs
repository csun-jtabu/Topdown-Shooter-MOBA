using UnityEngine;
using System.Collections;

public class Tower : Entity
{
    public bool MainTower = false;
    private float SpawnTimer = 15f;
    private int SpawnCount = 3;
    public GameObject MinionPrefab;
    public GameObject EnemyMainTowerPrefab;
    public Tower EnemyMainTower;

    public void SpawnIncrement()
    {
        this.SpawnCount++;
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(SpawnTimer);
        SpawnMinions();
        StartCoroutine(Spawn());
    }

    private void SpawnMinions()
    {
        if (this.MainTower)
        {
            for (int i = 1; i <= this.SpawnCount; i++)
            {
                //create an enemy
                Vector3 randomPosition = Random.insideUnitCircle * 5;
                randomPosition += transform.position;
                Instantiate(MinionPrefab, randomPosition, transform.rotation);
            }
        }
    }

    public override void Damage(int dmg, int team)
    {
        if (team != this.Team)
        {
            this.Hp -= dmg;
            if (this.Hp <= 0)
            {
                if (!MainTower)
                    EnemyMainTower.SpawnIncrement();
                else
                {
                    //game over, victory for opposing team
                }
                Destroy(this);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (this.MainTower)
            StartCoroutine(Spawn());
        else
            EnemyMainTower = EnemyMainTowerPrefab.GetComponent<Tower>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
