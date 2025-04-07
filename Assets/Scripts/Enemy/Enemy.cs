using UnityEngine;
using System.Linq;
using System.Collections;
using System.IO;
using System;
using Unity.VisualScripting;

public class Enemy : Entity
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // This is to make it so that the enemy will only shoot at a certain distance
    private float _distanceToShoot = 5f;
    private Transform _player;
     private Transform _objectToFollow;

    public GameObject spawnOnObject1;
    public GameObject spawnOnObject2;

    private Collider2D colider2DEnemy;
    private Collider2D colider2DFloor1;
    private Collider2D colider2DFloor2;

    // this variable stores whether it is multiplayer or singleplayer.
    [SerializeField] public bool multiplayer = false;
    [SerializeField] public string singleplayerTag;
    [SerializeField] public string multiplayerTag;

    private PlayerAwarenessController _playerAwarenessController;
    private MinionAwarenessController _minionAwarenessController;


    void Start()
    {
        this.Speed = 5f;
        this.Hp = 10;
        this.Dmg = 1;

        
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _minionAwarenessController = GetComponent<MinionAwarenessController>();

        // this finds the player's transform/location
        //_player = FindAnyObjectByType<Player>().transform;
        if (multiplayer == true) {
            _player = GameObject.FindGameObjectsWithTag(multiplayerTag)[0].transform;
            //_player = _playerAwarenessController.getEnemyPlayerOptionB().transform;
        } else {
            _player = GameObject.FindGameObjectsWithTag(singleplayerTag)[0].transform;
            //_player = _playerAwarenessController.getEnemyPlayerOptionA().transform;
        }

        colider2DEnemy = GetComponent<Collider2D>();
        colider2DFloor1 = spawnOnObject1.GetComponent<Collider2D>();
        colider2DFloor2 = spawnOnObject2.GetComponent<Collider2D>();
        UnityEngine.Debug.Log("Object Type: " + colider2DEnemy.gameObject);
        if (colider2DEnemy.IsTouching(colider2DFloor1) || colider2DEnemy.IsTouching(colider2DFloor2)) {
            Destroy(this);
        }
        
        //Physics2D.IgnoreCollision(colider2DEnemy, colider2DFloor);
    }

    // Update is called once per frame
    void Update()
    {        
        bool playerAwarenessControllerCheck = _playerAwarenessController.AwareOfPlayer;
        bool minionAwarenessControllerCheck = _minionAwarenessController.AwareOfEnemyMinion;
        _objectToFollow = _player;


        bool overrideMinionAwarenessController = false;

        if (playerAwarenessControllerCheck == true) {
            overrideMinionAwarenessController = true;
        } else {
            overrideMinionAwarenessController = false;
        }

        if (overrideMinionAwarenessController == false) {
            if (minionAwarenessControllerCheck == true) {
                _objectToFollow = _minionAwarenessController.get__enemy_minion_transform();
            }
        }
        
        
        if(Vector2.Distance(_objectToFollow.position, transform.position) <= _distanceToShoot)
        {
            Fire();
        }
        
        
    }


}
