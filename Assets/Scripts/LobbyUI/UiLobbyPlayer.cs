using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UiLobbyPlayer : MonoBehaviour
{
    //[seializedfield]
    [SerializeField] TextMeshProUGUI _PlayerNameText;
    [SerializeField] Image _ReadyState;
    [SerializeField] GameObject _HostIcon;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetPlayerState(string PlayerName, bool isReady, bool isLocalPlayer, bool isHost)
    {
        _PlayerNameText.text = PlayerName;

        _PlayerNameText.color = isLocalPlayer ? Color.green : Color.black;

        _ReadyState.color = isReady ? Color.green : Color.red;

        _HostIcon.SetActive(isHost);

    }
}
