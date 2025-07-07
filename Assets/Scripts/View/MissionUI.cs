using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class MissionUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup missionContainer;
    [SerializeField] private TMP_Text missionTitleText;
    [SerializeField] private TMP_Text missionProgressText;

    private int currentMissionId = -1;

    private void OnEnable()
    {
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.OnMissionUpdated += UpdateMissionUI;
            MissionManager.Instance.OnMissionProgressUpdated += UpdateMissionProgress;

            if (MissionManager.Instance.CurrentMission != null)
                UpdateMissionUI(MissionManager.Instance.CurrentMission);
        }

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted += HandleDialogueStarted;
            DialogueManager.Instance.OnDialogueEnded += HandleDialogueEnded;
        }
    }

    private void OnDisable()
    {
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.OnMissionUpdated -= UpdateMissionUI;
            MissionManager.Instance.OnMissionProgressUpdated -= UpdateMissionProgress;
        }

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted -= HandleDialogueStarted;
            DialogueManager.Instance.OnDialogueEnded -= HandleDialogueEnded;
        }
    }

    private void HandleDialogueStarted(NPCDialogueModel model, NPCController speaker)
    {
        missionContainer.gameObject.SetActive(false);
    }

    private void HandleDialogueEnded()
    {
        missionContainer.gameObject.SetActive(true);
    }



    private void UpdateMissionUI(MissionModel mission)
    {
        if (mission == null)
        {
            // Hide UI if no active mission
            StartCoroutine(FadeOut());
            return;
        }

        if (mission.id != currentMissionId)
        {
            currentMissionId = mission.id;

            // Update title and progress, then fade in
            // Debug.Log("Current mission" + currentMissionId);
            missionTitleText.text = mission.title;
            missionProgressText.text = $"0/{mission.qty}";
            StartCoroutine(FadeIn());
        }
    }

    public void UpdateMissionProgress(int currentProgress)
    {
        missionProgressText.text = $"{currentProgress}/{MissionManager.Instance?.CurrentMission?.qty}";
    }

    private IEnumerator FadeIn()
    {
        missionContainer.alpha = 0;
        missionContainer.gameObject.SetActive(true);

        while (missionContainer.alpha < 1f)
        {
            missionContainer.alpha += Time.deltaTime * 2f;
            yield return null;
        }

        missionContainer.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        while (missionContainer.alpha > 0f)
        {
            missionContainer.alpha -= Time.deltaTime * 2f;
            yield return null;
        }

        missionContainer.alpha = 0f;
        missionContainer.gameObject.SetActive(false);
    }
}
