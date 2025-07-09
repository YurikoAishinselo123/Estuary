using UnityEngine;
using UnityEngine.UI;

public class PhotoDisplayUI : MonoBehaviour
{
    public static PhotoDisplayUI Instance { get; private set; }

    [SerializeField] private GameObject photoDisplayPanel;
    [SerializeField] private RawImage photoImage;

    
    [SerializeField] private GameObject photoPanel;
    [SerializeField] private RawImage dirtyPhotoImage;
    [SerializeField] private RawImage cleanPhotoImage;

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

        HidePhotos();
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

    public void ShowPhotos(Texture2D dirtyPhoto, Texture2D cleanPhoto)
    {
        photoPanel.SetActive(true);

        if (dirtyPhoto != null)
            dirtyPhotoImage.texture = dirtyPhoto;
        if (cleanPhoto != null)
            cleanPhotoImage.texture = cleanPhoto;
    }

    public void HidePhotos()
    {
        photoPanel.SetActive(false);
    }

    public bool IsVisible()
    {
        return photoDisplayPanel != null && photoDisplayPanel.activeSelf;
    }
}
