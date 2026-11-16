
using Fusion;

public struct LobbyPlayerData : INetworkStruct
{
    public NetworkBool IsReady;
    public NetworkString<_32> PlayerName;
}
