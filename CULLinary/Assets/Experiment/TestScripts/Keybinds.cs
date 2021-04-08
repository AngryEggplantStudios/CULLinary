using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Keybind {
    Interact,
    OpenInventory,
    OpenRecipeBook,
    Trash,
    CloseMenu,
    StatsMenu
}

public class Keybinds {    
    private static Dictionary<Keybind, KeyCode> keybinds = new Dictionary<Keybind, KeyCode>{
        { Keybind.Interact, KeyCode.F },
        { Keybind.OpenInventory, KeyCode.I },
        { Keybind.OpenRecipeBook, KeyCode.R },
        { Keybind.Trash, KeyCode.Delete },
        { Keybind.CloseMenu, KeyCode.Escape },
        { Keybind.StatsMenu, KeyCode.Q }
    };

    public static bool WasTriggered(Keybind key) {
        return Input.GetKeyDown(keybinds[key]);
    }
}