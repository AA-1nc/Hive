using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameOver : MonoBehaviour
{
    [SerializeField] private UnityEvent deathEvent;
    [SerializeField] private TextMeshProUGUI roundText;

    [SerializeField] private float clickTime = 4;

    private void Start()
    {
        deathEvent.Invoke();
        roundText.text += (FindObjectOfType<WaveSpawner>().wave + 1);
        Time.timeScale = 0;

        StartCoroutine(EndRoutine());
    }

    private IEnumerator EndRoutine()
    {
        yield return new WaitForSecondsRealtime(clickTime);

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
                FindObjectOfType<SceneSwitcher>().LoadScene("MainMenu");
            yield return null;
        }
    }
}
