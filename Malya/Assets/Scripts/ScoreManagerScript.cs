using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagerScript : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;
    int score;
    public GameObject scoreTextObj;
    public GameObject panelObj;


    public static ScoreManagerScript current;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartScore()
    {
        InvokeRepeating("IncrementScore", 0.1f, 0.5f);
        scoreTextObj.SetActive(true);
    }

    void IncrementScore()
    {
        score += 1;
        scoreText.text = score.ToString();
    }

    public void DiamondScore()
    {
        score += 10;
        scoreText.text = score.ToString();
    }

    public void StopScore()
    {
        CancelInvoke("IncrementScore");

        //calculate/record highest score
        PlayerPrefs.SetInt("score", score);

        if(PlayerPrefs.HasKey("highScore"))
        {
            if(score > PlayerPrefs.GetInt("highScore"))
            {
                PlayerPrefs.SetInt("highScore", score);
            }
        }
        else
        {
            PlayerPrefs.SetInt("highScore", score);
        }

        highScoreText.text = PlayerPrefs.GetInt("highScore").ToString();
        panelObj.SetActive(true);
    }
}
