using UnityEngine;

public class AmmoPickup : Pickup
{
    protected override bool OnPickup(Collider2D other)
    {
        if(other.TryGetComponent(out FiringAction firingAction))
        {
            firingAction.ReplenishAmmo();
            return true;
        }
        
        return false;
    }
}
