using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabControl : MonoBehaviour
{
    void Start()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Login OK!/Conta criada!");
        StartCoroutine(GetHighestLeaderboard());
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Erro no login");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "GeniusScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        StartCoroutine(GetHighestLeaderboard());
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Novo placar aplicado!");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "GeniusScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    public IEnumerator GetHighestLeaderboard()
    {
        yield return new WaitForSeconds(1);
        var request = new GetLeaderboardRequest
        {
            StatisticName = "GeniusScore",
            StartPosition = 0,
            MaxResultsCount = 1
        };
        PlayFabClientAPI.GetLeaderboard(request, OnHighestLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }

    void OnHighestLeaderboardGet(GetLeaderboardResult result)
    {
        if (result.Leaderboard.Count > 0)
        {
            int pos = result.Leaderboard[0].Position;
            string id = result.Leaderboard[0].PlayFabId;
            int score = result.Leaderboard[0].StatValue;
            Debug.Log(pos + " " + id + " " + score);
            Score.instance.highestScore = score;
        }
        else
            Score.instance.highestScore = 0;

        Score.instance.ResetarPlacar();
    }
}