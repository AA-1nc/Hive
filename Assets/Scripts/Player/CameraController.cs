using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float panningMultiplier = 0.3f;
    [SerializeField] private float lerpSpeed = 5;

    private Vector2 screenSize;
    private Vector2 targetCamPos;

    private void Start()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        targetCamPos = (mousePos / screenSize * 2 - Vector2.one) * panningMultiplier;

        transform.position = Vector2.Lerp(transform.position, targetCamPos, Time.deltaTime * lerpSpeed);
    }
}
