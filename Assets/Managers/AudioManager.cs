using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //AudioManager Variables
    public static AudioManager instance;
    [SerializeField] private AudioSource backgroundMusicSource, impactAudioSource, bulletFireSource, playerSource;
    [SerializeField] private AudioClip playerHitSound, bulletHitSound, civilianHitSound;

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

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusicSource.Play();
    }

    public void playImpactAudio(Vector2 impactLocation, string hitObject)
    {
        impactAudioSource.transform.position = impactLocation;
        if (hitObject == "PLAYER")
        {
            impactAudioSource.clip = playerHitSound;
        }
        else if (hitObject == "BULLET")
        {
            impactAudioSource.clip = bulletHitSound;
        }
        else if (hitObject == "CIVILIAN")
        {
            impactAudioSource.clip = civilianHitSound;
        }
        impactAudioSource.Play();
    }

    public void playFireBullet(Vector2 fireLocation)
    {
        bulletFireSource.transform.position = fireLocation;
        bulletFireSource.Play();
    }

    public void playPlayerSound(Vector2 playerLocation)
    {
        playerSource.transform.position = playerLocation;
        playerSource.Play();
    }
}
