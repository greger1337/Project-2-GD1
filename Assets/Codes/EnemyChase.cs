
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform player;

    private bool hasCaughtPlayer = false;

    void Update()
    {
        if (player == null || hasCaughtPlayer)
            return;

        Vector2 direction =
            (player.position - transform.position).normalized;

        transform.position +=
            (Vector3)(direction * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasCaughtPlayer = true;

            Debug.Log("PLAYER CAUGHT!");

            GameManager.Instance.LoseGame();
        }
    }
}
