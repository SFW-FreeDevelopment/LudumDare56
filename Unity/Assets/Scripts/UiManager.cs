using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : SceneSingleton<UiManager>
{
    public static UiManager instance;

    public TMP_Text timeText;
    public TMP_Text algaeText;
    public TMP_Text coinCountText;
    private float timeRemaining = 120f;
    private int algaeRemaining = 0;
    private int coinCount = 0;


    void OnEnable()
    {
        EventManager.OnAlgaeCollected += AlgaeCollected;
        EventManager.OnCoinCollected += CollectCoin;
    }

    void OnDisable()
    {
        EventManager.OnAlgaeCollected -= AlgaeCollected;
        EventManager.OnCoinCollected -= CollectCoin;
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
            EventManager.LevelCompletion();
        }
    }

    private void SetAlgaeRemaining()
    {
        algaeRemaining = GameManager.Instance.GetAlgaeCount();
    }
    
    public void AlgaeCollected()
    {
        algaeRemaining--;
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
        algaeText.text = "Algae: " + algaeRemaining.ToString();
        coinCountText.text = "Coins: " + GameManager.Instance.GetCoinCount().ToString();
    }

    protected override void InitSingletonInstance()
    {
        SetAlgaeRemaining();
        UpdateUI();
        InvokeRepeating("UpdateTime", 0f, 1f);
    }
}
