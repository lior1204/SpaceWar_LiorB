using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 70f;
    [SerializeField] private float ProjectileSpeed = 20f;
    [SerializeField] Projectile _projectilePrefab;
    private Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movement.x * speed * Time.deltaTime, 0, movement.y * speed * Time.deltaTime);
    }

    public void MovementInput(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    public void ShootProjectile(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Projectile p = Projectile.Shoot(_projectilePrefab, transform.forward, ProjectileSpeed);
            p.transform.position = transform.position;
        }
    }
}
