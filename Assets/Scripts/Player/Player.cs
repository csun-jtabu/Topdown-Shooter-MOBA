using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Player : Entity
{
    [SerializeField]
    private int MaxShield = 10;
    [SerializeField]
    private int Shield = 10;
    private float Spread = 0f;
    [SerializeField]
    private float RespawnTimer = 3f;
    public float ShieldRegenDelay = 5f;
    private bool ShieldDelayed = false;
    public bool secondController = false;

    public GameObject dashTrailPrefab;
    private Coroutine dashAnimationCoroutine;

    public GameObject graphics;

    [SerializeField] private float _speed;  // this controls the speed of the player
    [SerializeField] private float _walkSpeed = 10; // base walk speed
    [SerializeField] private float _runSpeed = 20; // sprint speed
    private WaitForSeconds sprintRefillTick = new WaitForSeconds(.1f); // .1 second delay for refilling stuff
    [SerializeField] private float sprintBar = 100; // sprint stamina
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private bool _canSprint = true;
    private Coroutine sprintRefillCoroutine; // coroutine to refill the sprint bar

    private float _dashSpeed = 20f;
    private float _dashDistance = 3f;
    private float _dashTime;
    private float _dashDuration = .2f;
    [SerializeField] private bool _isDashing = false;
    [SerializeField] private bool _canDash = true;
    private Coroutine dashRefillCoroutine;

    private float _dodgeSpeed = 10f;
    private float _dodgeTime;
    private float _dodgeDuration = .5f;
    [SerializeField] private bool _isDodging = false;
    [SerializeField] private bool _canDodge = true;
    private Coroutine dodgeRefillCoroutine;

    [SerializeField] private float _rotationSpeed;
    private Rigidbody2D _rigidBody;  // this 
    private UnityEngine.Vector2 _movementInput; // this is where input is stored
    private UnityEngine.Vector2 _smoothedMovementInput;  // this is used to smooth the movement of the player
    private UnityEngine.Vector2 _movementInputSmoothVelocity; // this keeps track of the velocity of movement


    // This is the method called when the scene first starts
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>(); // we store ridigbody to variable to allow for us to manipulate the component
    }

    public override void Damage(int damage, int team)
    {
        if (team != this.Team)
        {
            if (Shield == 0)
            {
                this.Hp -= damage;
                if (this.Hp <= 0)
                    Respawn();
            }
            else if (Shield >= damage)
                Shield -= damage;
            else
            {
                damage -= Shield;
                Shield = 0;
                this.Damage(damage, team);
            }
            if (this.Hp <= 0)
                Destroy(this.gameObject);
            else
            {
                ShieldDelayed = true;
                StartCoroutine(DelayShieldRegen());
            }
        }
    }

    public void Respawn()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.Shield = MaxShield;
    }

    IEnumerator ShieldRegen()
    {
        yield return new WaitForSeconds(2f);
        if (Shield < MaxShield && !ShieldDelayed)
            Shield++;
        StartCoroutine(ShieldRegen());
    }

    IEnumerator DelayShieldRegen()
    {
        yield return new WaitForSeconds(ShieldRegenDelay);
        ShieldDelayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        // checks if the player has stamina left
        if (sprintBar <= 0)
        {
            isSprinting = false;
            _speed = _walkSpeed;
        }
        // checks if the player is sprinting
        if (isSprinting)
        {
            sprintBar -= 5;
        }
        else
        {
            if (sprintBar < 100 && sprintRefillCoroutine == null)
            {
                sprintRefillCoroutine = StartCoroutine(RechargeSprint());
            }
        }
    }

    // This is the method called at fixed time intervals when running
    private void FixedUpdate()
    {
        if (_isDodging)
        {
            _canDodge = false;
            _canDash = false;
            _canSprint = false;
            ApplyDodge();
        }
        else
        {
            SetPlayerVelocity();
            RotateInDirectionOfInput();
        }

        if (Team == 1 && !secondController)
        {
            // convert mouse position into world coordinates
            UnityEngine.Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // get direction you want to point at
            UnityEngine.Vector2 direction = (mouseScreenPosition - (UnityEngine.Vector2)transform.position).normalized;

            // set vector of transform directly
            transform.up = direction;
        }

    }

    // this method is a coroutine that recharges the sprint bar
    private IEnumerator RechargeSprint()
    {
        yield return new WaitForSeconds(2);
        while (sprintBar < 100)
        {
            sprintBar += 10;
            yield return sprintRefillTick;
        }
        sprintBar = 100;
        sprintRefillCoroutine = null;
    }
    private IEnumerator RechargeDash()
    {
        yield return new WaitForSeconds(2);
        _canDash = true;
        dashRefillCoroutine = null;
    }
    private IEnumerator RechargeDodge()
    {
        yield return new WaitForSeconds(2);
        _canDodge = true;
        dodgeRefillCoroutine = null;
    }
    private IEnumerator DestroyDashTrail(GameObject dashTrail)
    {
        Animator dashTrailAnimator = dashTrail.GetComponent<Animator>();
        if (dashTrailAnimator != null)
        {
            yield return new WaitForSeconds(dashTrailAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
        Destroy(dashTrail);
    }

    private void ApplyDash()
    {
        UnityEngine.Vector2 dashDirection = transform.up;
        UnityEngine.Vector2 currentPosition = _rigidBody.position;
        GameObject dashTrail = Instantiate(dashTrailPrefab, currentPosition, UnityEngine.Quaternion.identity);
        float playerRotationZ = transform.rotation.eulerAngles.z;
        dashTrail.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, playerRotationZ - 90);
        if (dashAnimationCoroutine == null)
        {
            StartCoroutine(DestroyDashTrail(dashTrail));
        }
        _rigidBody.position += dashDirection * _dashDistance;
        _canDash = false;
        if (dashRefillCoroutine == null)
        {
            dashRefillCoroutine = StartCoroutine(RechargeDash());
        }
    }
    private void ApplyDodge() //needs a visual rotation
    {
        UnityEngine.Vector2 dodgeDirection = transform.up;
        _rigidBody.linearVelocity = dodgeDirection * _dodgeSpeed;
        graphics.transform.Rotate(0, 0, 15);

        if (Time.time - _dodgeTime >= _dodgeDuration)
        {
            graphics.transform.localRotation = UnityEngine.Quaternion.Euler(0, 0, 90);
            _isDodging = false;
            _canDodge = false;
            _canDash = true;
            _canSprint = true;
            if (dodgeRefillCoroutine == null)
            {
                dodgeRefillCoroutine = StartCoroutine(RechargeDodge());
            }
        }
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
        if (_movementInput != UnityEngine.Vector2.zero)
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
        //Debug.Log(_movementInput);
    }
    // This is the method called when the sprint button (left shift) is pressed
    private void OnSprint(InputValue inputValue)
    {
        float SprintingInput = inputValue.Get<float>();
        Debug.Log(SprintingInput);
        if (SprintingInput == 1 && _canSprint)
        {
            isSprinting = true;
            _speed = _runSpeed;
        }
        else
        {
            isSprinting = false;
            _speed = _walkSpeed;
        }
    }

    // This is the method called when the dash button (space) is pressed
    private void OnDash(InputValue inputValue)
    {
        // float dashInput = inputValue.Get<float>();
        // Debug.Log(dashInput);
        if (_canDash)
        {
            _canDash = false;
            _dashTime = Time.time;
            ApplyDash();
        }
    }

    private void OnAttack(InputValue inputValue)
    {
        Fire();
    }

    // This is the method called when the dodge button (c) is pressed
    private void OnDodge(InputValue inputValue)
    {
        //float dodgeInput = inputValue.Get<float>();
        //Debug.Log(dodgeInput);
        if (_canDodge)
        {
            _isDodging = true;
            _canDodge = false;
            _dodgeTime = Time.time;
        }
    }
}
