using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //GameManager Variables
    public static GameManager instance;
    private bool isGameOver;
    private float timeElapsed;

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
        timeElapsed = 0;
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver == false)
        {
            timeElapsed += 1.0f * Time.deltaTime;
        }
    }

    public void gameOver()
    {
        isGameOver = true;
    }

    public bool getGameOverStatus()
    {
        return isGameOver;
    }

    public int getTimeElapsed()
    {
        return (int)timeElapsed;
    }
}
