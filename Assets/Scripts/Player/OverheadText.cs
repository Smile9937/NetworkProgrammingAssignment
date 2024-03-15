using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class OverheadText : NetworkBehaviour
{
    [SerializeField] private TMP_Text textField;

    [SerializeField] private NetworkObject networkObject;

    public NetworkVariable<FixedString128Bytes> playerName =
     new NetworkVariable<FixedString128Bytes>();

    void Awake()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
    }

    public override void OnNetworkDespawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= ClientConnected;
    }

    private void ClientConnected(ulong obj)
    {
        textField.text = playerName.Value.ToString();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            playerName.Value = SavedClientInformationManager.GetUserData(networkObject.OwnerClientId).userName;
        }
        else
        {
            SetNameClientRpc(SavedClientInformationManager.GetUserData(networkObject.OwnerClientId).userName);
        }
    }

    [ClientRpc]
    public void SetNameClientRpc(string name)
    {
        playerName.Value = name;
    }
}