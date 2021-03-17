using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// A small script to trigger events with the "close menu" keybind.
// The event must be provided by the user.
public class EventTriggerOnCloseMenu : MonoBehaviour
{
    public UnityEvent closeEvent;

    void Update()
    {
        if (Keybinds.WasTriggered(Keybind.CloseMenu))
        {
            closeEvent.Invoke();
        }
    }
}
