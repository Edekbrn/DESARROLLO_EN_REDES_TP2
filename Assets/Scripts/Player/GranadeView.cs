using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;

public class GranadeView : NetworkBehaviour
{
    [SerializeField] GameObject _visualMesh;
    [SerializeField] ParticleSystem explosionparticles;

    [Networked, OnChangedRender(nameof(ExplotionParticles))]
    NetworkBool _granade { get; set; }
    public override void Spawned()
    {
        var granade = GetComponentInParent<ShotHandler>();
        if (granade!=null)
        {
            granade.ThrowGranadeA += () => {
                if (HasStateAuthority)
                {
                    _granade = true; 
                }
            };
        }
    }

    void ExplotionParticles()
    {
        Debug.Log("particulas");
        explosionparticles.Play();
    }
}
