using System;
using Unity.Netcode;
using UnityEngine;

public class FiringAction : NetworkBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject clientSingleBullet;
    [SerializeField] private GameObject serverSingleBullet;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private int maxBulletCount;

    private int bulletCount;
    private NetworkVariable<int> networkBulletCount = new NetworkVariable<int>();

    public void ReplenishAmmo()
    {
        if(networkBulletCount.Value >= maxBulletCount) return;
        networkBulletCount.Value = maxBulletCount;
    }

    public override void OnNetworkSpawn()
    {
        bulletCount = maxBulletCount;
        playerController.onFireEvent += Fire;
        networkBulletCount.OnValueChanged += ChangeBulletCount;

        if(!IsServer) return;
        networkBulletCount.Value = maxBulletCount;
    }

    public override void OnNetworkDespawn()
    {
        playerController.onFireEvent -= Fire;
        networkBulletCount.OnValueChanged -= ChangeBulletCount;
    }

    private void ChangeBulletCount(int previousValue, int newValue)
    {
        bulletCount = newValue;
    }

    private void Fire(bool isShooting)
    {
        if (isShooting)
        {
            ShootLocalBullet();
        }
    }

    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        if(networkBulletCount.Value <= 0) return;
        networkBulletCount.Value--;

        GameObject bullet = Instantiate(serverSingleBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        bullet.GetComponent<SingleBullet>().isServerSpawn = true;
        ShootBulletClientRpc();
    }

    [ClientRpc]
    private void ShootBulletClientRpc()
    {
        if(IsOwner) return;
        GameObject bullet = Instantiate(clientSingleBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }

    private void ShootLocalBullet()
    {
        if(bulletCount <= 0) return;

        GameObject bullet = Instantiate(clientSingleBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        ShootBulletServerRpc();
    }
}
