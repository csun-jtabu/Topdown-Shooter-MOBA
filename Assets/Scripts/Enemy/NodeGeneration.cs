using System.Collections.Generic;
using UnityEngine;

public class NodeGeneration : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    // this field will store the image we'll be using for the map
    [SerializeField]
    private Texture2D image;

    // this field will store the node prefab
    public Node nodePrefab;
    public List<Node> nodeList;
    
    public EnemyMovement enemy;
    public bool canDrawGizmos;

    void Awake()
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

        // for loops to traverse the world's spawn points
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
        
        // reset counter
        counter = 0;

        // we check all the positions now in the array
        foreach(Vector3 position in spawnPositions)
        {
            // grab the color from the array that stores the colors
            Color c = pix[counter];

            // now spawn the nodes at each corresponding spaan position
            if(c.Equals(Color.white)) 
            {
                // we spawn the ground tile at the specified position and with no rotation
                Node node = Instantiate(nodePrefab, position, Quaternion.identity);
                nodeList.Add(node);
            }
            counter++;
        }
        CreateConnections();
    }

    void CreateConnections()
    {
        // basically iterate for every node in the nodeList
        for(int i = 0; i < nodeList.Count; i++)
        {
            // compare with every other node in the nodeList
            for(int j = i + 1; j < nodeList.Count; j++)
            {
                // only create a connection if the nodes are close enough (1 unit distance)
                if(Vector2.Distance(nodeList[i].transform.position, nodeList[j].transform.position) <= 1.0f)
                {
                    // basically makes the nodes neighbors
                    ConnectNodes(nodeList[i], nodeList[j]);
                    ConnectNodes(nodeList[j], nodeList[i]);
                }
            }
        }
        canDrawGizmos = true;

    }

    void ConnectNodes(Node from, Node to)
    {
        // makes sure we don't put a connection between a node to itself
        if(from == to)
        {
            return;
        }
        else
        {
            from.neighbors.Add(to);
        }
    }

    void OnDrawGizmos()
    {
        if(canDrawGizmos == true)
        {
            Gizmos.color = Color.blue;
            for(int i = 0; i < nodeList.Count; i++)
            {
                for(int j = 0; j < nodeList[i].neighbors.Count; j++)
                {
                    Gizmos.DrawLine(nodeList[i].transform.position, nodeList[i].neighbors[j].transform.position);
                }
            }
        }    
    }
}
