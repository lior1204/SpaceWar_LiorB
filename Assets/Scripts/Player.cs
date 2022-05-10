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

    [SerializeField] private float boostChargeClock = 1.3f;
    [SerializeField] private float boostCooldown = 5f;
    [SerializeField] private float boostSpeed = 150f;
    [SerializeField] [Range(0.05f, 0.5f)] private float boostAcceleration = 0.1f;
    [SerializeField] private float boostActiveTime= 2f;

    [SerializeField] private float projectileSpeed = 20f;


    //references
    [Space(2)][Header("References")]
    [SerializeField] Projectile _projectilePrefab;
    private Rigidbody rb;

    //state
    private Vector2 movementInput;
    private float currentRotation = 0;
    private float targetRotation = 0;

    private float currentSpeed = 0;
    private float targetSpeed = 0;

    private bool boostPressed=false;
    private float boostChargeTimer = 0;
    private bool isBoost = false;
    private float boostCooldownTimer = 0;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Movement();
        ChargeBoost();
    }

    

    private void Movement()
    {
        //accelerate rotation
        if (movementInput.x != 0)
        {
            targetRotation = rotateSpeed * Mathf.Sign(movementInput.x);
        }
        else targetRotation = 0;
        currentRotation =Mathf.Lerp(currentRotation,targetRotation, rotationAcceleration);
        transform.Rotate(0,currentRotation, 0);//rotate

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
        if (boostCooldownTimer < boostCooldown) boostCooldownTimer += Time.deltaTime;
        if (boostPressed&&!isBoost&& boostCooldownTimer > boostCooldown) boostChargeTimer += Time.deltaTime;
        else boostChargeTimer = 0;
        if (boostChargeTimer >= boostChargeClock)
        {
            isBoost = true;
            Invoke("TurnoffBoost", boostActiveTime);
        }
    }
    private void TurnoffBoost()
    {
        boostChargeTimer = 0; 
        boostCooldownTimer = 0;
        isBoost = false;
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
            Projectile p = Projectile.Shoot(_projectilePrefab, transform.forward, projectileSpeed);
            p.transform.position = transform.position;
        }
    }

}
