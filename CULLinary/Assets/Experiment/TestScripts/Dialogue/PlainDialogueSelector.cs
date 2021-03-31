using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlainDialogueSelector : MonoBehaviour, IPointerClickHandler {    
    public delegate void NextDialogueDelegate();
    public event NextDialogueDelegate DisplayNextDialogue;

    private void Update()
    {
        if (Keybinds.WasTriggered(Keybind.Interact)) {
            DisplayNextDialogue.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        DisplayNextDialogue.Invoke();
    }
}
