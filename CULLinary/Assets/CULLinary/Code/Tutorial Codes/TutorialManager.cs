using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Text instructionsText;

    public Animator animator;

    public Queue<string> sentences;

    bool stillTyping = false;
    string currSentence = "";

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartInstruction(Instruction instruction)
    {
        animator.SetBool("isOpen", true);

        sentences.Clear();

        foreach (string sentence in instruction.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndInstructions();
            return;
        }
               
        
        if (stillTyping == true) // show the complete sentence before moving on
        {
            StopAllCoroutines();
            instructionsText.text = currSentence;
            stillTyping = false;
        } else
        {
            string sentence = sentences.Dequeue();
            StartCoroutine(TypeSentence(sentence));
        }        
    }

    IEnumerator TypeSentence(string sentence)
    {
        stillTyping = true;
        currSentence = sentence;

        instructionsText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            instructionsText.text += letter;
            yield return null;
        }
        stillTyping = false;
    }

    void EndInstructions()
    {
        animator.SetBool("isOpen", false);
        Debug.Log("End of conversation");
    }
}
