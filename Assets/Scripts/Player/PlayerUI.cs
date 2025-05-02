using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Player currentPlayer;
    public bool secondPlayer = false;
    private bool multiplayer;
    public Image health;
    public TextMeshProUGUI healthText;
    public Image shield;
    public TextMeshProUGUI shieldText;
    private float originalHealthWidth;
    private Vector2 originalHealthPosition;

    public Image stamina;
    public TextMeshProUGUI staminaText;
    private float originalStaminaWidth;
    private Vector2 originalStaminaPosition;

    public Image dash;
    public Image dodge;

    void Start()
    {
        RectTransform healthBarRect = health.GetComponent<RectTransform>();
        originalHealthPosition = healthBarRect.anchoredPosition;
        originalHealthWidth = healthBarRect.rect.width;

        RectTransform staminaBarRect = stamina.GetComponent<RectTransform>();
        originalStaminaPosition = staminaBarRect.anchoredPosition;
        originalStaminaWidth = staminaBarRect.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        findPlayer();        

        updateHealth();
        updateShield();
        updateStamina();
        updateDash();
        updateDodge();
    }

    void findPlayer() {
        multiplayer = MainMenuScript.getIsMultiplayer();

        if (currentPlayer == null) {
            if (multiplayer) {
                if (secondPlayer) {
                    currentPlayer = GameObject.FindGameObjectsWithTag("MultiPlayerTwo")[0].GetComponent<Player>();
                } else {
                    currentPlayer = GameObject.FindGameObjectsWithTag("MultiPlayerOne")[0].GetComponent<Player>();
                }
            } else {
                currentPlayer = GameObject.FindGameObjectsWithTag("SinglePlayer")[0].GetComponent<Player>();
            }

            if (currentPlayer == null) {
                return; // Still not spawned
            }
        }
    }

    void updateHealth()
    {
        float healthRatio = (float)currentPlayer.Hp / (float)currentPlayer.MaxHp;
        RectTransform healthBarRect = health.GetComponent<RectTransform>();
        healthBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalHealthWidth * healthRatio);
        float currentWidth = healthBarRect.rect.width;
        float widthOffset = originalHealthWidth - currentWidth;
        healthBarRect.anchoredPosition = new Vector2(originalHealthPosition.x - (widthOffset / 2f), originalHealthPosition.y);

        RectTransform healthTextRect = healthText.GetComponent<RectTransform>();
        healthTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalHealthWidth * healthRatio);
    }

    void updateShield()
    {
        float shieldRatio = (float)currentPlayer.Shield / (float)currentPlayer.MaxShield;
        RectTransform shieldBarRect = shield.GetComponent<RectTransform>();
        shieldBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalHealthWidth * shieldRatio);
        float currentWidth = shieldBarRect.rect.width;
        float widthOffset = originalHealthWidth - currentWidth;
        shieldBarRect.anchoredPosition = new Vector2(originalHealthPosition.x - (widthOffset / 2f), originalHealthPosition.y);

        RectTransform shieldTextRect = shieldText.GetComponent<RectTransform>();
        shieldTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalHealthWidth * shieldRatio);
        //shieldTextRect.anchoredPosition = new Vector2(originalHealthPosition.x - (widthOffset / 2f), originalHealthPosition.y);
    }

    void updateStamina()
    {
        float staminaRatio = (float)currentPlayer.sprintBar / (float)currentPlayer.MaxSprint;
        RectTransform staminaBarRect = stamina.GetComponent<RectTransform>();
        staminaBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalStaminaWidth * staminaRatio);
        float currentWidth = staminaBarRect.rect.width;
        float widthOffset = originalStaminaWidth - currentWidth;
        staminaBarRect.anchoredPosition = new Vector2(originalStaminaPosition.x - (widthOffset / 2f), originalStaminaPosition.y);

        RectTransform staminaTextRect = staminaText.GetComponent<RectTransform>();
        staminaTextRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalStaminaWidth * staminaRatio);
    }

    void updateDash()
    {
        if (currentPlayer._canDash)
        {
            dash.color = new Color(0f, 0.65f, 0.65f);
        }
        else if (currentPlayer._isDashing)
        {
            dash.color = Color.red;
        }
        else
        {
            dash.color = Color.grey;
        }
    }

    void updateDodge()
    {
        if (currentPlayer._canDodge)
        {
            dodge.color = new Color(1f, 0.65f, 0.4f);
        }
        else if (currentPlayer._isDodging)
        {
            dodge.color = Color.red;
        }
        else
        {
            dodge.color = Color.grey;
        }
    }
}
