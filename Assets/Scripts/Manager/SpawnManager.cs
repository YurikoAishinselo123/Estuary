using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public DoorSO doorData;

    public void LoadSceneWithVideo(string targetSceneName, Vector3 spawnPosition)
    {
        TransitionData.NextSceneName = targetSceneName;
        TransitionData.TargetSpawnPosition = spawnPosition;

        Debug.Log("scenename : " + targetSceneName);
        Debug.Log("position : " + doorData);
        SceneManager.LoadScene("VideoTransition");
    }

    public void OpenDoor(string tag)
    {
        switch (tag)
        {
            case "RuangAtasan":
                LoadSceneWithVideo("Office1", doorData.RuangAtasan);
                break;
            case "RuangKerja":
                LoadSceneWithVideo("Office1", doorData.RuangRapat);
                break;
            case "RuangRapat":
                LoadSceneWithVideo("Office2", doorData.RuangRapatKeRuangKerja);
                break;
            case "RuangRapatKeRuangKerja":
                LoadSceneWithVideo("Office3", doorData.RuangAtasanKeRuangKerja);
                break;
            case "Ocean":
                LoadSceneWithVideo("Ocean", doorData.Ocean);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenDoor(gameObject.tag);
        }
    }
}
