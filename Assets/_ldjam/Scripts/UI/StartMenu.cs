using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class StartMenu : MonoBehaviour
{
    [Header("Listening Events")]
    [SerializeField]
    private VoidEventChannelSO roundStartedEvent;

    private void OnEnable()
    {
        roundStartedEvent.onEventRaised += OnRoundStarted;
    }

    private void OnDisable()
    {
        roundStartedEvent.onEventRaised -= OnRoundStarted;
    }

    private void OnRoundStarted()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
