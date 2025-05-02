using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] public static bool isMultiplayer = false;
    [SerializeField] public bool inputMultiplayerOption = false;

    [SerializeField] public GameObject eventSystemObject;

    private Toggle checkbox;

    public static bool getIsMultiplayer() {
        return isMultiplayer;
    }

    public static void setIsMultiplayer(bool inputMultiplayerOption) {
        isMultiplayer = inputMultiplayerOption;
    }

    public void Update()
    {
        checkbox = GetComponent<Toggle>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGame()
    {
        //setIsMultiplayer(inputMultiplayerOption);
        Destroy(eventSystemObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void SingeplayerOrMultiplayer(bool toggleValue)
    {
        if (toggleValue) {
            setIsMultiplayer(toggleValue);
        } else {
            setIsMultiplayer(toggleValue);
        }

        setIsMultiplayer(getIsMultiplayer());

    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
