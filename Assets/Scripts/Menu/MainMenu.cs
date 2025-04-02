using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider sliderSensivity;

    [SerializeField] private string Scenename;
    [SerializeField] private GameObject CreditsPanel;
    [SerializeField] private GameObject ControlPanel;
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject BackButton;
    [SerializeField] private GameObject OptionButton;
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void LoadScene1()
    {
        SceneManager.LoadScene(Scenename);
    }
    public void QuitGame()
    {
       
        Application.Quit();
    }
    //public void OpenCredits()
    //{
    //    CreditsPanel.SetActive(true);
    //    MainMenuPanel.SetActive(false);
    //    BackButton.SetActive(true);
    //}
    //public void OpenOptions()
    //{
    //    OptionButton.SetActive(true);
    //    MainMenuPanel.SetActive(false);
    //    BackButton.SetActive(true);
    //}
    //public void OpenControls()
    //{
    //    ControlPanel.SetActive(true);
    //    MainMenuPanel.SetActive(false);
    //    BackButton.SetActive(true);
    //}
    //public void Close()
    //{
    //    MainMenuPanel.SetActive(true);
    //    CreditsPanel.SetActive(false);
    //    ControlPanel.SetActive(false);
    //    BackButton.SetActive(false);
    //    OptionButton.SetActive(false);
    //}
}
