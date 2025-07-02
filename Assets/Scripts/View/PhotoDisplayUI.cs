using UnityEngine;
using UnityEngine.UI;

public class PhotoDisplayUI : MonoBehaviour
{
    [SerializeField] private GameObject photoDisplayPanel;
    [SerializeField] private RawImage photoImage;

    void Awake()
    {
        photoDisplayPanel.SetActive(false);
    }

    private void OnEnable()
    {
        CameraTool.OnPhotoCaptured += DisplayPhoto;
    }

    private void OnDisable()
    {
        CameraTool.OnPhotoCaptured -= DisplayPhoto;
    }

    private void DisplayPhoto(Texture2D texture)
    {
        photoImage.texture = texture;
        photoDisplayPanel.SetActive(true);
    }

    public void HidePhoto()
    {
        photoDisplayPanel.SetActive(false);
    }
}
