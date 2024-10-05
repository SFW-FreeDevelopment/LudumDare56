using System.Diagnostics;
using UnityEngine;
public class GameManager : GameSingleton<GameManager>
{
    public static GameManager instance;

    private int algaeCount;

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
        CountAlgae();
    }

    private void CountAlgae()
    {
        GameObject[] algaeObjects = GameObject.FindGameObjectsWithTag("Algae");
        algaeCount = algaeObjects.Length;
    }

    public int GetAlgaeCount()
    {
        return algaeCount;
    }

    protected override void InitSingletonInstance()
    {

    }
}