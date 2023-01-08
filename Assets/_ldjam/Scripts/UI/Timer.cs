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
        {
            var minutes = (int)GameManager.Score / 60;
            var seconds = (int)GameManager.Score % 60;
            var timerText = $"{minutes:D2}:{seconds:D2}";
            timer.text = timerText;
        }
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