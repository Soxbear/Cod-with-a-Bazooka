using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Soxbear.Channels;

public class LightObject : MonoBehaviour, Hittable
{
    public ChannelReadWrite<bool> setState;

    public ChannelReadWrite<bool> toggleState;

    [SerializeField]
    private bool on;

    [SerializeField]
    private int health;

    [SerializeField]
    private Light2D[] lights;

    [SerializeField]
    private Sprite onSprite;
    [SerializeField]
    private Sprite offSprite;

    [SerializeField]
    private Material onMaterial;
    [SerializeField]
    private Material offMaterial;

    [SerializeField]
    private GameObject destroyedLight;

    private SpriteRenderer spriteRend;

    public bool Hit(int damage, Vector2 direction, float knockback, HitType hitType) {
        if (hitType == HitType.Bullet || hitType == HitType.Explosion || hitType == HitType.Melee || hitType == HitType.Electric || hitType == HitType.Sharapnel) {
            health -= damage;
            if (health <= 0) {
                Instantiate(destroyedLight, transform.position, transform.rotation);
                setState.onUpdate -= OnSetStateUpdate;
                toggleState.onUpdate -= OnToggleStateUpdate;
                Destroy(gameObject);
            }
            LightState();
        }

        return false;
    }

    void Start() {
        setState.onUpdate += OnSetStateUpdate;
        toggleState.onUpdate += OnToggleStateUpdate;

        spriteRend = GetComponent<SpriteRenderer>();

        LightState();
    }

    void OnSetStateUpdate(bool value) {
        on = value;
            LightState();
    }

    void OnToggleStateUpdate(bool value) {
        if (value)
            on = !on;
        LightState();
    }

    void LightState() {
        foreach (Light2D light in lights) {
            light.intensity = (on) ? 1 : 0;
        }
        spriteRend.sprite = (on) ? onSprite : offSprite;
        spriteRend.material = (on) ? onMaterial : offMaterial;
    }
}
