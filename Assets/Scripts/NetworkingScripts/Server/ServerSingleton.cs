using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class ServerSingleton : Singleton<ServerSingleton>
{
    public ServerManager serverManager;
    public bool isAuth;

    public async Task InitServerAsync()
    {
        serverManager = ScriptableObject.CreateInstance<ServerManager>();
        serverManager.ServerManagerConfiguration(NetworkManager.Singleton, ApplicationData.IP(), ApplicationData.Port(), ApplicationData.QPort());
        isAuth = await serverManager.InitAsync();
    } 

    public async Task StartServerAsync()
    {
        await serverManager.StartServerAsync();
    }
}
