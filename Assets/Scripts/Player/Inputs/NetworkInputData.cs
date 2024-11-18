using Fusion;

public struct NetworkInputData : INetworkInput
{
    public float movementInput;
    public NetworkBool isFirePressed;
    public NetworkBool isgranade;

    public NetworkButtons networkButtons;
}

enum MyButtons
{
    Jump = 0,
}
