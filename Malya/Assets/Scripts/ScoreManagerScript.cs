using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagerScript : MonoBehaviour
{
    public Text scoreText;
    int score;
    public GameObject scoreTextObj;

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

    public void StopScore()
    {
        CancelInvoke("IncrementScore");
    }
}
