using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Keybind {
    Interact,
    Trash
}

public class Keybinds {    
    private static Dictionary<Keybind, KeyCode> keybinds = new Dictionary<Keybind, KeyCode>{
        { Keybind.Interact, KeyCode.F },
        { Keybind.Trash, KeyCode.Delete }
    };

    public static bool WasTriggered(Keybind key) {
        return Input.GetKeyDown(keybinds[key]);
    }
}