using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Keypad : MonoBehaviour, Interactable, Dynamic<bool>
{
    public int AccessLevel;

    [SerializeField]
    bool Enabled = true;

    bool Unlocked;

    public MonoBehaviour OutputClass;

    Dynamic<bool> Output;

    public Sprite UnlockedScreen;
    public Sprite LockedScreen;
    public Sprite DisabledScreen;
    SpriteRenderer Renderer;
    Light2D Light;

    public void Interact() {
        if (!Enabled) {
            return;
        }

        if (Unlocked) {
            Output.PassValue(!Output.GetValue());
        }
        else if (FindObjectOfType<Player>().IntUpgrades[(int) DefaultIntUpgrade.AccessLevel] >= AccessLevel) {
            Unlocked = true;

            Renderer.sprite = UnlockedScreen;
            Light.color = new Color(0f, 0.5f, 1f, 1f);

            Output.PassValue(!Output.GetValue());
        }
        else {
            
        }
    }

    public void PassValue(bool Value) {
        Enabled = Value;
        if (Value) {
            Light.enabled = true;

            if (Unlocked) {
                Renderer.sprite = UnlockedScreen;
                Light.color = new Color(0f, 0.5f, 1f, 1f);
            }
            else {
                Renderer.sprite = LockedScreen;
                Light.color = Color.red;
            }
        }
        else {
            Light.enabled = false;
            Renderer.sprite = DisabledScreen;
        }
    }

    public bool GetValue() {
        return Enabled;
    }

    void Start() {
        Light = transform.GetComponent<Light2D>();
        Renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        Unlocked = (AccessLevel == 0);

        if (Enabled) {
            Light.enabled = true;

            if (Unlocked) {
                Renderer.sprite = UnlockedScreen;
                Light.color = new Color(0f, 0.5f, 1f, 1f);
            }
            else {
                Renderer.sprite = LockedScreen;
                Light.color = Color.red;
            }
        }
        else {
            Light.enabled = false;
            Renderer.sprite = DisabledScreen;
        }

        if (OutputClass is Dynamic<bool>) {
            Output = (OutputClass as Dynamic<bool>);
        }
    }
}
