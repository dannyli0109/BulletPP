using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    public float currentTimerA = 0;
    public float currentTimerB = 0;

    public float waitTimeA = 0.1f;
    public float waitTimeB = 0.1f;

    public int currentCountingYourScore = 0;
    public int currentCountingHighScore = 0;

    public int holdingYourScore = 0;
    public int holdingHighScore = 0;
    public bool holdingAisBigger;


    public GameObject folderA;
    public GameObject folderB;

    public TextMeshProUGUI TotalYourScoreValueText;
    public TextMeshProUGUI TotalHighScoreValueText;

    private void Start()
    {
        //SaveNewHighscore();
        holdingYourScore = PlayerPrefs.GetInt("YourScore");
        Debug.Log("your"+holdingYourScore);
        Debug.Log("high " + PlayerPrefs.GetInt("HighScore"));
        Debug.Log("last " + PlayerPrefs.GetInt("LastHighScore"));

        if (PlayerPrefs.GetInt("YourScore") == PlayerPrefs.GetInt("HighScore"))
        {
            holdingHighScore = PlayerPrefs.GetInt("LastHighScore");
          
        }
        else
        {
            holdingHighScore = PlayerPrefs.GetInt("HighScore");
            holdingAisBigger = false;
        }

        if (holdingYourScore > holdingHighScore)
        {
            holdingAisBigger = true;
        }

        if(PlayerPrefs.GetInt("HighScore")< holdingYourScore)
        {
            PlayerPrefs.SetInt("HighScore", holdingYourScore);
        }
    }

    public void SaveNewHighscore()
    {
        PlayerPrefs.SetInt("LastHighScore", 0);

            PlayerPrefs.SetInt("HighScore", 121);

        PlayerPrefs.SetInt("YourScore", 62);
    }

    void Update()
    {
        IncreaseNumbers();
    }

    void IncreaseNumbers()
    {
        if (currentCountingYourScore < holdingYourScore)
        {
            int holding = (int)(currentTimerA / waitTimeA);
            if (holding < 1)
            {
                holding = 0;
            }
            if (currentTimerA > (waitTimeA))
            {
                currentCountingYourScore = Mathf.Clamp(currentCountingYourScore + holding, 0, holdingYourScore);
                  
                TotalYourScoreValueText.text = currentCountingYourScore.ToString();
            }

            currentTimerA -= holding * waitTimeA;

        }
        else
        {
            if (holdingAisBigger)
            {
                folderA.transform.localScale = Vector3.MoveTowards(folderA.transform.localScale, new Vector3(1.5f, 1.5f, 1.5f),3* Time.deltaTime);
            }
        }

        if (currentCountingHighScore < holdingHighScore)
        {
            int holding = (int)(currentTimerB / waitTimeB);
            if (holding < 1)
            {
                holding = 0;
            }

            if (currentTimerB > (waitTimeB))
            {


                currentCountingHighScore += holding;
                TotalHighScoreValueText.text = currentCountingHighScore.ToString();
            }
            currentTimerB -= holding * waitTimeB;
        }
        else
        {
            if (!holdingAisBigger)
            {
                folderB.transform.localScale = Vector3.MoveTowards(folderB.transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), 3 * Time.deltaTime);
            }
        }
        currentTimerA += Time.deltaTime;
        currentTimerB += Time.deltaTime;
    }
}

