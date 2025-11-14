using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float boostedSpeed = 10f;
    [SerializeField] private float movementLimit = 8.5f;

    private float currentSpeed;
    private float moveInput;

    [Header("Happiness/Friendship System")]
    [SerializeField] private float happiness = 50f;
    [SerializeField] private float maxHappiness = 100f;
    [SerializeField] private float friendship = 60f;
    [SerializeField] private float maxFriendship = 100f;

    [Header("Hard Mode Settings")]
    [SerializeField] private bool hardMode = false;
    [SerializeField] private float hardModeDuration = 60f;
    [SerializeField] private float timeBeforeHardMode = 90f;
    private float hardModeTimer = 0f;
    private float preHardModeTimer = 0f;
    private bool hardModeActivated = false;

    [Header("Mood Change Values (Pre-Hardmode)")]
    [SerializeField] private int prePosMood = 10;
    [SerializeField] private int preNegMood = -5;
    [SerializeField] private int preFriend = 8;

    [Header("Mood Change Values (Hardmode)")]
    [SerializeField] private int hardPosMood = 5;
    [SerializeField] private int hardNegMood = -15;
    [SerializeField] private int hardFriend = 25;

    [Header("Missed Message Penalties")]
    [SerializeField] private int preMissFriend = 3;
    [SerializeField] private int hardMissMood = 25;
    [SerializeField] private int hardMissFriend = 25;

    [Header("UI References")]
    [SerializeField] private Slider happinessSlider;
    [SerializeField] private Slider friendshipSlider;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TextMeshProUGUI debugText;

    private string lastDialogue = "";

    public static event System.Action OnHardModeStart;
    public bool HardMode => hardMode;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentSpeed = moveSpeed;
        happinessSlider.maxValue = maxHappiness;
        friendshipSlider.maxValue = maxFriendship;
        UpdateUI();
    }

    private void Update()
    {
        HandleMovement();
        HandleHardModeProgress();
        UpdateDebugUI();
    }

    private void HandleMovement()
    {
        transform.Translate(new Vector3(moveInput, 0f, 0f) * currentSpeed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -movementLimit, movementLimit);
        transform.position = pos;
    }

    private void HandleHardModeProgress()
    {
        if (!hardModeActivated)
        {
            preHardModeTimer += Time.deltaTime;
            if (preHardModeTimer >= timeBeforeHardMode || friendship <= 30)
            {
                ActivateHardMode();
            }
        }

        if (hardModeActivated)
        {
            hardModeTimer += Time.deltaTime;
            if (hardModeTimer >= hardModeDuration)
                TriggerDialogWin();
        }
        else
        {
            if (preHardModeTimer >= 120f)
                TriggerDialogWin();
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 move = ctx.ReadValue<Vector2>();
        moveInput = -Mathf.Clamp(move.x, -1f, 1f);
    }

    public void OnMoveBoost(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            currentSpeed = boostedSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
    }

    public void HandleMessage(MessageType messageType, bool isHard)
    {
        int moodChange = 0;
        int friendChange = 0;

        if (!isHard)
        {
            if (messageType == MessageType.Positive)
            {
                moodChange = prePosMood;
            }
            else
            {
                moodChange = preNegMood;
            }
            friendChange = preFriend;
        }
        else
        {
            if (messageType == MessageType.Positive)
            {
                moodChange = hardPosMood;
            }
            else
            {
                moodChange = hardNegMood;
            }
            friendChange = hardFriend;
        }

        happiness += moodChange;
        friendship += friendChange;

        happiness = Mathf.Clamp(happiness, 0, maxHappiness);
        friendship = Mathf.Clamp(friendship, 0, maxFriendship);

        UpdateUI();

        if (happiness <= 0 || friendship <= 0)
        {
            if (!hardModeActivated)
            {
                lastDialogue = "I still want to be their friend";
            }
            else
            {
                TriggerDialogLose();
            }
        }
    }

    public void MissedMessage(MessageType type, bool isHard)
    {
        if (!isHard)
        {
            if (type == MessageType.Negative)
            {
                friendship -= preMissFriend;
                if (friendship < 0) friendship = 0;
            }
        }
        else
        {
            if (type == MessageType.Negative)
            {
                happiness -= hardMissMood;
                friendship -= hardMissFriend;
                if (happiness < 0) happiness = 0;
                if (friendship < 0) friendship = 0;

                if (happiness <= 0 || friendship <= 0)
                {
                    TriggerDialogLose();
                }
            }
        }

        UpdateUI();
        if (type == MessageType.Negative)
        {
            lastDialogue = "Missed Negative";
        }
        else
        {
            lastDialogue = "Missed Positive";
        }
    }

    private void UpdateUI()
    {
        if (happinessSlider) happinessSlider.value = happiness;
        if (friendshipSlider) friendshipSlider.value = friendship;
    }

    private void ActivateHardMode()
    {
        if (hardModeActivated) return;

        hardModeActivated = true;
        hardMode = true;
        OnHardModeStart?.Invoke();

        if (friendship < 30)
        {
            lastDialogue = "I still do";
            friendship += 30;
        }
        else
        {
            lastDialogue = "You're mature for your age";
            friendship += 30;
            happiness += 25;
        }

        if (!mainCamera) mainCamera = Camera.main;
        mainCamera.backgroundColor = new Color(0.05f, 0.08f, 0.20f);
    }

    private void TriggerDialogWin()
    {
        lastDialogue = "I think we're a perfect match";
        UpdateUI();
        Time.timeScale = 0;
    }

    private void TriggerDialogLose()
    {
        lastDialogue = "You're the only one I can trust";
        UpdateUI();
        Time.timeScale = 0;
    }

    private void UpdateDebugUI()
    {
        if (!debugText) return;

        int currentRed = 0;
        int currentGreen = 0;
        int currentMiss = 0;

        if (!hardMode)
        {
            currentRed = preNegMood;
            currentGreen = prePosMood;
            currentMiss = preMissFriend;
        }
        else
        {
            currentRed = hardNegMood;
            currentGreen = hardPosMood;
            currentMiss = hardMissMood;
        }

        string hardModeStatus = "OFF";
        if (hardMode) hardModeStatus = "ON";

        debugText.text =
            "Last Dialogue: " + lastDialogue + "\n" +
            "Hardmode: " + hardModeStatus + "\n" +
            "Red Message: " + currentRed + "\n" +
            "Green Message: " + currentGreen + "\n" +
            "Missed Message: " + currentMiss + "\n" +
            "Time to Hardmode: " + Mathf.Max(0, timeBeforeHardMode - preHardModeTimer).ToString("F1") + "s";
    }
}
