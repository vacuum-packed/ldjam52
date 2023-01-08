using UnityEngine;
using UnityEngine.Events;

namespace _ldjam.Scripts.ScriptableObjects.Events
{
    [CreateAssetMenu(menuName = "Events/Boolean Event Channel")]
    public class BoolEventChannelSO : ScriptableObject
    {
        public UnityAction<bool> onEventRaised;

        public void RaiseEvent(bool value)
        {
            onEventRaised?.Invoke(value);
        }
    }
}