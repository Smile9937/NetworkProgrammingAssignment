using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(NetworkObject))]
public abstract class Pickup : NetworkBehaviour
{
    [SerializeField] protected bool respawn;
    protected Collider2D myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myCollider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(IsServer)
        {
            if(!OnPickup(other)) return;

            RespawnFunctionality();
        }
    }

    protected virtual void RespawnFunctionality()
    {
        if(respawn)
        {
            transform.position = new Vector2(Random.Range(-4, 4), Random.Range(-2, 2));
        }
        else
        {
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
    protected abstract bool OnPickup(Collider2D other);
}
