using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] // this allows us to edit in inspector/unity gui
    private float _speed;  // this controls the speed of the player
    
    [SerializeField]
    private float _rotationSpeed;
    private Rigidbody2D _rigidBody;  // this 
    private UnityEngine.Vector2 _movementInput; // this is where input is stored
    private UnityEngine.Vector2 _smoothedMovementInput;  // this is used to smooth the movement of the player
    private UnityEngine.Vector2 _movementInputSmoothVelocity; // this keeps track of the velocity of movement
    

    // This is the method called when the scene first starts
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>(); // we store ridigbody to variable to allow for us to manipulate the component
    }

    // This is the method called at fixed time intervals when running
    private void FixedUpdate()
    {
        SetPlayerVelocity();
        RotateInDirectionOfInput();
    }

    private void SetPlayerVelocity()
    {
        _smoothedMovementInput = UnityEngine.Vector2.SmoothDamp( // this basically smooths the movement
                    _smoothedMovementInput, // this will be the initial movement
                    _movementInput,  // this is what we're smoothing towards
                    ref _movementInputSmoothVelocity, // this is where we store the velocity for smoothing 
                    0.1f);  // the seconds it takes to go from smoothVelocity to linear velocity
        // The player then moves to wherever our input tells it to go at _speed
        _rigidBody.linearVelocity = _smoothedMovementInput * _speed;
    }

    private void RotateInDirectionOfInput()
    {
        // If there is user input
        if(_movementInput != UnityEngine.Vector2.zero)
        {
            // creates a tuple of 4 rotation values that defines where we want the object to face
            // transform.forward is the current direction the object is facing. smootherMovementInput is the goal
            UnityEngine.Quaternion targetRotation = UnityEngine.Quaternion.LookRotation(transform.forward, _smoothedMovementInput);
            // this will cause the actual rotation from the current state to the new state
            UnityEngine.Quaternion rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            // apply the rotation to the object
            _rigidBody.MoveRotation(rotation);
        }
    }
    // This is the method called when there is input/movement
    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<UnityEngine.Vector2>(); // we get the input from the player
    }

}
