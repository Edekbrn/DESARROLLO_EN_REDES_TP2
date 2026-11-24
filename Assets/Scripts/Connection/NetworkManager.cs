using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{

    (GameRules, int)? _Ruleset;

    public void GetRuleset(GameRules Rules, int Quantity)
    {
        // Limpio antigua referencia (en caso de exista) para luego guardar.
        if(_Ruleset.HasValue) _Ruleset = null;
        _Ruleset = (Rules, Quantity);
    }


    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        if (!NetworkPlayer.Local) return;

        input.Set(NetworkPlayer.Local.Inputs.GetLocalInputs());
    }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) 
    {
        runner.Shutdown();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (!runner.IsServer) return;

        var gm = FindObjectOfType<GameManager>();

        Debug.Log("Pass1");
        if (gm == null) return; // por ejemplo, si cargó el lobby y no la escena Game
        Debug.Log("Pass2");

        // Preparar 
        gm.PrepareGame(runner, null);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {}
    public void OnSceneLoadStart(NetworkRunner runner) {}
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
}
