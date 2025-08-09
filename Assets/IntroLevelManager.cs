using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroLevelManager : MonoBehaviour
{
    [SerializeField] float timeToChangeLevel;
    [SerializeField] string nextSceneName;
    [SerializeField] GameObject skipText;
    void Start()
    {
        StartCoroutine(ChangeNextLevelOnSeconds(timeToChangeLevel));
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            ShowSkip();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeNextLevel();
        }
    }
    private void ShowSkip()
    {
        if (!skipText.activeInHierarchy) skipText.SetActive(true);
    }
    private IEnumerator ChangeNextLevelOnSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ChangeNextLevel();
    }
    public void ChangeNextLevel()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
