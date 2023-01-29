using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public InputMaster Controls;

    public Player Player;

    bool MenuOpen;

    public void OpenMenu(int Menu) {
        MenuOpen = true;
        Target = 0;
        foreach (Menu M in Menus) {
            M.Close();
        }
        Menus[Menu].Open();
    }

    public void CloseMenu() {
        MenuOpen = false;
        Target = FadeTime;
        foreach(Menu M in Menus) {
            M.Close();
        }
    }

    public CanvasGroup Hud;

    public float FadeTime;

    public Menu[] Menus;
    public int PauseMenuIndex;

    float Target = 1;
    float Actual = 1;

    void Update() {


        if (Actual == Target)
            return;

        Actual = Mathf.MoveTowards(Actual, Target, UnityEngine.Time.unscaledDeltaTime);

        Hud.alpha = 1 * DynamicUtil.EvaluateCurve(DynamicUtil.EvalCurve.Sine, Actual / FadeTime);
    }

    [Header("Health")]
    public Color OffColor;
    public int MaxHealth {
        get {
            return Player.MaxHealth + Player.IntUpgrades[(int) DefaultIntUpgrade.Health];
        }
    }
    public int Health {
        set {
            Text.text = ((int) Mathf.Clamp(value, 0, Mathf.Infinity)).ToString();

            if (value <= 0) {
                for (int i = 0; i < 8; i++) {
                    Segments[0].CrossFadeColor(OffColor, 0.25f, true, false);
                }
                return;
            }

            for (int i = 0; i < Mathf.FloorToInt((MaxHealth - value) / (MaxHealth / 8)); i++) {
                Segments[7-i].CrossFadeColor(OffColor, 0.25f, true, false);
            }
            for (int i = 0; i < Mathf.FloorToInt(value / (MaxHealth / 8)); i++) {
                Segments[i].CrossFadeColor(Color.red, 0.25f, true, false);
            }
        }
    }

    
    public TextMeshProUGUI Text;
    public Image[] Segments;



    [Header("DNA & Tech")]
    public TextMeshProUGUI DnaCounter;
    public TextMeshProUGUI TechCounter;

    public int Dna {
        set {
            DnaCounter.text = value.ToString();
            UpgradeDNACounter.text = value.ToString();
        }
    }
    public int Tech {
        set {
            TechCounter.text = value.ToString();
            UpgradeTechCounter.text = value.ToString();
        }
    }

    [Header("Upgrades")]
    public TextMeshProUGUI UpgradeTitleComponent;
    public string UpgradeTitle {
        set {
            UpgradeTitleComponent.text = value;
        }
    }
    public TextMeshProUGUI UpgradeDNACounter;
    public TextMeshProUGUI UpgradeTechCounter;
    public UpgradeCardUIController[] UpgradeCards;

    public void Start() {
        Target = FadeTime;
        Actual = FadeTime;

        Player = FindObjectOfType<Player>();

        Controls = new InputMaster();
        Controls.Menu.Exit.performed += (e) => {
            if (MenuOpen)
                CloseMenu();
            else
                OpenMenu(PauseMenuIndex);
        };
        Controls.Enable();
    }
}
