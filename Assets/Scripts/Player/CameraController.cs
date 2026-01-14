using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector2 panningMultiplier;
    [SerializeField] private float lerpSpeed = 5;

    private Vector2 screenSize;
    private Vector2 targetCamPos;

    private void Start()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 mousePos = Input.mousePosition;
        targetCamPos = (mousePos / screenSize * 2 - Vector2.one) * panningMultiplier;

        transform.position = Vector2.Lerp(transform.position, targetCamPos, Time.unscaledDeltaTime * lerpSpeed);
    }

    public void GameOver()
    {
        panningMultiplier = Vector2.zero;
        lerpSpeed = 10;
    }
}
