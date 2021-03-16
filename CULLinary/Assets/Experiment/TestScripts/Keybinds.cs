using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Keybind {
    Interact,
    Trash,
    CloseMenu
}

public class Keybinds {    
    private static Dictionary<Keybind, KeyCode> keybinds = new Dictionary<Keybind, KeyCode>{
        { Keybind.Interact, KeyCode.F },
        { Keybind.Trash, KeyCode.Delete },
        { Keybind.CloseMenu, KeyCode.Escape }
    };

    public static bool WasTriggered(Keybind key) {
        return Input.GetKeyDown(keybinds[key]);
    }
}