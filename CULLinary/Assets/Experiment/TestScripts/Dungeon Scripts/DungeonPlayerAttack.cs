using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonPlayerAttack : DungeonPlayerAction
{
    [SerializeField] private UnityEvent OnPlayerAttack;
    [SerializeField] private UnityEvent OnPlayerStopAttack;
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public UnityEvent GetOnPlayerAttack()
    {
        return this.OnPlayerAttack;
    }

    public UnityEvent GetOnPlayerStopAttack()
    {
        return this.OnPlayerStopAttack;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            this.setIsInvoking(true);
            OnPlayerAttack?.Invoke();
        }
        else
        {
            this.setIsInvoking(false);
            OnPlayerStopAttack?.Invoke();
        }
    }
}
