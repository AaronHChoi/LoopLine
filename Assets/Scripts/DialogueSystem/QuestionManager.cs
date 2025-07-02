using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Transform optionsContainer;
    private List<Button> poolButtons = new List<Button>();

    public void ActivateButtons(int _amount, string _title, Options[] _options)
    {
        questionText.text = _title;

        if(poolButtons.Count >= _amount)
        {
            for (int i = 0; i < poolButtons.Count; i++)
            {
                if (i < _amount)
                {
                    int index = i;
                    poolButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = _options[i].option;
                    poolButtons[i].onClick.RemoveAllListeners();
                    //DialogueSO dial = _options[i].dialogue;
                    poolButtons[i].onClick.AddListener(() =>
                    {
                        _options[index].Choosen = true;
                        //_options[index].Hide = true;
                        GiveFunctionToTheButton(_options[index].dialogue);
                    });
                    poolButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    poolButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            int remainingAmount = (_amount - poolButtons.Count);
            for (int i = 0; i < remainingAmount; i++)
            {
                var newButton = Instantiate(buttonPrefab, optionsContainer).GetComponent<Button>();
                newButton.gameObject.SetActive(true);
                poolButtons.Add(newButton);
            }
            ActivateButtons(_amount, _title, _options);
        }
    }
    public void GiveFunctionToTheButton(DialogueSO _dialogue)
    {
        DialogueManager.Instance.SetDialogue(_dialogue, null);
    }
}
