using System.Numerics;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private float _followSpeed = 2f; // this is the speed the camera will follow
    [SerializeField]
    private Transform target; // this is what we'll be tracking

    // Update is called once per frame
    void Update()
    {
        // basically gets the player's position
        UnityEngine.Vector3 newPosition = new UnityEngine.Vector3(target.position.x, target.position.y, -10);
        // basically moves the camera to the player
        transform.position = newPosition;
    }
}
