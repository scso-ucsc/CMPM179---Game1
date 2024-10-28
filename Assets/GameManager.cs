using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //GameManager Variables
    public static GameManager instance;
    private bool isGameOver;
    private float timeElapsed;
    private Vector2 screenBounds;

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

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
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
        UIManager.instance.showGameOverUI();
    }

    public bool getGameOverStatus()
    {
        return isGameOver;
    }

    public int getTimeElapsed()
    {
        return (int)timeElapsed;
    }

    public Vector2 getScreenBounds()
    {
        return screenBounds;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
