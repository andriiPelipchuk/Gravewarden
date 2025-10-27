using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Checkpoint : MonoBehaviour
{
    public float radius = 3f; 

    public CheckpointUI uiInstance;

    public int checkpointId;         

    bool playerInside = false;
    Transform playerTransform;

    void Awake()
    {
        EventManager.PlayerInteract += PlayerInteract;

        var col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = radius;

        if (uiInstance == null)
        {
            uiInstance = GetComponentInChildren<CheckpointUI>(true);
        }
    }
    private void OnDestroy()
    {
        EventManager.PlayerInteract -= PlayerInteract;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
        playerTransform = other.transform;

        if (uiInstance != null)
            uiInstance.ShowPrompt("Tap here");
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        playerTransform = null;

        if (uiInstance != null)
            uiInstance.HidePrompt();
    }

/*    void Update()
    {
        // fallback: якщо хочеш використовувати дистанцію замість тригерів
        if (!playerInside && playerTransform == null)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                float d = Vector3.Distance(player.transform.position, transform.position);
                if (d <= radius)
                {
                    // увійшли в радіус вручну
                    playerInside = true;
                    playerTransform = player.transform;
                    if (showPromptWhenNear && uiInstance != null) uiInstance.ShowPrompt("Натисни щоб зберегтись");
                }
            }
        }

        
    }*/

    private void PlayerInteract()
    {
        if (playerInside)
        {
            RestAtCheckpoint();
        }
    }

    public void RestAtCheckpoint()
    {
        EventManager.TriggerCheckpointReached(checkpointId);
        if (uiInstance != null) uiInstance.PlayConfirm();
    }

    // Водночас можна викликати зовні:
/*    public void ForceShowPrompt(string text)
    {
        if (uiInstance != null) uiInstance.ShowPrompt(text);
    }*/
}
