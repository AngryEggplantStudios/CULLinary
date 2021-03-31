using UnityEngine;
using System;

// A simple class to parse a string into a dialogue tree.
public static class DialogueParser
{
    // If not formatted properly, parse may throw exceptions or return null
    public static Dialogue Parse(string input)
    {
        if (input[0] != '{') {
            return null;
        }

        if (input[1] == '[') {
            // parse plain dialogue
            bool isLeft = false;
            if (input[2] == 'L') {
                isLeft = true;
            } else if (input[2] == 'R') {
                isLeft = false;
            } else {
                Debug.Log("Parse1. Expected L or R, got " + input[2]);
                return null;
            }

            if (input[3] != ']') {
                Debug.Log("Parse2. Expected ], got " + input[3]);
                return null;
            }

            int currentChar = 4;
            string rawSpriteId = "";
            while (input[currentChar] != '}') {
                rawSpriteId = rawSpriteId + input[currentChar];
                currentChar++;
            }
            int spriteId = int.Parse(rawSpriteId);
            // Skip over '}'
            currentChar++;

            // Parse until the next dialogue
            string rawText = "";
            while (currentChar < input.Length && input[currentChar] != '{') {
                rawText = rawText + input[currentChar];
                currentChar++;
            }
            
            if (currentChar >= input.Length) {
                // ended parsing
                return new PlainDialogue(isLeft, spriteId, rawText);
            } else {
                Dialogue restOfDialogue = DialogueParser
                    .Parse(input.Substring(currentChar));
                return new PlainDialogue(isLeft, spriteId,
                                         rawText, restOfDialogue);
            }
        } else {
            // parse choice dialogue
            int currentChar = 1;
            string rawNumChoices = "";
            while (input[currentChar] != '}') {
                rawNumChoices = rawNumChoices + input[currentChar];
                currentChar++;
            }
            // Skip over '}'
            currentChar++;

            int numberOfChoices;
            try {
                numberOfChoices = int.Parse(rawNumChoices);
            } catch (FormatException) {
                Debug.Log("Parse3. Expected number, got " + rawNumChoices);
                return null;
            }

            string[] choicesTexts = new string[numberOfChoices];
            Dialogue[] choicesDialogues = new Dialogue[numberOfChoices];
            for (int j = 0; j < numberOfChoices; j++) {
                if (input[currentChar] != '[') {
                    Debug.Log("Parse4. Expected [, got " + input[currentChar]);
                    return null;
                }
                currentChar++;

                if (input[currentChar] != 'C') {
                    Debug.Log("Parse5. Expected C, got " + input[currentChar]);
                    return null;
                }
                currentChar++;

                if (input[currentChar] != ',') {
                    Debug.Log("Parse6. Expected , , got " + input[currentChar]);
                    return null;
                }
                currentChar++;

                int numberOfBracketsOpened = 1;
                string text = "";
                while (numberOfBracketsOpened > 0) {
                    if (input[currentChar] == ']') {
                        numberOfBracketsOpened--;
                    }
                    if (input[currentChar] == '[') {
                        numberOfBracketsOpened++;
                    }
                    text = text + input[currentChar];
                    currentChar++;
                }
                // remove last ] to get the choice text
                text = text.Substring(0, text.Length - 1);
                choicesTexts[j] = text;

                if (input[currentChar] != '<') {
                    Debug.Log("Parse7. Expected <, got " + input[currentChar]);
                    return null;
                }
                currentChar++;

                // find next choice
                string dialogue = "";
                int numberOfAnglesOpened = 1;
                while (numberOfAnglesOpened > 0) {
                    if (input[currentChar] == '>') {
                        numberOfAnglesOpened--;
                    }
                    if (input[currentChar] == '<') {
                        numberOfAnglesOpened++;
                    }
                    dialogue = dialogue + input[currentChar];
                    currentChar++;
                }

                // remove last > to get all the dialogue text
                dialogue = dialogue.Substring(0, dialogue.Length - 1);
                // recursive call to parse() to parse the nested dialogue
                Dialogue parsedDialogue = DialogueParser.Parse(dialogue);
                choicesDialogues[j] = parsedDialogue;
                
                // now input[currentChar] == '[' for the next choice
                // the for-loop will handle the next choice
            }
            return new ChoiceDialogue(choicesDialogues, choicesTexts);
        }
    }

    // A manually constructed Dialogue tree for testing
    public static Dialogue GetTestDialogue()
    {
        Dialogue endHappy = new PlainDialogue(true, 0, "I'm great, thank you!");
        Dialogue endRude = new PlainDialogue(true, 0, "Keep quiet! Silence in the restaurant please.");

        Dialogue choice = new ChoiceDialogue(new Dialogue[]{endHappy, endRude},
                                             new string[]{
                                                 "Great (Happy)",
                                                 "Keep quiet (Rude)"
                                             });

        Dialogue initial = new PlainDialogue(false, 1, "How are you today?", choice);
        return initial;
    }
}
