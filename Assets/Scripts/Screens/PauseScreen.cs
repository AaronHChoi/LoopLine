using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour, IScreen
{
    [SerializeField] private Button resumeButton;
    private void Awake()
    {
        resumeButton.onClick.AddListener(() => GameManager.Instance.ScreenManager.Pop());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.Instance.ScreenManager.Pop();
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Free()
    {
        gameObject.SetActive(false);
    }
}
