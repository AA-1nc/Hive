using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameOver : MonoBehaviour
{
    [SerializeField] private UnityEvent deathEvent;
    [SerializeField] private TextMeshProUGUI roundText;

    [SerializeField] private GameObject deathTitleText;
    [SerializeField] private GameObject congratsText;

    [SerializeField] private Transform leaderboardEntryParent;
    [SerializeField] private GameObject leaderboardEntryPrefab;
    [SerializeField] private Color playerColor;

    [SerializeField] private AudioSource[] musicSources;
    
    private int round;
    private Animator animator;

    private bool newScoreEntered = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        deathEvent.Invoke();

        round = FindObjectOfType<WaveSpawner>().wave;
        roundText.text += (round + 1);
        Time.timeScale = 0;

        foreach (AudioSource source in musicSources)
            source.Stop();
        
        AddLeaderboardEntry();
        DisplayLeaderboard();
    }

    private void AddLeaderboardEntry()
    {
        LootLockerSDKManager.SubmitScore(PlayerPrefs.GetString("playerIdentifier"), round + 1, "round", (response) =>
        {
            if (!response.success)
            {
                Debug.Log("Could not submit score!");
                Debug.Log(response.errorData.ToString());
                return;
            }
            Debug.Log("Successfully submitted score!");
        });
    }

    private void DisplayLeaderboard()
    {
        LootLockerSDKManager.GetScoreList("round", 10, 0, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("Could not get score list!");
                Debug.Log(response.errorData.ToString());
                return;
            }
            Debug.Log("Successfully got score list!");

            for (int i = 0; i < response.items.Length; i++)
            {
                GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardEntryParent);

                string place;
                if (i == 0)
                    place = "1ST";
                else if (i == 1)
                    place = "2ND";
                else if (i == 2)
                    place = "3RD";
                else
                    place = (i + 1) + "TH";

                entry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = place;
                entry.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = response.items[i].player.name;
                entry.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = response.items[i].score.ToString();

                if (response.items[i].player.id == PlayerPrefs.GetInt("playerId"))
                {
                    foreach (TextMeshProUGUI child in entry.transform.GetComponentsInChildren<TextMeshProUGUI>())
                        child.color = playerColor;

                    if (response.items[i].score - 1 == round)
                        newScoreEntered = true;
                }
            }
        });
    }

    private IEnumerator EndRoutine()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
                FindObjectOfType<SceneSwitcher>().LoadScene("MainMenu");
            yield return null;
        }
    }

    private IEnumerator ContinueToLeaderboard()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;

        if (newScoreEntered)
            animator.Play("WaveToCongrats");
        else
            animator.Play("ShowLeaderboard");
    }

    private IEnumerator ContinueToLeaderboardFromMessage()
    {
        while (!Input.GetMouseButtonDown(0))
            yield return null;

        roundText.gameObject.SetActive(false);
        deathTitleText.SetActive(false);
        congratsText.SetActive(true);

        animator.Play("ShowLeaderboard");
    }
}
