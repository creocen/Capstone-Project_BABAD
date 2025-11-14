using UnityEngine;

public class FallingMessage : MonoBehaviour
{
    [Header("Message Settings")]
    [SerializeField] private MessageType messageType;
    [SerializeField] private float fallSpeed = 3f;

    private bool spawnedInHardMode;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        spawnedInHardMode = false;
        if (PlayerManager.Instance != null)
        {
            spawnedInHardMode = PlayerManager.Instance.HardMode;
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (!IsVisibleToCamera())
        {
            Destroy(gameObject);
        }
    }

    private bool IsVisibleToCamera()
    {
        if (mainCamera == null) return false;

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPos.x >= 0f && viewportPos.x <= 1f &&
            viewportPos.y >= 0f && viewportPos.y <= 1f)
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.HandleMessage(messageType, spawnedInHardMode);
            }
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (PlayerManager.Instance != null)
        {
            if (!IsVisibleToCamera())
            {
                PlayerManager.Instance.MissedMessage(messageType, spawnedInHardMode);
            }
        }
    }
}
