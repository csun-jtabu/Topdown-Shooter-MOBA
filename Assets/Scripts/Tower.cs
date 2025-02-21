using UnityEngine;
using System.Collections;

public class Tower : Entity
{
    public bool MainTower = false;
    private float SpawnTimer = 15f;
    private int SpawnCount = 3;
    public GameObject MinionPrefab;
    public GameObject ParentMainTowerPrefab;
    public Tower ParentMainTower;

    public void SpawnIncrement()
    {
        this.SpawnCount++;
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(SpawnTimer);
        SpawnMinions();
        Spawn();
    }

    private void SpawnMinions()
    {
        if (this.MainTower)
        {
            for (int i = 1; i <= this.SpawnCount; i++)
            {
                //create an enemy
                Instantiate(MinionPrefab, transform);
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
                    ParentMainTower.SpawnIncrement();
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
        if (MainTower)
            Spawn();
        else
            ParentMainTower = ParentMainTowerPrefab.GetComponent<Tower>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
