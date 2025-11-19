using UnityEngine;

public class TriggerAZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<CameraSwitcher>().TriggerA();
        }
    }
}
