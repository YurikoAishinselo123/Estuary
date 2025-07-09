using UnityEngine;
using UnityEngine.UI;

public class PhotoDisplayUI : MonoBehaviour
{
    public static PhotoDisplayUI Instance { get; private set; }

    [SerializeField] private GameObject photoDisplayPanel;
    [SerializeField] private RawImage photoImage;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (photoDisplayPanel != null)
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
        ShowPhoto(texture); 
    }

    public void ShowPhoto(Texture2D texture)
    {
        Debug.Log("show photo display");

        if (photoImage != null)
            photoImage.texture = texture;

        if (photoDisplayPanel != null)
            photoDisplayPanel.SetActive(true);
    }

    public void HidePhoto()
    {
        Debug.Log("hide photo display");
        if (photoDisplayPanel != null)
        {
            photoDisplayPanel.SetActive(false);
        }
        GameStateManager.Instance?.ResumeGameplay();
    }


    public bool IsVisible()
    {
        return photoDisplayPanel != null && photoDisplayPanel.activeSelf;
    }
}
