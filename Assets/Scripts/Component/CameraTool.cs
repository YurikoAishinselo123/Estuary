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
        Debug.Log("CameraTool: Activated");
        cameraFrame.SetActive(true);
    }

    public void Deactivate()
    {
        Debug.Log("CameraTool: Deactivated");
        cameraFrame.SetActive(false);
    }

    public void Use()
    {
        StartCoroutine(CapturePhotoCoroutine());
    }

    private Texture2D CloneTexture(Texture2D source)
    {
        Texture2D newTex = new Texture2D(source.width, source.height, source.format, false);
        newTex.SetPixels(source.GetPixels());
        newTex.Apply();
        return newTex;
    }

    private IEnumerator CapturePhotoCoroutine()
    {
        OnPhotoCaptureStarted?.Invoke();
        cameraFrame.SetActive(false);

        captureCamera.targetTexture = renderTexture;
        yield return new WaitForEndOfFrame();

        RenderTexture.active = renderTexture;
        captureCamera.Render();

        Texture2D newTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        newTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        newTexture.Apply();

        captureCamera.targetTexture = null;
        RenderTexture.active = null;

        var currentMission = MissionManager.Instance.CurrentMission;

        if (currentMission != null)
        {
            // Mission 2: Take dirty environment photo
            if (currentMission.id == 2 && !GameProgressManager.Instance.HasCapturedDirtyEnvironmentForTheFirstTime)
            {
                GameProgressManager.Instance.HasCapturedDirtyEnvironmentForTheFirstTime = true;
                Texture2D clone = CloneTexture(newTexture);
                GameProgressManager.Instance.SetDirtyPhoto(clone);
                PhotoSaveHelper.SavePhoto(clone);
                MissionManager.Instance.ReportMissionProgress(1);
                MissionManager.Instance.CompleteCurrentMission();
            }

            // Mission 4: Take clean environment photo
            else if (currentMission.id == 4 && !GameProgressManager.Instance.HasCapturedCleanEnvironmentForTheFirstTime)
            {
                GameProgressManager.Instance.HasCapturedCleanEnvironmentForTheFirstTime = true;
                Texture2D clone = CloneTexture(newTexture);
                GameProgressManager.Instance.SetCleanPhoto(clone);
                PhotoSaveHelper.SavePhoto(clone);
                MissionManager.Instance.ReportMissionProgress(1);
            }
        }

        OnPhotoCaptured?.Invoke(newTexture);
        GameStateManager.Instance.SetState(GameState.PhotoReview);
    }


    

    
}
