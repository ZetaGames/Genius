using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score instance;
    PlayFabControl PFControl;
    public Text txtScore;
    public Text txtHighestScore;
    public int highestScore = 0;
    public int score = 0;


    void Awake()
    {
        instance = this;
        PFControl = FindObjectOfType<PlayFabControl>();
    }

    void Start()
    {
        txtScore.text = string.Format("Score: {0}", score);
        txtHighestScore.text = string.Format("Highest score: {0}", highestScore);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            score++;
            txtScore.text = string.Format("Score: {0}", score);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            PFControl.SendLeaderboard(score);
        }
    }

    public void ResetarPlacar()
    {
        score = 0;
        txtScore.text = string.Format("Score: {0}", score);
        txtHighestScore.text = string.Format("Highest score: {0}", highestScore);
        Debug.Log("Placar atualizado!");
    }
}