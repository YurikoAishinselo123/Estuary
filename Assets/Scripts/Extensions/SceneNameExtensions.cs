using UnityEngine;

public static class SceneNameExtensions
{
    public static string GetDisplayName(this SceneName scene)
    {
        return scene switch
        {
            SceneName.RuangAtasan => "Ruang Atasan",
            SceneName.RuangRapat => "Ruang Rapat",
            SceneName.RuangKerja => "Ruang Kerja",
            SceneName.Laut => "Laut",
            _ => scene.ToString()
        };
    }
}
