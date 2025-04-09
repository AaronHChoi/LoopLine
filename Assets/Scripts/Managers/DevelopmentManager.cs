using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevelopmentManager : MonoBehaviour
{

    public bool developmentMode { get; private set; } = false;
    [SerializeField] private KeyCode developmentHacks;
    [SerializeField] private KeyCode resetLevel;
    [SerializeField] private KeyCode MenuLevel;
    [SerializeField] private KeyCode Mute;
    [SerializeField] private GameObject bgm;

    void Start()
    {
        developmentMode = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(developmentHacks))
        {
            developmentMode = !developmentMode;;
            Debug.Log("Development Mode: " + developmentMode);
        }
        if (developmentMode)
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(resetLevel))
            {
                Debug.Log("Comando CTRL + " + (resetLevel.ToString()) + " presionado");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            } 
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(MenuLevel))
            {
                Debug.Log("Comando CTRL + " + (MenuLevel.ToString()) + " presionado");
                SceneManager.LoadScene("MainMenu");
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(Mute))
            {
                Debug.Log("Comando CTRL + " + (MenuLevel.ToString()) + " presionado");
                bgm.SetActive(!bgm.activeInHierarchy);
            }
        }
    }
}
