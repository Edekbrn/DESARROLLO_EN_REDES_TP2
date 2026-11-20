using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public float movementInput;
    public Vector2 MousePosition;
    public NetworkBool isFirePressed;
    public NetworkBool isgranade;

    public NetworkButtons networkButtons;
}

enum MyButtons
{
    Jump = 0,
}
