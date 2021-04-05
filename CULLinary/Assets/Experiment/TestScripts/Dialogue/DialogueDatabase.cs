using UnityEngine;

// A store of all dialogue strings and their random weights.
// 
// Dialogue Format:
// text        - any string of characters (avoid using: {}<>[])
// number      - any string of digits
//
// sprite_id   - number
// num_choices - number
//
// dialogue    - (plain|choice)*
// plain       - { + ([L]|[R]) + sprite_id + } + text
// choice      - { + num_choices + } + ([C + , + text + ] + < + dialogue + >)*
//
// L for head sprite to be placed on the left
// R for head sprite to be placed on the right
// 
// Example:
// {[R]1}How are you today?{2}[C,Great (Happy)]<{[L]0}I'm great, thank you!>[C,Keep quiet (Rude)]<{[L]0}Keep quiet! Silence in the restaurant please.>
public static class DialogueDatabase
{
    // Add dialogues to this array with weights for random chance
    private static (string, int)[] rawDialoguesWithWeights = {
        ("{[R]1}Yum, smells delicious!{[R]1}I swear, this is the only decent restaurant in town...", 10),
        ("{[R]1}How are you today?{2}[C,Great]<{[L]0}I'm great, thank you!>[C,Keep quiet]<{[L]0}Keep quiet! Silence in the restaurant please.{[R]1}Are you kidding me? I'm never coming here again.>", 10),
        ("{[R]1}Can I see the manager?{3}[C,No.]<{[R]1}What a horrible restaurant.>[C,I am the manager.]<{[R]1}Could I get a discount? It's my birthday...{2}[C,Yes.]<{[R]1}Hooray!>[C,No.]<{[R]1}Aww... Worst restaurant ever.>>[C,You are the manager.]<{[R]1}Wow! That's great! I've never held a job before.>", 10),
        ("{[R]1}After the plants started attacking, all my friends have moved out...{[R]1}You're the only one left who I can talk to. *sob*", 10),
        ("{[R]1}Have you ever been to the factory?{2}[C,Yes.]<{[L]0}Yep, I have.{[R]1}Oh, then you must have seen the chemical spills...{[R]1}The poor animals and plants...>[C,No.]<{[L]0}Nope, should I?{[R]1}Oh. Don't go there! The chemical spills smell horrible.>", 10),
        ("{[L]0}How's the food?{[R]1}It's so good! Thanks for asking.", 5),
        ("{[L]0}How's the food?{[R]1}Is that eggplant?{[R]1}Uuf, I think I'm going to be sick...", 5),
        ("{[R]1}I once had a dream about crystals in a forest.{[R]1}I had to collect all of them while zombies were chasing me.", 1),
        ("{[R]1}I once had a dream where I was fighting this knight guy.{[R]1}It was snowy and I had to stand in a fire to warm up!", 1),
        ("{[R]1}Have you ever had a dream where you were stuck in a room?{[R]1}I did and 3 other guys were trying to kill me with lasers...", 1),
        ("{[R]1}What other dishes can you cook?{7}[C,Eggplant Salad]<{[L]0}How about an Eggplant Salad?{[R]1}You have to be kidding me.{[R]1}No more eggplants, please!!>[C,Gazpacho]<{[L]0}We serve gazpacho and other Andalusian dishes.{[R]1}Wow! Where do you even get the ingredients for that?!>[C,Nasi Lemak]<{[L]0}Would you like some nasi lemak?{[R]1}That's my favourite dish!{2}[C,Mine too!]<{[R]1}I've got to try your nasi lemak someday!>[C,I lied, I can't actually cook that]<{[L]0}Sorry, I don't actually know how to cook nasi lemak...{[R]1}It's okay. You can learn before I come back the next time!>>[C,Instant Ramen]<{[L]0}I can cook instant ramen...{[R]1}That's not impressive! I can do that too!!>[C,Anuflora]<{[L]0}I can cook \"Anuflora\"...{[R]1}Never heard of that, what's in it?{3}[C,Apples and Oranges]<{[R]1}Wait, it's a fruit salad?>[C,Fish and Tomatoes]<{[R]1}Sounds good.>[C,Just kidding, there's no such thing]<{[R]1}I knew it! You liar!>>[C,Crispy Bacon]<{[L]0}I can fry up some bacon if you want.{[R]1}Uhh... That's okay... I'm a vegan.>[C,Foie Gras]<{[L]0}Monsieur, we serve the best foie gras... Délicieuse!{[R]1}Uhh... I'm afraid I don't have the money for that...>", 1),
        ("{[L]0}How are you doing?{[R]1}Not so good...{[R]1}I had a nightmare where I was stuck in space...{[R]1}I had to place guns to fend off hostile invaders!", 1),
        ("{[L]0}How are you doing?{[R]1}Okay, I guess.{[R]1}Other than that dream I had where I was stuck in a maze.{2}[C,What happened?]<{[R]1}There were chemical spills...{[R]1}I was being chased by some horrifying ghost thing... *shudder*>[C,Sorry to hear that.]<{[L]0}I'm sorry to hear that.{[R]1}Yeah, I had to control my heartbeat and catch glowing orbs to get out...>", 1),
        ("{[R]1}Hi! Random question, have you been stuck in a laboratory?{2}[C,Can't say I have.]<{[R]1}If you do, bringing a drone might be helpful!>[C,Yes.]<{[R]1}If you get stuck again, you should look for some letters!{[R]1}The owner of the lab may have left his password lying around...>", 1),
        ("{[R]1}I once had a dream where everything was white.{[R]1}I had to find colour pools to restore the colour with my paint gun!", 1),
        ("{[R]1}Rumour has it that the one behind this chaos is a clown- Wait... What's that sound?", 0) // Rigged for tutorial (0 as in 0 weight?)
    };

