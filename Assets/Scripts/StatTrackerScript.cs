using UnityEngine;
using UnityEngine.InputSystem;

public class StatTrackerScript : MonoBehaviour
{
    // tracks the stats for the end screen
    public int winner = 0;
    public int loser = 0;
    public int towers1 = 0;
    public int towers2 = 0;
    public int minions1 = 0;
    public int minions2 = 0;
    public int kills1 = 0;
    public int kills2 = 0;

    public GameObject plaerUICanvas;
    public GameObject gameOverCanvas;

    public void setWinner(int winner)
    {
        this.winner = winner;
    }
    public void setLoser(int loser)
    {
        this.loser = loser;
    }
    public void incTowers1()
    {
        towers1++;
    }
    public void incTowers2()
    {
        towers2++;
    }
    public void incMinions1()
    {
        minions1++;
    }
    public void incMinions2()
    {
        minions2++;
    }
    public void incKills1()
    {
        kills1++;
    }
    public void incKills2()
    {
        kills2++;
    }
    public void GameOver()
    {
        // Disable EnemyMovement for all minions with tags "minionteam1" and "minionteam2"
        string[] minionTags = { "MinionTeam1", "MinionTeam2"};
        foreach (string tag in minionTags)
        {
            GameObject[] minions = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject minion in minions)
            {
                minion.SetActive(false);
            }
        }

        // Disable PlayerInput for all players with tags "singleplayer", "multiplayer1", and "multiplayer2"
        string[] playerTags = { "SinglePlayer", "MultiPlayerOne", "MultiPlayerTwo" };
        foreach (string tag in playerTags)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject player in players)
            {
                var playerInput = player.GetComponent<PlayerInput>();
                if (playerInput != null)
                {
                    playerInput.enabled = false;
                }
            }
        }
        if (plaerUICanvas != null)
        {
            plaerUICanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("PlayerUICanvas not found in the scene!");
        }
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("GameOverCanvas not found in the scene!");
        }
    }
}
