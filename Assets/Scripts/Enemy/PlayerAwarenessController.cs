using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    // this is just a boolean variable to indicate whether enemy is aware of player
    // we make the getters and setters too
    public bool AwareOfPlayer{ get; private set;}
    
    // this stores the direction of the enemy to the player
    public UnityEngine.Vector2 DirectionToPlayer {get; private set; }

    [SerializeField]
    // distance at which the enemy will notice the player
    private float _playerAwarenessDistance; 

    // this variable will be used to locate the player
    private Transform _player;

    // when the scene is first loaded
    private void Awake()
    {
        // this finds the player's transform/location
        _player = FindAnyObjectByType<PlayerMovement>().transform;
    }

    // Update runs at the speed of the frames
    // FixedUpdate runs at a constant rate
    void Update()
    {
        // this gets the distance between the player and enemy
        Vector2 enemyToPlayerVector =   _player.position - transform.position;
        // we then normalize the magnitude (turns to 1) to get just the direction.
        DirectionToPlayer = enemyToPlayerVector.normalized;

        // if enemy is close enough it will be aware of the player
        if (enemyToPlayerVector.magnitude <= _playerAwarenessDistance)
        {
           AwareOfPlayer = true;
        }
        else
        {
           AwareOfPlayer = false;
        }

    }
}
