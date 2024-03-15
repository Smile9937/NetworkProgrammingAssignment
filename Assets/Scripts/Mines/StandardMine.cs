using Unity.Netcode;
using UnityEngine;

public class StandardMine : NetworkBehaviour
{
    [SerializeField] private GameObject minePrefab;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(IsServer)
        {
            if(other.TryGetComponent(out Health health))
            {
                health.TakeDamage(25);
                transform.position = new Vector2(Random.Range(-4, 4), Random.Range(-2, 2));
            }
        }
    }
}
