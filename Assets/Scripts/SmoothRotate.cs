using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 10;

    private Quaternion targetRotation;

    public void Rotate(Quaternion rot)
    {
        if (targetRotation == null)
            transform.rotation = rot;

        targetRotation = rot;
    }

    private void Update()
    {
        if (targetRotation == null) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
