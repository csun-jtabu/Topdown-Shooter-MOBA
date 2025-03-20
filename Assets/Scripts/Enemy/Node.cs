using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Node : MonoBehaviour
{

    public Node fromNode;
    public List<Node> neighbors;
    public float gScore;
    public float hScore;
    public float fScore;

    // this is to calculate the overall cost when it comes to the Astar algorithm
    public float FScore()
    {
        fScore = gScore + hScore;
        return fScore;
    }

    // this is just to show the connections between the nodes
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(neighbors.Count > 0)
        {
            for(int i = 0; i < neighbors.Count; i++)
            {
                Gizmos.DrawLine(transform.position, neighbors[i].transform.position);
            }
        }
            
    }
}
