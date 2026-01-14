using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private float sceneSwitchTime = 1.2f;

    private bool switchingScenes = false;
    private string sceneToSwitchTo;

    public void LoadScene(string scene)
    {
        if (switchingScenes) return;

        StartCoroutine(LoadSceneRoutine(scene));
    }

    private IEnumerator LoadSceneRoutine(string scene)
    {
        switchingScenes = true;
        GetComponent<Animator>().SetTrigger("SwitchScene");
        yield return new WaitForSecondsRealtime(sceneSwitchTime);
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }
}
