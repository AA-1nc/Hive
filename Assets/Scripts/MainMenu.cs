using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string Name;

    [SerializeField] private SceneSwitcher sceneSwitcher;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameObject nameInputBox;

    private void Start()
    {
        Name = PlayerPrefs.GetString("Name");
        nameInputBox.SetActive(Name == string.Empty);
    }

    public void PlayGame()
    {
        sceneSwitcher.LoadScene("Game");
    }

    public void SetName()
    {
        if (nameInput.text == string.Empty) return;

        PlayerPrefs.SetString("Name", nameInput.text);
        PlayerPrefs.Save();

        Name = PlayerPrefs.GetString("Name");
        nameInputBox.SetActive(false);
    }
}
