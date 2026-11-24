using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;

public class LocalInputs : MonoBehaviour
{
    [SerializeField] float laneZ;

    NetworkInputData _inputData;

    Vector3 _MouseInput;

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


        _isFirePressed = Input.GetMouseButton(0);
        

        if (Input.GetKeyDown(KeyCode.G))
        {
            _isgranade = true;
        }

        //_isFirePressed |= Input.GetKeyDown(KeyCode.Space);
        
        Vector3 mouse = Input.mousePosition;
        mouse.z = Mathf.Abs(Camera.main.transform.position.z - laneZ); // distancia de la cámara al plano del jugador
        _MouseInput = Camera.main.ScreenToWorldPoint(mouse);
        _MouseInput.z = laneZ;


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

        _inputData.MousePosition = _MouseInput;

        return _inputData;
    }
    

}
