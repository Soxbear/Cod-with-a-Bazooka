using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIComponent : MonoBehaviour
{
    protected UIController UIController;

    protected Player Player;

    void Awake() {
        UIController = FindObjectOfType<UIController>();
        Player = FindObjectOfType<Player>();
    }
}
