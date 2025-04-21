using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] public static bool isMultiplayer = false;
    [SerializeField] public bool inputMultiplayerOption = false;

    [SerializeField] public GameObject eventSystemObject;

    public static bool getIsMultiplayer() {
        return isMultiplayer;
    }

    public static void setIsMultiplayer(bool inputMultiplayerOption) {
        isMultiplayer = inputMultiplayerOption;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        setIsMultiplayer(inputMultiplayerOption);
        Destroy(eventSystemObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
