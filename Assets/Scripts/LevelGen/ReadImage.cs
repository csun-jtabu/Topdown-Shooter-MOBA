using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

using System.Linq;
using System.Collections;
using System.Text;
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;

using System.Linq;
using System.Collections;
using System.Text;
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;

public class ReadImage : MonoBehaviour
{

    [SerializeField]
    private GenerateMap mapGenerator = new GenerateMap();

    // this field will store the image we'll be using for the map
    [SerializeField]
    private Texture2D image;

    // this field will store the object prefab for the walls
    [SerializeField]
    private GameObject wallObject;
    // this field will store the tile prefab for the ground/floor
    [SerializeField]
    private GameObject groundObject;

    [SerializeField]
    private GameObject friendlyMainTowerObject;
    [SerializeField]
    private GameObject enemyMainTowerObject;
    [SerializeField]
    private GameObject friendlyIntermediaryTowerObject;
    [SerializeField]
    private GameObject enemyIntermediaryTowerObject;

    [SerializeField]
    private GameObject centerObject;

    [SerializeField]
    private GameObject outsideColliderObject;


    private float friendly_xCoordinate;
    private float friendly_yCoordinate;
    private float enemy_xCoordinate;
    private float enemy_yCoordinate;


    void ReadFromCoordianteFile()
    {
        string line = "";
        string[] friendlyInformation = new string[3];
        string[] enemyInformation = new string[3];
        
        try
        {
            using (StreamReader reader = new StreamReader(new FileStream(Application.dataPath + "/Scripts/LevelGen/coordinate_file.txt", FileMode.Open)))
            {
                //UnityEngine.Debug.Log("FilePath: " + Application.dataPath + "/Scripts/LevelGen/coordinate_file.txt");
                UnityEngine.Debug.Log("FilePath: " + Application.dataPath);
                while ((line = reader.ReadLine()) != null)
                {
                    UnityEngine.Debug.Log("Current Line: " + line);
                    if (line.Contains("Friendly Tower Coordinates: "))
                    {
                        friendlyInformation = line.Split(')');
                        for (var i = 0; i < friendlyInformation.Count(); i++)
                        {
                            UnityEngine.Debug.Log(friendlyInformation[i]);
                        }

                        string currentElement = friendlyInformation[0];
                        string coordinates = currentElement.Substring(currentElement.IndexOf('(') + 1);
                        UnityEngine.Debug.Log("Returned Coordinate: " + coordinates);
                        var results = coordinates.Split(',');
                        friendly_xCoordinate = float.Parse(results[0]);
                        friendly_yCoordinate = float.Parse(results[1]);
                    }
                    if (line.Contains("Enemy Tower Coordinates: "))
                    {
                        enemyInformation = line.Split(')');
                        for (var i = 0; i < enemyInformation.Count(); i++)
                        {
                            UnityEngine.Debug.Log(enemyInformation[i]);
                        }

                        string currentElement = enemyInformation[0];
                        string coordinates = currentElement.Substring(currentElement.IndexOf('(') + 1);
                        UnityEngine.Debug.Log("Returned Coordinate: " + coordinates);
                        var results = coordinates.Split(',');
                        enemy_xCoordinate = float.Parse(results[0]);
                        enemy_yCoordinate = float.Parse(results[1]);
                    }
                    
                }
                
                reader.Close();
                UnityEngine.Debug.Log("friendly_xCoordinate: " + friendly_xCoordinate);
                UnityEngine.Debug.Log("enemy_xCoordinate: " + enemy_xCoordinate);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("The file could not be read:");
            UnityEngine.Debug.Log(e.Message);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapGenerator.Start();
        UnityEngine.Debug.Log("Here is the coordinates output: " + mapGenerator.generatedOutput);

        // this is how we get the pixels from the image
        // the array will hold the color
        // goes from bottom left of image to top right
        Color[] pix = image.GetPixels();

        // this is the dimensions of the map. basically equal to the image dimensions
        int worldX = image.width;
        int worldY = image.height;
        
        //GameObject outsideColliderObject1 = Instantiate(outsideColliderObject, new Vector3(image.width, image.height*2, 0), Quaternion.identity);
        GameObject outsideColliderObject1 = Instantiate(outsideColliderObject, new Vector3(image.width*1.5f - 0.5f, image.height*0.5f - 0.5f, 0), Quaternion.identity);
        outsideColliderObject1.transform.localScale = new Vector3(image.width + 0.5f, image.width + 0.5f, 0);

        //GameObject outsideColliderObject2 = Instantiate(outsideColliderObject, new Vector3(image.width*2, image.height, 0), Quaternion.identity);
        GameObject outsideColliderObject2 = Instantiate(outsideColliderObject, new Vector3(image.width*0.5f - 0.5f, image.height*1.5f - 0.5f, 0), Quaternion.identity);
        outsideColliderObject2.transform.localScale = new Vector3(image.width + 0.5f, image.width + 0.5f, 0);
        
        //GameObject outsideColliderObject3 = Instantiate(outsideColliderObject, new Vector3(-image.width, image.height, 0), Quaternion.identity);
        GameObject outsideColliderObject3 = Instantiate(outsideColliderObject, new Vector3(-image.width*0.5f - 0.5f, image.height*0.5f - 0.5f, 0), Quaternion.identity);
        outsideColliderObject3.transform.localScale = new Vector3(image.width + 0.5f, image.width + 0.5f, 0);

        //GameObject outsideColliderObject4 = Instantiate(outsideColliderObject, new Vector3(image.width, -image.height, 0), Quaternion.identity);
        GameObject outsideColliderObject4 = Instantiate(outsideColliderObject, new Vector3(image.width*0.5f - 0.5f, -image.height*0.5f - 0.5f, 0), Quaternion.identity);
        outsideColliderObject4.transform.localScale = new Vector3(image.width + 0.5f, image.width + 0.5f, 0);
        

        // each pixel position corresponds to a spawn position in world
        Vector3[] spawnPositions = new Vector3[pix.Length];
        UnityEngine.Debug.Log("This is how many pixels we have: " + spawnPositions.Length);
        // this will reference the center of the world
        //Vector3 startingSpawnPosition = new Vector3(-Mathf.Round(worldX/2), -Mathf.RoundToInt(worldY/2), 0);
        Vector3 startingSpawnPosition = new Vector3(0, 0, 0);
        //Vector3 startingSpawnPosition = new Vector3(Mathf.RoundToInt(worldX/2), Mathf.RoundToInt(worldY/2), 0);

        // we will be iterating the position through the position in the world so we use a new variable
        Vector3 currentSpawnPos = startingSpawnPosition;

        // this will help us track where we are in the array
        //int counter = 0;
        int counter = spawnPositions.Length - 1;

        UnityEngine.Debug.Log("spawnPositions.Length: " + spawnPositions.Length);
        UnityEngine.Debug.Log("worldX: " + worldY);

        // for loops to traverse the world's span points
        // left to right, bottom to top
        ////for(int y = 0; y < worldY; y++)
        ////{
        ////    for(int x = 0; x < worldX; x++)
        ////    {
        //for(int x = 0; x < worldX; x++)
        //{
        //    for(int y = 0; y < worldY; y++)
        //    {
        //        // we're creating a new spawn position in the spawnPositions array
        //        spawnPositions[counter] = currentSpawnPos;
        //        counter++;
        //        // this traverses through the row
        //        currentSpawnPos.y++; 
        //    }
        //
        //    // once we reach the end of the current row, we want to go back to the beginning of the next row
        //    //currentSpawnPos.x = startingSpawnPosition.x;
        //    //currentSpawnPos.y++;
        //    currentSpawnPos.y = startingSpawnPosition.y;
        //    currentSpawnPos.x++;
        //}
        
        for(int y = worldY - 1; y >= 0; y--)
        {
            for(int x = worldX - 1; x >= 0; x--)
            {
                // we're creating a new spawn position in the spawnPositions array
                spawnPositions[counter] = currentSpawnPos;
                counter--;
                // this traverses through the row
                currentSpawnPos.x++; 
            }

            // once we reach the end of the current row, we want to go back to the beginning of the next row
            //currentSpawnPos.x = startingSpawnPosition.x;
            //currentSpawnPos.y++;
            currentSpawnPos.x = startingSpawnPosition.x;
            currentSpawnPos.y++;
        }


        ////////////////// debugging to see pixel color
        // string colorStrings = "";
        // foreach (Color c in pix)
        // {
        //     colorStrings += $"({c.r}, {c.g}, {c.b}, {c.a}), ";
        // }

        // // Remove the last comma and space
        // if (colorStrings.Length > 2)
        //     colorStrings = colorStrings.Substring(0, colorStrings.Length - 2);

        // Debug.Log("Pixels: " + colorStrings);

        //////////////////


        // reset counter
        counter = 0;

        // we check all the positions now in the array
        //foreach(Vector3 position in spawnPositions)
        foreach(Vector3 position in spawnPositions.Reverse())
        {
            // grab the color from the array that stores the colors
            Color c = pix[counter];

            // now spawn the objects at each corresponding spaan position
            if(c.Equals(Color.white)) {

                // we spawn a ground tile at the specified position and with no rotation
                Instantiate(groundObject, position, Quaternion.identity);

            }
            else if(c.Equals(Color.black)) {

                // we spawn a wall tile at the specified position and with no rotation
                Instantiate(wallObject, position, Quaternion.identity);

            } else if(c.Equals(new Color(1.0f, 1.0f, 0.0f))) {
            
                // we spawn a enemy intermediary tower at the specified position and with no rotation
                Instantiate(enemyIntermediaryTowerObject, position, Quaternion.identity);

            } else if(c.Equals(new Color(0.0f, 0.5019608f, 0.5019608f))) {
            
                // we spawn a friendly intermediary tower at the specified position and with no rotation
                Instantiate(friendlyIntermediaryTowerObject, position, Quaternion.identity);

            } else if(c.r.ToString() == "1" && c.g.ToString("0.#######") == "0.6470588" && c.b.ToString() == "0") {
                
                // we spawn the enemy main tower at the specified position and with no rotation
                Instantiate(enemyMainTowerObject, position, Quaternion.identity);

            }
            else if(c.r.ToString("0.#######") == "0.7529413" && c.g.ToString("0.#######") == "0.7529413" && c.b.ToString("0.#######") == "0.7529413") {

                // we spawn a center object at the specified position and with no rotation
                Instantiate(centerObject, position, Quaternion.identity);

            }
            else if(c.r.ToString() == "0" && c.g.ToString() == "0" && c.b.ToString("0.#######") == "0.9137256") {
            
                // we spawn the friendly main tower at the specified position and with no rotation
                Instantiate(friendlyMainTowerObject, position, Quaternion.identity);

            }
            else {
                UnityEngine.Debug.Log("Current Color: " + c.r + " " + c.g + " " + c.b + " " + c.a);
            }

            counter++;
        }

        AssetDatabase.Refresh();
        ReadFromCoordianteFile();
        //Instantiate(friendlyMainTowerObject, new Vector3(friendly_xCoordinate, friendly_yCoordinate, 0), Quaternion.identity);
        //Instantiate(enemyMainTowerObject, new Vector3(enemy_xCoordinate, enemy_yCoordinate, 0), Quaternion.identity);
        //Instantiate(friendlyMainTowerObject, new Vector3(friendly_yCoordinate, friendly_xCoordinate, 0), Quaternion.identity);
        //Instantiate(enemyMainTowerObject, new Vector3(enemy_yCoordinate, enemy_xCoordinate, 0), Quaternion.identity);


    }

    
}
