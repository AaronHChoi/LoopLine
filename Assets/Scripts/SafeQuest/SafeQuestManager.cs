using UnityEngine;

public class SafeQuestManager : MonoBehaviour
{
    [SerializeField] private int[] result, correctCombination;
    void Start()
    {
        //result = new int[] { 1, 1, 1};
        //correctCombination = new int[] { 2, 4, 3};
       
    }

    private void OnEnable()
    {
        Dial.OnDialRotated += CheckResult;
    }
    private void OnDisable()
    {
        Dial.OnDialRotated -= CheckResult;
    }

    private void CheckResult(string dialName, int indexShown)
    {
        switch (dialName)
        {
            case "Dial1":
                result[0] = indexShown;
                break;
            case "Dial2":
                result[1] = indexShown;
                break;
            case "Dial3":
                result[2] = indexShown;
                break;
        }
        if (result[0] == correctCombination[0] &&
            result[1] == correctCombination[1] &&
            result[2] == correctCombination[2])
        {
            Debug.Log("Safe Unlocked!");
        }
    }


}
