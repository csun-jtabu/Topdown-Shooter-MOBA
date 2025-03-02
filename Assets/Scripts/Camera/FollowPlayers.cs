using System.Numerics;
using UnityEngine;

public class FollowPlayers : MonoBehaviour
{
    [SerializeField]
    private float _followSpeed = 2f; // this is the speed the camera will follow
    [SerializeField]
    private Transform targetPlayer; // this is what we'll be tracking
    private Transform targetPlayer2; // this is also what we'll be tracking

    // Update is called once per frame
    void Update()
    {
        // basically gets the player's position
        UnityEngine.Vector3 newPosition = new UnityEngine.Vector3(targetPlayer.position.x, targetPlayer.position.y, -10);
        // basically moves the camera to the player
        transform.position = newPosition;
    }
}
