using UnityEngine.SceneManagement;
using UnityEngine;
using System;


public static class SceneManagerHelper
{
    public static SceneName GetCurrentSceneName()
    {
        return Enum.TryParse(SceneManager.GetActiveScene().name, out SceneName sceneName)
            ? sceneName
            : SceneName.Mainmenu;
    }

    public static bool HasVisited(SceneName sceneName)
    {
        return PlayerPrefs.GetInt($"Visited_{sceneName}", 0) == 1;
    }

    public static void MarkVisited(SceneName sceneName)
    {
        PlayerPrefs.SetInt($"Visited_{sceneName}", 1);
    }
}
