using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointID;

    public void EnterSaveZone()
    {
        EventManager.TriggerCheckpointReached(checkpointID);
    }
}