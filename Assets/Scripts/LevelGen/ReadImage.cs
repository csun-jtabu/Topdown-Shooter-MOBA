using UnityEngine;

public class ReadImage : MonoBehaviour
{
    // this field will store the image we'll be using for the map
    [SerializeField]
    private Texture2D image;

    // this field will store the object prefab for the walls
    [SerializeField]
    private GameObject wallObject;
    // this field will store the tile prefab for the ground/floor
    [SerializeField]
    private GameObject groundObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // this is how we get the pixels from the image
        // the array will hold the color
        // goes from bottom left of image to top right
        Color[] pix = image.GetPixels();

        // this is the dimensions of the map. basically equal to the image dimensions
        int worldX = image.width;
        int worldY = image.height;

        // each pixel position corresponds to a spawn position in world
        Vector3[] spawnPositions = new Vector3[pix.Length];
        Debug.Log("This is how many pixels we have: " + spawnPositions.Length);
        // this will reference the center of the world
        Vector3 startingSpawnPosition = new Vector3(-Mathf.Round(worldX/2), -Mathf.RoundToInt(worldY/2), 0);
        // we will be iterating the position through the position in the world so we use a new variable
        Vector3 currentSpawnPos = startingSpawnPosition;

        // this will help us track where we are in the array
        int counter = 0;

        // for loops to traverse the world's span points
        // left to right, bottom to top
        for(int y = 0; y < worldY; y++)
        {
            for(int x = 0; x < worldX; x++)
            {
                // we're creating a new spawn position in the spawnPositions array
                spawnPositions[counter] = currentSpawnPos;
                counter++;
                // this traverses through the row
                currentSpawnPos.x++; 
            }

            // once we reach the end of the current row, we want to go back to the beginning of the next row
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
        foreach(Vector3 position in spawnPositions)
        {
            // grab the color from the array that stores the colors
            Color c = pix[counter];

            // now spawn the objects at each corresponding spaan position
            if(c.Equals(Color.white)) 
            {
                // we spawn the ground tile at the specified position and with no rotation
                Instantiate(groundObject, position, Quaternion.identity);
            }
            else if(c.Equals(Color.black))
            {
                // we spawn the wall tile at the specified position and with no rotation
                Instantiate(wallObject, position, Quaternion.identity);
            }
            else
            {
                // we spawn the wall tile (temporarily) at the specified position and with no rotation
                Instantiate(groundObject, position, Quaternion.identity);
            }
            counter++;
        }

    }

    
}
