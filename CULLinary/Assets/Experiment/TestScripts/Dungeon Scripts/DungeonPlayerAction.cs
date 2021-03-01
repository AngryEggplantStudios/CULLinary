using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlayerAction : MonoBehaviour
{
    private bool isInvoking = false;

    protected void setIsInvoking(bool b) 
    {
        this.isInvoking = b;
    }

    protected bool getIsInvoking()
    {
        return this.isInvoking;
    }
}
