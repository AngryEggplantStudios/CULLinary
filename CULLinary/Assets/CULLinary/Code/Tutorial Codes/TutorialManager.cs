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
    bool currWaiting = false;
    string currSentence = "";

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("f")) // Show next sentence if player presses 'F' key
            DisplayNextSentence();
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
        if (!currWaiting)
        {
            if (sentences.Count == 0)
            {
                animator.SetBool("CanGoNext", false);

                if (stillTyping == true)
                {
                    StopAllCoroutines();
                    instructionsText.text = currSentence;
                    StartCoroutine(WaitBeforeClosing());
                }
                else
                {
                    EndInstructions();
                }


                return;
            }

            if (stillTyping == true) // show the complete sentence before moving on
            {
                StopAllCoroutines();
                instructionsText.text = currSentence;
                StartCoroutine(WaitAWhile(1.5f)); // Short delay before showing the next sentence
            }
            else
            {
                string sentence = sentences.Dequeue();
                StartCoroutine(TypeSentence(sentence));
            }

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
        StartCoroutine(WaitAWhile(1.5f)); // Short delay before showing the next sentence
        animator.SetBool("CanGoNext", true);
        Debug.Log("Yo I set the bool to go next");
    }

    IEnumerator WaitAWhile(float delay)
    {
        currWaiting = true;

        Debug.Log("Waiting for " + delay + " seconds!");

        yield return new WaitForSeconds(delay);

        stillTyping = false;
        currWaiting = false;
        //animator.SetBool("CanGoNext", true);
        //Debug.Log("Yo I set the bool to go next");
        // where to put animator.SetBool("CanGoNext", false);??
    }

    IEnumerator WaitBeforeClosing() 
    {
        yield return new WaitForSeconds(1.5f);
        stillTyping = false;

        EndInstructions();
    }

    void EndInstructions()
    {
        animator.SetBool("isOpen", false);
        Debug.Log("End of conversation");
    }
}
