using UnityEngine;

public class WindowEvent : MonoBehaviour, IObserver, IDependencyInjectable
{
    Subject eventManager;
    [SerializeField] string id;
    [SerializeField] private GameObject crystal;
    [SerializeField] private GameObject crystalBreakEffect;

    void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void OnNotify(Events _event, string _id = null)
    {
        if (_event == Events.BreakCrystal)
        {
            breakCrystal();
        }
    }
    private void OnEnable()
    {
        if (eventManager != null)
            eventManager.AddObserver(this);
    }
    private void OnDisable()
    {
        if (eventManager != null)
            eventManager.RemoveObserver(this);
    }
    private void breakCrystal()
    {
        if (crystal != null)
        {
            crystal.SetActive(false);
        }
        crystalBreakEffect.SetActive(true);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        eventManager = provider.SubjectEventManager;
    }
}