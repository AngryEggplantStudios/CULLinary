using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerRange : DungeonPlayerAction
{
    public delegate void PlayerAimDelegate();
    public delegate void PlayerStopDelegate();
    public event PlayerAimDelegate OnPlayerAim;
    public event PlayerStopDelegate OnPlayerStop;

    //Weapon References (Current fix until we have more weapons)
    [SerializeField] private GameObject meleeWeapon;
    [SerializeField] private GameObject rangedWeapon;

    
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            this.SetIsInvoking(true);
            OnPlayerAim?.Invoke();
            meleeWeapon.SetActive(false);
            rangedWeapon.SetActive(true);
        }
        else
        {
            this.SetIsInvoking(false);
            OnPlayerStop?.Invoke();
            meleeWeapon.SetActive(true);
            rangedWeapon.SetActive(false);
        }
    }
}
