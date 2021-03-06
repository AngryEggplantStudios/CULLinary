using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerMelee : DungeonPlayerAction
{
    public delegate void PlayerMeleeDelegate();
    public delegate void PlayerStopDelegate();
    public event PlayerMeleeDelegate OnPlayerMelee;
    public event PlayerStopDelegate OnPlayerStop;

    //Actions prevented
    private DungeonPlayerRange dungeonPlayerRange;
    private void Start()
    {
        dungeonPlayerRange = GetComponent<DungeonPlayerRange>();
    }

    private void Update()
    {
        bool isRangeInvoked = dungeonPlayerRange ? dungeonPlayerRange.GetIsInvoking() : false;
        if (Input.GetMouseButton(0) && !isRangeInvoked)
        {
            this.SetIsInvoking(true);
            OnPlayerMelee?.Invoke();
        }
        else
        {
            this.SetIsInvoking(false);
            OnPlayerStop?.Invoke();
        }
    }
}
