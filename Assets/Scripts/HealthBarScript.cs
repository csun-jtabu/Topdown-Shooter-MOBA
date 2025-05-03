using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Entity currentEntitee;
    //public Image emptyHealth;
    public Image health;
    private float originalHealthWidth;

    private void Start()
    {
        health.gameObject.SetActive(false);
        RectTransform healthBarRect = health.GetComponent<RectTransform>();
        originalHealthWidth = healthBarRect.rect.width;
    }

    void updateHealth()
    {
        RectTransform healthBarRect = health.GetComponent<RectTransform>();
        float healthRatio = (float)currentEntitee.Hp / (float)currentEntitee.MaxHp;
        float newWidth = originalHealthWidth * healthRatio; // Scale based on the original width
        healthBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }
    private void Update()
    {
        if (currentEntitee.Hp < currentEntitee.MaxHp)
        {
            //emptyHealth.gameObject.SetActive(true);
            health.gameObject.SetActive(true);
            updateHealth();
        }
    }
}

