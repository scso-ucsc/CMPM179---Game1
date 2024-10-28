using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunScript : MonoBehaviour
{
    /// <summary>
    /// This script is responsible for spawning bullets and managing the bullet pool. Its methods are called by the Player script.
    /// </summary>

    // Bullet Prefab
    [SerializeField] private GameObject bulletPrefab;

    // Bullet Pool
    private List<GameObject> bulletPool;
    private int bulletPoolSize = 10;

    // Bullet Stats
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifeTime = 1f;
    [SerializeField] private Transform bulletParent;

    // Ammo
    // Player has 3 bullets which regenerate by 1 every 2 seconds
    [SerializeField] private int ammo = 3;
    private int maxAmmo = 3;
    private float ammoRegenRate = 2f;
    private bool regeneratingAmmo = false;
    [SerializeField] private SpriteRenderer[] ammoSprites;

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new List<GameObject>();

        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity, bulletParent);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    void Update()
    {
        if (ammo < maxAmmo && !regeneratingAmmo)
        {
            regeneratingAmmo = true;
            StartCoroutine(RegenerateAmmo());
        } else if (ammo == maxAmmo)
        {
            regeneratingAmmo = false;
        }
    }

    public void UpdateAmmoUI()
    {
        for (int i = 0; i < ammoSprites.Length; i++)
        {
            if (i < ammo)
            {
                // Set alpha to 1
                Color tempColor = ammoSprites[i].color;
                tempColor.a = 1f;
                ammoSprites[i].color = tempColor;
            }
            else
            {
                // Set alpha to 0.5
                Color tempColor = ammoSprites[i].color;
                tempColor.a = 0.2f;
                ammoSprites[i].color = tempColor;
            }
        }
    }

    public void ShootBullet(Vector3 gunPosition, Vector3 gunScale, Vector3 gunDirection)
    {
        // Find an inactive bullet in the pool
        GameObject bullet = bulletPool.Find(b => b.activeSelf == false);

        if (bullet != null)
        {
            bullet.transform.position = gunPosition + 0.5f * gunDirection;
            bullet.transform.localScale = gunScale;

            bullet.SetActive(true);
            bullet.GetComponent<TrailRenderer>().Clear();
            bullet.GetComponent<TrailRenderer>().enabled = true;
            bullet.GetComponent<Rigidbody2D>().velocity = gunDirection * bulletSpeed;
            AudioManager.instance.playPlayerSound(gunPosition);

            ammo--;
            UpdateAmmoUI();

            StartCoroutine(DeactivateBullet(bullet, bulletLifeTime));
        }
    }

    public float getAmmo()
    {
        return ammo;
    }

    private IEnumerator DeactivateBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
    }

    private IEnumerator RegenerateAmmo()
    {
        yield return new WaitForSeconds(ammoRegenRate);
        if (ammo < maxAmmo)
        {
            ammo++;
            UpdateAmmoUI();
        }
        regeneratingAmmo = false;
    }
}
