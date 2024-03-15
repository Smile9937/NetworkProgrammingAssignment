using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostManager : ScriptableObject
{
    public string joinCode = "";
    private const int maxConnections = 20;

    public string lobbyID;

    Allocation allocation;

    public async Task<bool> InitSeverAsync(){
    await Task.Delay(0); 
    return true;
}
    public async Task StartHostAsync()
    {
        allocation = await Relay.Instance.CreateAllocationAsync(maxConnections);

        if (allocation != null) joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
        Debug.Log("JoinCode: " + joinCode);

        if (!NetworkManager.Singleton.TryGetComponent(out UnityTransport transport)) return;

        RelayServerData relayServerData = new RelayServerData(allocation, "udp");
        transport.SetRelayServerData(relayServerData);

        CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions();
        createLobbyOptions.IsLocked = false;
        createLobbyOptions.IsPrivate = false;
        createLobbyOptions.Data = new Dictionary<string, DataObject>();
        createLobbyOptions.Data.Add(
            "JoinCode",
            new DataObject(visibility: DataObject.VisibilityOptions.Member, joinCode)
        );
        var lobby = await Lobbies.Instance.CreateLobbyAsync("No Name Lobby", maxConnections, createLobbyOptions);
        lobbyID = lobby.Id;

        NetworkServer networkServer = new NetworkServer(NetworkManager.Singleton);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = UserDataWrapper.PayLoadInBytes();

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public async void PingServer()
    {
        await Lobbies.Instance.SendHeartbeatPingAsync(lobbyID);
    }
}
