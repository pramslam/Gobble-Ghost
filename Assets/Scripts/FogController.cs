using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] ParticleSystem fogParticles;
    [SerializeField] ParticleSystem.ShapeModule shapeModule;

    void Awake()
    {
        fogParticles = GetComponent<ParticleSystem>();
        shapeModule = fogParticles.shape;
    }

    // Public Functions
    #region
    public void FogDecrease()
    {
        shapeModule.radius += 0.25f;
        Debug.Log("Decrease Fog. New Value = " + shapeModule.radius);
    }

    public void FogReset()
    {
        shapeModule.radius = 7;
        Debug.Log("Reset Fog. New Value = " + shapeModule.radius);
    }
    #endregion
}
