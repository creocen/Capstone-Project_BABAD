using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    [Header("Message Lists")]
    [SerializeField] private List<MessageSO> positiveMessages = new();
    [SerializeField] private List<MessageSO> negativeMessages = new();

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private float positiveChance = 0.5f;
    [SerializeField] private float hardModePositiveChance = 0.2f;
    [SerializeField] private float spawnHeightOffset = 0f;

    private float timer;
    private float camHalfWidth;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("GeneratorManager: Main Camera not found!");
            enabled = false;
            return;
        }

        camHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < spawnInterval) return;

        timer = 0f;
        SpawnMessage();
    }

    private void SpawnMessage()
    {
        bool hardMode = false;
        if (PlayerManager.Instance != null)
        {
            hardMode = PlayerManager.Instance.HardMode;
        }

        float currentPositiveChance = positiveChance;
        if (hardMode)
        {
            currentPositiveChance = hardModePositiveChance;
        }

        bool spawnPositive = false;
        float roll = Random.value;
        if (roll < currentPositiveChance)
        {
            spawnPositive = true;
        }

        MessageSO selected;
        if (spawnPositive)
        {
            selected = GetRandomMessage(positiveMessages);
        }
        else
        {
            selected = GetRandomMessage(negativeMessages);
        }

        if (selected == null || selected.messagePrefab == null) return;

        float x = Random.Range(-camHalfWidth, camHalfWidth);
        Vector3 spawnPos = new Vector3(x, transform.position.y + spawnHeightOffset, 0f);

        Instantiate(selected.messagePrefab, spawnPos, Quaternion.identity);
    }

    private MessageSO GetRandomMessage(List<MessageSO> list)
    {
        if (list == null || list.Count == 0) return null;
        int index = Random.Range(0, list.Count);
        return list[index];
    }
}
