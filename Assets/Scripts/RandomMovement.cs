using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float range;

    private float xPos = 0;
    private float yPos = 0;

    private void Awake()
    {
        xPos = Random.value * 100f;
        yPos = Random.value * 100f;
    }

    private void Update()
    {
        xPos += Time.deltaTime * speed;
        yPos -= Time.deltaTime * speed;

        float x = Mathf.PerlinNoise1D(xPos) - 0.5f;
        float y = Mathf.PerlinNoise1D(yPos) - 0.5f;

        transform.localPosition = new Vector3(x, y, 0) * range;
    }
}
