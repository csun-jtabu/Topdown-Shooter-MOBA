using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenuScript : MonoBehaviour
{
    public GameObject victoryText;
    public GameObject defeatText;

    void Start()
    {
        setWinText();
    }

    public void setWinText()
    {
        StatTrackerScript statTracker = GameObject.Find("StatTracker").GetComponent<StatTrackerScript>();

        victoryText.GetComponent<TextMeshProUGUI>().text = "Player " + statTracker.winner.ToString() + " wins";
        defeatText.GetComponent<TextMeshProUGUI>().text = "Player " + statTracker.loser.ToString() + " loses";
    }

    public void Back()
    {
        SceneManager.LoadScene(0);

    }
}
