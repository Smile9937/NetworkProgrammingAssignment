using Unity.Netcode;
using UnityEngine.SceneManagement;

public class ClientDisconnect
{
    NetworkManager networkManager;
    public ClientDisconnect(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
        networkManager.OnClientStarted += OnClientStartedAlready;
    }

    private void OnClientStartedAlready()
    {
        networkManager.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientDisconnected(ulong clientID)
    {
        if (networkManager.IsClient && networkManager.IsConnectedClient && networkManager.LocalClientId == clientID && clientID != 0)
        {
            networkManager.Shutdown();
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
