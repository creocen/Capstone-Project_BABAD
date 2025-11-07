using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Ball & Paddles")]
    [SerializeField] private Ball ball;
    [SerializeField] private Rigidbody2D ballRB;
    [SerializeField] private PlayerPaddle player;
    [SerializeField] private ComputerPaddle cpu;

    [Header("Hard Mode Settings")]
    [SerializeField] private float hardModeTime = 60f;
    [SerializeField] private float hardModeSpeedMultiplier = 1.5f;
    [SerializeField] private float hardModePaddleSpeedMultiplier = 1.3f;

    private bool hardModeActive;
    private float timer;

    [Header("Score System")]
    [SerializeField] private int playerScore;
    [SerializeField] private int cpuScore;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI cpuScoreText;

    private Camera mainCamera;

    private void Start()
    {
        timer = 0f;
        mainCamera = Camera.main;

        UpdateScoreUI();
    }

    private void Update()
    {
        timer = timer + Time.deltaTime;

        if (!hardModeActive)
        {
            if (timer >= hardModeTime)
            {
                ActivateHardMode();
            }
        }

        UpdateDebugHUD();
    }

    public void PlayerScores()
    {
        playerScore = playerScore + 1;
        UpdateScoreUI();
        ResetBall();
    }

    public void CpuScores()
    {
        cpuScore = cpuScore + 1;
        UpdateScoreUI();
        ResetBall();
    }

    private void UpdateScoreUI()
    {
        if (playerScoreText)
        {
            playerScoreText.text = playerScore.ToString();
        }

        if (cpuScoreText)
        {
            cpuScoreText.text = cpuScore.ToString();
        }
    }

    private void ResetBall()
    {
        ball.ResetBall();
    }

    private void ActivateHardMode()
    {
        hardModeActive = true;

        ball.IncreaseSpeed(hardModeSpeedMultiplier);
        player.speed = player.speed * hardModePaddleSpeedMultiplier;
        cpu.speed = cpu.speed * hardModePaddleSpeedMultiplier;

        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }

        mainCamera.backgroundColor = new Color(0.05f, 0.08f, 0.20f);
    }

    private void UpdateDebugHUD()
    {
        float ballSpeed = 0f;
        if (ballRB != null)
        {
            ballSpeed = ballRB.linearVelocity.magnitude;
        }

        float remainingTime = hardModeTime - timer;
        if (remainingTime < 0f)
        {
            remainingTime = 0f;
        }

        if (debugText != null)
        {
            debugText.text =
                "Player: " + playerScore +
                " | CPU: " + cpuScore +
                "\nBall Speed: " + ballSpeed.ToString("F2") +
                "\nHardmode In: " + remainingTime.ToString("F1") + "s";
        }
    }
}
