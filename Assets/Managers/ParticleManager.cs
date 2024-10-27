using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;
    [SerializeField] private GameObject deathParticleSource, bulletParticleSource;
    [SerializeField] private ParticleSystem deathParticles, bulletParticles;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void emitDeathParticles(Vector2 deathLocation)
    {
        deathParticleSource.transform.position = deathLocation;
        deathParticles.Play();
    }

    public void emitBulletParticles(Vector2 hitLocation)
    {
        bulletParticleSource.transform.position = hitLocation;
        bulletParticles.Play();
    }
}
