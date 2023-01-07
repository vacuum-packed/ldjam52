using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int score;

    [Header("Listening Events")]
    [SerializeField]
    private IntegerEventChannelSO pointsEvent;

    [Header("Broadcasting Events")]
    [SerializeField]
    private IntegerEventChannelSO scoreBroadcastEvent;
    


    private void OnEnable()
    {
        pointsEvent.onEventRaised += OnPointsGained;
    }

    private void OnPointsGained(int value)
    {
        score += value;
        scoreBroadcastEvent.RaiseEvent(score);
    }
    

    void Start()
    {
        score = 0;
    }
}