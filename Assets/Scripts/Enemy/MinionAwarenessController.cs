using System;
using Unity.VisualScripting;
using UnityEngine;

public class MinionAwarenessController : MonoBehaviour
{
    // this is just a boolean variable to indicate whether minion is aware of enemy minion
    // we make the getters and setters too
    public bool AwareOfEnemyMinion{ get; private set;}
    
    // this stores the direction of the minion to the enemy minion
    public UnityEngine.Vector2 DirectionToEnemyMinion {get; private set; }

    [SerializeField]
    // distance at which the minion will notice the enemy minion.
    private float _minionAwarenessDistance; 

    // this variable will be used to locate the enemy minion.
    private Transform _minion;

    // this variable stores the relevant enemy minion.
    [SerializeField] public GameObject enemyMinion;
    [SerializeField] public string enemyMinionTag;

    // when the scene is first loaded
    private void Awake()
    {
        //_minion = GameObject.FindGameObjectsWithTag(enemyMinionTag)[0].transform;

        // this finds the enemy minion's transform/location.
        // modified code from here: https://discussions.unity.com/t/find-nearest-object-with-tag/750830/6
        Vector3 pos = this.transform.position;
        float dist = float.PositiveInfinity;
        foreach(var obj in GameObject.FindGameObjectsWithTag(enemyMinionTag)) {
            var d = (pos - obj.transform.position).sqrMagnitude;
            if(d < dist) {
                _minion = obj.transform;
                dist = d;
            }
        }

    }

    // get _minion element from the MinionAwarenessController.
    public Transform get__enemy_minion_transform(){
        return _minion;
    }

    // get enemy minion option.
    public GameObject getEnemyMinion() {
        return enemyMinion;
    }
    

    // Update runs at the speed of the frames
    // FixedUpdate runs at a constant rate
    void Update()
    {
        try {
            // this gets the distance between the minion and enemy minion.
            Vector2 minionToEnemyMinionVector = _minion.position - transform.position;
            // we then normalize the magnitude (turns to 1) to get just the direction.
            DirectionToEnemyMinion = minionToEnemyMinionVector.normalized;

            // if enemy is close enough it will be aware of the player
            if (minionToEnemyMinionVector.magnitude <= _minionAwarenessDistance) {
                AwareOfEnemyMinion = true;
            } else {
                AwareOfEnemyMinion = false;
                Awake();
            }
        } catch (Exception) {
            Awake();
        }

    }
}
