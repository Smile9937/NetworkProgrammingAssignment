using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] private int healthAmount = 5;
    protected override bool OnPickup(Collider2D other)
    {
        if(other.TryGetComponent(out Health health))
        {
            health.RestoreHealth(healthAmount);
            return true;
        }

        return false;
    }
}
