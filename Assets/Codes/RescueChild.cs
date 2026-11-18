
using UnityEngine;

public class RescueChild : MonoBehaviour
{
    private bool rescued = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (rescued)
            return;

        if (other.CompareTag("Player"))
        {
            rescued = true;

            GameManager.Instance.SaveChild();

            gameObject.SetActive(false);
        }
    }
}
