// Classes that represents a dialogue option.
// Forms a tree data structure with its child classes.
public class Dialogue
{
    // Is this the last dialogue box to show?
    public bool isLast;
    // Used to safely case a Dialogue into Plain or Choice
    public bool isPlain;

    public Dialogue(bool isLastBox, bool isPlainDialogue) {
        this.isLast = isLastBox;
        this.isPlain = isPlainDialogue;
    }
}

// Represents a dialogue spoken by someone.
public class PlainDialogue: Dialogue
{
    // Is this spoken by the player (or others)?
    // Player is on left of the dialogue box
    public bool isPlayer;
    // The next dialogue in the sequence
    public Dialogue next;
    // ID of the sprite to use.
    public int spriteId;
    // The dialogue text to use
    public string displayedText;

    // Constructs a message that is the last one
    public PlainDialogue(bool isPlayer, int spriteId, string text):
        base(true, true)
    {
        this.isPlayer = isPlayer;
        this.next = null;
        this.spriteId = spriteId;
        this.displayedText = text;
    }

    // Constructs a message that is followed by another
    public PlainDialogue(bool isPlayer, int spriteId, string text, Dialogue nextDialogue):
        base(false, true)
    {
        this.isPlayer = isPlayer;
        this.next = nextDialogue;
        this.spriteId = spriteId;
        this.displayedText = text;
    }
}

// Represents 
public class ChoiceDialogue: Dialogue
{
    // All the choices for this choice
    public Dialogue[] choices;
    // The text for each choice, must correspond to
    // the same index as the dialogue choices
    public string[] choicesText;

    public ChoiceDialogue(Dialogue[] allChoices, string[] choicesDisplayText):
        base(false, false)
    {
        choices = allChoices;
        choicesText = choicesDisplayText;
    }
}

