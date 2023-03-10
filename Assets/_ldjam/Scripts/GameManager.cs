using System.Globalization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        RoundStarted,
        RoundEnded
    }
    
    private static int pickupCount;
    private static int totalPickupCount;
    private static float startTime;

    public static float Score => Time.time - startTime;
    
    public static string ScoreString
    {
        get
        {
            NumberFormatInfo format = new() { NumberDecimalDigits = 1 };
            return Score.ToString("N", format);
        }
    }

    [Header("Listening Events")]
    [SerializeField]
    private VoidEventChannelSO roundStartedEvent;

    [SerializeField]
    private IntegerEventChannelSO harvestEvent;

    [Header("Broadcasting Events")]

    [SerializeField]
    private VoidEventChannelSO roundEndedEvent;

    public GameState CurrentGameState { get; private set; }

    private void Start()
    {
        CurrentGameState = GameState.RoundEnded;
        totalPickupCount = GetComponentsInChildren<Harvestable>(false).Length;
    }

    private void OnEnable()
    {
        roundStartedEvent.onEventRaised += OnRoundStarted;
        harvestEvent.onEventRaised += OnHarvest;
    }

    private void OnDisable()
    {
        roundStartedEvent.onEventRaised -= OnRoundStarted;
        harvestEvent.onEventRaised -= OnHarvest;
    }

    private void OnHarvest(int value)
    {
        if (++pickupCount >= totalPickupCount)
            EndRound();
    }

    private void OnRoundStarted()
    {
        foreach (Harvestable pickup in GetComponentsInChildren<Harvestable>(true))
            pickup.gameObject.SetActive(true);

        pickupCount = 0;
        startTime = Time.time;
        CurrentGameState = GameState.RoundStarted;
    }

    private void EndRound()
    {
        roundEndedEvent.RaiseEvent();
        CurrentGameState = GameState.RoundEnded;
    }
}
