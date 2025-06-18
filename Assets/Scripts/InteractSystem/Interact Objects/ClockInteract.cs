using UnityEngine;

public class ClockInteract : MonoBehaviour, IInteract
{
    [SerializeField] private string interactText;

    [SerializeField] private DialogueSO dialogueSO;
    private TimeManager timeManager;
    public string GetInteractText()
    {
        return interactText;
    }

    public void Interact()
    {
       
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeManager = FindFirstObjectByType<TimeManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        dialogueSO.Dialogues[0].dialogue = "Faltan " + timeManager.returnTimeInMinutes()+ " para llegar a la proxima estacion";
        //dialogueSO.ResetValues();
    }
}
