using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreUI : UIWindow
{
    public RectTransform rankPanel;
    public GameObject rankgingList;

    public TextMeshProUGUI userscore;
    private IngameController ingameController;


    private void Start()
    {
        ingameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<IngameController>();
        Close();
    }

    public void UpdateBoard()
    {
        int score = ingameController.totalScore;
        int maxScore = Mathf.Max(score, ClientUserData.score);
        int coin = ingameController.totalScore / 100;

        UserDataManager.UpdatUserData(ClientUserData.name, maxScore, ClientUserData.coin + coin, ClientUserData.knight);

        var allRank = UserDataManager.GetSortedRank();
        var userRank = UserDataManager.GetUserRank(ClientUserData.name);

        foreach (var rank in allRank)
        {
            string format = string.Format("{0}. {1} : {2}", rank.rank, rank.userName, rank.score);
            MakePanel(format);
        }

        userscore.text = score.ToString();
    }

    private void MakePanel(string format)
    {
        GameObject instance = Instantiate(rankgingList, rankPanel);

        TextMeshProUGUI text = instance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.text = format;
    }

    public void OnClickToMain()
    {
        GameManager.Instance.gameState = GameManager.GameState.main;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
