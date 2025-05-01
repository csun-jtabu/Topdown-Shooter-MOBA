using System;
using Unity.VisualScripting;
using UnityEngine;

public class TowerAwarenessController : MonoBehaviour
{
    // this is just a boolean variable to indicate whether tower is aware of enemy tower
    // we make the getters and setters too
    public bool AwareOfEnemyTower{ get; private set;}
    
    // this stores the direction of the tower to the enemy tower
    public UnityEngine.Vector2 DirectionToEnemyTower {get; private set; }

    [SerializeField]
    // distance at which the tower will notice the enemy tower.
    private float _towerAwarenessDistance; 

    // this variable will be used to locate the enemy tower.
    private Transform _tower;

    // this variable stores the relevant enemy tower.
    [SerializeField] public GameObject enemyTower;
    [SerializeField] public string enemyTowerTag;

    // when the scene is first loaded
    private void Awake()
    {
        //_tower = GameObject.FindGameObjectsWithTag(enemyTowerTag)[0].transform;

        // this finds the enemy tower's transform/location.
        // modified code from here: https://discussions.unity.com/t/find-nearest-object-with-tag/750830/6
        Vector3 pos = this.transform.position;
        float dist = float.PositiveInfinity;
        foreach(var obj in GameObject.FindGameObjectsWithTag(enemyTowerTag)) {
            var d = (pos - obj.transform.position).sqrMagnitude;
            if(d < dist) {
                _tower = obj.transform;
                dist = d;
            }
        }

    }

    // get _tower element from the TowerAwarenessController.
    public Transform get__enemy_tower_transform(){
        return _tower;
    }

    // get enemy tower option.
    public GameObject getEnemyTower() {
        return enemyTower;
    }
    

    // Update runs at the speed of the frames
    // FixedUpdate runs at a constant rate
    void Update()
    {
        try {
            // this gets the distance between the tower and enemy tower.
            Vector2 towerToEnemyTowerVector = _tower.position - transform.position;
            // we then normalize the magnitude (turns to 1) to get just the direction.
            DirectionToEnemyTower = towerToEnemyTowerVector.normalized;

            // if enemy is close enough it will be aware of the player
            if (towerToEnemyTowerVector.magnitude <= _towerAwarenessDistance) {
                AwareOfEnemyTower = true;
            } else {
                AwareOfEnemyTower = false;
                Awake();
            }
        } catch (Exception) {
            Awake();
        }

    }
}
