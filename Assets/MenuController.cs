using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static Dictionary<MenuType, Menu> menus = new Dictionary<MenuType, Menu>();    

    public static Menu _hud;
    public static Menu _shade;

    public enum MenuType {
        NONE,
        PAUSE,
        BIOTECH
    }

    static MenuType openMenu = MenuType.NONE;

    public static void OpenMenu(MenuType menu) {
        openMenu = menu;

        if (menus[menu].PauseGame)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;

        if (menus[menu].hideHud)
            _hud.Close();
        else 
            _hud.Open();

        _shade.Open();

        menus[menu].Open();
        foreach (MenuType m in menus.Keys) {
            if (m != menu)
                menus[m].Close();
        }
    }

    public static void CloseMenus() {
        openMenu = MenuType.NONE;

        foreach (MenuType m in menus.Keys) {
            menus[m].Close();
        }

        _shade.Close();
        _hud.Open();
    }

    public static void Exit() {
        if (openMenu == MenuType.NONE)
            OpenMenu(MenuType.PAUSE);
        else
            menus[openMenu].Exit();
    }

    void Start() {
        _hud = hud;

        _shade = shade;

        menus.Add(MenuType.PAUSE, pause);

        menus.Add(MenuType.BIOTECH, biotech);
    }

    public Menu hud;

    public Menu shade;

    public Menu pause;

    public Menu biotech;
}
