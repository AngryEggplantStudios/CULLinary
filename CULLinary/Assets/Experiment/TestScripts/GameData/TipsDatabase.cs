using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Tips Database", menuName="Tips/TipsDatabase")]
public class TipsDatabase : ScriptableObject
{
    public List<string> tips = new List<string>{
        "Beware of the corn! Try to stay clear of its line of fire before you get corn cobbed!",
        "The eggplant monster can either drop the regular eggplant or a golden eggplant.",
        "The potato's ability is to headbutt you, use ranged attacks if you can!",
        "Don't waste food! People in Clownville are really big on reducing food wastage. You will still earn cash even if you serve up the wrong dish!",
        "You can heal back to 100HP by going back to the restaurant if you are low on health.",
        "Hmm... that clown detector in the shop seems important, maybe you should buy it?",
        "Customers' orders are random each time you go back to the restaurant, do prepare the necessary ingredients for all the recipes!",
        "Beware of the red circle upon facing the eggplant if you do not want to end up on the wrong end of a devastating attack!",
        "Hold shift to run",
        "Psst, the final boss is strong. Do prepare properly before facing him!",
        "Vote for us in STEPS pl0x"
    };
}


