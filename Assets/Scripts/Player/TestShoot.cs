using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShoot : NetworkBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] PlayerController controller;   // antes era PlayerAim

    [SerializeField, Range(0f, 1f)] float bodyWeight = 0.8f;
    [SerializeField, Range(0f, 1f)] float headWeight = 0.3f;
    [SerializeField] float smoothing = 20f;

    Vector3 smoothedAim;
    bool initialized;
    int aimLayer;

    void Reset() => animator = GetComponent<Animator>();
    void Awake() => aimLayer = animator.GetLayerIndex("Aim"); 

    void OnAnimatorIK(int layerIndex)
    {
        if (layerIndex != aimLayer) return;  
        if (controller == null || !controller.Object || !controller.Object.IsValid) return;

        Vector3 target = controller.AimPoint;
        if (!initialized) { smoothedAim = target; initialized = true; }
        smoothedAim = Vector3.Lerp(smoothedAim, target, smoothing * Time.deltaTime);

        animator.SetLookAtWeight(1f, bodyWeight, headWeight, 0f, 0.5f);
        animator.SetLookAtPosition(smoothedAim);
    }
}
