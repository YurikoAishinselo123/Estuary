using UnityEngine;

public static class SceneNameExtensions
{
    public static string GetDisplayName(this SceneName scene)
    {
        return scene switch
        {
            SceneName.RuangAtasan => "Ruang Atasan",
            SceneName.RuangRapat => "Ruang Rapat",
            //SceneName.RuangKerja => "Ruang Kerja", ///Original Branch
            SceneName.RuangKerja => "WW_RuangKerja", 
            SceneName.Laut => "Laut",
            _ => scene.ToString()
        };
    }
}
