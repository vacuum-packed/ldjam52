using _ldjam.Scripts.ScriptableObjects.Events;
using UnityEngine;

namespace _ldjam.Scripts
{
    public class Indicator : MonoBehaviour
    {
        [SerializeField]
        private Color validPositionColor;

        [SerializeField]
        private Color invalidPositionColor;


        [Header("Listening Events")]
        [SerializeField]
        private BoolEventChannelSO isPositionValidEvent;


        private Material _material;

        private void OnEnable()
        {
            isPositionValidEvent.onEventRaised += OnIsPositionValid;
        }

        private void Start()
        {
            _material = GetComponent<MeshRenderer>().material;
            transform.position = new Vector3(0, 100, 0);
        }

        private void OnIsPositionValid(bool value)
        {
            _material.color = value ? validPositionColor : invalidPositionColor;
        }
    }
}