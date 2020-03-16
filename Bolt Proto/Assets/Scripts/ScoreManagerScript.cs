using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagerScript : MonoBehaviour
{
    public GameObject scoreTxtObj;
    public static ScoreManagerScript current;

    public Text scoreText;
    int score;
    

    private void Awake(){
        current = this;
    }

    public void StartScore()
    {
        scoreTxtObj.SetActive(true);
        InvokeRepeating("IncrementScore", 0.1f, 0.5f);
        
    }

    void Start()
    {
        scoreText.text = "0";
        score = 0;
        
    }

    public void StopScore()
    {
        CancelInvoke("IncrementScore");
    }

    void IncrementScore(){
        score = score + 1;
        scoreText.text = score.ToString();
    }

}
