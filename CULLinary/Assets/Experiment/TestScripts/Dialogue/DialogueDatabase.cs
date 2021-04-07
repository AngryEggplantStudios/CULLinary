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
        /*    0 */ ("{[R]1}Yum, smells delicious!{[R]1}I swear, this is the only decent restaurant in town...", 10),
        /*    1 */ ("{[R]1}How are you today?{2}[C,Great]<{[L]0}I'm great, thank you!{[R]1}That's great!>[C,Keep quiet]<{[L]0}Keep quiet! Silence in the restaurant please.{[R]1}Are you kidding me? I'm never coming here again.>", 10),
        /*    2 */ ("{[R]1}Can I see the manager?{3}[C,No.]<{[L]0}No, you can't.{[R]1}What a horrible restaurant.>[C,I am the manager.]<{[L]0}You are speaking to the manager.{[R]1}Could I get a discount? It's my birthday...{2}[C,Yes.]<{[R]1}Hooray!>[C,No.]<{[R]1}Aww... Worst restaurant ever.>>[C,You are the manager.]<{[L]0}You are the manager.{[R]1}Wow! That's great! I've never held a job before.{[L]0}No, that's not... Never mind.>", 10),
        /*    3 */ ("{[R]1}After the plants started attacking, all my friends have moved out...{[R]1}You're the only one left who I can talk to. *sob*", 10),
        /*    4 */ ("{[R]1}Have you ever been to the factory?{2}[C,Yes.]<{[L]0}Yep, I have.{[R]1}Oh, then you must have seen the chemical spills...{[R]1}The poor animals and plants...>[C,No.]<{[L]0}Nope, should I?{[R]1}Oh. Don't go there! The chemical spills smell horrible.>", 10),
        /*    5 */ ("{[L]0}How's the food?{[R]1}It's so good! Thanks for asking.", 5),
        /*    6 */ ("{[L]0}How's the food?{[R]1}Is that eggplant?{[R]1}Uuf, I think I'm going to be sick...", 5),
        /*    7 */ ("{[R]1}I once had a dream about crystals in a forest.{[R]1}I had to collect all of them while zombies were chasing me.", 1),
        /*    8 */ ("{[R]1}I once had a dream where I was fighting this knight guy.{[R]1}It was snowy and I had to stand in a fire to warm up!", 1),
        /*    9 */ ("{[R]1}Have you ever had a dream where you were stuck in a room?{[R]1}I did and 3 other guys were trying to kill me with lasers...", 1),
        /*   10 */ ("{[R]1}What other dishes can you cook?{7}[C,Eggplant Salad]<{[L]0}How about an Eggplant Salad?{[R]1}You have to be kidding me.{[R]1}No more eggplants, please!!>[C,Gazpacho]<{[L]0}We serve gazpacho and other Andalusian dishes.{[R]1}Wow! Where do you even get the ingredients for that?!>[C,Nasi Lemak]<{[L]0}Would you like some nasi lemak?{[R]1}That's my favourite dish!{2}[C,Mine too!]<{[R]1}I've got to try your nasi lemak someday!>[C,I lied, I can't actually cook that]<{[L]0}Sorry, I don't actually know how to cook nasi lemak...{[R]1}It's okay. You can learn before I come back the next time!>>[C,Instant Ramen]<{[L]0}I can cook instant ramen...{[R]1}That's not impressive! I can do that too!!>[C,Anuflora]<{[L]0}I can cook \"Anuflora\"...{[R]1}Never heard of that, what's in it?{3}[C,Apples and Oranges]<{[R]1}Wait, it's a fruit salad?>[C,Fish and Tomatoes]<{[R]1}Sounds good.>[C,Just kidding, there's no such thing]<{[R]1}I knew it! You liar!>>[C,Crispy Bacon]<{[L]0}I can fry up some bacon if you want.{[R]1}Uhh... That's okay... I'm a vegan.>[C,Foie Gras]<{[L]0}Monsieur, we serve the best foie gras... Délicieuse!{[R]1}Uhh... I'm afraid I don't have the money for that...>", 1),
        /*   11 */ ("{[L]0}How are you doing?{[R]1}Not so good...{[R]1}I had a nightmare where I was stuck in space...{[R]1}I had to place guns to fend off hostile invaders!", 1),
        /*   12 */ ("{[L]0}How are you doing?{[R]1}Okay, I guess.{[R]1}Other than that dream I had where I was stuck in a maze.{2}[C,What happened?]<{[R]1}There were chemical spills...{[R]1}I was being chased by some horrifying ghost thing... *shudder*>[C,Sorry to hear that.]<{[L]0}I'm sorry to hear that.{[R]1}Yeah, I had to control my heartbeat and catch glowing orbs to get out...>", 1),
        /*   13 */ ("{[R]1}Hi! Random question, have you been stuck in a laboratory?{2}[C,Can't say I have.]<{[R]1}If you do, bringing a drone might be helpful!>[C,Yes.]<{[R]1}If you get stuck again, you should look for some letters!{[R]1}The owner of the lab may have left his password lying around...>", 1),
        /*   14 */ ("{[R]1}I once had a dream where everything was white.{[R]1}I had to find colour pools to restore the colour with my paint gun!", 1),
        /*   15 */ ("{[R]1}Rumour has it that the one behind this chaos is a clown-{[L]0}AAHHH!!{[R]1}...{[R]1}Wait, what did I say? I don't know what got into me there...{[R]1}Also, why do you look so frightened?{[L]0}Uhh... Never mind.", 2),
        /*   16 */ ("{[R]1}You're not from around these parts, are you?{[R]1}Where are you from?{8}[C,Doesn't matter]<{[L]0}It doesn't matter where I'm from.{[L]0}What matters is I'm here now!{[R]1}That's a nice attitude to have!{[R]1}But I was just curious...>[C,Africa]<{[L]0}I'm from Africa.{[R]1}Wow! Is it really true that every minute, 60 seconds pass in Africa?{[L]0}...>[C,Antarctica]<{[L]0}I'm from Antartica.{[R]1}Really! You must see a lot of polar bears!{[L]0}What?! Polar bears are not found at the south pole!>[C,Asia]<{[L]0}I'm from Asia.{[R]1}Ah! You must be very good at math.{[L]0}Uh... What?>[C,Australia]<{[L]0}I'm from Australia.{[R]1}Ooh... G'day mate! How many kangaroos have you caught?{[L]0}...>[C,Europe]<{[L]0}I'm from Europe.{[R]1}Ah! Guten tag! Bonjour!{[L]0}There's more than two countries in Europe...>[C,North America]<{[L]0}I'm from North America.{[R]1}Oh... The land of the free!{[L]0}Hey... America is not just the United States, okay?>[C,South America]<{[L]0}I'm from South America.{[R]1}Oh, I can speak Brazilian! Bom dia!{[L]0}Who says I'm Brazilian? Also, the language is Portuguese!>", 2),
        /*   17 */ ("{[R]1}Have you heard about the ancient curse of Corona?{[R]1}It is said to affect those who eat bats!{[L]0}What rubbish. We don't serve any bats here.{[R]1}Why not? They are a good source of protein!{[L]0}But you just said... Never mind...", 2),
        /*   18 */ ("{[R]1}The prophecies tell of the chosen one...{[R]1}\"Thou shall not feareth the cloyne...\"{[R]1}\"The blight did casteth upon ye land. Yield to purifying light!\"{[R]1}Any idea what that means?{3}[C,Nope]<{[L]0}Uh, nope?{[R]1}Shame. You could have been the chosen one!>[C,Yes, of course]<{[L]0}Of course! It means the soil will become more fertile soon.{[R]1}Really? Well that must be good for the farmers!>[C,Nonsense]<{[L]0}What nonsense. I don't believe in such things.{[L]0}I only believe in science.{[R]1}Oh... Well... Can your science explain why it rains?{[R]1}Checkmate, loser!{[L]0}YES! Yes it can!>", 2),
        /*   19 */ ("{[R]1}The rare golden eggplant used to be the speciality of the town.{[R]1}Do you serve it?{[L]0}We do! But it's a seasonal dish.{[R]1}Ok, let me know when you do have it!", 3),
        /*   20 */ ("{[R]1}We're no strangers to love...{[R]1}You know the rules and so do I...{[L]0}I'll leave you to your singing...{[R]1}A full commitment's what I'm thinking of...{[R]1}You wouldn't get this from any other guy!{[L]0}Please, stop.{[R]1}Iiiii just wanna tell you how I'm feeling...{[R]1}Gotta make you... understand!{[R]1}NeVer goNna giVe yOuuUuuU uPPP!{[L]0}Oh god.{2}[C,Run away]<{[R]1}Never goonnna let youUuuUU dow- Wait, where did my audience go?>[C,Stay and listen]<{[L]0}No! Why would you pick that?!{[R]1}Never goonnna let youUuuUU downnnn...{[R]1}Never gonna ruuunn arooundDd and... deSeERRt yoOOu!{[R]1}Never gonnnaa maaake you cryyy...{[R]1}Never goonnna say gooodbye!{[R]1}Never gonna teeeelll a lieeee and huurt yooUuuuU!{[L]0}You are officially banned from my restaurant.>", 1),
        /*   21 */ ("{[R]1}I love eggplants!{[L]0}That's great! We only serve eggplants here.{[R]1}This is the best restaurant!", 3),
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
