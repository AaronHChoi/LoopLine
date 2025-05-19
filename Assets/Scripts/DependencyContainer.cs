using UnityEngine;

public class DependencyContainer : MonoBehaviour
{
    public static DependencyContainer Instance { get; private set; }

    public UIManager UIManager { get; private set; }
    public DevelopmentManager DevelopmentManager { get; private set; }
    public Subject SubjectEventManager { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SubjectEventManager = FindFirstObjectByType<Subject>();
        DevelopmentManager = FindFirstObjectByType<DevelopmentManager>();
        UIManager = FindFirstObjectByType<UIManager>();
    }
}
