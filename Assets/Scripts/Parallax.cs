using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxAmount;

    private void Update()
    {
        transform.position = (Camera.main.transform.position - new Vector3(0, 0, Camera.main.transform.position.z)) / parallaxAmount;
    }
}
