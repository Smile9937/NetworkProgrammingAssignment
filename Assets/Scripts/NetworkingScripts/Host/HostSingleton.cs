using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : Singleton<HostSingleton>
{
    HostManager hostManager;

    public async Task<bool> InitServerAsync()
    {
        await Task.Delay(0);
        hostManager = ScriptableObject.CreateInstance<HostManager>();
        return true;
    }

    public async Task StartHost()
    {
        await hostManager.StartHostAsync();
        InvokeRepeating(nameof(PingServer), 10, 10);
    }

    private void PingServer()
    {
        hostManager.PingServer();
    }

    public string GetJoinCode()
    {
        return hostManager.joinCode;
    }

    public string GetLobbyID()
    {
        return hostManager.lobbyID;
    }
}
