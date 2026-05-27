using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneSwitcher sceneSwitcher;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameObject nameInputBox;

    [SerializeField] private TextMeshProUGUI testText;

    private string playerIdentifier;

    private void Start()
    {
        playerIdentifier = PlayerPrefs.GetString("playerIdentifier");
        nameInputBox.SetActive(playerIdentifier == string.Empty);

        if (playerIdentifier == string.Empty)
        {
            LootLockerSDKManager.StartGuestSession((response) =>
            {
                if (!response.success)
                {
                    testText.text = "Error starting LootLocker session";
                    Debug.Log("Error starting LootLocker session");
                    return;
                }

                testText.text = "Successfully starting LootLocker session";
                Debug.Log("Successfully started LootLocker session");
                playerIdentifier = response.player_identifier;
                PlayerPrefs.SetInt("playerId", response.player_id);
            });
        }
        else
        {
            LootLockerSDKManager.StartGuestSession(playerIdentifier, (response) =>
            {
                if (!response.success)
                {
                    testText.text = "Error starting LootLocker session from PI";
                    Debug.Log("Error starting LootLocker session from PI");
                    return;
                }

                testText.text = "Successfully started LootLocker session from PI to " + response.player_name;
                Debug.Log("Successfully started LootLocker session from PI");
                PlayerPrefs.SetInt("playerId", response.player_id);
            });
        }
    }

    public void PlayGame()
    {
        sceneSwitcher.LoadScene("Game");
    }

    public void SetName()
    {
        if (nameInput.text == string.Empty) return;

        nameInputBox.SetActive(false);

        SetLeaderboardName(nameInput.text);
    }

    private void SetLeaderboardName(string name)
    {
        if (name == string.Empty) return;

        LootLockerSDKManager.SetPlayerName(name, (response) =>
        {
            if (!response.success)
            {
                testText.text = "error setting player name";
                Debug.Log("error setting player name");
                return;
            }

            Debug.Log("Successfully stored player's name");
            testText.text = "Successfully setting player name";
            PlayerPrefs.SetString("playerIdentifier", playerIdentifier);
            PlayerPrefs.Save();
        });
    }
}
