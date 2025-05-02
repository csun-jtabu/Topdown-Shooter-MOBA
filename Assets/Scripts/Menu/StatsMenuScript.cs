using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenuScript : MonoBehaviour
{
    public GameObject TowersText1;
    public GameObject MinionText1;
    public GameObject KillsText1;
    public GameObject TowersText2;
    public GameObject MinionText2;
    public GameObject KillsText2;

    
    void Start()
    {
        // Get the stats from the StatTracker
        getStats();
    }
    public void getStats()
    {
        StatTrackerScript statTracker = GameObject.Find("StatTracker").GetComponent<StatTrackerScript>();

        TowersText1.GetComponent<TextMeshProUGUI>().text = "Towers Destroyed: " + statTracker.towers1.ToString();
        MinionText1.GetComponent<TextMeshProUGUI>().text = "Minions Killed: " + statTracker.minions1.ToString();
        KillsText1.GetComponent<TextMeshProUGUI>().text = "Kills: " + statTracker.kills1.ToString();
        TowersText2.GetComponent<TextMeshProUGUI>().text = "Towers Destroyed: " + statTracker.towers2.ToString();
        MinionText2.GetComponent<TextMeshProUGUI>().text = "Minions Killed: " + statTracker.minions2.ToString();
        KillsText2.GetComponent<TextMeshProUGUI>().text = "Kills: " + statTracker.kills2.ToString();
    }
}
