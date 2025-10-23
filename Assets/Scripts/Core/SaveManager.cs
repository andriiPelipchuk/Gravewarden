using UnityEditor.Overlays;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void SaveGame(int checkpointID, int souls)
    {
        CheckpointData data = new CheckpointData
        {
            checkpoint = checkpointID,
            soulsData = souls
        };

        string json = JsonUtility.ToJson(data);
        SecurePlayerPrefs.SetEncryptedString("save_data", json);
        Debug.Log("✅ Збереження виконано!");
    }

    public CheckpointData LoadGame()
    {
        string json = SecurePlayerPrefs.GetDecryptedString("save_data");
        if (string.IsNullOrEmpty(json))
        {
            return null;
        }

        CheckpointData data = JsonUtility.FromJson<CheckpointData>(json);
        Debug.Log($"✅ Завантажено: Level={data.checkpoint}, Souls={data.soulsData}");
        return data;
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey("save_data");
        Debug.Log("🗑️ Збереження видалено");
    }
}
