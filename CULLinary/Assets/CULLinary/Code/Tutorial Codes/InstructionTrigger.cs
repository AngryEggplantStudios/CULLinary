using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionTrigger : MonoBehaviour
{
    public Instruction instruction;

    public void TriggerInstruction()
    {
        FindObjectOfType<TutorialManager>().StartInstruction(instruction);
    }
}
