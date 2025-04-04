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

    // this variable stores the relevant enemy players.
    [SerializeField] public GameObject enemyPlayerOptionA;
    [SerializeField] public GameObject enemyPlayerOptionB;

    // this variable stores whether it is multiplayer or singleplayer.
    [SerializeField] public bool multiplayer = false;
    [SerializeField] public string singleplayerTag;
    [SerializeField] public string multiplayerTag;

    // when the scene is first loaded
    private void Awake()
    {
        // this finds the player's transform/location
        //_player = FindAnyObjectByType<Player>().transform;
        if (multiplayer == true) {
            _player = GameObject.FindGameObjectsWithTag(multiplayerTag)[0].transform;
            //_player = enemyPlayerOptionB.transform;
            //_player = GameObject.Find(enemyPlayerOptionB.name).transform;
        } else {
            _player = GameObject.FindGameObjectsWithTag(singleplayerTag)[0].transform;
            //_player = enemyPlayerOptionA.transform;
            //_player = GameObject.Find(enemyPlayerOptionA.name).transform;
        }

    }

    // get _player element from the PlayerAwarenessController.
    public Transform get__player_transform(){
        return _player;
    }

    // get multiplayer boolean.
    public bool getMultiplayerBoolean(){
        return multiplayer;
    }

    // get player options.
    public GameObject getEnemyPlayerOptionA() {
        return enemyPlayerOptionA;
    }
    public GameObject getEnemyPlayerOptionB() {
        return enemyPlayerOptionB;
    }
    

    // Update runs at the speed of the frames
    // FixedUpdate runs at a constant rate
    void Update()
    {
        // this gets the distance between the player and enemy
        Vector2 enemyToPlayerVector = _player.position - transform.position;
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
