using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    private int AlgaeRemaining;
    private GameObject[] AlgaeObject;
    private int SecondsElapsed;
    
    private void Start()
    {
        AlgaeObject = GameObject.FindGameObjectsWithTag("Algae");
        AlgaeRemaining = AlgaeObject.Length;
        SyncUI();
    }

    public void CollectAlgae()
    {
        AlgaeRemaining--;
        SyncUI();
    }

    public void CountTime()
    {
        
        SyncUI();
    }

    public void SyncUI()
    {
        // TODO: Update algae count
        // TODO: Update time
    }
}
