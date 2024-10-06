using UnityEngine;

public class GameManager : GameSingleton<GameManager>
{
    private int algaeCount;
    private void CountAlgae()
    {
        GameObject[] algaeObjects = GameObject.FindGameObjectsWithTag("Algae");
        algaeCount = algaeObjects.Length;
    }

    public int GetAlgaeCount()
    {
        return algaeCount;
    }

    private void Start()
    {
        CountAlgae();
    }

    protected override void InitSingletonInstance()
    {

    }
}