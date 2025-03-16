using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2; // controls speed of enemy
    [SerializeField]
    private float _rotationSpeed; // controls how fast it rotates
    private Rigidbody2D _rigidbody; // this stores the reference to the body of the enemy
    private PlayerAwarenessController _playerAwarenessController; // this controls the behavior/logic of the Enemy
    private Vector2 _targetDirection; // this is where we want the enemy to go to
    
    //////////////////////////////////////////////////////////////////////////
    // This is to make it so that the enemy will stop advancing at a certain point
    private float _distanceToStop = 2f;
    // This variable will be used to locate the player
    private Transform _player;
    //////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////
    // This is the max time it will go either left or right
    private float _maxStrafeTime = 1f;
    private float _currentStrafeTime;
    // Variable to hold the direction the enemy is currently strafing toward
    private Vector2 _currentStrafeDirection;
    // Variable to decide whether to strafe left or right
    private float strafeDecider = 0;
    //////////////////////////////////////////////////////////////////////////
    
    //////////////////////////////////////////////////////////////////////////
    // Variables For Enemy attacks
    private float fireRate;
    private float timeToFire;
    public Transform firingPoint;


    // Plays when the scene starts
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); // reference to enemy rigidbody
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();  // references the PlayerAwareness Script
        _player = FindAnyObjectByType<Player>().transform; // this finds the player's transform/location
        timeToFire = fireRate;

    }

    void Update()
    {
        
    }

    // FixedUpdate is ran constantly
    void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        // if the enemy is far from the player
        if(Vector2.Distance(_player.position, transform.position) > _distanceToStop + 1f)
        {
            SetVelocity();
        }
        // if the enemy is too close to the player
        else if(Vector2.Distance(_player.position, transform.position) < _distanceToStop - 1f)
        {
            BackUp();
        }

        // // if the enemy is within the range of the player 
        if(Vector2.Distance(_player.position, transform.position) < _distanceToStop + 4f)
        {
            // will randomly strafe left or right
            if(Random.value > 0.8f)
            {
                Strafe();
            }
        }
        
    }

    // this will update where the enemy will head towards
    private void UpdateTargetDirection()
    {
        // if the enemy is aware of the player then it will move towards player
        if(_playerAwarenessController.AwareOfPlayer == true)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
        else  // if not, then it will idle
        {
            _targetDirection = Vector2.zero;
        }
    }

    // this will control where the enemy will face
    private void RotateTowardsTarget()
    {
        // if the enemy is not aware of the player do nothing / not moving
        if (_targetDirection == Vector2.zero)
        {
            return; 
        }
        else // if the enemy is aware of the player / moving
        {
            // calculates the rotation of the enemy
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
            // rotate the enemy by starting from its current rotation, then moving it to the targetRotation. Do it at the following speed
            // Time.delta turns the speed into per second instead of by frame
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            // this officially rotates the enemy
            _rigidbody.SetRotation(rotation);
            _rigidbody.angularVelocity = 0f; // this makes sure that the bullet doesn't affect rotation of enemy
        }
    }

    // this will set the velocity of the enemy
    private void SetVelocity()
    {
        // if the enemy is not aware of the player / not moving
        if (_targetDirection == Vector2.zero)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            return;
        }
        // if the enemy is aware of the player / moving
        else
        {
            // this moves the enemy at a certain speed to the calculated direction
            // transform.up is used to move forward
            _rigidbody.linearVelocity = transform.up * _speed;
        }
    }

    // this makes it so that the enemy backs up if too close to player
    private void BackUp()
    {
        // backs up if it gets too close to player
        _rigidbody.linearVelocity = transform.up * (_speed) * (-0.5f);
    }

    private void Strafe()
    {   
        
        if(_currentStrafeTime <= 0)
        {
            _currentStrafeDirection = Vector2.Perpendicular(_targetDirection);
            _currentStrafeTime = _maxStrafeTime;
            if(Random.value > 0.5f)
            {
                strafeDecider = 1;
            }
            else
            {
                strafeDecider = -1;
            }

        }

        // after deciding to go left or right, 
        _rigidbody.linearVelocity = _currentStrafeDirection * (_speed) * strafeDecider;

        // decreases time left until the enemy picks a new left/right direction
        _currentStrafeTime = _currentStrafeTime - Time.deltaTime;
        
    }
}
