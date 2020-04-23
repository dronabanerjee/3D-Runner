using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 *  The ScoreManagerScript class manages the score updates in the game. 
 *  The score is incremented by 1 in every 0.5 seconds and score is incremented by 10 if a diamond is collected.
 */
public class ScoreManagerScript : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;
    int score;
    public GameObject scoreTxtObj;
    public GameObject panelObj;
    public static ScoreManagerScript current;

    private void Awake(){
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        score =0;
        scoreText.text = "0";


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartScore(){
        InvokeRepeating("IncrementScore",0.1f,0.5f);
        scoreTxtObj.SetActive(true);
    }

    void IncrementScore(){
        score+=1;
        scoreText.text = score.ToString();
    }

    public void DiamondScore(){
        score += 10;

        scoreText.text = score.ToString();
    }

    public void StopScore(){
        CancelInvoke("IncrementScore");

        //save the result
        PlayerPrefs.SetInt("score",score);

        //save the highest score
        if(PlayerPrefs.HasKey("highScore")){
            if(score>PlayerPrefs.GetInt("highScore")){
                PlayerPrefs.SetInt("highScore",score);
            }
        }else{
                PlayerPrefs.SetInt("highScore",score);
        }

        //set the value of highscore to the label
        highScoreText.text = PlayerPrefs.GetInt("highScore").ToString();

        //make appear the game over panel
        panelObj.SetActive(true);
    }
}
