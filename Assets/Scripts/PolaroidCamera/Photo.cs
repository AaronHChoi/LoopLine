using UnityEngine;

public class Photo : MonoBehaviour
{
    bool isClue;
    string clueId;
    Texture2D texture;

    public bool IsClue => isClue;
    public string ClueId => clueId;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void SetPhoto(Texture2D tex, bool clue = false, string id = "")
    {
        texture = tex;
        isClue = clue;
        clueId = id;
    }
}
