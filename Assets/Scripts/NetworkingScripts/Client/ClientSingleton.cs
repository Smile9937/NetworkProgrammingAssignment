using System.Threading.Tasks;
using UnityEngine;

public class ClientSingleton : Singleton<ClientSingleton>
{
    public ClientManager clientManager;
    public bool isAuth = false;

    public async Task InitClientAsync()
    {
        clientManager = ScriptableObject.CreateInstance<ClientManager>();
        isAuth = await clientManager.InitAsync();
    }

    public async Task StartClientAsync(string  joinCode)
    {
        await clientManager.StartClientAsync(joinCode);
    }

    public void StartMatchMaking()
    {
        clientManager.StartMatchMaking();
    }
}
