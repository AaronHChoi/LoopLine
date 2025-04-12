using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevelopmentManager : MonoBehaviour
{

    [SerializeField] private GameObject UIPrinciplal;
    [SerializeField] private GameObject UIDeveloperMode;
    [SerializeField] private GameObject bgm;

    void Start()
    {
        GameManager.Instance.changeLoopTime = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && UIPrinciplal != null)
        {
            UIPrinciplal.SetActive(!UIPrinciplal.activeInHierarchy);
            UIDeveloperMode.SetActive(!UIDeveloperMode.activeInHierarchy);
        }

        if (UIDeveloperMode.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MenuLevel()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Mute()
    {
        bgm.SetActive(!bgm.activeInHierarchy);
    }

    public void LoadMainLevel()
    {
        if (SceneManager.GetActiveScene().name == "ThinkingWorld")
        {
            SceneManager.LoadScene("Main");
        }
    }

    public void CutTime()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            if (GameManager.Instance.LoopTime > 5f)
            {
                GameManager.Instance.changeLoopTime = true;
            }
            else
            {
                GameManager.Instance.changeLoopTime = false;
            }
        }    
    }
}
