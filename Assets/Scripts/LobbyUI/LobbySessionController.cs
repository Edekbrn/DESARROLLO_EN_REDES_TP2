
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class LobbySessionController : NetworkBehaviour, IPlayerLeft, IStateAuthorityChanged
{
    [Header("Prefab")]
    [SerializeField] UiLobbyPlayer _uiLobbyPlayerPrefab;

    public const int MaxPlayerCount = 8;


    [Header("UI")]
    [SerializeField] Transform _contentTransform;
    [SerializeField] Button _toggleReadyButton;
    [SerializeField] Button _startGameButton;
    [SerializeField] Button _LeaveRoomButton;

    // WHERE TO SPAWN
    [SerializeField] string _GameSceneName;

    [Networked, OnChangedRender(nameof (OnPlayerDataChanged))]
    private NetworkDictionary<PlayerRef, LobbyPlayerData> _PlayerData => default;
    
    private UiLobbyPlayer[] _uiPlayerElements;
    private readonly List<KeyValuePair<PlayerRef, LobbyPlayerData>> _reusablePlayerSortList
         = new List<KeyValuePair<PlayerRef, LobbyPlayerData>>(MaxPlayerCount);


    void Awake()
    {
        _toggleReadyButton.onClick.AddListener(OnToggleReadyClick);
        _startGameButton.onClick.AddListener(OnClickStartGame);
        _LeaveRoomButton.onClick.AddListener(OnClickLeaveRoom);

        _startGameButton.interactable = false;
        CreateUILobbyEntries();
    }

    private void CreateUILobbyEntries()
    {
        _uiPlayerElements = new UiLobbyPlayer[MaxPlayerCount];

        for(int i = 0; i < MaxPlayerCount; i++)
        {
            var Instance = Instantiate(_uiLobbyPlayerPrefab, _contentTransform);
            _uiPlayerElements[i] = Instance;
        }
    }

    public override void Spawned()
    {
        var data = new LobbyPlayerData()
        {
            PlayerName = PlayerPrefs.GetString("Nickname"), // VER

            //PlayerName = "Boliviano", // VER
            IsReady = false
        };

        RPC_SetPlayerData(data);
        UpdatStartGameButton();
        OnPlayerDataChanged();
    }
    private PlayerRef ResolveSource(RpcInfo info)
        => info.Source.IsNone ? Runner.LocalPlayer : info.Source;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SetPlayerData(LobbyPlayerData data, RpcInfo info = default)
    {
        var source = ResolveSource(info);
        data.IsHost = source == Runner.LocalPlayer;

        if (_PlayerData.ContainsKey(source))
            _PlayerData.Set(source, data);
        else
            _PlayerData.Add(source, data);
    }

    
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_ToggleReadyState(RpcInfo info = default)
    {
        var source = ResolveSource(info);

        if (_PlayerData.TryGet(source, out LobbyPlayerData data))
        {
            data.IsReady = !data.IsReady;
            _PlayerData.Set(source, data);
        }

    }

    private void UpdatStartGameButton()
    {
        _startGameButton.interactable = CanStartGame(); // ss
    }

    private bool CanStartGame()
    {
        if(!HasStateAuthority) return false;

        foreach(var entry in _PlayerData)
        {
            if(!entry.Value.IsReady) return false;
        }


        return _PlayerData.Count > 1;

    }


    void OnClickLeaveRoom()
    {
        Runner.Shutdown(); // check further
    }

    void OnClickStartGame()
    {
        if(!CanStartGame()) return;

        Runner.SessionInfo.IsVisible = false;
        Runner.SessionInfo.IsOpen = false;
        Runner.LoadScene(_GameSceneName);
    }

    void OnToggleReadyClick()
    {
        RPC_ToggleReadyState();
    }

    void OnPlayerDataChanged()
    {
        _reusablePlayerSortList.Clear();
        foreach(var p in _PlayerData)
        {
            _reusablePlayerSortList.Add(p);
        }

        _reusablePlayerSortList.Sort((pair, pair2) => pair.Key.PlayerId.CompareTo(pair2.Key.PlayerId));

        for(int i = 0; i < _uiPlayerElements.Length; i++)
        {
            if(i < _reusablePlayerSortList.Count)
            {
                var value = _reusablePlayerSortList[i].Value;
                var isLocalPlayer = _reusablePlayerSortList[i].Key == Runner.LocalPlayer;
                var isHost = value.IsHost;

                _uiPlayerElements[i].SetPlayerState(value.PlayerName.Value, value.IsReady, isLocalPlayer, isHost);
                _uiPlayerElements[i].gameObject.SetActive(true);
            } else _uiPlayerElements[i].gameObject.SetActive(false);
        }

        UpdatStartGameButton();
    }

    public void PlayerLeft(PlayerRef player)
    {
        if(HasStateAuthority)
        {
            _PlayerData.Remove(player);

            UpdatStartGameButton();
        }
    }
    public void StateAuthorityChanged()
    {
        if(HasStateAuthority)
        {
            foreach(var playerData in _PlayerData)
            {
                bool IsActivePlayer = false;
                foreach(var activePlayerRef in Runner.ActivePlayers)
                {
                    if(playerData.Key == activePlayerRef)
                    {
                        IsActivePlayer = true;
                        break;
                    }
                }

                if(!IsActivePlayer)
                    _reusablePlayerSortList.Add(playerData);
            }

            foreach(var PlayerToRemove in _reusablePlayerSortList)
                _PlayerData.Remove(PlayerToRemove.Key);
            


            //todo updateStartGameButton
            UpdatStartGameButton();
        } 
    }
}
