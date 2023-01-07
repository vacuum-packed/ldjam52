using UnityEngine;

public class Harvestable : MonoBehaviour
{
    [SerializeField]
    private int points;

    [SerializeField]
    private IntegerEventChannelSO pickupEvent;

    private void OnTriggerEnter(Collider other)
    {
        pickupEvent.RaiseEvent(points);
        Destroy(gameObject);
    }
}
