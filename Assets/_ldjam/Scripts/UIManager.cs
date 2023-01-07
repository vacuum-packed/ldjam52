using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI points;

    [Header("Listening Events")]
    [SerializeField]
    private IntegerEventChannelSO onPointsUpdated;

    private void OnEnable()
    {
        onPointsUpdated.onEventRaised += OnPointsUpdated;
    }

    private void Start()
    {
        points.text = "0";
    }

    private void OnPointsUpdated(int value)
    {
        points.text = value.ToString();
    }

    private void OnDisable()
    {
        onPointsUpdated.onEventRaised -= OnPointsUpdated;
    }
}
