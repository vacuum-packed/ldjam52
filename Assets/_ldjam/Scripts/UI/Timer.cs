using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool updateTimer;

    [SerializeField]
    private TextMeshProUGUI timer;

    [Header("Listening Events")]
    [SerializeField]
    private VoidEventChannelSO roundStartedEvent;

    [SerializeField]
    private VoidEventChannelSO roundEndedEvent;

    private void OnEnable()
    {
        roundStartedEvent.onEventRaised += OnRoundStarted;
        roundEndedEvent.onEventRaised += OnRoundEnded;
    }

    private void OnDisable()
    {
        roundStartedEvent.onEventRaised -= OnRoundStarted;
        roundEndedEvent.onEventRaised -= OnRoundEnded;
    }

    private void Update()
    {
        if (updateTimer)
            timer.text = GameManager.ScoreString;
    }

    private void OnRoundStarted()
    {
        updateTimer = true;
    }

    private void OnRoundEnded()
    {
        updateTimer = false;
    }
}
