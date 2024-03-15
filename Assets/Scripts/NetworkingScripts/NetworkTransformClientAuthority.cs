using Unity.Netcode.Components;

public class NetworkTransformClientAuthority : NetworkTransform
{
    protected override bool OnIsServerAuthoritative() => false;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }

    protected override void Update()
    {
        base.Update();
        
        if(!IsOwner) return;
        if(IsHost || IsServer) return;

        CanCommitToTransform = IsOwner;
        if(NetworkManager.IsConnectedClient)
        {
            TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
        }
    }
}
