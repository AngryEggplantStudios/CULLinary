using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerAction : MonoBehaviour
{
    private bool isInvoking = false;

    protected void SetIsInvoking(bool b) 
    {
        this.isInvoking = b;
    }

    public bool GetIsInvoking()
    {
        return this.isInvoking;
    }
}
