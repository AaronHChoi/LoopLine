using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevelopmentManager : MonoBehaviour
{

    private bool developmentMode = false;
    [SerializeField] private KeyCode developmentHacks;
    [SerializeField] private KeyCode resetLevel;
    [SerializeField] private KeyCode MenuLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(developmentHacks))
        {
            developmentMode = !developmentMode;
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
                SceneManager.LoadScene("Main Menu");
            }
        }
    }
}
