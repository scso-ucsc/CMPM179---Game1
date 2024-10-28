using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    //Bullet Manager Variables
    private List<GameObject> bulletsList = new List<GameObject>();
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private Transform bulletParent, leftFireSpawnPoint, rightFireSpawnPoint;
    [SerializeField] private string firingDirection; //LEFT Z=90; RIGHT Z=-90
    [SerializeField] private int maxBullets;
    [SerializeField] private float bulletSpeed, yMin, yMax;
    private float bulletFireRateMin, bulletFireRateMax, directionMultiple;

    // Start is called before the first frame update
    void Start()
    {
        bulletFireRateMax = 5.0f;
        bulletFireRateMax = 4.0f;

        for (int i = 0; i < maxBullets; i++)
        {
            Vector3 spawnPoint = new Vector3(0, 0);
            GameObject newBullet = Instantiate(bulletObj, spawnPoint, Quaternion.Euler(0, 0, 90), bulletParent);
            newBullet.SetActive(false);
            bulletsList.Add(newBullet);
        }

        StartCoroutine(activateBulletFires());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        updateBulletSpawnRate();
    }

    private void fireBullet()
    {
        GameObject chosenBullet = getBullet();
        if (chosenBullet != null)
        {
            if (firingDirection == "LEFT")
            {
                chosenBullet.transform.position = leftFireSpawnPoint.position;
                directionMultiple = -1f;
            }
            else
            { //firingDirection == "RIGHT"
                chosenBullet.transform.position = rightFireSpawnPoint.position;
                directionMultiple = 1f;
            }
            chosenBullet.SetActive(true);
            chosenBullet.GetComponent<TrailRenderer>().enabled = true;
            chosenBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(directionMultiple * bulletSpeed, 0f);
            AudioManager.instance.playFireBullet(this.transform.position);

            // disable bullet after 2 seconds
            StartCoroutine(disableBullet(chosenBullet));
        }
    }

    private GameObject getBullet()
    {
        for (int i = 0; i < maxBullets * 2; i++)
        {
            if (!bulletsList[i].activeInHierarchy)
            {
                return bulletsList[i];
            }
        }
        return null;
    }

    private void updateBulletSpawnRate()
    {
        if (GameManager.instance.getTimeElapsed() <= 15)
        {
            bulletFireRateMax = 5.0f;
            bulletFireRateMin = 4.0f;
        }
        else if (GameManager.instance.getTimeElapsed() <= 30)
        {
            bulletFireRateMax = 4.0f;
            bulletFireRateMin = 3.0f;
        }
        else if (GameManager.instance.getTimeElapsed() <= 45)
        {
            bulletFireRateMax = 3.0f;
            bulletFireRateMin = 2.0f;
        }
        else if (GameManager.instance.getTimeElapsed() <= 60)
        {
            bulletFireRateMax = 1.5f;
            bulletFireRateMin = 0.5f;
        }
        else
        {
            bulletFireRateMax = 0.5f;
            bulletFireRateMin = 0.4f;
        }
    }

    IEnumerator activateBulletFires()
    {
        while (GameManager.instance.getGameOverStatus() == false)
        {
            yield return new WaitForSeconds(Random.Range(bulletFireRateMin, bulletFireRateMax));
            Vector2 randomSpawnPoint = new Vector2(this.transform.position.x, Random.Range(yMin, yMax));
            if (GameManager.instance.getGameOverStatus() == false)
            {
                this.transform.position = randomSpawnPoint;
                fireBullet();
            }
        }
    }

    private IEnumerator disableBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(6.0f);
        bullet.SetActive(false);
    }
}
