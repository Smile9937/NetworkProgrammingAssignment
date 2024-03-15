using UnityEngine;

public class ShieldPickup : Pickup
{
    protected override bool OnPickup(Collider2D other)
    {
        if(other.TryGetComponent(out Health health))
        {
            health.EquipShield();
            return true;
        }

        return false;
    }
}
