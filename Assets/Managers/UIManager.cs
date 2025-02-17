using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI endScoreText;

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
        gameOverUI.SetActive(false);
    }

    void FixedUpdate()
    {
        updateScoreUI();
    }

    private void updateScoreUI()
    {
        scoreText.text = GameManager.instance.getTimeElapsed().ToString();
    }

    public void showGameOverUI()
    {
        StartCoroutine(spawnGameOverOverlay());
    }

    IEnumerator spawnGameOverOverlay()
    {
        yield return new WaitForSeconds(2f);
        gameOverUI.SetActive(true);
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
        endScoreText.text = "Score: " + GameManager.instance.getTimeElapsed().ToString();
    }
}
