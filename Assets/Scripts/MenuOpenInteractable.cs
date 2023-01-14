using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpenInteractable : MonoBehaviour, Interactable
{
    public int MenuIndex;

    UIController UI;

    public void Interact() {
        UI.OpenMenu(MenuIndex);
    }

    void Start() {
        UI = FindObjectOfType<UIController>();
    }
}
