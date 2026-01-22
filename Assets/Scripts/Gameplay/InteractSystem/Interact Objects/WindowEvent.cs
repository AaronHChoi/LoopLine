using UnityEngine;
using Core.DependencyInjection;
public class WindowEvent : MonoBehaviour, IObserver
{
    IEventManager eventManager;
    [SerializeField] private GameObject crystal;
    [SerializeField] private GameObject crystalBreakEffect;
    public string Id { get; }

    private void Start()
    {
        eventManager = InterfaceDependencyInjector.Instance.Resolve<IEventManager>();
    }
    public void OnNotify(Events _event, string _id = null)
    {
        if (_event == Events.BreakCrystal)
            breakCrystal();
    }
    private void OnEnable()
    {
        eventManager.AddNewObserver(this);
    }
    private void OnDisable()
    {
        eventManager.RemoveOldObserver(this);
    }
    private void breakCrystal()
    {
        if (crystal != null)
        {
            crystal.SetActive(false);
        }
        crystalBreakEffect.SetActive(true);
    }
    public string GetObserverID()
    {
        return Id;
    }
}