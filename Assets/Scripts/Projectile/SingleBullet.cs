using UnityEngine;

public class SingleBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int bulletSpeed = 200;

    [SerializeField] private float lifespan = 2;

    [HideInInspector]
    public bool isServerSpawn;

    void Start()
    {
        rb.velocity = transform.up * bulletSpeed;
        Invoke("KillBullet", lifespan);
    }

    [SerializeField] private int damage = 5;

    void OnTriggerEnter2D(Collider2D other)
    {
        KillBullet();

        if(isServerSpawn && other.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
        }
    } 

    private void KillBullet()
    {
        if(gameObject) Destroy(gameObject);
    }
}
