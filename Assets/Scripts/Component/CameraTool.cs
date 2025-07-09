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
    private int photoCount = 0;

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
        Debug.Log("camera deactive");
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

        Texture2D newTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        newTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        newTexture.Apply();

        captureCamera.targetTexture = null;
        RenderTexture.active = null;

        var currentMission = MissionManager.Instance.CurrentMission;

        if (currentMission != null)
        {
            // Misi 2: Ambil Foto Awal
            if (currentMission.id == 2 && !GameProgressManager.Instance.HasCapturedDirtyEnvironmentForTheFirstTime)
            {
                GameProgressManager.Instance.HasCapturedDirtyEnvironmentForTheFirstTime = true;
                PhotoSaveHelper.SavePhoto(newTexture);
                MissionManager.Instance.ReportMissionProgress(1);
                MissionManager.Instance.CompleteCurrentMission();
            }
            // Misi 4: Ambil Foto Akhir
            else if (currentMission.id == 4 && !GameProgressManager.Instance.HasCapturedCleanEnvironmentForTheFirstTime)
            {
                GameProgressManager.Instance.HasCapturedCleanEnvironmentForTheFirstTime = true;
                Debug.Log("HasCapturedCleanEnvironmentForTheFirstTime : " + GameProgressManager.Instance.HasCapturedCleanEnvironmentForTheFirstTime);
                PhotoSaveHelper.SavePhoto(newTexture);
                MissionManager.Instance.ReportMissionProgress(1);
            }
        }

        // Tetap kirim ke UI, meskipun tidak disimpan
        OnPhotoCaptured?.Invoke(newTexture);
        // PhotoDisplayUI.Instance.HidePhoto();
        GameStateManager.Instance.SetState(GameState.PhotoReview);
    }

}
