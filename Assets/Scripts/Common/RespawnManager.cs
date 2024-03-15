using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RespawnManager : Singleton<RespawnManager>
{
    public UnityAction<ulong> onPlayerDeath;
    public UnityAction<ulong> onPlayerRespawn;

    void Awake() => SceneManager.activeSceneChanged += SceneChanged;

    private void SceneChanged(Scene scene, Scene scene2) => StopAllCoroutines();

    public void PlayerDied(ulong id, PlayerController playerController)
    {
        if(playerController == null) return;

        onPlayerDeath?.Invoke(id);

        playerController.gameObject.SetActive(false);
        playerController.PlayerDied();

        StartCoroutine(RespawnPlayer(id, playerController));
    }

    private IEnumerator RespawnPlayer(ulong id, PlayerController playerController)
    {
        yield return new WaitForSeconds(1);

        if(playerController != null)
        {
            playerController.PlayerRespawn();
            playerController.gameObject.SetActive(true);
            
            onPlayerRespawn?.Invoke(id);
        }
    }
}
