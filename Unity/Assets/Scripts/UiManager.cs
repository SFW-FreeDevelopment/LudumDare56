using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TMP_Text timeText;
    public TMP_Text algaeText;
    public TMP_Text coinCountText;
    private float timeRemaining = 120f;
    private int algaeRemaining = 0;
    private int coinCount = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
        InvokeRepeating("UpdateTime", 0f, 1f);
    }

    void UpdateTime()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= 1;
            UpdateUI();
        }
        else
        {
            // Game Over Logic
        }
    }

    public void AlgaeRemaining(int amount)
    {
        algaeRemaining -= amount;
        UpdateUI();
    }

    public void CollectCoin()
    {
        coinCount++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        timeText.text = "Time: " + timeRemaining.ToString();
        algaeText.text = "Score: " + algaeRemaining.ToString();
        coinCountText.text = "Coins: " + coinCount.ToString();
    }
}