    private static (Dialogue, double)[] dialoguesWithCumulativeChance = null;
    private static bool hasGenerated = false;

    public static void GenerateDialogues() {
        int numberOfDialogues = rawDialoguesWithWeights.Length;
        dialoguesWithCumulativeChance = new (Dialogue, double)[numberOfDialogues];

        int totalWeight = 0;
        for (int i = 0; i < numberOfDialogues; i++) {
            int weight = rawDialoguesWithWeights[i].Item2;
            totalWeight = totalWeight + weight;
        }

        double currentWeight = 0.0;
        for (int i = 0; i < numberOfDialogues; i++) {
            (string rawDialogue, int weight) = rawDialoguesWithWeights[i];
            Dialogue parsedDialogue = DialogueParser.Parse(rawDialogue);
            if (parsedDialogue == null) {
                Debug.Log("Oops, dialogue #" + i + " is malformed!");
                break;
            }

            currentWeight = currentWeight + (double)weight / (double)totalWeight;
            double cumulWeightForI = currentWeight;
            dialoguesWithCumulativeChance[i] = (parsedDialogue, cumulWeightForI);
        }
        hasGenerated = true;
    }
    
    public static Dialogue GetDialogue(int index) {
        if (!hasGenerated) {
            GenerateDialogues();
        }
        return dialoguesWithCumulativeChance[index].Item1;
    }

    public static Dialogue GetRandomDialogue() {
        if (!hasGenerated) {
            GenerateDialogues();
        }
        // Create new random with time seed
        System.Random rand = new System.Random();
        double randomDouble = rand.NextDouble();

        int numberOfDialogues = dialoguesWithCumulativeChance.Length;
        for (int i = 0; i < numberOfDialogues; i++) {
            (Dialogue dialogue, double weight) = dialoguesWithCumulativeChance[i];
            if (randomDouble < weight) {
                return dialogue;
            }
        }
        return dialoguesWithCumulativeChance[numberOfDialogues - 1].Item1;
    }
}
