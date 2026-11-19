using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnPlayerDefeat = delegate { };

    [Header("Player Spawns")]
    [SerializeField] NetworkPrefabRef _playerPrefab;
    readonly Dictionary<PlayerRef, NetworkObject> _playersInGame = new();


    [Header("UI")]
    public Canvas _winImage;
    public Canvas _defeatImage;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    public void SpawnAllPlayers(NetworkRunner runner)
    {
        if (!runner.IsServer) return;

        int i = 0;
        foreach (var player in runner.ActivePlayers)
            SpawnPlayer(runner, player, i++);
    }

    void SpawnPlayer(NetworkRunner runner, PlayerRef player, int index)
    {
        if (_playersInGame.ContainsKey(player)) return; // evita duplicados

        //var point = spawnPoints[index % spawnPoints.Length];
        //var obj = runner.Spawn(playerPrefab, point.position, point.rotation, inputAuthority: player);
        var obj = runner.Spawn(_playerPrefab, null, null, inputAuthority: player);
        runner.SetPlayerObject(player, null);
        _playersInGame[player] = obj;
    }
    public void TriggerWin()
    {
        if (Runner.LocalPlayer == Object.InputAuthority)
        {
            _winImage.gameObject.SetActive(true);
            _defeatImage.gameObject.SetActive(false);
        }
    }

    public void Defeat()
    {
            OnPlayerDefeat();
            _defeatImage.gameObject.SetActive(true);
    }
}