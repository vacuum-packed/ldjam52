using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Integer Event Channel")]
public class IntegerEventChannelSO : ScriptableObject
{
    public UnityAction<int> onEventRaised;

    public void RaiseEvent(int value)
    {
        onEventRaised?.Invoke(value);
    }
}