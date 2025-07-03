using UnityEngine;
using System.Collections.Generic;

namespace Manager
{
    public class SpawnToolManager : MonoBehaviour
    {
        [SerializeField] private List<ToolSpawnData> toolsToSpawn;

        private void Start()
        {
            SpawnToolsIfNotCollected();
        }

        private void SpawnToolsIfNotCollected()
        {
            foreach (var toolData in toolsToSpawn)
            {
                if (!HasItemBeenCollected(toolData.ItemID))
                {
                    Quaternion rotation = Quaternion.Euler(toolData.spawnRotation);
                    Instantiate(toolData.toolPrefab, toolData.spawnPosition, rotation);
                    Debug.Log($"[SpawnToolManager] Spawned tool: {toolData.itemSO.itemName}");
                }
            }
        }

        private bool HasItemBeenCollected(string itemID)
        {
            var items = InventoryManager.Instance.GetItems();
            foreach (var item in items)
            {
                if (item.itemSO.itemID == itemID)
                    return true;
            }
            return false;
        }
    }

    [System.Serializable]
    public class ToolSpawnData
    {
        public ItemSO itemSO;
        public GameObject toolPrefab;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;

        public string ItemID => itemSO != null ? itemSO.itemID : string.Empty;
    }
}
