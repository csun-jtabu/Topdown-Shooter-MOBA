using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System;

public class Tower : Entity
{
    public bool MainTower = false;
    private float SpawnTimer = 15f;
    private int SpawnCount = 3;
    public GameObject MinionPrefab;
    public GameObject EnemyMainTowerPrefab;
    public Tower EnemyMainTower;

    private float xCoordinate;
    private float yCoordinate;

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
                Vector3 randomPosition = UnityEngine.Random.insideUnitCircle * 5;
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

    // References:
    // http://stackoverflow.com/questions/45958061/c-sharp-read-text-file-line-by-line-and-edit-specific-line
    // https://stackoverflow.com/questions/76614080/parse-complex-formula-string-with-nested-parenthesis-in-c-sharp
    void ReadFromCoordianteFile()
    {
        string line = "";
        string[] words = new string[3];
        
        try
        {
            using (StreamReader reader = new StreamReader(new FileStream(Application.dataPath + "/Scripts/LevelGen/coordinate_file.txt", FileMode.Open)))
            {
                Debug.Log("FilePath: " + Application.dataPath + "/Scripts/LevelGen/coordinate_file.txt");
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine("Current Line: " + line);
                    if (this.MainTower)
                        if (line.Contains("Friendly Tower Coordinates: "))
                        {
                            words = line.Split(')');
                            for (var i = 0; i < words.Count(); i++)
                            {
                                Debug.Log(words[i]);
                            }
                        }
                    else
                        if (line.Contains("Enemy Tower Coordinates: "))
                        {
                            words = line.Split(')');
                            for (var i = 0; i < words.Count(); i++)
                            {
                                Debug.Log(words[i]);
                            }
                        }
                }
                reader.Close();
            }

            string currentElement = words[0];
            string coordinates = currentElement.Substring(currentElement.IndexOf('(') + 1);
            Debug.Log("Returned Coordinate: " + coordinates);
            var results = coordinates.Split(',');
            xCoordinate = float.Parse(results[0]);
            yCoordinate = float.Parse(results[1]);
        }
        catch (Exception e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ReadFromCoordianteFile();
        Vector2 point = new Vector2(0.0f, 0.0f);
        //Instantiate(new Tower(), point, Quaternion.identity);
        //transform.position = new Vector2(xCoordinate, yCoordinate);

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
