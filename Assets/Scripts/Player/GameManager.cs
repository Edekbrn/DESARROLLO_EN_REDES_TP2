using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;


public enum GameRules
{
    Time,
    Lives,
}

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnPlayerDefeat = delegate { };


    

    [Header("Player Spawns")]
    [SerializeField] NetworkPrefabRef _playerPrefab;
    [SerializeField] GameObject _spawns;
    readonly Dictionary<PlayerRef, NetworkObject> _playersInGame = new();


    [Header("UI")]
    [SerializeField] Canvas _winImage;
    [SerializeField] Canvas _defeatImage;


    int _MaxPlayerCount;
    int _CurrentPlayerCount;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    public void PrepareGame(NetworkRunner runner, (GameRules, int)? Rules)
    {
        if (!runner.IsServer) return;

    

        // Spawn Players
        int i = 0;
        foreach (var player in runner.ActivePlayers)
            SpawnPlayer(runner, player, i++);

        // Set ruleset, victory and lose conditions.
        if(Rules.HasValue)
        {
            switch(Rules.Value.Item1)
            {
                case GameRules.Lives:
                    SetGamemodeLives();
                    break;
                case GameRules.Time:
                    break;


            }
        } else
        {
            // Debug purposes
            SetGamemodeLives();
        }


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
        var obj = runner.Spawn(_playerPrefab, _spawns.transform.position, _spawns.transform.rotation, inputAuthority: player);
        runner.SetPlayerObject(player, null);
        _playersInGame[player] = obj;
    }



    void SetGamemodeLives(int Lives = 3)
    {
        Debug.Log("Here");
        foreach(var t in _playersInGame.Values)
            t.GetComponent<LifeHandler>().SetLives(Lives);
        
    }

    public void LivesMode_OnPlayerEliminated(PlayerRef player)
    {
        if (!Runner.IsServer) return;

        PlayerRef LastAlive = PlayerRef.None;

        int PlayerAlives = 0;
        foreach(var t in _playersInGame)
        {
            if(!t.Value.GetComponent<LifeHandler>().NoLives)
            {
                LastAlive = t.Key;
                PlayerAlives++;
            }
        }

        if(PlayerAlives <= 1)
        {
            // Fin de ronda, comprobar quien queda vivo y coronarlo como Ganador.
            RPC_ShowResult(LastAlive);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_ShowResult(PlayerRef winner)
    {
        bool isWinner = winner != PlayerRef.None && winner == Runner.LocalPlayer;

        _winImage.gameObject.SetActive(isWinner);
        _defeatImage.gameObject.SetActive(!isWinner);
    }




    // UIS
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