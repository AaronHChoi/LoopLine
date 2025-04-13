using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Objects/Dialogue Round")]
public class DialogueRoundSO : ScriptableObject
{
    [SerializeField] private List<DialogueTurn> dialogueTurnsList;
    public List<DialogueTurn> DialogueTurnsList => dialogueTurnsList;
}
