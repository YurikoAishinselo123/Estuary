using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DoorSO")]
public class DoorSO : ScriptableObject
{
    public Vector3 RuangAtasanSpawn;
    public Vector3 RuangRapatSpawn;
    public Vector3 RuangKerjaDefaultSpawn; 
    public Vector3 LautSpawn;

    // Cross-scene specific spawn points
    public Vector3 SpawnInRuangKerja_FromRuangAtasan;
    public Vector3 SpawnInRuangKerja_FromRuangRapat;

    public Vector3 GetSpawnPosition(SceneName currentScene, SceneName fromScene)
    {
        return (currentScene, fromScene) switch
        {
            (SceneName.RuangKerja, SceneName.RuangAtasan) => SpawnInRuangKerja_FromRuangAtasan,
            (SceneName.RuangKerja, SceneName.RuangRapat) => SpawnInRuangKerja_FromRuangRapat,
            (SceneName.RuangKerja, _) => RuangKerjaDefaultSpawn, 
            (SceneName.RuangAtasan, _) => RuangAtasanSpawn,
            (SceneName.RuangRapat, _) => RuangRapatSpawn,
            (SceneName.Laut, _) => LautSpawn,
            _ => Vector3.zero
        };
    }
}
