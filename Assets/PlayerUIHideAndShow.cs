using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHideAndShow : MonoBehaviour
{

    [SerializeField] public GameObject secondPlayerUI;

    void Start()
    {
       bool multiplayer = MainMenuScript.getIsMultiplayer();
       secondPlayerUI.SetActive(multiplayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
