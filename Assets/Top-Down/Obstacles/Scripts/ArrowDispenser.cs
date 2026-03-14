using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowDispenser : MonoBehaviour
{
    //Setting cardinal directions
    public enum FireDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    [Header("Arrow Setup")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Timing")]
    [SerializeField] private float fireInterval = 1.5f;
    [SerializeField] private bool startFiringOnStart = true;

    [Header("Direction")]
    [SerializeField] private FireDirection fireDirection = FireDirection.Right;

    private float fireTimer;
    private bool isFiring;

    private void Start()
    {
        //Establishing dispenser settings
        isFiring = startFiringOnStart;
        fireTimer = fireInterval;
    }

    private void Update()
    {
        //Checks if dispenser is not firing or arrow setup variables are invalid
        if (!isFiring || arrowPrefab == null || firePoint == null)
            return;

        fireTimer -= Time.deltaTime;

        //If appropriate fire interval has passed, fire next projectile and reset fire interval
        if (fireTimer <= 0f)
        {
            FireArrow();
            fireTimer = fireInterval;
        }
    }

    //Handles projectile firing
    private void FireArrow()
    {
        //Gets the fire direction
        Vector2 direction = GetDirectionVector();

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        //Gets arrow script and tells arrow which way to move after it is spawned
        ArrowProjectile projectile = arrow.GetComponent<ArrowProjectile>();
        if (projectile != null)
        {
            projectile.SetDirection(direction);
        }

        // Rotate sprite so it visually points in the fire direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    //Gets the direction vector
    private Vector2 GetDirectionVector()
    {
        switch (fireDirection)
        {
            case FireDirection.Up:
                return Vector2.up;
            case FireDirection.Down:
                return Vector2.down;
            case FireDirection.Left:
                return Vector2.left;
            case FireDirection.Right:
            default:
                return Vector2.right;
        }
    }

    //Dispenser begins firing
    public void StartFiring()
    {
        isFiring = true;
    }

    //Dispenser stops firing
    public void StopFiring()
    {
        isFiring = false;
    }
}
