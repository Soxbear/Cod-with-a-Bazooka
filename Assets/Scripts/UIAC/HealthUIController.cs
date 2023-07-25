using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUIController : UIController
{
    public float barChangeRate;

    private int healthValue;

    private float barValue;

    public int health {
        set {
            healthValue = value;
            number.text = value.ToString();
        }
        get {
            return healthValue;
        }
    }

    private int maxHealthValue;

    public int maxHealth {
        set {
            maxHealthValue = value;
            health = healthValue;
        }
        get {
            return maxHealthValue;
        }
    }

    public RectTransform bar;
    public TextMeshProUGUI number;

    void Update() {
        barValue += Mathf.Clamp((float) healthValue - barValue, -barChangeRate * Time.deltaTime, barChangeRate * Time.deltaTime);
        bar.anchoredPosition = new Vector3(-400f * (((float) maxHealthValue - barValue) / (float) maxHealthValue), bar.anchoredPosition.y);
    }

    public HealthUIController() {
        UIManager.healthUI = this;
    }
}
