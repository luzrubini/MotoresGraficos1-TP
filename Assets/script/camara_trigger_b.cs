using UnityEngine;

public class TriggerBZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<CameraSwitcher>().TriggerB();
        }
    }
}
