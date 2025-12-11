using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float deletionTime = 3;

    private void Start()
    {
        Destroy(gameObject, deletionTime);
    }

    private void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }
}
