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
    [SerializeField] private float bulletSpeed;
    private float bulletFireRate, directionMultiple;

    // Start is called before the first frame update
    void Start()
    {
        bulletFireRate = 5.0f;

        for (int i = 0; i < maxBullets; i++)
        {
            Vector3 spawnPoint = new Vector3(0, 0);
            GameObject newBullet = Instantiate(bulletObj, spawnPoint, Quaternion.Euler(0, 0, 90), bulletParent);
            newBullet.SetActive(false);
            bulletsList.Add(newBullet);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void fireBullet(string firingDirection)
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
            chosenBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(directionMultiple * bulletSpeed, 0f);
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
}
