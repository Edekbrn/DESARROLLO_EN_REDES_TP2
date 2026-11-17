
using Fusion;

public struct LobbyPlayerData : INetworkStruct
{
    public NetworkBool        IsReady;
    public NetworkBool        IsHost;
    public NetworkString<_32> PlayerName;
}
