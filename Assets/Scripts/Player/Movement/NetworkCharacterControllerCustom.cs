using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCharacterControllerCustom : NetworkCharacterController
{
    public event Action<float> OnMovement = delegate { };

    [Header("Feel")]
    [Tooltip("Aceleración al invertir la dirección (más alta = giro más seco)")]
    public float turnAcceleration = 100f;
    [Range(0f, 1f)] public float airControl = 0.5f;
    public override void Move(Vector3 direction)
    {
        var deltaTime = Runner.DeltaTime;
        var previousPos = transform.position;
        var velocity = Velocity;

        // Tu eje de movimiento: input Z -> eje X del mundo
        float input = Mathf.Clamp(direction.z, -1f, 1f);

        // Gravedad
        if (Grounded && velocity.y < 0f) velocity.y = 0f;
        velocity.y += gravity * deltaTime;

        // Horizontal
        float target = input * maxSpeed;
        float vx = velocity.x;

        float rate;
        if (Mathf.Approximately(input, 0f))
            rate = braking;
        else if (Mathf.Abs(vx) > 0.01f && Mathf.Sign(input) != Mathf.Sign(vx))
            rate = turnAcceleration;
        else
            rate = acceleration;

        if (!Grounded) rate *= airControl;

        velocity.x = Mathf.MoveTowards(vx, target, rate * deltaTime);
        velocity.z = 0f;

        // Rotación suave
        if (!Mathf.Approximately(input, 0f))
        {
            var targetRot = Quaternion.Euler(0f, input > 0f ? 90f : -90f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * deltaTime);
        }

        Controller.Move(velocity * deltaTime);

        // Velocidad real tras colisiones: si chocas con una pared, vuelve a 0 sola
        Velocity = (transform.position - previousPos) * Runner.TickRate;
        Grounded = Controller.isGrounded;

        OnMovement(input);
    }
}
