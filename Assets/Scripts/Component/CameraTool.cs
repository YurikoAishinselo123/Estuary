using System.Collections;
using UnityEngine;

public class CameraTool : MonoBehaviour, ITool
{
    public static CameraTool Instance { get; private set; }
    public static event System.Action<Texture2D> OnPhotoCaptured;
    public static event System.Action OnPhotoCaptureStarted;


    [SerializeField] private Camera captureCamera;
    [SerializeField] private GameObject cameraFrame;

    private RenderTexture renderTexture;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        int width = Screen.width;
        int height = Screen.height;
        renderTexture = new RenderTexture(width, height, 24);
    }


    public void Activate()
    {
        Debug.Log("camera active");
        cameraFrame.SetActive(true);
    }

    public void Deactivate()
    {
        cameraFrame.SetActive(false);
    }

    public void Use()
    {
        StartCoroutine(CapturePhotoCoroutine());
    }

    private IEnumerator CapturePhotoCoroutine()
    {
        OnPhotoCaptureStarted?.Invoke();
        cameraFrame.SetActive(false);

        captureCamera.targetTexture = renderTexture;
        yield return new WaitForEndOfFrame();

        RenderTexture.active = renderTexture;
        captureCamera.Render();

        // Create a new Texture2D every time
        Texture2D newTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        newTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        newTexture.Apply();

        captureCamera.targetTexture = null;
        RenderTexture.active = null;

        // Save and display the captured photo
        PhotoSaveHelper.SavePhoto(newTexture);
        OnPhotoCaptured?.Invoke(newTexture);
    }
}
