using UnityEngine;
using System.Collections.Generic;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private GameObject missionItemPrefab;
    [SerializeField] private Transform missionListContainer;

    private List<MissionItemUI> activeItems = new();

    private void OnEnable()
    {
        if (MissionManager.Instance != null)
            MissionManager.Instance.OnChapterLoaded += PopulateMissions;
    }

    private void OnDisable()
    {
        if (MissionManager.Instance != null)
            MissionManager.Instance.OnChapterLoaded -= PopulateMissions;
    }

    private void PopulateMissions(Chapter chapter)
    {
        foreach (Transform child in missionListContainer)
            Destroy(child.gameObject);
        activeItems.Clear();

        foreach (var mission in chapter.missions)
        {
            GameObject go = Instantiate(missionItemPrefab, missionListContainer);
            MissionItemUI item = go.GetComponent<MissionItemUI>();
            item.SetMission(mission);
            activeItems.Add(item);
        }
    }

    public void UpdateMissionProgress(int missionId, int currentProgress)
    {
        foreach (var item in activeItems)
        {
            if (item.MissionId == missionId)
            {
                item.UpdateProgress(currentProgress);
                break;
            }
        }
    }
}
