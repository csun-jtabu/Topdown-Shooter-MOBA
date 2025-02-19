using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed; // controls speed of enemy
    [SerializeField]
    private float _rotationSpeed; // controls how fast it rotates
    private Rigidbody2D _rigidbody; // this stores the reference to the body of the enemy
    private PlayerAwarenessController _playerAwarenessController; // this controls the behavior/logic of the Enemy
    private Vector2 _targetDirection; // this is where we want the enemy to go to
    
    // Plays when the scene starts
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); // reference to enemy rigidbody
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();  // references the PlayerAwareness Script     
    }

    // FixedUpdate is ran constantly
    void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
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
}
