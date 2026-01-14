using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float movementRadius = 3f;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float moveLerpSpeed = 10;
    [SerializeField] private float recoilRadius = 1.5f;
    [SerializeField] private float recoilLerpSpeed = 15;
    [SerializeField] private GameObject sprite;

    private float currentSpeed = 0;
    private float currentRadius;
    private float theta = 0;

    private Shoot shoot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        shoot = GetComponent<Shoot>();
        currentRadius = movementRadius;
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
        sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void HandleMovement()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, -Input.GetAxisRaw("Horizontal") * moveSpeed, Time.deltaTime * moveLerpSpeed);
        theta +=  currentSpeed * Time.deltaTime;

        currentRadius = Mathf.Lerp(currentRadius, movementRadius, Time.deltaTime * recoilLerpSpeed);
        transform.position = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * currentRadius;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * theta - 90);
    }

    private void HandleShooting()
    {
        if (Input.GetKey(KeyCode.W))
            shoot.ShootBullet();
    }

    public void BulletShotEvent()
    {
        currentRadius = recoilRadius;
    }
}
