using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ScoreMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI score;

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

    private void OnRoundStarted()
    {
        GetComponent<Canvas>().enabled = false;
    }

    private void OnRoundEnded()
    {
        score.text = GameManager.ScoreString;
        GetComponent<Canvas>().enabled = true;
    }
}
