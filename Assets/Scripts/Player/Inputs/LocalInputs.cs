using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class LocalInputs : MonoBehaviour
{
    NetworkInputData _inputData;

    bool _isJumpPressed;
    bool _isFirePressed;
    bool _isgranade;

    void Start()
    {
        _inputData = new NetworkInputData();
    }

    void Update()
    {
        _inputData.movementInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isFirePressed = true;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            _isgranade = true;
        }

        //_isFirePressed |= Input.GetKeyDown(KeyCode.Space);

        _isJumpPressed |= Input.GetKeyDown(KeyCode.W);
    }
    
    public NetworkInputData GetLocalInputs()
    {
        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _inputData.isgranade = _isgranade;
        _isgranade = false;

        _inputData.networkButtons.Set(MyButtons.Jump, _isJumpPressed);
        _isJumpPressed = false;

        return _inputData;
    }

}
