using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //Parameters
    [Header("Parameters")]
    [SerializeField] private float forwardMooveSpeed = 70f;
    [SerializeField] private float backwardMooveSpeed = 30f;
    [SerializeField] [Range(0.005f, 0.05f)] private float movementAcceleration = 0.01f;

    [SerializeField] private float rotateSpeed = 0.5f;
    [SerializeField][Range (0.005f,0.05f)] private float rotationAcceleration = 0.01f;
    [SerializeField] private float tiltSpeed = 0.01f;
    [SerializeField] private float tiltMax = 30f;

    [SerializeField] private float boostChargeClock = 1.3f;
    [SerializeField] private float boostCooldownClock = 5f;
    [SerializeField] private float boostSpeed = 150f;
    [SerializeField] [Range(0.05f, 0.5f)] private float boostAcceleration = 0.1f;
    [SerializeField] private float boostActiveTime= 2f;

    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float shootDelay = 1.3f;

    //references
    [Space(2)][Header("References")]
    [SerializeField] Projectile _projectilePrefab;
    private Rigidbody rb;
    private Camera _camera;

    //state
    private Vector2 movementInput;
    private float currentRotation = 0;
    private float targetRotation = 0;

    private float tiltInput = 0;
    private float tilt = 0;

    private float currentSpeed = 0;
    private float targetSpeed = 0;

    private bool boostPressed=false;
    private float boostChargeTimer = 0;
    private bool isBoost = false;
    public bool IsBoost { get { return isBoost; }private set { isBoost = value; } }
    private float boostCooldownTimer = 0;

    private bool isShooting=false;
    Coroutine shootingCoroutine;

    //raycast
    private float distance = 100f;
    [SerializeField] private LayerMask layer;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }
    void Update()
    {
        Movement();
        ChargeBoost();
    }

    private void InputMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hit, distance, layer)){

        }
    }

    private void Movement()
    {
        tiltInput = 0;
        //accelerate rotation
        if (movementInput.x != 0)
        {
            targetRotation = rotateSpeed * Mathf.Sign(movementInput.x);
            tiltInput = -movementInput.x;
        }
        else targetRotation = 0;
        currentRotation =Mathf.Lerp(currentRotation,targetRotation, rotationAcceleration);
        float currTilt = transform.rotation.eulerAngles.z;
        if (tiltInput!=0)
        {
            if (currTilt < tiltMax || currTilt > 360 - tiltMax)
                tilt = tiltSpeed * tiltInput;
            else
                tilt = 0;
        }
        else if (currTilt < 0.7f || currTilt > 359.3f)
        {
            transform.eulerAngles= new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            tilt = 0;
        }
        else tilt = currTilt <= 180 ? -tiltSpeed : tiltSpeed;
        transform.eulerAngles += new Vector3(0, currentRotation, tilt);

        //accelerate speed
        if (isBoost)
        {
            targetSpeed = boostSpeed;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, rotationAcceleration);
        }
        else
        {
            if (movementInput.y != 0)
            {
                targetSpeed = (movementInput.y > 0 ? forwardMooveSpeed : backwardMooveSpeed) * Mathf.Sign(movementInput.y);
            }
            else targetSpeed = 0;
            currentSpeed = Mathf.Lerp(Mathf.Sign(currentSpeed)* rb.velocity.magnitude, targetSpeed, movementAcceleration);
        }
        rb.velocity = transform.forward * currentSpeed;

    }
    private void ChargeBoost()
    {
        if (boostCooldownTimer < boostCooldownClock) boostCooldownTimer += Time.deltaTime;
        if (boostPressed&&!isBoost&& boostCooldownTimer >= boostCooldownClock) boostChargeTimer += Time.deltaTime;
        else boostChargeTimer = 0;
        if (boostChargeTimer >= boostChargeClock)
        {
            isBoost = true;
            boostCooldownTimer = 0;
            Invoke("TurnoffBoost", boostActiveTime);
        }
    }
    private void TurnoffBoost()
    {
        boostChargeTimer = 0; 
        isBoost = false;
    }
    public float GetBoostCharge()
    {
        return boostChargeTimer / boostChargeClock;
    } 
    public float GetBoostCooldown()
    {
        return 1-(boostCooldownTimer / boostCooldownClock);
    }
    //Input Methods 
    public void MovementInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    public void BoostInput(InputAction.CallbackContext context)
    {
        boostPressed = context.phase == InputActionPhase.Performed;
    }

    public void ShootProjectile(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            isShooting = true;
            if(shootingCoroutine==null)shootingCoroutine=StartCoroutine(Shoot());
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            isShooting = false;
            shootingCoroutine = null;
        }
    }
    private IEnumerator Shoot()
    {
        while (isShooting)
        {
            Projectile p = Projectile.Shoot(_projectilePrefab, transform.forward, projectileSpeed);
            p.transform.position = transform.position;
            yield return new WaitForSeconds(shootDelay);
        }
    }
}
