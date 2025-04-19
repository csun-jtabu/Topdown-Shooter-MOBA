using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class Tower : Entity
{
    [SerializeField] public bool mainTower = true;

    [SerializeField] public int numberOfMinions = 15;
    private int numberLeft = 0;

    //private float SpawnTimer = 15f;
    private float SpawnTimer = 5f;
    private int SpawnCount = 3;
    public GameObject MinionPrefab;

    public GameObject singlePlayer; 
    public GameObject multiplayerPlayer;

    [SerializeField] public bool multiplayer = false;

    private bool alreadySpawnedPlayer = false;

    public GameObject EnemyMainTowerPrefab;
    private Tower EnemyMainTower;

    private float xCoordinate;
    private float yCoordinate;    

    public void SpawnIncrement()
    {
        this.SpawnCount++;
    }    


    bool CheckBoxSize(float check_x, float check_y, float center_x, float center_y, float width, float height)
    {
        float left_boundary = 0;
        float right_boundary = 0;
        float bottom_boundary = 0;
        float top_boundary = 0;
        float half_width = 0;
        float half_height = 0;

        if (width % 2 != 0 && height % 2 != 0) {
            width = width - 1;
            height = height - 1;

            // Calculate half-width and half-height
            half_width = width / 2;
            half_height = height / 2;

            // Calculate boundaries
            left_boundary = center_x - half_width;
            right_boundary = center_x + half_width;
            bottom_boundary = center_y - half_height;
            top_boundary = center_y + half_height;

        } else {
            // Calculate half-width and half-height
            half_width = width / 2;
            half_height = height / 2;

            // Calculate boundaries
            left_boundary = center_x - half_width;
            right_boundary = center_x + half_width;
            bottom_boundary = center_y - half_height;
            top_boundary = center_y + half_height;
        }
        

        // Check if the point is outside the cube
        bool isOutside = check_x < left_boundary || check_x > right_boundary || check_y < bottom_boundary || check_y > top_boundary;

        if (isOutside)
        {
            return true;
        }
        else
        {
            print("Inside box.");
            return false;
        }
    }





    IEnumerator Spawn()
    {   
        if (mainTower == true){
            if (alreadySpawnedPlayer == false) {
                SpawnPlayer();
                alreadySpawnedPlayer = true;
            }
            
            yield return new WaitForSeconds(SpawnTimer);
            
            if (numberLeft > 0) {
                SpawnMinions();
                StartCoroutine(Spawn());
            }
        }
    }

    private void SpawnMinions()
    {
        float setSpawnTimer = SpawnTimer;
        
        if (mainTower == true)
        {
            for (int i = 1; i <= this.SpawnCount; i++)
            {
                if (numberLeft > 0) {
                    //create an enemy
                    Vector3 randomPosition = UnityEngine.Random.insideUnitCircle * 5;
                    randomPosition += transform.position;

                    float width = ReadImage.getBoxSizeX();
                    float height = ReadImage.getBoxSizeY();
                    float center_x = width/2;
                    float center_y = height/2;

                    width = width - 2;
                    height = height - 2;

                    if (CheckBoxSize(randomPosition.x, randomPosition.y, center_x, center_y, width, height) == false) {
                        numberLeft = numberLeft - 1;
                        Instantiate(MinionPrefab, randomPosition, transform.rotation);
                        SpawnTimer = setSpawnTimer;
                    } else {
                        if (i > 1) {
                            i = i - 1;
                        }
                        SpawnTimer = 0f;
                    }
                }

            }
        }
    }

    private void SpawnPlayer() {
        bool spawnedPlayer = false;

        while (spawnedPlayer == false) {
            Vector3 randomPosition = UnityEngine.Random.insideUnitCircle * 2;
            randomPosition += transform.position;

            float width = ReadImage.getBoxSizeX();
            float height = ReadImage.getBoxSizeY();
            float center_x = width/2;
            float center_y = height/2;

            width = width - 2;
            height = height - 2;

            if (CheckBoxSize(randomPosition.x, randomPosition.y, center_x, center_y, width, height) == false) {
                if (multiplayer) {
                    Instantiate(multiplayerPlayer, randomPosition, transform.rotation);
                } else {
                    try {
                        Instantiate(singlePlayer, randomPosition, transform.rotation);
                    } catch (Exception) {
                        // Single player character wasn't defined here because it is an enemy tower.
                    }
                }

                spawnedPlayer = true;
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
                //if (mainTower == false)
                //    EnemyMainTower.SpawnIncrement();
                ////else
                ////{
                ////    //game over, victory for opposing team
                ////}
                Destroy(this.gameObject);
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
                    if (mainTower == true)
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
        multiplayer = MainMenuScript.getIsMultiplayer();
        
        ReadFromCoordianteFile();
        Vector2 point = new Vector2(0.0f, 0.0f);
        //Instantiate(new Tower(), point, Quaternion.identity);
        //transform.position = new Vector2(xCoordinate, yCoordinate);

        numberLeft = numberOfMinions;

        if (mainTower == true)
            StartCoroutine(Spawn());
        else
        {
            // if (this.Team == 1)
            //     EnemyMainTower = GameObject.Find("Main Tower Team 2").GetComponent<Tower>();
            // else
            //     EnemyMainTower = GameObject.Find("Main Tower Team 1").GetComponent<Tower>();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
