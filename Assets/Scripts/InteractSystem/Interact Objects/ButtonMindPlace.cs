using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class ButtonMindPlace : MonoBehaviour, IInteract
{
    [SerializeField] private string interactText = "Volver al tren";

    [SerializeField] private float transitionVolumeSpeed = 2f;
    private float volumeWeight = 1f;
    private Volume volumeToTrain;
    private bool isVolumeActive = false;

    private void Update()
    {
        UpdateVolumeTransition();
    }
    public string GetInteractText()
    {
        return interactText;
    }

    public void Interact()
    {
        InitializeVolume();
        isVolumeActive = true;
    }

    private void InitializeVolume()
    {
        GameObject volume = GameObject.Find("VolumeInsideMindPlace");
        if (volume != null)
        {
            volumeToTrain = volume.GetComponent<Volume>();
            if (volumeToTrain != null)
                volumeToTrain.weight = 0f;
        }
    }

    private void UpdateVolumeTransition()
    {
        if (!isVolumeActive || volumeToTrain == null)
            return;

        volumeToTrain.weight = Mathf.Lerp(volumeToTrain.weight, volumeWeight, Time.deltaTime * transitionVolumeSpeed);

        if (Mathf.Abs(volumeToTrain.weight - volumeWeight) < 0.01f)
            volumeToTrain.weight = volumeWeight;

        if (isVolumeActive == true && volumeToTrain.weight >= 1f)
            SceneManager.LoadScene("04. Train");
    }
}
