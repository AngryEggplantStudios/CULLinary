using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueLoader : MonoBehaviour
{
    public GameObject theyPanel;
    public Text theyPanelText;
    public Image theyPanelSprite;

    public GameObject choicePanel;
    // Container to add the choices to
    public GameObject choicePanelContainer;

    public GameObject mePanel;
    public Text mePanelText;
    public Image mePanelSprite;

    // The prefab to use for the choice selection
    public GameObject choicePrefab;
    // Array of sprites for the dialogue boxes
    public Sprite[] sprites;
    // CookingStation to re-enable movement
    public CookingStation movementController;

    private Dialogue currentDialogue;
    private Dialogue nextDialogue;
    // Delegate to be run after dialogue ends
    private static readonly Action defaultDialogueAction = () => {};
    private Action endDialogueAction = defaultDialogueAction;

    private Restaurant_CustomerController currentCustomer;

    private void DisplayNextAndCloseMePanel()
    {
        mePanel.SetActive(false);
        if (!currentDialogue.isLast) {
            currentDialogue = nextDialogue;
            RunCurrentDialogue();
        } else {
            if (movementController != null) movementController.EnableMovementOfPlayer();
            if (currentCustomer != null) StartCoroutine(currentCustomer.TimeToLeave());
            // Invoke the ending action
            endDialogueAction.Invoke();
            endDialogueAction = defaultDialogueAction;
        }
    }

    private void DisplayNextAndCloseTheyPanel()
    {
        theyPanel.SetActive(false);
        if (!currentDialogue.isLast) {
            currentDialogue = nextDialogue;
            RunCurrentDialogue();
        } else {
            movementController.EnableMovementOfPlayer();
            StartCoroutine(currentCustomer.TimeToLeave());
            // Invoke the ending action
            endDialogueAction.Invoke();
            endDialogueAction = defaultDialogueAction;
        }
    }
    
    private void Start()
    {
        PlainDialogueSelector meSelector = mePanel.GetComponent<PlainDialogueSelector>();
        meSelector.DisplayNextDialogue += DisplayNextAndCloseMePanel;

        PlainDialogueSelector theySelector = theyPanel.GetComponent<PlainDialogueSelector>();
        theySelector.DisplayNextDialogue += DisplayNextAndCloseTheyPanel;

        DialogueDatabase.GenerateDialogues();

        // For debug purposes
        // LoadAndRunDebug(21);
    }

    private void RunMeDialogue(PlainDialogue meDialogue)
    {
        mePanelText.text = meDialogue.displayedText;
        mePanelSprite.sprite = sprites[meDialogue.spriteId];

        nextDialogue = meDialogue.next;
        mePanel.SetActive(true);
    }

    private void RunTheyDialogue(PlainDialogue theyDialogue)
    {
        theyPanelText.text = theyDialogue.displayedText;
        theyPanelSprite.sprite = sprites[theyDialogue.spriteId];

        nextDialogue = theyDialogue.next;
        theyPanel.SetActive(true);
    }

    private void RunChoiceDialogue(ChoiceDialogue choiceDialogue) 
    {
        // Clear the choices menu
        foreach (Transform child in choicePanelContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }

        // Add new choices to the menu
        int numberOfChoices = choiceDialogue.choices.Length;
        for (int i = 0; i < numberOfChoices; i++) {
            GameObject choiceBox = Instantiate(choicePrefab,
                                               new Vector3(0, 0, 0),
                                               Quaternion.identity,
                                               choicePanelContainer.transform) as GameObject;
            
            Text choiceText = choiceBox.GetComponent<Text>();
            choiceText.text = choiceDialogue.choicesText[i];

            ChoiceSelector choiceOnClick = choiceBox.GetComponent<ChoiceSelector>();

            // Capture the value of i for the lambda
            int currentI = i;
            choiceOnClick.SelectThisChoice += () =>
            {
                choicePanel.SetActive(false);
                // Assume choice box is never last
                currentDialogue = choiceDialogue.choices[currentI];
                RunCurrentDialogue();
            };
        }
        choicePanel.SetActive(true);
    }

    private void LoadDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
    }

    // Checks what kind of dialogue it is and calls the correct function.
    // Returns the next dialogue, or null, if it is the last dialogue.
    private void RunCurrentDialogue()
    {
        if (currentDialogue.isPlain) {
            PlainDialogue plain = (PlainDialogue)currentDialogue;
            if (plain.isPlayer) {
                RunMeDialogue(plain);
            } else {
                RunTheyDialogue(plain);
            }
        } else {
            ChoiceDialogue choice = (ChoiceDialogue)currentDialogue;
            RunChoiceDialogue(choice);
        }
    }

    // Loads and runs the dialogue tree provided
    // Takes in a Customer that will be leaving after the dialogue is done
    public void LoadAndRun(Dialogue dialogue, Restaurant_CustomerController customerToLeave)
    {
        currentCustomer = customerToLeave;
        LoadDialogue(dialogue);
        RunCurrentDialogue();
    }

    public void LoadAndRunWithoutCustomer(Dialogue dialogue)
    {
        LoadDialogue(dialogue);
        RunCurrentDialogue();
    }

    // For debugging, dialogue index refers to index
    // rawDialoguesWithWeights in DialogueDatabase
    public void LoadAndRunDebug(int dialogueIndex)
    {
        LoadDialogue(DialogueDatabase.GetDialogue(dialogueIndex));
        RunCurrentDialogue();
    }

    // Sets the ending callback for the next dialogue.
    // This callback will be unset after the next time it is triggered.
    public void SetDialogueEndCallback(Action a)
    {
        endDialogueAction = a;
    }
}
