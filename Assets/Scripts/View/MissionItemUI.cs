using UnityEngine;
using TMPro;

public class MissionItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text progressText;

    private Mission mission;
    public int MissionId => mission.id;

    public void SetMission(Mission mission)
    {
        this.mission = mission;
        titleText.text = mission.title;
        descriptionText.text = mission.description;
        UpdateProgress(0);
    }

    public void UpdateProgress(int current)
    {
        progressText.text = $"{current}/{mission.qty}";
    }
}
