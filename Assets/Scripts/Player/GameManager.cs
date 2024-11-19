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

    public Canvas _winImage;
    public Canvas _defeatImage;

    private void Awake()
    {
        Instance = this;
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