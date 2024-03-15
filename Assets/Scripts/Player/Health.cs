using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private PlayerController playerController;
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    public NetworkVariable<int> currentShield = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;

        currentHealth.Value = maxHealth;
        currentShield.Value = 0;

        RespawnManager.GetInstance().onPlayerRespawn += PlayerRespawn;
    }

    public override void OnNetworkDespawn()
    {
        RespawnManager.GetInstance().onPlayerRespawn -= PlayerRespawn;
    }

    public void PlayerRespawn(ulong id)
    {
        if(id == GetComponent<NetworkBehaviour>().OwnerClientId)
        { 
            currentHealth.Value = maxHealth;
            currentShield.Value = 0;
        }
    }

    public void EquipShield()
    {
        currentShield.Value = 2;
    }

    public void TakeDamage(int damage)
    {
        if(currentShield.Value > 0)
        {
            currentShield.Value--;
            return;
        }

        damage = damage < 0 ? damage : -damage;
        currentHealth.Value += damage;

        if(currentHealth.Value <= 0)
        {
            PlayerDied();
        }
    }

    private void PlayerDied()
    {
        PlayerDiedClientRpc();
    }

    [ClientRpc]
    private void PlayerDiedClientRpc()
    {
        if(deathParticles != null)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            RespawnManager.GetInstance().PlayerDied(playerController.GetComponent<NetworkBehaviour>().OwnerClientId, playerController);
        }
    }

    public void RestoreHealth(int health)
    {
        health = health < 0 ? -health : health;
        currentHealth.Value = currentHealth.Value + health > maxHealth ? currentHealth.Value + health : maxHealth;
    }
}
