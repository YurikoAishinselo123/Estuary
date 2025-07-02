using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoTransitionManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private float transitionTime = 1f;

    private void Start()
    {
        StartCoroutine(PlayVideoAndLoadScene());
    }

    private IEnumerator PlayVideoAndLoadScene()
    {
        //UIManager.Instance.HideGameplayUI();
        string nextScene = TransitionData.NextSceneName;
        //if (string.IsNullOrEmpty(nextScene))
        //{
        //    Debug.LogError("NextSceneName is null or empty!");
        //    yield break;
        //}

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
        asyncLoad.allowSceneActivation = false;

        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("VideoPlayer not assigned!");
        }

        yield return new WaitForSeconds(transitionTime);
        asyncLoad.allowSceneActivation = true;


        if (TransitionData.TargetSpawnPosition.HasValue)
        {
            Vector3 pos = TransitionData.TargetSpawnPosition.Value;
            Debug.Log("Spawning player at: " + pos);

        }
        else
        {
            Debug.LogWarning("No TargetSpawnPosition set.");
        }
    }

}
