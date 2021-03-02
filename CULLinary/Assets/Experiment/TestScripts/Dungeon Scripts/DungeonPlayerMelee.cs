using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerMelee : DungeonPlayerAction
{
    public delegate void PlayerMeleeDelegate();
    public delegate void PlayerStopDelegate();
    public event PlayerMeleeDelegate OnPlayerMelee;
    public event PlayerStopDelegate OnPlayerStop;

    private void Update()
    {
        if (Input.GetMouseButton(0))
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
